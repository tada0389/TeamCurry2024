using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Actor.Player.State
{
    /// <summary>
    /// プレイヤーの待機State処理
    /// </summary>
    [System.Serializable]
    public class StateIdle : StateMachine.StateBase
    {
        #region static関数
        public static void ChangeState(GameObject obj)
        {
            var state = obj.GetComponent<StateMachine>().GetStateInstance<StateIdle>();
            state.ChangeState(typeof(StateIdle));
        }
        #endregion

        #region プロパティ
        #endregion

        #region StateMachine.StateBaseの実装
        // ステートが始まった時に呼ばれるメソッド
        public override void OnStart()
        {
            //obj.GetComponent<Animator>().SetBool("IsGround", true);
        }

        // ステートが終了したときに呼ばれるメソッド
        public override void OnEnd()
        {
        }

        // 毎フレーム呼ばれる関数
        public override void OnUpdate()
        {
            if (StateCrouch.TryChangeState(obj))
            {
                return;
            }

            if (StateJump.TryChangeState(obj, StateJump.JumpPowerKind.Medium))
            {
                return;
            }

            if (!obj.GetComponent<TadaRigidbody2D>().IsGround)
            {
                StateFall.ChangeState(obj);
                return;
            }

            // 入力が入ったらRunへ
            var axisX = InputUtil.GetAxis(obj, AxisCode.Horizontal);
            if (Mathf.Abs(axisX) >= 1e-4)
            {
                StateRun.ChangeState(obj);
                return;
            }
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        #endregion
    }
}