using Cysharp.Threading.Tasks;
using DG.Tweening;
using InputSystem;
using System.Collections;
using System.Collections.Generic;
using Ui;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class StatueStage : Stage
{
    public List<Sprite> sprites;
    [SerializeField] private int correctSpriteIndex;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private SwitchPoseAnimation switchPoseAnimation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerStartPosition;
    [SerializeField] private Animator playerAnimator;

    private Sprite originalPlayerSprite;
    private int currentSpriteIndex = -1;
    private float stickXPrev = 0.0f;

    private void Awake()
    {
        originalPlayerSprite = playerSpriteRenderer.sprite;
        this.gameObject.SetActive(false);
        KanKikuchi.AudioManager.SEManager.Instance.Play(KanKikuchi.AudioManager.SEPath.SYSTEM20);
    }

    public override StagePhaseState Setup()
    {
        playerAnimator.enabled = false;
        player.position = playerStartPosition.position;
        this.gameObject.SetActive(true);
        gameTimer.SetTimer(this.timeLimit);
        gameTimer.PauseTimer();
        this.StageOutcome = StageOutcome.Undefined;
        guard.Reset();
        currentSpriteIndex = -1;
        playerSpriteRenderer.sprite = originalPlayerSprite;
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
            return StagePhaseState.Done;
        }
        else
        {

            var isLeftDown = false;
            var isRightDown = false;

            var stickX = InputSystem.JoyconInput.Instance.GetAxis(AxisCode.Horizontal);
            switch (stickX)
            {
                case < 0:
                    if (stickXPrev >= 0.0f)
                    {
                        isLeftDown = true;
                    }
                    break;
                case > 0:
                    if (stickXPrev >= 0.0f)
                    {
                        isRightDown = true;
                    }
                    break;
            }

            stickXPrev = stickX;
            if (isLeftDown || Input.GetKeyDown(KeyCode.A))
            {
                currentSpriteIndex++;
                if (currentSpriteIndex >= sprites.Count)
                {
                    currentSpriteIndex = 0;
                }

                SwitchSprite();
            }
            else if (isRightDown || Input.GetKeyDown(KeyCode.D))
            {
                currentSpriteIndex--;
                if (currentSpriteIndex < 0)
                {
                    currentSpriteIndex = sprites.Count - 1;
                }

                SwitchSprite();
            }

            return StagePhaseState.Active;
        }
    }

    public override StagePhaseState End()
    {
        switch (guard.AnimState)
        {
            case AnimState.Inactive:
                this.StageOutcome = playerSpriteRenderer.sprite == sprites[correctSpriteIndex] ? StageOutcome.Win : StageOutcome.Lose;
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
        this.gameObject.SetActive(false);
        playerSpriteRenderer.sprite = originalPlayerSprite;
        gameTimer.PauseTimer();
        player.gameObject.SetActive(false);
        return StagePhaseState.Done;
    }

    private void SwitchSprite()
    {
        playerSpriteRenderer.sprite = sprites[currentSpriteIndex];
        switchPoseAnimation.Do();
    }
}
