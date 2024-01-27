using UnityEngine;

public class Chandelier : Stage
{
    private Vector3 chandelierMovement = Vector3.zero;
    private bool playerAttached = false;
    
    [SerializeField] private GameTimer timer;

    public override StagePhaseState Setup()
    {
        timer.SetTimer(this.timeLimit);
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        return timer.TimerDone() ? StagePhaseState.Done : StagePhaseState.Active;
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
