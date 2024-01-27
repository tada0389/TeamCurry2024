using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStage : Stage
{
    public void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public override StagePhaseState Setup()
    {
        this.gameObject.SetActive(true);
        Debug.Log("Stage setup");
        return StagePhaseState.Done;
    }

    public override StagePhaseState StartStage()
    {
        Debug.Log("Stage start");
        return StagePhaseState.Done;
    }

    public override StagePhaseState Play()
    {
        Debug.Log("Stage play");
        if (Input.GetKeyDown(KeyCode.W))
        {
            return StagePhaseState.Done;
        }

        return StagePhaseState.Active;
    }

    public override StagePhaseState End()
    {
        Debug.Log("Stage end");
        return StagePhaseState.Done;
    }

    public override StagePhaseState Shutdown()
    {
        this.gameObject.SetActive(false);
        Debug.Log("Stage shutdown");
        return StagePhaseState.Done;
    }
}
