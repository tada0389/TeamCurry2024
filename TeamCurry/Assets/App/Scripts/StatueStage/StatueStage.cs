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
    }

    public override StagePhaseState Setup()
    {
        player.gameObject.SetActive(true);
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
            var stickX = JoyconInput.Instance.GetAxis(AxisCode.Horizontal);
            if ((stickX > 0.0f && stickXPrev <= 0.0f) || Input.GetKeyDown(KeyCode.A))
            {
                currentSpriteIndex++;
                if (currentSpriteIndex >= sprites.Count)
                {
                    currentSpriteIndex = 0;
                }

                SwitchSprite();
            }
            else if ((stickX < 0.0f && stickXPrev >= 0.0f) || Input.GetKeyDown(KeyCode.D))
            {
                currentSpriteIndex--;
                if (currentSpriteIndex < 0)
                {
                    currentSpriteIndex = sprites.Count - 1;
                }

                SwitchSprite();
            }
            stickXPrev = stickX;

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
