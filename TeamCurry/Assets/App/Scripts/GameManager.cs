using System.Collections.Generic;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        Menu,
        Load,
        Reload,
        Stage,
        EndScreen
    }

    [SerializeField] private StageManager stageManager;
    [SerializeField] private GameState gameState;
    [SerializeField] private Title.TitleStaging titleStaging;

    private void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
    }

    private void Start()
    {
        gameState = GameState.Menu;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.Menu:
                Menu();
                break;
            case GameState.Load:
                Load();
                break;
            case GameState.Reload:
                Reload();
                break;
            case GameState.Stage:
                UpdateStage();
                break;
            case GameState.EndScreen:
                EndScreen();
                break;
        }
    }

    private void Menu()
    {
        if (titleStaging.MenuComplete)
        {
            titleStaging.HideTitleScreen();
            gameState = GameState.Load;
        }
    }

    private void Load()
    {
        bool loaded = stageManager.LoadNextStage();
        if (loaded)
        {
            gameState = GameState.Stage;
        }
        else
        {
            gameState = GameState.EndScreen;
        }
    }

    private void Reload()
    {
        stageManager.ReloadStage();
        gameState = GameState.Stage;
    }

    private void UpdateStage()
    {
        StagePhaseState stagePhaseState = stageManager.UpdateStage();
        if (stagePhaseState == StagePhaseState.Done)
        {
            switch (this.stageManager.CurrentStage.StageOutcome)
            {
                case StageOutcome.Undefined:
                    Debug.LogError("o no");
                    break;
                case StageOutcome.Win:
                    this.gameState = GameState.Load;
                    break;
                case StageOutcome.Lose:
                    this.gameState = GameState.Reload;
                    break;
            }
        }
    }

    private void EndScreen()
    {
        Debug.Log("end screen");
    }
}
