using System;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    public StagePhase StagePhase = StagePhase.Unloaded;
    public StageOutcome StageOutcome = StageOutcome.Undefined;
    [SerializeField] public GuardAnimation guard;

    [SerializeField] protected float timeLimit;

    public abstract StagePhaseState Setup();

    public abstract StagePhaseState StartStage();

    public abstract StagePhaseState Play();

    public abstract StagePhaseState End();

    public abstract StagePhaseState Shutdown();
}