using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public Transform target;

    public GameUIClass GameUI;

    // Time Setting /////////////////////////////////

    private float totalTime = 180.0f; // 총 시간 : 3분
    private float remainTime; // 경과 시간

    // Distance Setting /////////////////////////////////
    private float distanceTraveled = 0f; // 주행 거리 (단위: km)

    private VehicleControl carScript;

    [System.Serializable]
    public class GameUIClass
    {
        public Text timeText; // 남은 시간
        public Text distanceText; // 주행 거리

    }

    void Start()
    {
        // 게임이 시작하면 남은 시간을 3분으로 초기화 한다.
        remainTime = totalTime;

        // 주행 거리를 초기화한다.
        distanceTraveled = 0.0f;
    }

    void Update()
    {
        
        if(remainTime > 0.0f)
        {
            // 시간 감소
            remainTime -= Time.deltaTime;

            if (!target) return;
            carScript = (VehicleControl)target.GetComponent<VehicleControl>();

            ShowGameUI();
        }


    }

    void ShowGameUI()
    {
        // Time UI /////////////////////////////////
        float minute, second;

        minute = Mathf.Floor(remainTime / 60); // 분을 구하기위해서 60으로 나눈다.
        second = Mathf.Floor(remainTime % 60); // 나머지 시간을 초로 계산한다.

        GameUI.timeText.text = $"{minute.ToString("00")} : {second.ToString("00")}";

        // Distance UI /////////////////////////////////
        // 주행 거리 계산 (거리 = 속도 × 시간)
        distanceTraveled += carScript.speed * ((Time.deltaTime) / 3600f); // 시간을 초로 변환

        GameUI.distanceText.text = $"{distanceTraveled.ToString("F2")}";
    }

}
