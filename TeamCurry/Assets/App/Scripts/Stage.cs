using System;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    public StagePhase stagePhase = StagePhase.Unloaded;

    [SerializeField] protected float timeLimit;

    public abstract StagePhaseState Setup();

    public abstract StagePhaseState StartStage();

    public abstract StagePhaseState Play();

    public abstract StagePhaseState End();

    public abstract StagePhaseState Shutdown();
}