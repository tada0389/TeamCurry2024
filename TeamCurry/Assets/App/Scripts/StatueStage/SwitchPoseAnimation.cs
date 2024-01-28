using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwitchPoseAnimation : MonoBehaviour
{
    private Tweener spriteTween;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float scaleFactorX;
    [SerializeField] private float scaleFactorY;
    [SerializeField] private float animationDuration;

    private void Awake()
    {
        var playerScale = playerTransform.localScale;
        spriteTween =
            playerTransform.DOScale(new Vector3(scaleFactorX * playerScale.x, scaleFactorY * playerScale.y, playerScale.z), animationDuration)
            .SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo).SetAutoKill(false).Pause();
    }

    public void Do()
    {
        spriteTween.Restart();
        spriteTween.Play();
    }
}
