using System.Collections.Generic;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
        var stagePhaseState = stageManager.UpdateStage();
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
