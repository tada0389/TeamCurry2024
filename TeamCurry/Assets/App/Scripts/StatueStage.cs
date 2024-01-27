using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueStage : Stage
{
    public List<SpriteRenderer> spriteRenderers;
    public SpriteRenderer correctSprite;
    [SerializeField] private SpriteRenderer playerSprite;

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
            if (currentSpriteIndex >= spriteRenderers.Count)
            {
                currentSpriteIndex = 0;
            }

            playerSprite = spriteRenderers[currentSpriteIndex];
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentSpriteIndex--;
            if (currentSpriteIndex <= 0)
            {
                currentSpriteIndex = spriteRenderers.Count + 1;
            }
        }

        return StagePhaseState.Active;
    }

    public override (StagePhaseState, StageOutcome) End()
    {
        StageOutcome stageOutcome = playerSprite == correctSprite ? StageOutcome.Win : StageOutcome.Lose;
        return (StagePhaseState.Done, stageOutcome);
    }

    public override StagePhaseState Shutdown()
    {
        return StagePhaseState.Done;
    }
}
