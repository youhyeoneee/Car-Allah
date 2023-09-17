using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RepairButton : MonoBehaviour
{
    
    public TMP_Text partName; // 부품 이름
    public TMP_Text lastRepairedDistanceText; // 최근 수리 이후 거리
    public Button repairBtn; // 수리하기 버튼
    public TMP_Text priceText; // 수리 가격 

    public CarData _carData;

    // 버튼 내용 세팅
    public void SetButton(CarData carData)
    {
        _carData = carData;
        repairBtn.onClick.AddListener(()=> UpdateButton());
        partName.text = _carData.PartNameString;
        lastRepairedDistanceText.text = $"마지막 수리 후 \n {_carData.lastRepairedDistance.ToString("F0")} km 주행함.";
        priceText.text = $"수리 {_carData.Price.ToString("#,##0")}";
    }

    
    // 활성화 비활성화
    public void IsAvailable(int coin)
    {
        if (coin >= _carData.Price)
            repairBtn.interactable = true;
        else
            repairBtn.interactable = false;
    }
    
    // 수리하기 버튼 : 수리 후 버튼 내용 갱신
    private void UpdateButton()
    {
        _carData.Repair();
        lastRepairedDistanceText.text = $"마지막 수리 후 \n {_carData.lastRepairedDistance.ToString("F0")} km 주행함.";
    }
    
}
