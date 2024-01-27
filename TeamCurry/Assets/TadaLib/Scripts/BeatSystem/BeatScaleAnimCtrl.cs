using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using DG.Tweening;

namespace TadaLib.BeatSystem
{
    /// <summary>
    /// BeatScaleAnimCtrl
    /// </summary>
    [RequireComponent(typeof(ObserverCtrl))]
    public class BeatScaleAnimCtrl
        : MonoBehaviour
        , IObserver
        , IScaleChanger
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region IObserver の実装
        public void OnBeat(in TimingInfo info)
        {
            DOTween.To(
                () => _scaleRate,
                x => _viewScale = x,
                1.0f,
                0.2f);
        }
        #endregion

        #region IScaleChanger の実装
        public Vector3 ScaleRate => Vector3.one;
        public Vector3 ViewScaleRate => Vector3.one * _viewScale;
        #endregion

        #region privateフィールド
        [SerializeField]
        float _scaleRate = 1.5f;
        float _viewScale = 1.0f;
        #endregion

        #region privateメソッド
        #endregion
    }
}