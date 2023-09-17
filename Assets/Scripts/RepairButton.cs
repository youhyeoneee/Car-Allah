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
    public CarData carData;


    // 버튼 내용 초기화
    public void InitButton(CarData _carData)
    {
        carData = _carData;
        repairBtn.onClick.AddListener(()=> Repair());
        partName.text = _carData.PartNameString;
        priceText.text = $"수리 {_carData.Price.ToString("#,##0")}";
    }
    
    
    // 활성화 비활성화
    public void IsAvailable(int coin)
    {
        if (coin >= carData.Price)
            repairBtn.interactable = true;
        else
            repairBtn.interactable = false;
    }
    
    
    // 수리하기 버튼 : 수리 후 버튼 내용 갱신
    private void Repair()
    {
        carData.Repair();
        UpdateButtn();
    }
    
    // 거리 갱신
    public void UpdateButtn()
    {
        lastRepairedDistanceText.text = $"마지막 수리 후 \n {carData.lastRepairedDistance.ToString("F0")} km 주행함.";
    }
    
}
