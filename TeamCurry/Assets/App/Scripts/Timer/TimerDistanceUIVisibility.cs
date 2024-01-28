using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDistanceUIVisibility : MonoBehaviour
{
    [SerializeField] private Transform dist1MTransform;
    [SerializeField] private Transform dist2MTransform;
    [SerializeField] private Transform dist3MTransform;
    [SerializeField] private Transform guardTransform;
    [SerializeField] private Renderer dist1MRenderer;
    [SerializeField] private Renderer dist2MRenderer;
    [SerializeField] private Renderer dist3MRenderer;

    // Start is called before the first frame update
    void Start()
    {
        dist1MRenderer.enabled = false;
        dist2MRenderer.enabled = false;
        dist3MRenderer.enabled = false;
    }

    public void Reset()
    {
        dist1MRenderer.enabled = false;
        dist2MRenderer.enabled = false;
        dist3MRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (guardTransform.position.x <= dist1MTransform.position.x)
        {
            dist1MRenderer.enabled = true;
        }
        if (guardTransform.position.x <= dist2MTransform.position.x)
        {
            dist2MRenderer.enabled = true;
        }
        if (guardTransform.position.x <= dist3MTransform.position.x)
        {
            dist3MRenderer.enabled = true;
        }
    }
}
