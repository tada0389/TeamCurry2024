using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Transform guard;
    [SerializeField] private Transform guardStart;
    [SerializeField] private Transform guardEnd;
    [SerializeField] private bool isTimerPaused = false;
    [SerializeField] private float maxTime = 30f;
    [SerializeField] private float time;
    [SerializeField] private TimerDistanceUIVisibility timerDistanceUIVisibility;
    private Vector3 guardPosCreep;
    private float prevTime;
    private float initialTime;
    
    void Awake()
    {
        isTimerPaused = true;
        time = maxTime;
        initialTime = maxTime;
        guardPosCreep = Vector3.zero;
    }
    
    void Update()
    {
        if (!TimerDone() && !isTimerPaused)
        {
            time -= Time.deltaTime;
            float timeElapsedRatio = (maxTime - time) / maxTime;
            float totalDistance = Vector3.Distance(guardStart.position, guardEnd.position);
            Vector3 guardDirection = guardEnd.position - guardStart.position;
            guardDirection.Normalize();
            guardDirection *= (timeElapsedRatio * totalDistance);
            guard.position = guardDirection + guardStart.position;
        }
    }

    public float RateTime01 => (initialTime - time) / initialTime;
    public bool TimerDone()
    {
        return time <= 0;
    }
    
    public void SetTimer(float timeLimit)
    {
        maxTime = timeLimit;
        time = timeLimit;
        guard.position = guardStart.position;
        timerDistanceUIVisibility.Reset();
    }

    public void PauseTimer()
    {
        isTimerPaused = true;
    }

    public void StartTimer()
    {
        isTimerPaused = false;       
    }
}