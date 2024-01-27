using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<Stage> stages;
    private int currentStageIndex = -1;

    private Stage CurrentStage => stages[currentStageIndex];

    public bool LoadNextStage()
    {
        currentStageIndex++;
        if (currentStageIndex == stages.Count)
        {
            currentStageIndex = -1;
            return false;
        }

        CurrentStage.stagePhase = StagePhase.Setup;
        return true;
    }

    public void ReloadStage()
    {
        CurrentStage.stagePhase = StagePhase.Setup;
    }

    public bool UpdateStage()
    {
        switch (CurrentStage.stagePhase)
        {
            case StagePhase.Unloaded:
                return true;
            case StagePhase.Setup:
                StagePhaseState setupState = CurrentStage.Setup();
                if (setupState == StagePhaseState.Done)
                {
                    CurrentStage.stagePhase = StagePhase.Start;
                }
                break;
            case StagePhase.Start:
                StagePhaseState startState = CurrentStage.StartStage();
                if (startState == StagePhaseState.Done)
                {
                    CurrentStage.stagePhase = StagePhase.Playing;
                }
                break;
            case StagePhase.Playing:
                StagePhaseState playState = CurrentStage.Play();
                if (playState == StagePhaseState.Done)
                {
                    CurrentStage.stagePhase = StagePhase.End;
                }
                break;
            case StagePhase.End:
                (StagePhaseState endState, StageOutcome stageOutcome) = CurrentStage.End();
                if (endState == StagePhaseState.Done)
                {
                    CurrentStage.stagePhase = StagePhase.Shutdown;
                }
                break;
            case StagePhase.Shutdown:
                StagePhaseState shutdownState = CurrentStage.Shutdown();
                if (shutdownState == StagePhaseState.Done)
                {
                    CurrentStage.stagePhase = StagePhase.Unloaded;
                }
                break;
            default:
                break;
        }

        return false;
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
    Win,
    Lose
}