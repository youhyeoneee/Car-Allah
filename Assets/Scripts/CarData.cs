using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPartInfos
{
    // 부품 - 한글 string 이름
    private Dictionary<CarPartName, string> partNames = new Dictionary<CarPartName, string>()
    {
        { CarPartName.ENGINE_OIL, "엔진 오일" },
        { CarPartName.TIRE, "타이어" },
        { CarPartName.MISSION_OIL, "미션 오일" },
        { CarPartName.WIPER, "와이퍼" },
        { CarPartName.BREAK_OIL, "브레이크 오일" },
        { CarPartName.TIMING_BELT, "타이밍 벨트" },

    };

    // 부품 - 수명
    private Dictionary<CarPartName, int> partLifes = new Dictionary<CarPartName, int>()
    {
        { CarPartName.ENGINE_OIL, 10000 },
        { CarPartName.TIRE, 50000 },
        { CarPartName.MISSION_OIL, 100000 },
        { CarPartName.WIPER, 20000 },
        { CarPartName.BREAK_OIL, 50000 },
        { CarPartName.TIMING_BELT, 80000 },

    };
    
    // 부품 - 가격
    private Dictionary<CarPartName, int> partPrices = new Dictionary<CarPartName, int>()
    {
        { CarPartName.ENGINE_OIL, 77000 },
        { CarPartName.TIRE, 15000 },
        { CarPartName.MISSION_OIL, 200000 },
        { CarPartName.WIPER, 34000 },
        { CarPartName.BREAK_OIL, 33000 },
        { CarPartName.TIMING_BELT, 270000 },

    };

    
    // 부품에 따른 한글 이름 반환
    public string GetPartName(CarPartName part)
    {
        if (partNames.ContainsKey(part))
        {
            return partNames[part];
        }
        return String.Empty;
    }
    
    // 부품에 따른 수명 반환
    public int GetPartLife(CarPartName part)
    {
        if (partLifes.ContainsKey(part))
        {
            return partLifes[part];
        }
        return 0;
    }
    
    // 부품에 따른 가격 반환
    public int GetPartPrice(CarPartName part)
    {
        if (partLifes.ContainsKey(part))
        {
            return partPrices[part];
        }
        return 0;
    }
}

[System.Serializable]
public class CarData 
{
    
    public CarPartName partName; // 부품 이름 (enum)
    public CarPartName PartName { get { return partName; }}
    public string partNameString; // 부품 이름 (string)
    public string PartNameString { get { return partNameString; }}
    
    public int lifespan;            // 수명 (km)
    public int LifeSpan { get { return lifespan;  }}

    public int price;
    public int Price { get { return price;  }}
    
    public int lastRepairedDistance; //  최근 정비 이후 거리 (km)
    private CarPartInfos partInfos = new CarPartInfos();

    // 생성자
    public CarData()
    {
        partNameString = String.Empty;
    }
    public CarData(CarPartName partName)
    {
        this.partName = partName;
        lifespan = partInfos.GetPartLife(partName);
        partNameString = partInfos.GetPartName(partName);
        price = partInfos.GetPartPrice(partName);
    }

    // 수리 메서드 : 부품 수리 후 마지막 정비 거리 업데이트
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