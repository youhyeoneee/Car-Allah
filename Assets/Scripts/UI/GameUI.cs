using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


namespace UI
{
    
    [Serializable]
    public class GameUI : UIBase
    {
        public Text timeText; // 남은 시간
        public Text distanceText;  // 주행 거리
        public TMP_Text coinText; // 소지 코인
        
        private string time;
        private int distanceTraveled;
        private int coin;

        public override void InitUI()
        {
            ActivateUI(false);
        }

        public override void UpdateUI()
        {
            // 갱신 로직 추가
            timeText.text = time;
            distanceText.text = $"{distanceTraveled.ToString()}";
            coinText.text = coin.ToString("#,##0");
        }

        // 추가된 매개변수를 사용하여 UpdateUI 메서드 오버로드
        public void UpdateUI(string time, int distanceTraveled, int coin)
        {
            this.time = time;
            this.distanceTraveled = distanceTraveled;
            this.coin = coin;

            UpdateUI(); // 이전에 정의한 메서드를 재사용
        }
    }
}