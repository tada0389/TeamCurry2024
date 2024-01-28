using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Ui;
using UnityEngine;

public class StatueStage : Stage
{
    public List<Sprite> sprites;
    [SerializeField] private int correctSpriteIndex;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private SwitchPoseAnimation switchPoseAnimation;

    private int currentSpriteIndex = 0;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        KanKikuchi.AudioManager.SEManager.Instance.Play(KanKikuchi.AudioManager.SEPath.SYSTEM20);
    }

    public override StagePhaseState Setup()
    {
        this.gameObject.SetActive(true);
        gameTimer.SetTimer(this.timeLimit);
        this.StageOutcome = StageOutcome.Undefined;
        guard.Reset();
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        Debug.Log("START");
        gameTimer.StartTimer();
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        Debug.Log("PLAY");
        if (gameTimer.TimerDone())
        {
            return StagePhaseState.Done;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentSpriteIndex++;
                if (currentSpriteIndex >= sprites.Count)
                {
                    currentSpriteIndex = 0;
                }

                SwitchSprite();
            }
            else if (Input.GetKeyDown(KeyCode.A))
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
        this.gameObject.SetActive(false);
        return StagePhaseState.Done;
    }

    private void SwitchSprite()
    {
        playerSpriteRenderer.sprite = sprites[currentSpriteIndex];
        switchPoseAnimation.Do();
    }
}
