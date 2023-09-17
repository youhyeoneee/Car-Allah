using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


namespace UI
{
    
    [Serializable]
    public class StartUI : UIBase
    {
        public Button gameStartBtn; // 게임 시작 버튼
        public TMP_InputField nickNameInput; // 닉네임 입력
        
        
        // 초기화
        public override void InitUI()
        {
            gameStartBtn.onClick.AddListener(() => GameManager.Instance.GameStart());
            
            ActivateUI(true);
        }

        public override void UpdateUI() {}
    }
}