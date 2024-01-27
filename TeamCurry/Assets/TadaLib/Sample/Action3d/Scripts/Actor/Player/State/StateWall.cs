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
    /// 壁キック前の壁滑り状態
    /// </summary>
    [System.Serializable]
    public class StateWall : StateMachine.StateBase
    {
        #region static関数
        public static bool TryChangeState(GameObject obj)
        {
            // 速度が上向きの場合は遷移できない
            if (obj.GetComponent<MoveCtrl>().Velocity.y > 0.0f)
            {
                return false;
            }

            // 壁に張り付いていないといけない
            var rigidbody = obj.GetComponent<TadaRigidbody2D>();
            var axisX = InputUtil.GetAxis(obj, AxisCode.Horizontal);

            // 左壁チェック
            if (rigidbody.IsLeftCollide && axisX < 0.0f)
            {
                ChangeState(obj, isRightWall: false);
                return true;
            }

            // 右壁チェック
            if (rigidbody.IsRightCollide && axisX > 0.0f)
            {
                ChangeState(obj, isRightWall: true);
                return true;
            }

            return false;
        }

        public static void ChangeState(GameObject obj, bool isRightWall)
        {
            var state = obj.GetComponent<StateMachine>().GetStateInstance<StateWall>();
            state._isRightWall = isRightWall;
            state.ChangeState(typeof(StateWall));
        }
        #endregion

        #region プロパティ
        #endregion

        #region StateMachine.StateBaseの実装
        // ステートが始まった時に呼ばれるメソッド
        public override void OnStart()
        {
            //ActorUtil.PlayAnim("Wall");

            SetStateFlag(StateInfoKind.IsWall);

            // 移動速度を遅くする
            obj.GetComponent<MoveCtrl>().MaxVelocityRateYState = _speedRateY;

            UpdateDecalGravityRate();
        }

        // ステートが終了したときに呼ばれるメソッド
        public override void OnEnd()
        {
            var animator = obj.GetComponent<Animator>();
            //animator.SetBool("Wall", false);

            obj.GetComponent<MoveCtrl>().MaxVelocityRateYState = 1.0f;
        }

        // 毎フレーム呼ばれる関数
        public override void OnUpdate()
        {
            var rigidbody = obj.GetComponent<TadaRigidbody2D>();

            // 壁キック やさしさのため、壁キックを最優先に判定する
            if (StateJump.TryChangeStateByWallKick(obj, (_isRightWall) ? StateJump.WallKickKind.ToLeft : StateJump.WallKickKind.ToRight))
            {
                return;
            }

            // 接地したら終了
            if (rigidbody.IsGround)
            {
                StateIdle.ChangeState(obj);
                return;
            }


            // 壁から離れたら終わり
            if (!rigidbody.IsLeftCollide && !rigidbody.IsRightCollide)
            {
                StateFall.ChangeState(obj);
                return;
            }

            UpdateDecalGravityRate();
        }
        #endregion

        #region privateメソッド
        void UpdateDecalGravityRate()
        {
            // 上限速度を超えていたら減速重力を大きくする
            var moveCtrl = obj.GetComponent<MoveCtrl>();
            moveCtrl.DecalGravityRateState = 1.0f;
            if (moveCtrl.Velocity.y < -moveCtrl.MaxVelocity.y)
            {
                moveCtrl.DecalGravityRateState = 2.0f;
            }
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        float _speedRateY = 0.5f;
        bool _isRightWall;
        #endregion
    }
}