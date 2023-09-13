using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    
    public string playerName { get; set; } //플레이어 이름 
    public bool   isReady    { get; set; } // 플레이어 준비 상태 
    
    // 플레이어 UI 
    [Header("Name")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button         nameSaveBtn;
    [SerializeField] private TMP_Text       nameText;

    /// <summary>
    /// by 유현.
    /// [이름 설정 메서드]
    /// 플레이어의 이름을 설정하고 준비 완료로 변경한다.
    /// </summary>
    private void SetName()
    {
        // 이름 설정 
        playerName = nameInput.text;
        nameText.text = playerName;
        
        // UI 비활성화
        nameInput.gameObject.SetActive(false);
        nameSaveBtn.gameObject.SetActive(false);
        
        // 준비 완료 
        isReady = true;
    }
}
