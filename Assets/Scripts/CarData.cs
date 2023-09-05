using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CarPartName
{
    ENGINE_OIL,
    TIRE,
}

public class CarPartStrings
{
    private Dictionary<CarPartName, string> partNames = new Dictionary<CarPartName, string>()
    {
        { CarPartName.ENGINE_OIL, "엔진 오일" },
        { CarPartName.TIRE, "타이어" },
    };

    public string GetPartName(CarPartName part)
    {
        if (partNames.ContainsKey(part))
        {
            return partNames[part];
        }
        return "Unknown";
    }
}

[System.Serializable]
public class CarData 
{
    public CarPartName partName;
    public float lifespan;            // 수명 (km)
    public float lastRepairedDistance; //  최근 정비 이후 거리 (km)
    private CarPartStrings partStrings = new CarPartStrings();
    
    
    // 이름 반환 메서드 : Enum -> String 이름 반환
    public string GetName()
    {
        return partStrings.GetPartName(partName);
    }

    // 정비 메서드 : 부품 정비 후 마지막 정비 거리 업데이트
    public void Repair()
    {
        lastRepairedDistance = 0;
    }

    // 부품 상태 확인 메서드 : 사용 가능 여부 반환
    public bool IsFunctional()
    {
        return (lifespan - lastRepairedDistance) > 0;
    }
}