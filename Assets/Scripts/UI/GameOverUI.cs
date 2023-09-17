using UnityEngine;
using System;
using TMPro;
    
namespace UI
{
       
    [Serializable]
    public class GameOverUI : UIBase
    {
        public TMP_Text gameOverReasonText;
        public TMP_Text timeText;
        public TMP_Text distanceText;
        
        private string time;
        private int distance;
        private string reason;

        public override void InitUI()
        {
            ActivateUI(false);
        }

        public override void UpdateUI()
        {
            // 결과
            timeText.text = $"남은 시간 : {time}";
            distanceText.text = $"주행 거리 : {distance} km";
            gameOverReasonText.text = reason;
        }

        // 추가된 매개변수를 사용하여 UpdateUI 메서드 오버로드
        public void UpdateUI(string time, int distance, string reason)
        {
            this.time = time;
            this.distance = distance;
            this.reason = reason;

            UpdateUI(); // 이전에 정의한 메서드를 재사용
        }
        
        
    }
}