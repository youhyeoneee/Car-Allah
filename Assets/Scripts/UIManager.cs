using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

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


    public GameUIClass GameUI;
    public CarUIClass CarUI;
    public RepairShopUIClass RepairShopUI;
    public GameOverUIClass GameOverUI;
    
    [System.Serializable]
    public class GameOverUIClass
    {
        public GameObject gameOverImg;
        public TMP_Text gameOverReasonText;
    }
    
    [System.Serializable]
    public class RepairShopUIClass
    {
        public GameObject repairPanel;
        public GameObject backBtn;
        public GameObject repairBtnPrefab;
    }
    
    [System.Serializable]
    public class GameUIClass
    {
        public GameObject uiObject;
        public Text timeText; // 남은 시간
        public Text distanceText;  // 주행 거리

        public void DeactiveUI()
        {
            uiObject.SetActive(false);
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
        public void DeactiveUI()
        {
            uiObject.SetActive(false);
        }
    }
    
    private int gearst = 0;
    private float thisAngle = -150;
    private float restTime = 0.0f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.onRepairShop += ShowRepairShopUI;


        InitRepairShopUI();
        ShowRepairShopUI(false);

    }

    void Update()
    {
    }


    private void InitRepairShopUI()
    {
                
        // 정비소 수리 패널 초기화 & 비활성화
        for (int i = 0; i < gameManager.carDatas.Count; i++)
        {
            
            GameObject btnObj = Instantiate(RepairShopUI.repairBtnPrefab);
            btnObj.transform.position = RepairShopUI.repairPanel.transform.position;
    
            RectTransform btnpos = btnObj.GetComponent<RectTransform>();
            btnpos.SetParent(RepairShopUI.repairPanel.transform);
            
            // 내용 세팅 
            RepairButton btn = btnObj.GetComponent<RepairButton>();
    
            if (btn != null)
                btn.SetButton(gameManager.carDatas[i]);
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
        
        RepairShopUI.repairPanel.SetActive(isRepairShop);
        RepairShopUI.backBtn.SetActive(isRepairShop);
    }
    
    public void ShowGameUI(float minute, float second, float distanceTraveled)
    {
        // Distance UI /////////////////////////////////
        // 주행 거리 계산 (거리 = 속도 × 시간)
        GameUI.timeText.text = $"{minute.ToString("00")} : {second.ToString("00")}";
        GameUI.distanceText.text = $"{distanceTraveled.ToString("F0")}";
    }
    
    public void ShowCarUI(VehicleControl carScript)
    {
        gearst = carScript.currentGear;
        CarUI.speedText.text = ((int)carScript.speed).ToString();

        if (carScript.carSetting.automaticGear)
        {

            if (gearst > 0 && carScript.speed > 1)
            {
                CarUI.GearText.color = Color.green;
                CarUI.GearText.text = gearst.ToString();
            }
            else if (carScript.speed > 1)
            {
                CarUI.GearText.color = Color.red;
                CarUI.GearText.text = "R";
            }
            else
            {
                CarUI.GearText.color = Color.white;
                CarUI.GearText.text = "N";
            }

        }
        else
        {

            if (carScript.NeutralGear)
            {
                CarUI.GearText.color = Color.white;
                CarUI.GearText.text = "N";
            }
            else
            {
                if (carScript.currentGear != 0)
                {
                    CarUI.GearText.color = Color.green;
                    CarUI.GearText.text = gearst.ToString();
                }
                else
                {

                    CarUI.GearText.color = Color.red;
                    CarUI.GearText.text = "R";
                }
            }

        }
        
        thisAngle = (carScript.motorRPM / 20) - 175;
        thisAngle = Mathf.Clamp(thisAngle, -180, 90);

        CarUI.tachometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -thisAngle);
        CarUI.barShiftGUI.rectTransform.localScale = new Vector3(carScript.powerShift / 100.0f, 1, 1);
    }

    public void ShowGameOverUI(string partName)
    {
        GameOverUI.gameOverImg.SetActive(true);
        GameOverUI.gameOverReasonText.text = $"{partName}의 수명이 다했습니다..";
        
        GameUI.DeactiveUI();
        CarUI.DeactiveUI();
    }

}
