using System;
using System.Collections.Generic;
using UnityEngine;

// 랭킹 데이터를 직렬화 가능하게 만든 클래스
[Serializable]
public class RankData
{
    public string carNumber;
    public int distance;
    public string time;
}

// 직렬화 가능한 랭킹 데이터 리스트를 만드는 클래스
[Serializable]
public class SerializableRankData
{
    public List<RankData> rankList;

    public SerializableRankData(List<RankData> list)
    {
        rankList = list;
    }
}

public class RankManager : MonoBehaviour
{
    #region singleton

    private static RankManager instance = null;

    // GameManager 인스턴스에 접근하는 프로퍼티
    public static RankManager Instance
    {
        get
        {
            if (!instance)
            {                
                // Debug.LogWarning("Rank Manager Instance is null");
            }

            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        // GameManager 인스턴스가 이미 있는 경우에는 이 인스턴스를 파괴
        if (Instance && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    // 랭킹 데이터 리스트
    private List<RankData> rankList;

    private void Start()
    {
        // 랭킹 데이터 초기화
        rankList = LoadRankData();
    }

    // 랭킹 데이터를 JSON으로 저장
    public void SaveRankData(List<RankData> rankList)
    {
        string json = JsonUtility.ToJson(new SerializableRankData(rankList));
        PlayerPrefs.SetString(Constants.RANKING_KEY, json);
        PlayerPrefs.Save();
    }

    // JSON을 랭킹 데이터로 불러오기
    public List<RankData> LoadRankData()
    {
        string json = PlayerPrefs.GetString(Constants.RANKING_KEY);
        if (!string.IsNullOrEmpty(json))
        {
            SerializableRankData serializedData = JsonUtility.FromJson<SerializableRankData>(json);
            return serializedData.rankList;
        }
        else
        {
            return new List<RankData>();
        }
    }
    
    // 새로운 랭킹 데이터 추가
    public void UpdateRankData(RankData newRankData)
    {
        // 중복된 carNumber가 있는지 확인
        bool isDuplicate = false;
        for (int i = 0; i < rankList.Count; i++)
        {
            if (rankList[i].carNumber == newRankData.carNumber)
            {
                // 중복된 데이터를 찾았을 때, 랭킹 비교 후 랭킹 갱신
                if (newRankData.distance > rankList[i].distance || 
                    (newRankData.distance == rankList[i].distance && TimeSpan.Parse(newRankData.time) < TimeSpan.Parse(rankList[i].time)))
                {
                    rankList[i] = newRankData; // 랭킹 갱신
                }
                isDuplicate = true;
                break;
            }
        }

        if (!isDuplicate)
        {
            // 중복된 데이터가 없을 경우, 그대로 추가
            rankList.Add(newRankData);
        }

        // 랭킹 데이터 리스트를 정렬 (거리가 큰 순, 거리가 같다면 시간이 짧은 순)
        rankList.Sort((a, b) =>
        {
            if (a.distance != b.distance)
            {
                return b.distance.CompareTo(a.distance); // 거리가 큰 순으로 정렬
            }
            else
            {
                // 시간을 TimeSpan으로 변환하여 비교
                TimeSpan timeA = TimeSpan.Parse(a.time);
                TimeSpan timeB = TimeSpan.Parse(b.time);
                return timeA.CompareTo(timeB); // 시간이 짧은 순으로 정렬
            }
        });

        // 정렬된 랭킹 데이터 리스트를 저장
        SaveRankData(rankList);
    }
}