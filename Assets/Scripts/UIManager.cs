using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    #region singleton
    private static UIManager instance = null;
    // UIManager 인스턴스에 접근하는 프로퍼티
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 UIManager를 찾아보고 없으면 새로 생성
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    instance = go.AddComponent<UIManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    #endregion
    
    private void Awake()
    {
        // UIManager 인스턴스가 이미 있는 경우에는 이 인스턴스를 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    public StartUIClass StartUI;
    public GameUIClass GameUI;
    public CarUIClass CarUI;
    public RepairShopUIClass RepairShopUI;
    public GameOverUIClass GameOverUI;
    
    [System.Serializable]
    public class GameOverUIClass
    {
        public GameObject gameOverImg;
        public TMP_Text gameOverReasonText;
        public TMP_Text timeText;
        public TMP_Text distanceText;
        public void ActivateUI(bool isActivated)
        {
            gameOverImg.SetActive(isActivated);
        }

    }
    
    [System.Serializable]
    public class RepairShopUIClass
    {
        public GameObject uiObject;
        public GameObject repairPanel;
        public Button backBtn;
        public GameObject repairBtnPrefab;
        private GameManager gameManager;
        public void ActivateUI(bool isActivated)
        {
            uiObject.SetActive(isActivated);
        }

        public void InitUI()
        {
            backBtn.onClick.AddListener(()=> backBtn.gameObject.GetComponent<ChangeScene>().LoadScene(SceneNames.RacingScene));
            
            if (!gameManager)
                gameManager = GameManager.Instance;
            // 정비소 수리 패널 초기화 & 비활성화
            for (int i = 0; i < gameManager.carDatas.Count; i++)
            {
            
                GameObject btnObj = Instantiate(repairBtnPrefab);
                btnObj.transform.position = repairPanel.transform.position;
    
                RectTransform btnpos = btnObj.GetComponent<RectTransform>();
                btnpos.SetParent(repairPanel.transform, false);
            
                // 내용 세팅 
                RepairButton btn = btnObj.GetComponent<RepairButton>();
    
                if (btn != null)
                    btn.SetButton(gameManager.carDatas[i]);
            }
        }
    }
    
    [System.Serializable]
    public class StartUIClass
    {
        public GameObject uiObject;
        public Button gameStartBtn; // 게임 시작 버튼
        public TMP_InputField carNumberInput; // 차량 번호 입력 

        public void ActivateUI(bool isActivated)
        {
            uiObject.SetActive(isActivated);
        }

        public void InitUI()
        {
            gameStartBtn.onClick.AddListener(() => GameManager.Instance.GameStart());
        }
    }
    
    
    [System.Serializable]
    public class GameUIClass
    {
        public GameObject uiObject;
        public Text timeText; // 남은 시간
        public Text distanceText;  // 주행 거리

        public void ActivateUI(bool isActivated)
        {
            uiObject.SetActive(isActivated);
        }
    }
    
    [System.Serializable]
    public class CarUIClass
    {
        public GameObject uiObject;
        public Image tachometerNeedle;
        public Image barShiftGUI;

        public Text speedText;
        public Text GearText;
        private int gearst = 0;
        private float thisAngle = -150;

        public void ActivateUI(bool isActivated)
        {
            uiObject.SetActive(isActivated);
        }
        
        public void ShowCarUI(VehicleControl carScript)
        {
            gearst = carScript.currentGear;
            speedText.text = ((int)carScript.speed).ToString();

            if (carScript.carSetting.automaticGear)
            {

                if (gearst > 0 && carScript.speed > 1)
                {
                    GearText.color = Color.green;
                    GearText.text = gearst.ToString();
                }
                else if (carScript.speed > 1)
                {
                    GearText.color = Color.red;
                    GearText.text = "R";
                }
                else
                {
                    GearText.color = Color.white;
                    GearText.text = "N";
                }

            }
            else
            {

                if (carScript.NeutralGear)
                {
                    GearText.color = Color.white;
                    GearText.text = "N";
                }
                else
                {
                    if (carScript.currentGear != 0)
                    {
                        GearText.color = Color.green;
                        GearText.text = gearst.ToString();
                    }
                    else
                    {

                        GearText.color = Color.red;
                        GearText.text = "R";
                    }
                }

            }
        
            thisAngle = (carScript.motorRPM / 20) - 175;
            thisAngle = Mathf.Clamp(thisAngle, -180, 90);

            tachometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -thisAngle);
            barShiftGUI.rectTransform.localScale = new Vector3(carScript.powerShift / 100.0f, 1, 1);
        }
    }
    
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.onRepairShop += ShowRepairShopUI;
        
        RepairShopUI.InitUI();
        ShowRepairShopUI(false);
    }

    void Update()
    {
        switch (gameManager.gameState)
        {
            case GameState.Waiting:
                StartUI.ActivateUI(true);
                GameOverUI.ActivateUI(false);
                StartUI.InitUI();
                break;
            case GameState.Playing:
                StartUI.ActivateUI(false);
                break;
        }
    }


    
    private void ShowRepairShopUI(bool isRepairShop)
    {
        if (isRepairShop)
        {
            for (int i = 0; i < RepairShopUI.repairPanel.transform.childCount; i++)
            {
                GameObject btnObj = RepairShopUI.repairPanel.transform.GetChild(i).gameObject;
                // 내용 세팅 
                RepairButton btn = btnObj.GetComponent<RepairButton>();

                if (btn != null)
                    btn.SetButton(gameManager.carDatas[i]);
            }
        }
        
        if (isRepairShop)
        {
            RepairShopUI.ActivateUI(true);
            CarUI.ActivateUI(false);
        }
        else
        {
            RepairShopUI.ActivateUI(false);
            CarUI.ActivateUI(true); 
        }
    }
    
    public void ShowGameUI(string time,float distanceTraveled)
    {
        // Distance UI /////////////////////////////////
        // 주행 거리 계산 (거리 = 속도 × 시간)

        GameUI.timeText.text = time;
        GameUI.distanceText.text = $"{distanceTraveled.ToString("F0")}";
    }
    
    

    public void ShowGameOverUI(string partName = "")
    {

        GameOverUI.ActivateUI(true);
        
        if (partName.Length > 0)
            GameOverUI.gameOverReasonText.text = $"{partName}의 수명이 다했습니다..";
        else
            GameOverUI.gameOverReasonText.text = "축하합니다! 시간 내에 자동차를 고장내지 않았습니다!";

        // 결과
        GameOverUI.timeText.text = "남은 시간 : "+ GameUI.timeText.text;
        GameOverUI.distanceText.text = "주행 거리 : " + GameUI.distanceText.text + "km";
        
        GameUI.ActivateUI(false);
        CarUI.ActivateUI(false);
    }

}
