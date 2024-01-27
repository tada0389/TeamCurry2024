using System;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private enum StagePhase
    {
        Setup,
        Start,
        Playing,
        End,
        Shutdown,
    }

    [SerializeField] private StagePhase stagePhase;

    public void LoadStage()
    {
        stagePhase = StagePhase.Setup;
    }

    public void UpdateStage()
    {
        switch (stagePhase)
        {
            case StagePhase.Setup:
                SetupStage();
                break;
            case StagePhase.Start:
                StartStage();
                break;
            case StagePhase.Playing:
                PlayStage();
                break;
            case StagePhase.End:
                EndStage();
                break;
            case StagePhase.Shutdown:
                ShutdownStage();
                break;
            default:
                break;
        }
    }

    private void SetupStage()
    {
        // Do setup + start
        stagePhase = StagePhase.Start;
    }

    private void StartStage()
    {
        stagePhase = StagePhase.Playing;
    }

    private void PlayStage()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Win();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Lose();
        }
    }

    private void Win()
    {
        Debug.Log("win!");
        stagePhase = StagePhase.End;
    }

    private void Lose()
    {
        Debug.Log("lose!");
        stagePhase = StagePhase.End;
    }

    private void EndStage()
    {
        // Do end stage stuff
        stagePhase = StagePhase.Shutdown;
    }

    private void ShutdownStage()
    {
        // Shutdown the stage
    }
}