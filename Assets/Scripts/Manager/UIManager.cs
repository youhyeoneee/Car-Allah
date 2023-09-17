using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UI;

public class UIManager : MonoBehaviour
{
    
    #region singleton
    private static UIManager instance = null;
    // UIManager 인스턴스에 접근하는 프로퍼티
    public static UIManager Instance
    {
        get
        {
            if (!instance)
            {
                // Debug.LogWarning("UI Manager Instance is null");
            }
            return instance;
        }
    }
    #endregion
    
    private void Awake()
    {
        // UIManager 인스턴스가 이미 있는 경우에는 이 인스턴스를 파괴
        if (Instance && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    public StartUI startUI;
    public GameUI gameUI;
    public CarUI carUI;
    public RepairShopUI repairShopUI;
    public GameOverUI gameOverUI;
 
    private GameManager gameManager;
    private GameState previousGameState = GameState.Waiting;

    void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.onRepairShop += ShowRepairShopUI;
        gameManager.gameOver += ShowGameOverUI;
        
        
        // UI 초기화 
        startUI.InitUI();
        carUI.InitUI();
        gameOverUI.InitUI();
        gameUI.InitUI();
    }

    void Update()
    {
        if (gameManager.gameState != previousGameState)
        {
            previousGameState = gameManager.gameState;
            HandleGameStateChange();
        }
    }

    // 게임 스테이트 별로 UI 활성화, 비활성화
    private void HandleGameStateChange()
    {
        switch (gameManager.gameState)
        {
            case GameState.Waiting:
                startUI.ActivateUI(true);
                gameUI.ActivateUI(false);
                gameOverUI.ActivateUI(false);
                carUI.ActivateUI(true);
                break;
            case GameState.Playing:
                startUI.ActivateUI(false);
                gameUI.ActivateUI(true);
                break;
            case GameState.GameOver:
                gameOverUI.ActivateUI(true);
                gameUI.ActivateUI(false);
                repairShopUI.ActivateUI(false);
                carUI.ActivateUI(false);
                break;
        }
    }
    
    // 게임 UI 보여주기
    public void ShowGameUI(string time, int distanceTraveled, int coin)
    {
        gameUI.UpdateUI(time, distanceTraveled, coin);
    }


    // 정비소 UI 초기화
    public void InitRepairUI()
    {
        repairShopUI.InitUI();
        ShowRepairShopUI(false);
    }

    // 정비소 나왔다 들어갔다 할 때 UI 보여주기
    private void ShowRepairShopUI(bool isRepairShop)
    {
        if (isRepairShop)
        {
            for (int i = 0; i < repairShopUI.repairBtnList.Count; i++)
            {
                // 내용 세팅 
                repairShopUI.repairBtnList[i].UpdateButtn();
            }
            
            repairShopUI.UpdateUI();
            repairShopUI.ActivateUI(true);
            carUI.ActivateUI(false);
        }
        else
        {
            repairShopUI.ActivateUI(false);
            carUI.ActivateUI(true); 
        }
    }
    
    // 게임 오버 UI 보여주기
    public void ShowGameOverUI(bool isWin, string time, int distance)
    {

        string reason = String.Empty;
        
        if (isWin)
            reason = "축하합니다! 시간 내에 자동차를 고장내지 않았습니다!";
        else
        {
            if (gameManager.brokenCarData.PartNameString.Length > 0)
                reason = $"{gameManager.brokenCarData.PartNameString}의 수명이 다했습니다..";
            else
                reason = "운전에 주의하세요!";
        }

        gameOverUI.UpdateUI(time, distance, reason);
        
    }

}
