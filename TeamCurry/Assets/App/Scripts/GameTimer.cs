using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private bool isTimerPaused = false;
    [SerializeField] private float maxTime = 30f;
    [SerializeField] private float time;
    
    void Awake()
    {
        time = maxTime;
    }
    
    void Update()
    {
        if (!TimerDone() && !isTimerPaused)
        {
            time -= Time.deltaTime;    
        }
    }

    public bool TimerDone()
    {
        return time <= 0;
    }
    
    public void SetTimer(float timeLimit)
    {
        time = timeLimit;
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