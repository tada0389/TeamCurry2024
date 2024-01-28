using System;
using DG.Tweening;
using UnityEngine;

public enum AnimState
{
    Inactive,
    Active,
    Done
}

public class GuardAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer flashlightSpriteRenderer;
    [SerializeField] private float winAnimDuration = 2f;
    [SerializeField] private float loseAnimDuration = 2f;
    public AnimState AnimState = AnimState.Inactive;

    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private Transform player;

    public void Reset()
    {
        flashlightSpriteRenderer.enabled = false;
        AnimState = AnimState.Inactive;
        transform.position = start.position;
    }

    public void PlayerLoses()
    {
        flashlightSpriteRenderer.enabled = true;
        transform.DOMoveX(player.position.x - 3, winAnimDuration).OnComplete(() => { AnimState = AnimState.Done; });
    }

    public void PlayerWins()
    {
        flashlightSpriteRenderer.enabled = true;
        transform.DOMoveX(end.position.x, winAnimDuration).OnComplete(() => { AnimState = AnimState.Done; });
    }
}
