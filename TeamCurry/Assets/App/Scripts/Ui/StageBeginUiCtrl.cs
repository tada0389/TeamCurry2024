﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KanKikuchi.AudioManager;

namespace Ui
{
    /// <summary>
    /// StageBeginUiCtrl
    /// </summary>
    public class StageBeginUiCtrl
        : MonoBehaviour
    {
        #region プロパティ
        #endregion

        #region メソッド
        public async UniTask AppearText(Sprite sprite, Sprite? controlSprite)
        {
            SEManager.Instance.Play(SEPath.MENU_SELECTION);

            _appearImageControl.enabled = controlSprite != null;

            // initialize
            _appearImage.rectTransform.localScale = Vector3.zero;
            _appearImageControl.rectTransform.localScale = Vector3.zero;
            var color = _appearImage.color;
            color.a = 1.0f;
            _appearImage.color = color;
            _appearImageControl.color = color;

            _appearImage.sprite = sprite;
            _appearImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sprite.texture.width);
            _appearImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sprite.texture.height);

            if(controlSprite != null)
            {
                _appearImageControl.sprite = controlSprite;
                _appearImageControl.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, controlSprite.texture.width);
                _appearImageControl.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, controlSprite.texture.height);
            }

            _appearImage.rectTransform.DOScale(1.0f, _appearDuratinoSec).SetEase(Ease.InCirc);
            _appearImageControl.rectTransform.DOScale(1.0f, _appearDuratinoSec).SetEase(Ease.InCirc);

            await UniTask.WaitForSeconds(_appearDuratinoSec + _appearAfterWaitDurationSec);

            _appearImage.DOFade(0.0f, _fadeoutDurationSec);
            _appearImageControl.DOFade(0.0f, _fadeoutDurationSec);

            await UniTask.WaitForSeconds(_fadeoutDurationSec);
        }
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            _appearImage.rectTransform.localScale = Vector3.zero;
            _appearImageControl.rectTransform.localScale = Vector3.zero;
            var color = _appearImage.color;
            color.a = 1.0f;
            _appearImage.color = color;
            _appearImageControl.color = color;
        }
        #endregion


        #region privateフィールド
        [SerializeField]
        UnityEngine.UI.Image _appearImage;
        [SerializeField]
        UnityEngine.UI.Image _appearImageControl;
        [SerializeField]
        float _appearDuratinoSec = 0.5f;
        [SerializeField]
        float _appearAfterWaitDurationSec = 0.5f;
        [SerializeField]
        float _fadeoutDurationSec = 0.2f;
        #endregion

        #region privateメソッド
        #endregion
    }
}