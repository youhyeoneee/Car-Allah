using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    
    public delegate void OnRepairShop(bool isRepairShop);
    public OnRepairShop onRepairShop;
    public bool isRacingScene = true;
    
    // Player 
    public Transform target;
    public Vector3 spawnPos;
    public List<CarData> carDatas;
    
    // Time Setting /////////////////////////////////

    private float totalTime = 180.0f; // 총 시간 : 3분
    private float remainTime; // 경과 시간

    // Distance Setting /////////////////////////////////
    private float distanceTraveled = 0f; // 주행 거리 (단위: km)

    private VehicleControl carScript;
    
    // Singleton
    private UIManager uiManager;
    void Start()
    {
        uiManager = UIManager.Instance;
        
        // 게임이 시작하면 남은 시간을 3분으로 초기화 한다.
        remainTime = totalTime;

        // 주행 거리를 초기화한다.
        distanceTraveled = 0.0f;

        onRepairShop += IsRacingScene;
    }

    void Update()
    {
        
        if (remainTime > 0.0f)
        {
            if (GameOver())
            {
                Debug.Log("GAME OVER");
                return;
            }
            
            // 시간 계산 및 UI 표시
            remainTime -= Time.deltaTime;
            float minute, second;
            minute = Mathf.Floor(remainTime / 60); // 분을 구하기위해서 60으로 나눈다.
            second = Mathf.Floor(remainTime % 60); // 나머지 시간을 초로 계산한다.
            uiManager.ShowGameUI(minute, second, distanceTraveled);

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
                uiManager.ShowCarUI(carScript);
            
            
                // 거리 계산
                float distance = carScript.speed * ((Time.deltaTime) / 3600f); // 시간을 초로 변환
                distanceTraveled += distance;
                UpdateCarData(distance);
            }


        }


    }


    private void IsRacingScene(bool isRepairshop)
    {
        isRacingScene = !isRepairshop;
    }

    private void UpdateCarData(float distance)
    {
        for (int i = 0; i < carDatas.Count; i++)
        {
            carDatas[i].lastRepairedDistance += distance;
        }
    }


    public bool GameOver()
    {
        for (int i = 0; i < carDatas.Count; i++)
        {
            if (!carDatas[i].IsFunctional())
                return true;
        }

        return false;
    }
}
