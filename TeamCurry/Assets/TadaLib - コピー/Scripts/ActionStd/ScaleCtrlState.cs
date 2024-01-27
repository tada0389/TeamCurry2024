using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// ステートで使えるスケール変動処理
    /// </summary>
    public class ScaleCtrlState
        : MonoBehaviour
        , IScaleChanger
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            // ステート開始時にスケールリセット
            if (TryGetComponent<StateMachine>(out var stateMachine))
            {
                stateMachine.AddStateStartCallback(() =>
                {
                    ScaleRate = Vector3.one;
                    ViewScaleRate = Vector3.one;
                });
            }
        }
        #endregion

        #region IScaleChangerの実装
        public Vector3 ScaleRate { set; get; } = Vector3.one;

        public Vector3 ViewScaleRate { set; get; } = Vector3.one;
        #endregion

        #region privateフィールド
        #endregion

        #region privateメソッド
        #endregion
    }
}