
using UnityEngine;

public class VaseStage : Stage
{
    [SerializeField] private Transform thiefTransform;
    [SerializeField] private GameTimer timer;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

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
        if (thiefTransform.position.x == 21)
        {
            StageOutcome = StageOutcome.Win;
            return StagePhaseState.Done;
        }

        if (StageOutcome == StageOutcome.Lose)
        {
             return StagePhaseState.Done;           
        }

        return timer.TimerDone() ? StagePhaseState.Done : StagePhaseState.Active;
    }

    public override StagePhaseState End()
    {
        this.gameObject.SetActive(false);
        return StagePhaseState.Done;
    }

    public override StagePhaseState Shutdown()
    {
        return StagePhaseState.Done;
    }
}
