using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ChangeScene : MonoBehaviour
{
    private GameManager gameManager;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        if (GameManager.Instance != null)
        {
            if (sceneName == SceneNames.RepairShopScene)
            {
                GameManager.Instance.onRepairShop.Invoke(true);
            }
            else if (sceneName == SceneNames.RacingScene)
            {
                GameManager.Instance.onRepairShop.Invoke(false);
            }
        }
    }
    
    
    // Racing -> RepairShop
    private void OnTriggerStay(Collider other)
    {
        // 게임 플레이 중일 때만 !
        if (GameManager.Instance.gameState == GameState.Playing)
           LoadScene(SceneNames.RepairShopScene);
    }
    
    
}
