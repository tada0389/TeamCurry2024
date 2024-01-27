using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;
using DG.Tweening;

namespace TadaLib.BeatSystem
{
    /// <summary>
    /// BeatScaleAnimCtrl
    /// </summary>
    [RequireComponent(typeof(ObserverCtrl))]
    public class UiBeatScaleAnimCtrl
        : MonoBehaviour
        , IObserver
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region IObserver の実装
        public void OnBeat(in TimingInfo info)
        {
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one * _scaleRate;
            rectTransform.DOScale(1.0f, 0.2f);
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        float _scaleRate = 1.5f;
        #endregion

        #region privateメソッド
        #endregion
    }
}