
using UnityEngine;

public class VaseStage : Stage
{
    [SerializeField] private Transform thiefTransform;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private App.Actor.Player.MoveCtrl playerMoveCtrl;
    [SerializeField] private Transform playerStartPos;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public override StagePhaseState Setup()
    {
        thiefTransform.gameObject.SetActive(true);
        gameTimer.SetTimer(this.timeLimit);
        gameTimer.PauseTimer();
        this.StageOutcome = StageOutcome.Undefined;
        guard.Reset();
        thiefTransform.position = playerStartPos.position;

        playerMoveCtrl.enabled = true;
        this.gameObject.SetActive(true);
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        gameTimer.StartTimer();
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        if (gameTimer.TimerDone())
        {
             StageOutcome = StageOutcome.Lose;
        } else if (thiefTransform.position.x >= 9)
        {
            StageOutcome = StageOutcome.Win;
            return StagePhaseState.Done;
        }

        // if a vase breaks, it will change the outcome to lose - see VaseCtrl.cs
        return (StageOutcome == StageOutcome.Lose) ? StagePhaseState.Done : StagePhaseState.Active;
    }

    public override StagePhaseState End()
    {
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
        thiefTransform.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        playerMoveCtrl.enabled = false;
        guard.Reset();
        gameTimer.PauseTimer();
        return StagePhaseState.Done;
    }
}
