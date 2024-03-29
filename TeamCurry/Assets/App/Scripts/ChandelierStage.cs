using Actor.Gimmick;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class ChandelierStage : Stage
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private GameObject stage;
    [SerializeField] private ChandelierCtrl chandelier;
    [SerializeField] private Animator playerAnimator;

    private void Awake()
    {
        stage.gameObject.SetActive(false);
    }

    public override StagePhaseState Setup()
    {
        playerAnimator.enabled = false;
        stage.gameObject.SetActive(true);
        gameTimer.SetTimer(this.timeLimit);
        gameTimer.PauseTimer();
        this.StageOutcome = StageOutcome.Undefined;
        guard.Reset();

        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        Debug.Log("playing");
        gameTimer.StartTimer();
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        if (gameTimer.TimerDone())
        {
            StageOutcome = StageOutcome.Win;
            return StagePhaseState.Done;
        }
        else
        {
            if (gameTimer.RateTime01 > 0.90f)
            {
                if (Mathf.Abs(chandelier.AngleDeg) > 40.0f || Mathf.Abs(chandelier.AngularVelocity) > 0.9f)
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
        gameTimer.PauseTimer();

        switch (guard.AnimState)
        {
            case AnimState.Inactive:
                if (StageOutcome == StageOutcome.Win)
                {
                    guard.PlayerWins();
                }
                else
                {
                    guard.PlayerLoses();
                }
                break;
            case AnimState.Active:
                break;
            case AnimState.Done:
                return StagePhaseState.Done;
            default:
                break;
        }

        return StagePhaseState.Active;
    }

    public override StagePhaseState Shutdown()
    {
        guard.Reset();
        stage.gameObject.SetActive(false);
        gameTimer.PauseTimer();

        return StagePhaseState.Done;
    }
}
