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
        var stageNextStep = stageManager.UpdateStage();
        if (stageNextStep.Item1 == StagePhaseState.Done)
        {
            switch (stageNextStep.Item2)
            {
                case StageOutcome.Undefined:
                    throw new System.InvalidOperationException();
                case StageOutcome.Win:
                    this.gameState = GameState.Load;
                    break;
                case StageOutcome.Lose:
                    this.gameState = GameState.Reload;
                    break;
                default:
                    break;
            }
        }
    }

    private void EndScreen()
    {
        Debug.Log("end screen");
    }
}
