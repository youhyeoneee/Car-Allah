using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankDataUI : MonoBehaviour
{
    // 랭크 UI 
    [Header("Rank")]
    [SerializeField] private TMP_Text       rankText; // 순위
    [SerializeField] private TMP_Text       carNumberText; // 차량 번호
    [SerializeField] private TMP_Text       distanceText; // 거리
    [SerializeField] private TMP_Text       timeText; // 시간
    
    public int Rank
    {
        set
        {
            if (value > 0)
            {
                rankText.text = value.ToString() + "위";
            }
            else
            {
                rankText.text = "-";
            }
        }
    }

    public string CarNumber
    {
        set
        {
            carNumberText.text = value;
        }
    }
    
    public int Distance
    {
        set
        {
            distanceText.text = value + "km";
        }
    }

    public string Time
    {
        set
        {
            timeText.text = value;
        }
    }
    
    // 랭크 데이터 설정
    public void SetRankData(int rank, RankData rankData)
    {
        Rank = rank;
        CarNumber = rankData.carNumber;
        Distance = rankData.distance;
        Time = rankData.time;
    }
}
