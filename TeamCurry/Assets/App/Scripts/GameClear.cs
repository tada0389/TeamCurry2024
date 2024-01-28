using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : Stage
{
    [SerializeField] private float time = 8f;

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
        // time -= Time.deltaTime;
        // return time >= 0 ? StagePhaseState.Active : StagePhaseState.Done;
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
