using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RepairButton : MonoBehaviour
{
    
    public TMP_Text partName; // 부품 이름
    public TMP_Text lastRepairedDistanceText; // 최근 정비 이후 거리
    public Button repairBtn; // 정비하기 버튼

    private CarData _carData;
    private GameManager gameManager;

    public void SetButton(CarData carData)
    {
        _carData = carData;
        repairBtn.onClick.AddListener(()=> UpdateButton());
        partName.text = _carData.GetName();
        lastRepairedDistanceText.text = "최근 수리 : " + _carData.lastRepairedDistance.ToString("F2") + "km";
    }

    private void UpdateButton()
    {
        _carData.Repair();
        lastRepairedDistanceText.text = "최근 수리 : " + _carData.lastRepairedDistance.ToString("F2") + "km";
    }
    
}
