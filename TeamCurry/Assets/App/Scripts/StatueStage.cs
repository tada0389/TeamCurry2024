using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueStage : Stage
{
    public List<Sprite> sprites;
    [SerializeField] private int correctSpriteIndex;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private int currentSpriteIndex = 0;

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
            if (currentSpriteIndex <= 0)
            {
                currentSpriteIndex = sprites.Count + 1;
            }
        }

        return StagePhaseState.Active;
    }

    public override (StagePhaseState, StageOutcome) End()
    {
        StageOutcome stageOutcome = playerSpriteRenderer.sprite == sprites[correctSpriteIndex] ? StageOutcome.Win : StageOutcome.Lose;
        return (StagePhaseState.Done, stageOutcome);
    }

    public override StagePhaseState Shutdown()
    {
        return StagePhaseState.Done;
    }
}
