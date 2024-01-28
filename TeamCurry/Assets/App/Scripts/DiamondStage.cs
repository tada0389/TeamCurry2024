using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondStage : Stage
{
    [SerializeField] private Transform thiefTransform;
    [SerializeField] private GameTimer timer;

    public override StagePhaseState Setup()
    {
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        if (thiefTransform.position.x == 0)
        {
            StageOutcome = StageOutcome.Win;
            return StagePhaseState.Done;
        }

        if (timer.TimerDone())
        {
            StageOutcome = StageOutcome.Lose;
        }
        
        return timer.TimerDone() ? StagePhaseState.Done : StagePhaseState.Active;
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