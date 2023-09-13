using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankLoader : MonoBehaviour
{
     [SerializeField] private GameObject  rankRataPrefab;
     [SerializeField] private Scrollbar   scrollbar;
     [SerializeField] private Transform   rankRataParent;
     [SerializeField] private RankDataUI    myRankDataUI;
     [SerializeField] private RankManager rankManager;
     
     private List<RankDataUI> rankDataUIList;
     
     void Awake()
     {
         rankDataUIList = new List<RankDataUI>();

         // 20개의 랭크 UI 생성
         for (int i = 0; i < Constants.MAX_RANK_LIST; i++)
         {
             GameObject rankData = Instantiate(rankRataPrefab, rankRataParent);
             rankDataUIList.Add(rankData.GetComponent<RankDataUI>());
         }
     }


     private void OnEnable()
     {
         SetRankListUI();
         // 1위 값이 보이도록 설정
         scrollbar.value = 1;
     }

     
     // 랭킹 UI 설정
     void SetRankListUI()
     {
         // 저장된 랭킹 데이터 불러오기
         List<RankData> loadedRankList = rankManager.LoadRankData();
         int rankCnt = loadedRankList.Count;
         
         for (int i = 0; i <  Constants.MAX_RANK_LIST; i++)
         {
             if (i < rankCnt)
             {
                 var rankData = loadedRankList[i];
                 rankDataUIList[i].SetRankData(i + 1, rankData);
                 
                 if (rankData.carNumber == GameManager.Instance.carNumber)
                     myRankDataUI.SetRankData(i+1, rankData);
             }
         }
     }

}
