using System;
using UnityEngine;


[Serializable]
public abstract class UIBase : MonoBehaviour
{
    [SerializeField] private GameObject uiObject;
    
    public void ActivateUI(bool isActivated) // 활성화, 비활성화
    {
        uiObject.SetActive(isActivated);
    }

    public abstract void InitUI(); // 초기화
    public abstract void UpdateUI(); // 업데이트

}
