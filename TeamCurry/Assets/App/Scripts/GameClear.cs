using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : Stage
{
    [SerializeField] private float time = 8f;
    [SerializeField] private GameObject body;

    float restartTime = 0.0f;

    public override StagePhaseState Setup()
    {
        body.SetActive(true);
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        SEManager.Instance.Play(SEPath.GAME_CLEAR_CHEER);
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        // time -= Time.deltaTime;
        // return time >= 0 ? StagePhaseState.Active : StagePhaseState.Done;

        restartTime += Time.deltaTime;
        if (restartTime > 5.0f)
        {
            if (Input.GetKeyDown(KeyCode.Space) || InputSystem.JoyconInput.Instance.IsAnyButtonDown)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        return StagePhaseState.Active;
    }

    public override StagePhaseState End()
    {
        return StagePhaseState.Done;
    }

    public override StagePhaseState Shutdown()
    {
        return StagePhaseState.Done;
    }
}
