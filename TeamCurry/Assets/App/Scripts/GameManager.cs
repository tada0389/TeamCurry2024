using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        Menu,
        Load,
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

    private void UpdateStage()
    {
        bool stageEnded = stageManager.UpdateStage();
        if (stageEnded)
        {
            this.gameState = GameState.Load;
        }
    }

    private void EndScreen()
    {
        Debug.Log("end screen");
    }
}
