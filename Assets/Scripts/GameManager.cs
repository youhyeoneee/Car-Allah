using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Waiting, // 대기
    Playing, // 진행중 
    GameOver // 종료
}

public class GameManager : MonoBehaviour
{
    #region singleton

    private static GameManager instance = null;

    // GameManager 인스턴스에 접근하는 프로퍼티
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 GameManager를 찾아보고 없으면 새로 생성
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }

            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        // GameManager 인스턴스가 이미 있는 경우에는 이 인스턴스를 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Game Setting
    public GameState gameState = GameState.Waiting;

    public delegate void OnRepairShop(bool isRepairShop);

    public OnRepairShop onRepairShop;
    public bool isRacingScene = true;
    public bool GameWin = true;

    // Player Setting /////////////////////////////////
    public Transform target;
    public List<CarData> carDatas;
    public string carNumber;
    
    // Time Setting /////////////////////////////////
    private float totalTime = 90.0f; // 총 시간 : 1분 30초
    private float remainTime; // 경과 시간
    private float minute, second;

    // Distance Setting /////////////////////////////////
    private float distanceTraveled = 0f; // 주행 거리 (단위: km)

    private VehicleControl carScript;

    // Singleton Object /////////////////////////////////
    private UIManager uiManager;
    private RankManager rankManager;

    void Start()
    {
        uiManager = UIManager.Instance;
        rankManager = RankManager.Instance;
        
        onRepairShop += IsRacingScene;
    }

    void Init()
    {
        // 게임이 시작하면 남은 시간을 3분으로 초기화 한다.
        remainTime = totalTime;

        // 주행 거리를 초기화한다.
        distanceTraveled = 0.0f;

        // 차량 데이터 초기화
        InitCarData();
    }

    void Update()
    {

        switch (gameState)
        {
            case GameState.Waiting:
                
                break;
            case GameState.Playing:
                if (remainTime > 0.0f)
                {
                    if (GameOver())
                    {
                        gameState = GameState.GameOver;
                        return;
                    }

                    // 시간 계산 및 UI 표시
                    remainTime -= Time.deltaTime;

                    minute = Mathf.Floor(remainTime / 60); // 분을 구하기위해서 60으로 나눈다.
                    second = Mathf.Floor(remainTime % 60); // 나머지 시간을 초로 계산한다.
                    uiManager.ShowGameUI(ChangeTimeToString(minute, second), distanceTraveled);

                    if (!target)
                    {
                        target = GameObject.FindObjectOfType<VehicleControl>().transform;
                        if (!target)
                            return;
                    }


                    if (isRacingScene)
                    {
                        carScript = (VehicleControl)target.GetComponent<VehicleControl>();

                        // 계기판 UI 표시 
                        uiManager.CarUI.ShowCarUI(carScript);


                        // 거리 계산 (과장된 거리)
                        float distance = carScript.speed * ((Time.deltaTime) / 3600f) * 10000; // 시간을 초로 변환
                        distanceTraveled += distance;
                        UpdateCarData(distance);
                    }


                }
                else
                {
                    // 게임 종료
                    gameState = GameState.GameOver;
                    UpdateRank();
                    uiManager.ShowGameOverUI();
                }

                break;
            case GameState.GameOver:
                Debug.Log("GAME OVER");
                carScript.activeControl = false;
                break;
        }

    }


    private void IsRacingScene(bool isRepairshop)
    {
        isRacingScene = !isRepairshop;
    }

    private void InitCarData()
    {
        for (int i = 0; i < carDatas.Count; i++)
        {
            carDatas[i].lastRepairedDistance = 0;
        }
    }
    
    private void UpdateCarData(float distance)
    {
        for (int i = 0; i < carDatas.Count; i++)
        {
            carDatas[i].lastRepairedDistance += distance;
        }
    }


    public void GameStart()
    {
        gameState = GameState.Playing;
        carNumber = uiManager.StartUI.carNumberInput.text; // 차량 넘버
        Init();
    }

    private string ChangeTimeToString(float minute, float second)
    {
        return $"{minute.ToString("00")}:{second.ToString("00")}";
    }
    
    // 부품이 하나라도 이용 불가능한 상태라면 게임 종료
    public bool GameOver()
    {
        for (int i = 0; i < carDatas.Count; i++)
        {
            if (!carDatas[i].IsFunctional())
            {
                GameWin = false;
                carScript.brokenPart = carDatas[i].partName; 
                UpdateRank();
                uiManager.ShowGameOverUI (carDatas[i].GetName()); // UI에 터진 부품 전달
                return true;
            }
        }
        
        return false;
    }

    // 랭크 추가
    public void UpdateRank()
    {
        RankData newRankData = new RankData // 랭크 업데이트
        {
            carNumber = this.carNumber, 
            distance = (int)distanceTraveled, 
            time = ChangeTimeToString(minute, second)
        };
        
        Debug.Log($"{carNumber} {distanceTraveled} {ChangeTimeToString(minute, second)}");

        rankManager.UpdateRankData(newRankData);
    }
}
