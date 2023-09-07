using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ChangeScene : MonoBehaviour
{
    private GameManager gameManager;
    
    public static class SceneNames
    {
        public const string RacingScene = "Racing";
        public const string RepairShopScene = "RepairShop";
    }
    
    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    

    public void LoadScene(string sceneName)
    {
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
        if (gameManager.gameState == GameState.Playing)
           LoadScene(SceneNames.RepairShopScene);
    }
    
    
}
