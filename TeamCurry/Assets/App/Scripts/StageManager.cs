using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static bool IsEnglish = true;

    [SerializeField] private List<Stage> stages;
    private int currentStageIndex = -1;
    [SerializeField] private Title.TitleStaging titleStaging;
    [SerializeField] private TimerDistanceUIVisibility timerDistanceUi;

    public Stage CurrentStage => stages[currentStageIndex];

    public bool LoadNextStage()
    {
        titleStaging.IncrementSpriteCtr();
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
                    titleStaging.OpenCurtains();
                    CurrentStage.StagePhase = StagePhase.Start;
                }
                break;
            case StagePhase.Start:
                if (titleStaging.CurtainState == Title.CurtainState.Open)
                {
                    // 楽をするために強引にやる
                    if (CurrentStage.name != "DiamondStage")
                    {
                        timerDistanceUi.FadeUi(1.0f, 0.3f);
                    }

                    StagePhaseState startState = CurrentStage.StartStage();
                    if (startState == StagePhaseState.Done)
                    {
                        CurrentStage.StagePhase = StagePhase.Playing;
                    }
                }
                break;
            case StagePhase.Playing:
                StagePhaseState playState = CurrentStage.Play();
                if (playState == StagePhaseState.Done)
                {
                    switch (this.CurrentStage.StageOutcome)
                    {
                        case StageOutcome.Lose:
                            SEManager.Instance.Play(SEPath.SPOTTED_SFX);
                            break;
                    }
                    timerDistanceUi.FadeUi(0.0f, 0.3f);
                    CurrentStage.StagePhase = StagePhase.End;
                }
                break;
            case StagePhase.End:
                StagePhaseState endState = CurrentStage.End();
                if (endState == StagePhaseState.Done)
                {
                    titleStaging.CloseCurtains();
                    CurrentStage.StagePhase = StagePhase.Shutdown;

                    switch (this.CurrentStage.StageOutcome)
                    {
                        case StageOutcome.Win:
                            SEManager.Instance.Play(SEPath.GAME_CLEAR_CHEER);
                            break;
                        case StageOutcome.Lose:
                            SEManager.Instance.Play(SEPath.GAME_OVERSCREEN_SFX);
                            break;
                    }
                }
                break;
            case StagePhase.Shutdown:
                if (titleStaging.CurtainState == Title.CurtainState.Closed)
                {
                    StagePhaseState shutdownState = CurrentStage.Shutdown();
                    if (shutdownState == StagePhaseState.Done)
                    {
                        CurrentStage.StagePhase = StagePhase.Unloaded;
                    }
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
