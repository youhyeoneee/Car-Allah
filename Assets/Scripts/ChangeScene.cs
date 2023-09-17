using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ChangeScene : MonoBehaviour
{
    
    public void LoadScene(string sceneName)
    {
        var gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogWarning("GameManager is null");
            return;
        }

        SceneManager.LoadScene(sceneName);

        if (sceneName == SceneNames.RepairShopScene)
        {
            gameManager.onRepairShop.Invoke(true);
        }
        else if (sceneName == SceneNames.RacingScene)
        {
            gameManager.onRepairShop.Invoke(false);
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
