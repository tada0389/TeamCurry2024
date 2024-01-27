using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;
using TadaLib.Extension;

namespace TadaLib.Sample.Action2d.Actor.Player.State
{

    /// <summary>
    /// State処理
    /// </summary>
    [System.Serializable]
    public class StateJump : StateMachine.StateBase
    {
        #region 定義
        public enum JumpPowerKind
        {
            Low = 0,
            Medium,
            High,
            Spring,

            TERM,
        }

        public enum WallKickKind
        {
            ToLeft,
            ToRight,

            TERM,
        }
        #endregion

        #region static関数
        public static bool TryChangeState(GameObject obj, JumpPowerKind jumpPowerKind)
        {
            // 地上にいないと使えない
            if (!obj.GetComponent<TadaRigidbody2D>().IsGround)
            {
                // 地上から離れてすぐならジャンプできる
                if (obj.GetComponent<DataHolder>().NoGroundDurationSec > 0.15f)
                {
                    return false;
                }
            }

            if (!InputUtil.IsButtonDown(obj, ButtonCode.Jump, 0.1f))
            {
                return false;
            }

            // 過去のジャンプ入力をすべてONにしてIsButtonDownをfalseにする
            // 連続してジャンプするのを防ぐため
            InputUtil.ForceFlagOnHistory(obj, ButtonCode.Jump);

            ChangeState(obj, jumpPowerKind);
            return true;
        }

        public static bool TryChangeStateByWallKick(GameObject obj, WallKickKind wallKickKind = WallKickKind.TERM)
        {
            // 壁が近くなくちゃ使えない
            // ただし、方向が決まっていないときのみ
            if (wallKickKind == WallKickKind.TERM)
            {
                var rigidbody = obj.GetComponent<TadaRigidbody2D>();
                if (rigidbody.IsLeftCollide)
                {
                    wallKickKind = WallKickKind.ToRight;
                }
                else if (rigidbody.IsRightCollide)
                {
                    wallKickKind = WallKickKind.ToLeft;
                }
                else if (rigidbody.IsLeftNearCollide)
                {
                    wallKickKind = WallKickKind.ToRight;
                }
                else if (rigidbody.IsRightNearCollide)
                {
                    wallKickKind = WallKickKind.ToLeft;
                }
            }

            // 壁がない
            if (wallKickKind == WallKickKind.TERM)
            {
                return false;
            }

            if (!InputUtil.IsButtonDown(obj, ButtonCode.Jump, 0.1f))
            {
                return false;
            }

            // 過去のジャンプ入力をすべてONにしてIsButtonDownをfalseにする
            // 連続してジャンプするのを防ぐため
            InputUtil.ForceFlagOnHistory(obj, ButtonCode.Jump);

            // パラメータ吸出し
            var state = obj.GetComponent<StateMachine>().GetStateInstance<StateJump>();
            // 壁と逆方向にジャンプ
            var velX = state._jumpSpeedWallKickX;
            if (wallKickKind == WallKickKind.ToLeft)
            {
                velX *= -1.0f;
            }
            obj.GetComponent<MoveCtrl>().SetVelocityForceX(velX);

            state._jumpVelXInitial = velX;

            // ヒットストップを入れる
            obj.GetComponent<HitStopCtrl>().StartStop(state._wallKickStartHitStopDuration, state._wallKickStartTimeScale);

            ChangeState(obj, state._jumpPowerWallKick, state._uncontrollableDurationSec);
            return true;
        }

        public static void ChangeState(GameObject obj, JumpPowerKind jumpPowerKind, float uncontrollableDurationSec = 0.0f)
        {
            var state = obj.GetComponent<StateMachine>().GetStateInstance<StateJump>();
            state._jumpPower = jumpPowerKind;
            state._forceWallKickMoveTimer = new TadaLib.Util.Timer(uncontrollableDurationSec);
            state.ChangeState(typeof(StateJump));
        }
        #endregion

        #region プロパティ
        #endregion

