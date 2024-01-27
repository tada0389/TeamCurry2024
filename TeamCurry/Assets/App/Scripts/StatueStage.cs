using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueStage : Stage
{
    public List<Sprite> sprites;
    [SerializeField] private int correctSpriteIndex;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private GameTimer gameTimer;

    private int currentSpriteIndex = 0;

    public override StagePhaseState Setup()
    {
        gameTimer.SetTimer(this.timeLimit);
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

                playerSpriteRenderer.sprite = sprites[currentSpriteIndex];
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                currentSpriteIndex--;
                if (currentSpriteIndex < 0)
                {
                    currentSpriteIndex = sprites.Count - 1;
                }

                playerSpriteRenderer.sprite = sprites[currentSpriteIndex];
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
        return StagePhaseState.Done;
    }
}
