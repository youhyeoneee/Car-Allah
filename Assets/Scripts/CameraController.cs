using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float yOffset;
    private float zOffset;

    
    // Shake Setting /////////
    private bool isShaked = false;
    private bool isOnShake = false;
    public float shakeTime;
    public float shateIntensity;

    void Start()
    {
        yOffset = transform.position.y - target.position.y;
        zOffset = transform.position.z - target.position.z;
    }

    private void Update()
    {
        if (isOnShake)
            return;

        if (GameManager.Instance.gameState == GameState.GameOver && !isShaked)
        {
            isShaked = true;
            OnShakeCamera();
            return;
        }
  
    }

    void LateUpdate()
    {
        
        if (isOnShake)
            return;
        
        float yPos = target.position.y + yOffset;
        float zPos = target.position.z + zOffset;

        transform.position = new Vector3(target.position.x, yPos, zPos);
        
        
    }

    public void OnShakeCamera()
    {
        StopCoroutine(ShakeByRotation());
        StartCoroutine(ShakeByRotation());
    }

    private IEnumerator ShakeByPosition()
    {
        Vector3 startPosition = transform.position;

        isOnShake = true;
        
        while (shakeTime > 0.0f)
        {
            transform.position = startPosition + Random.insideUnitSphere * shateIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;

        isOnShake = false;
    }

    private IEnumerator ShakeByRotation()
    {
        Vector3 startRotation = transform.eulerAngles;
        isOnShake = true;

        float power = 10f;
        
        while (shakeTime > 0.0f)
        {
            Debug.Log("Shake Time");
            float x = 0; // Random.Range(-1f, 1f);
            float y = 0; // Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);

            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shateIntensity * power);

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.rotation = Quaternion.Euler(startRotation);
        isOnShake = false;
    }
    
}
