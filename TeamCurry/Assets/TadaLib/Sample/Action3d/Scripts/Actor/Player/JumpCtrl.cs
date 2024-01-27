using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action3d.Actor.Player
{
    /// <summary>
    /// Jump呼び出し制御
    /// </summary>
    public class JumpCtrl : MonoBehaviour
    {
        #region プロパティ
        public bool IsEnableState { get; set; } = false;
        #endregion

        #region メソッド
        #endregion

        #region Monobehavior の実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        public void Start()
        {
            // ステート開始時に初期化させる
            var stateMachine = GetComponent<StateMachine>();
            stateMachine.AddStateStartCallback(() => 
            {
                IsEnableState = false;
            });
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        [SerializeField]
        float _precedeTimeSec = 0.2f;
        #endregion
    }
}