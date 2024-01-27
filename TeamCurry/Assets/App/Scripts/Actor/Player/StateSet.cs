using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace App.Actor.Player
{
    /// <summary>
    /// Component処理
    /// </summary>
    public class StateSet : MonoBehaviour
    {
        #region プロパティ
        [SerializeField]
        State.StateIdle _stateIdle;
        [SerializeField]
        State.StateRun _stateRun;
        #endregion

        #region メソッド
        #endregion

        #region MonoBehaviour の実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        void Start()
        {
            // StateMachineに登録する
            var stateMachine = GetComponent<StateMachine>();
            stateMachine.AddState(_stateIdle);
            stateMachine.AddState(_stateRun);

            stateMachine.SetInitialState(_stateIdle.GetType());
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        #endregion
    }
}