using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondStage : Stage
{
    [SerializeField] private Transform thiefTransform;
    [SerializeField] private GameTimer timer;
    [SerializeField] private Renderer diamondRenderer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerStart;
    [SerializeField] private App.Actor.Player.MoveCtrl playerMove;

    private void Awake()
    {
        this.gameObject.SetActive(false);    
    }

    public override StagePhaseState Setup()
    {
        guard.Reset();
        playerTransform.gameObject.SetActive(true);
        playerTransform.position = playerStart.position;
        playerMove.enabled = true;
        this.gameObject.SetActive(true);
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        if (timer.TimerDone())
        {
            SEManager.Instance.Play(SEPath.GAME_CLEAR_DIAMONDAPPEARS);
             StageOutcome = StageOutcome.Win;
            return StagePhaseState.Done;
        }

        if (thiefTransform.position.x >= -.5)
        {
            diamondRenderer.enabled = false;
            StageOutcome = StageOutcome.Win;
            SEManager.Instance.Play(SEPath.GAME_CLEAR_DIAMONDAPPEARS);
            return StagePhaseState.Done;
        }
        
        return StagePhaseState.Active;
    }

    public override StagePhaseState End()
    {
        return StagePhaseState.Done;
    }

    public override StagePhaseState Shutdown()
    {
        guard.Reset();
        playerTransform.gameObject.SetActive(false);
        playerTransform.position = playerStart.position;
        playerMove.enabled = false;
        this.gameObject.SetActive(false);
        return StagePhaseState.Done;
    }
}