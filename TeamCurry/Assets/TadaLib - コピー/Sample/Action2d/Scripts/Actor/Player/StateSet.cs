using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Actor.Player
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
        [SerializeField]
        State.StateJump _stateJump;
        [SerializeField]
        State.StateFall _stateFall;
        [SerializeField]
        State.StateWall _stateWall;
        [SerializeField]
        State.StateCrouch _stateCrouch;
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
            stateMachine.AddState(_stateJump);
            stateMachine.AddState(_stateFall);
            stateMachine.AddState(_stateWall);
            stateMachine.AddState(_stateCrouch);

            stateMachine.SetInitialState(_stateIdle.GetType());
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        #endregion
    }
}