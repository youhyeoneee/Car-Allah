using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CarPartName
{
    ENGINE_OIL,
    TIRE,
}

[System.Serializable]
public class CarData
{
    public CarPartName partName;         // 부품 이름 (ex. 엔진오일, 바퀴 등)
    public float lifespan;            // 수명 (km)
    public float lastRepairedDistance; //  최근 정비 이후 거리 (km)
        
    public CarData(CarPartName name, int life)
    {
        partName = name;
        lifespan = life;
        lastRepairedDistance = 0; // 초기화
    }

    // 정비 메서드 : 부품 정비 후 마지막 정비 거리 업데이트
    public void Repair()
    {
        Debug.Log(partName + "REPAIRED");
        lastRepairedDistance = 0;
    }

    // 부품 상태 확인 메서드 : 사용 가능 여부 반환
    public bool IsFunctional()
    {
        return (lifespan - lastRepairedDistance) > 0;
    }
}