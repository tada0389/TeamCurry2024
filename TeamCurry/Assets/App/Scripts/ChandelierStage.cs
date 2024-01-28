using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class ChandelierStage : Stage
{
    private bool playerAttached;
    private StageOutcome outcome;
    private Tweener chandelierTweener;
    private Quaternion previousRotation;
    [SerializeField] private Transform chandelierOffsetTransform;
    [SerializeField] private Vector3 startRotation;
    [SerializeField] private Vector3 endRotation;
    [SerializeField] private GameTimer timer;
    [SerializeField] private float swingDuration;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public override StagePhaseState Setup()
    {
        this.gameObject.SetActive(true);
        chandelierOffsetTransform.rotation = new Quaternion(0, 0, 0, 0);
        previousRotation = chandelierOffsetTransform.rotation;
        chandelierOffsetTransform.rotation = IncreaseRotation();
        playerAttached = false;
        outcome = StageOutcome.Lose;
        timer.SetTimer(timeLimit);
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        // chandelierTweener = chandelierOffsetTransform.DORotate(endRotation, swingDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            outcome = StageOutcome.Win;
        }

        if (chandelierOffsetTransform.rotation.z > previousRotation.z)
        {
            if (chandelierOffsetTransform.rotation.z == 60f)
            {
                chandelierOffsetTransform.rotation = DecreaseRotation();
            }
            else
            {
                chandelierOffsetTransform.rotation = IncreaseRotation();
            }
        }
        else
        {
            if (chandelierOffsetTransform.rotation.z == -60f)
            {
                chandelierOffsetTransform.rotation = IncreaseRotation();
            }
            else
            {
                chandelierOffsetTransform.rotation = DecreaseRotation();
            }
        }

        return timer.TimerDone() ? StagePhaseState.Done : StagePhaseState.Active;
    }

    public override StagePhaseState End()
    {
        chandelierTweener.Kill();
        timer.PauseTimer();
        return StagePhaseState.Done;
    }

    public override StagePhaseState Shutdown()
    {
        this.gameObject.SetActive(false);
        return StagePhaseState.Done;
    }

    private Quaternion IncreaseRotation()
    {
        return new Quaternion(previousRotation.x, previousRotation.y, previousRotation.z++, previousRotation.w);
    }
    private Quaternion DecreaseRotation()
    {
        return new Quaternion(previousRotation.x, previousRotation.y, previousRotation.z--, previousRotation.w);
    }
}
