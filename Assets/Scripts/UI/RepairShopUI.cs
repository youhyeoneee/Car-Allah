using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
    
namespace UI
{
       
    [Serializable]
    public class RepairShopUI : UIBase
    {
        public GameObject repairPanel;
        public Button backBtn;
        public GameObject repairBtnPrefab;
        public List<RepairButton> repairBtnList;
        
        private GameManager gameManager;

        public override void InitUI()
        {
            // 뒤로가기 버튼 초기화
            backBtn.onClick.AddListener(()=> backBtn.gameObject.GetComponent<ChangeScene>().LoadScene(SceneNames.RacingScene));
            
            // 게임 인스턴스 찾기
            if (!gameManager)
            {
                gameManager = GameManager.Instance;
            }

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

                    if (!btn)
                        return;
                    
                    btn.InitButton(gameManager.carDatas[i]);
                    btn.repairBtn.onClick.AddListener(() =>
                    {
                        gameManager.coin -= btn.carData.Price; // 가격 지불
                        UpdateUI(); 
                    });
                }
                
            }
            
            ActivateUI(false);
        }
        
        
        // 업데이트
        public override void UpdateUI()
        {
            foreach (var repairBtn in repairBtnList)
            {
                repairBtn.IsAvailable(gameManager.coin);
            }
        }
    }
}