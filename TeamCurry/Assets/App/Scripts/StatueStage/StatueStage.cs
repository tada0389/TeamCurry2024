using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    }

    public override StagePhaseState Setup()
    {
        this.gameObject.SetActive(true);
        gameTimer.SetTimer(this.timeLimit);
        this.StageOutcome = StageOutcome.Undefined;
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
        this.StageOutcome = playerSpriteRenderer.sprite == sprites[correctSpriteIndex] ? StageOutcome.Win : StageOutcome.Lose;
        return StagePhaseState.Done;
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
