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
    /// State処理
    /// </summary>
    [System.Serializable]
    public class StateCrouch : StateMachine.StateBase
    {
        #region static関数
        public static bool TryChangeState(GameObject obj)
        {
            // 地面についているときだけ使える
            var rb = obj.GetComponent<TadaLib.ActionStd.TadaRigidbody2D>();
            if (!rb.IsGround)
            {
                return false;
            }

            // 下ボタンを押しているか
            var axisY = InputUtil.GetAxis(obj, AxisCode.Vertical);
            if (axisY > -0.2f)
            {
                return false;
            }

            //// ジャンプボタンも押されている
            //if (!InputUtil.IsButtonDown(obj, ButtonCode.Jump, 0.1f))
            //{
            //    return false;
            //}

            //// 過去のジャンプ入力をすべてONにしてIsButtonDownをfalseにする
            //// 即座にジャンプするのを防ぐため
            //InputUtil.ForceFlagOnHistory(obj, ButtonCode.Jump);

            // 状態遷移
            ChangeState(obj);
            return true;
        }

        public static void ChangeState(GameObject obj)
        {
            var state = obj.GetComponent<StateMachine>().GetStateInstance<StateCrouch>();
            state.ChangeState(typeof(StateCrouch));
        }
        #endregion

        #region プロパティ
        #endregion

        #region StateMachine.StateBaseの実装
        // ステートが始まった時に呼ばれるメソッド
        public override void OnStart()
        {
            // 移動速度を遅くする
            obj.GetComponent<MoveCtrl>().MaxVelocityRateXState = _maxVelocityRateX;

            // 当たり判定を消す
            //obj.GetComponent<TadaLib.ActionStd.HitSystem.OwnerCtrl>().Owner.IsEnabled = false;

            // しゃがみ開始
            obj.GetComponent<CrouchCtrl>().StartCrouchModeState();

            //obj.GetComponent<AudioSource>().PlayOneShot(_crouchSe);
        }

        // ステートが終了したときに呼ばれるメソッド
        public override void OnEnd()
        {
            //obj.GetComponent<TadaLib.ActionStd.HitSystem.OwnerCtrl>().Owner.IsEnabled = true;
        }

        // 毎フレーム呼ばれる関数
        public override void OnUpdate()
        {
            if (StateJump.TryChangeState(obj, StateJump.JumpPowerKind.Medium))
            {
                return;
            }
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        float _maxVelocityRateX = 0.25f;

        [SerializeField]
        AudioClip _crouchSe;

        TadaLib.Util.Timer _timer;
        #endregion

        #region privateメソッド
        #endregion
    }
}