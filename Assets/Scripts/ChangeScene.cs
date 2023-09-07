using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Button changeSceneBtn;

    private GameManager gameManager;
    public static class SceneNames
    {
        public const string RacingScene = "Racing";
        public const string RepairShopScene = "RepairShop";
    }
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        
        // 돌아가기 버튼에 할당
        if (changeSceneBtn != null)
        {
            changeSceneBtn.onClick.AddListener(()=> LoadScene(SceneNames.RacingScene));
        }
    }
    
    

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        
        // Bug :: GamaManager.instance가 씬 2번 전환시 null 
        
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
