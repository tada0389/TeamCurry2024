using Actor.Gimmick;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class ChandelierStage : Stage
{
    [SerializeField] private GameTimer timer;
    [SerializeField] private GameObject stage;
    [SerializeField] private ChandelierCtrl chandelier;

    public override StagePhaseState Setup()
    {
        timer.SetTimer(timeLimit);
        stage.gameObject.SetActive(true);
        StageOutcome = StageOutcome.Undefined;
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        if (timer.TimerDone())
        {
            StageOutcome = StageOutcome.Win;
            return StagePhaseState.Done;
        }
        else
        {
            if (timer.RateTime01 > 0.5f)
            {
                if (Mathf.Abs(chandelier.AngleDeg) > 30.0f)
                {
                    StageOutcome = StageOutcome.Lose;
                    return StagePhaseState.Done;
                }
            }

            return StagePhaseState.Active;
        }
    }

    public override StagePhaseState End()
    {
        timer.PauseTimer();
        return StagePhaseState.Done;
    }

    public override StagePhaseState Shutdown()
    {
        return StagePhaseState.Done;
    }
}
