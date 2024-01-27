using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwitchPoseAnimation : MonoBehaviour
{
    private Tweener spriteTween;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float targetScale;
    [SerializeField] private float animationDuration;

    private void Awake()
    {
        spriteTween = playerTransform.DOScaleY(targetScale, animationDuration)
            .SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo).SetAutoKill(false).Pause();
    }

    public void Do()
    {
        spriteTween.Restart();
        spriteTween.Play();
    }
}
