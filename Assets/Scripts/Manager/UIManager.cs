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
            if (!instance)
            {
                Debug.LogWarning("UI Manager Instance is null");
            }
            return instance;
        }
    }
    #endregion
    
    private void Awake()
    {
        // UIManager 인스턴스가 이미 있는 경우에는 이 인스턴스를 파괴
        if (Instance && Instance != this)
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
        public List<RepairButton> repairBtnList;

        public void ActivateUI(bool isActivated)
        {
            uiObject.SetActive(isActivated);
        }

        public void InitUI()
        {
            backBtn.onClick.AddListener(()=> backBtn.gameObject.GetComponent<ChangeScene>().LoadScene(SceneNames.RacingScene));
            
            if (!gameManager)
                gameManager = GameManager.Instance;

            repairBtnList = new List<RepairButton>();

            if (repairBtnList.Count == 0)
            {
                // 정비소 수리 패널 초기화 & 비활성화
                for (int i = 0; i < gameManager.carDatas.Count; i++)
                {
                    GameObject btnObj = Instantiate(repairBtnPrefab);
                
                    btnObj.transform.position = repairPanel.transform.position;
    
                    RectTransform btnpos = btnObj.GetComponent<RectTransform>();
                    btnpos.SetParent(repairPanel.transform, false);
            
                    // 내용 세팅 
                    RepairButton btn = btnObj.GetComponent<RepairButton>();
                    repairBtnList.Add(btnObj.GetComponent<RepairButton>());

                    if (btn != null)
                        btn.SetButton(gameManager.carDatas[i]);
                }
                
                // 수리하기 버튼에 업데이트 
                foreach (var r in repairBtnList)
                {
                    r.repairBtn.onClick.AddListener(() =>
                    {
                        gameManager.coin -= r._carData.Price; // 가격 지불
                        UpdateRepairBtns(); 
                    });
                }
            }
            
            
            
        }

            
        // 모든 수리하기 버튼 갱신
        public void UpdateRepairBtns()
        {
            foreach (var repairBtn in repairBtnList)
            {
                repairBtn.IsAvailable(gameManager.coin);
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
        public TMP_Text coinText; // 소지 코인

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
        gameManager.gameOver += ShowGameOverUI;
    }

    void Update()
    {
        switch (gameManager.gameState)
        {
            case GameState.Waiting:
                StartUI.ActivateUI(true);
                GameUI.ActivateUI(false);
                GameOverUI.ActivateUI(false);
                CarUI.ActivateUI(false);
                StartUI.InitUI();
                break;
            case GameState.Playing:
                CarUI.ActivateUI(false);
                GameUI.ActivateUI(true);
                StartUI.ActivateUI(false);
                break;
        }
    }


    
    private void ShowRepairShopUI(bool isRepairShop)
    {
        if (isRepairShop)
        {
            for (int i = 0; i < RepairShopUI.repairBtnList.Count; i++)
            {
                // 내용 세팅 
                RepairShopUI.repairBtnList[i].SetButton(gameManager.carDatas[i]);
            }
            
            RepairShopUI.UpdateRepairBtns();
            RepairShopUI.ActivateUI(true);
            CarUI.ActivateUI(false);
        }
        else
        {
            RepairShopUI.ActivateUI(false);
            CarUI.ActivateUI(true); 
        }
    }
    
    public void ShowGameUI(string time, int distanceTraveled, int coin)
    {
        // Distance UI /////////////////////////////////
        GameUI.timeText.text = time;
        GameUI.distanceText.text = $"{distanceTraveled.ToString()}";
        GameUI.coinText.text = coin.ToString("#,##0");
    }


    public void InitRepairUI()
    {
        RepairShopUI.InitUI();
        ShowRepairShopUI(false);
    }

    public void ShowGameOverUI(bool isWin, string time, int distance)
    {

        GameOverUI.ActivateUI(true);
        
        if (isWin)
            GameOverUI.gameOverReasonText.text = "축하합니다! 시간 내에 자동차를 고장내지 않았습니다!";
        else
        {
            if (gameManager.brokenCarData != null)
            {
                if (gameManager.brokenCarData.PartNameString.Length > 0)
                    GameOverUI.gameOverReasonText.text = $"{gameManager.brokenCarData.PartNameString}의 수명이 다했습니다..";
                else
                    GameOverUI.gameOverReasonText.text = "운전에 주의하세요!";
            }
           
        }

        // 결과
        GameOverUI.timeText.text = $"남은 시간 : {time}";
        GameOverUI.distanceText.text = $"주행 거리 : {distance} km";
        
        GameUI.ActivateUI(false);
        RepairShopUI.ActivateUI(false);
        CarUI.ActivateUI(false);
    }

}
