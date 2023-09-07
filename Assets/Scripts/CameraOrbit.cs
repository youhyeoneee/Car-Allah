using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{

    // 속도같은 경우 항상 deltaTime 으로 단위 맞추기

    [SerializeField] private Transform follow;
    [SerializeField] private float rotationSpeed; 


    void LateUpdate()       
    {
        transform.RotateAround(follow.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }


}