        #region StateMachine.StateBaseの実装
        // ステートが始まった時に呼ばれるメソッド
        public override void OnStart()
        {
            var animator = obj.GetComponent<Animator>();
            //animator.Play("Jump");
            //animator.SetBool("IsGround", false);
            var speedY = _jumpPowerArray[(int)_jumpPower];
            var moveCtrl = obj.GetComponent<MoveCtrl>();
            moveCtrl.SetVelocityForceY(speedY);
            moveCtrl.GravityRateState = 0.0f;
            _gravityTimer = new TadaLib.Util.Timer(_gravityIgnoreMaxSec);

            _isJumpButtonReleasedOnce = false;

            //obj.GetComponent<AudioSource>().PlayOneShot(_jumpSe, 0.45f);

            // ジャンプしたことを通知
            obj.GetComponent<DataHolder>().IsJumpStartFrame = true;

            if (_jumpEffectPrefab != null)
            {
                var rot = Quaternion.identity;
                var offset = Vector3.zero;
                //// 地面があったらその法線に方向を合わせる
                //var rigidbody = obj.GetComponent<TadaRigidbody2D>();
                //if (rigidbody.IsGround)
                //{
                //    var normal = rigidbody.GroundInfo.Normal;
                //    rot = Quaternion.FromToRotation(Vector3.up, new Vector3(normal.x, normal.y, 0.0f));

                //    // 坂道のときに自然な場所から出るように座標をずらす
                //    var centerPos = (Vector2)ActorUtil.GetCenterPosIfHasCollision(obj);
                //    var footPos = (Vector2)obj.transform.position;
                //    var theta = Vector2.Angle(footPos - centerPos , - normal);
                //    offset = centerPos - normal * ((footPos - centerPos).magnitude * Mathf.Abs(Mathf.Cos(theta))) - footPos;
                //}
                //Effect.Util.Play(_jumpEffectPrefab, obj.transform.position + offset, rot);
            }
        }

        // ステートが終了したときに呼ばれるメソッド
        public override void OnEnd()
        {
        }

        // 毎フレーム呼ばれる関数
        public override void OnUpdate()
        {
            var deltaTime = obj.DeltaTime();
            _gravityTimer.Advance(deltaTime);
            _forceWallKickMoveTimer.Advance(deltaTime);

            if (!_forceWallKickMoveTimer.IsTimout)
            {
                // 移動強制
                obj.GetComponent<MoveCtrl>().SetVelocityForceX(_jumpVelXInitial);
            }

            // 壁キック
            if (StateJump.TryChangeStateByWallKick(obj))
            {
                return;
            }

            if (StateWall.TryChangeState(obj))
            {
                return;
            }

            var speedY = obj.GetComponent<MoveCtrl>().Velocity.y;
            if (speedY < 0.0f)
            {
                StateFall.ChangeState(obj);
                return;
            }

            _isJumpButtonReleasedOnce = _isJumpButtonReleasedOnce || !InputUtil.IsButton(obj, ButtonCode.Jump);
            obj.GetComponent<MoveCtrl>().GravityRateState = CalcGravityRate();
        }
        #endregion

        #region privateメソッド
        float CalcGravityRate()
        {
            if (_isJumpButtonReleasedOnce)
            {
                return 1.0f;
            }

            var rate = _gravityTimer.TimeRate01;
            rate *= rate;
            return rate;
        }
        #endregion

        #region privateフィールド
        [SerializeField, TadaLib.Attribute.EnumList(typeof(JumpPowerKind))]
        float[] _jumpPowerArray;
        JumpPowerKind _jumpPower;
        [SerializeField]
        float _gravityIgnoreMaxSec = 1.0f;
        TadaLib.Util.Timer _gravityTimer;
        bool _isJumpButtonReleasedOnce;
        [SerializeField]
        ParticleSystem _jumpEffectPrefab;

        [SerializeField]
        AudioClip _jumpSe;

        [SerializeField]
        StateJump.JumpPowerKind _jumpPowerWallKick;
        [SerializeField]
        float _jumpSpeedWallKickX = 16.0f;
        [SerializeField]
        float _uncontrollableDurationSec = 0.3f;
        [SerializeField]
        float _wallKickStartTimeScale = 0.05f;
        [SerializeField]
        float _wallKickStartHitStopDuration = 0.1f;
        TadaLib.Util.Timer _forceWallKickMoveTimer = new TadaLib.Util.Timer(0.0f);
        float _jumpVelXInitial = 0.0f;
        #endregion
    }
}