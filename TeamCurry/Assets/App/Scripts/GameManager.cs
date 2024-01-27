using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        Menu,
        Load,
        Stage
    }

    [SerializeField] private List<Stage> stages;
    [SerializeField] private GameState gameState;

    private Stage currentStage;

    private void Start()
    {
        currentStage = null;
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
                currentStage.UpdateStage();
                break;
        }
    }

    private void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Load stage 1");
            currentStage = stages[0];
            gameState = GameState.Load;
        }
    }

    private void Load()
    {
        currentStage.LoadStage();
        gameState = GameState.Stage;
    }
}
