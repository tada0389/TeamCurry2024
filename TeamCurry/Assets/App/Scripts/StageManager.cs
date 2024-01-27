using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<Stage> stages;
    private int currentStageIndex = -1;

    public Stage CurrentStage => stages[currentStageIndex];

    public bool LoadNextStage()
    {
        currentStageIndex++;
        if (currentStageIndex == stages.Count)
        {
            currentStageIndex = -1;
            return false;
        }

        CurrentStage.StagePhase = StagePhase.Setup;
        return true;
    }

    public void ReloadStage()
    {
        CurrentStage.StagePhase = StagePhase.Setup;
    }

    public StagePhaseState UpdateStage()
    {
        switch (CurrentStage.StagePhase)
        {
            case StagePhase.Unloaded:
                return StagePhaseState.Done;
            case StagePhase.Setup:
                StagePhaseState setupState = CurrentStage.Setup();
                if (setupState == StagePhaseState.Done)
                {
                    CurrentStage.StagePhase = StagePhase.Start;
                }
                break;
            case StagePhase.Start:
                StagePhaseState startState = CurrentStage.StartStage();
                if (startState == StagePhaseState.Done)
                {
                    CurrentStage.StagePhase = StagePhase.Playing;
                }
                break;
            case StagePhase.Playing:
                StagePhaseState playState = CurrentStage.Play();
                if (playState == StagePhaseState.Done)
                {
                    CurrentStage.StagePhase = StagePhase.End;
                }
                break;
            case StagePhase.End:
                StagePhaseState endState = CurrentStage.End();
                if (endState == StagePhaseState.Done)
                {
                    CurrentStage.StagePhase = StagePhase.Shutdown;
                }
                break;
            case StagePhase.Shutdown:
                StagePhaseState shutdownState = CurrentStage.Shutdown();
                if (shutdownState == StagePhaseState.Done)
                {
                    CurrentStage.StagePhase = StagePhase.Unloaded;
                }
                break;
            default:
                break;
        }

        return StagePhaseState.Active;
    }
}

public enum StagePhase
{
    Unloaded = 0,
    Setup,
    Start,
    Playing,
    End,
    Shutdown,
}

public enum StagePhaseState
{
    Active,
    Done
}

public enum StageOutcome
{
    Undefined,
    Win,
    Lose
}
