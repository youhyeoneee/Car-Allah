using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float yOffset;
    private float zOffset;
    void Start()
    {
        yOffset = transform.position.y - target.position.y;
        zOffset = transform.position.z - target.position.z;
    }

    void LateUpdate()
    {
        float yPos = target.position.y + yOffset;
        float zPos = target.position.z + zOffset;

        transform.position = new Vector3(target.position.x, yPos, zPos);
    }
}
