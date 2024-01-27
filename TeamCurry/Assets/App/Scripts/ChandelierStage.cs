using UnityEngine;

public class ChandelierStage : Stage
{
    private Vector3 chandelierMovement;
    private bool playerAttached;
    private StageOutcome outcome;
    
    [SerializeField] private GameTimer timer;

    public override StagePhaseState Setup()
    {
        chandelierMovement = Vector3.zero;
        playerAttached = false;
        outcome = StageOutcome.Lose;
        timer.SetTimer(timeLimit);
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            outcome = StageOutcome.Win;
        }

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
