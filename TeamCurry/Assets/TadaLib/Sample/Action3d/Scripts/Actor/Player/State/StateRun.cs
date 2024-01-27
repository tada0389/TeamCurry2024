using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action3d.Actor.Player.State
{
    /// <summary>
    /// State処理
    /// </summary>
    [System.Serializable]
    public class StateRun : StateMachine.StateBase
    {
        #region static関数
        public static void ChangeState(GameObject obj)
        {
            var state = obj.GetComponent<StateMachine>().GetStateInstance<StateRun>();
            state.ChangeState(typeof(StateRun));
        }
        #endregion

        #region プロパティ
        #endregion

        #region StateMachine.StateBaseの実装
        // ステートが始まった時に呼ばれるメソッド
        public override void OnStart()
        {
        }

        // ステートが終了したときに呼ばれるメソッド
        public override void OnEnd()
        {
        }

        // 毎フレーム呼ばれる関数
        public override void OnUpdate()
        {
            if (StateJump.TryChangeState(obj, StateJump.JumpPowerKind.Medium))
            {
                return;
            }

            if (!obj.GetComponent<TadaRigidbody2D>().IsGround)
            {
                StateFall.ChangeState(obj);
                return;
            }

            // 入力がなくなったらIdleへ
            var axisX = InputUtil.GetAxis(obj, AxisCode.Horizontal);
            if (Mathf.Abs(axisX) < 1e-4)
            {
                StateIdle.ChangeState(obj);
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