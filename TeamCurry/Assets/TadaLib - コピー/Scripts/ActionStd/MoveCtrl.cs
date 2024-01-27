using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.Extension;
using TadaLib.ProcSystem;
using TadaLib.Input;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// 移動処理
    /// </summary>
    public class MoveCtrl
        : BaseProc
        , IProcMove
    {
        #region プロパティ
        public bool IsUncontrollable { get; set; } = false;
        public bool IsUncontrollableState { get; set; } = false;
        public float GravityRateState { get; set; } = 1.0f;
        public float DecalGravityRateState { get; set; } = 1.0f;

        public Vector2 Velocity { get; private set; } = Vector2.zero;

        public float SpeedRateX { get; set; } = 1.0f;
        public float SpeedRateY { get; set; } = 1.0f;
        public float MaxVelocityRateXState { get; set; } = 1.0f;
        public float MaxVelocityRateYState { get; set; } = 1.0f;
        public Vector2 MaxVelocityIgnoreRate => _maxVelocity;
        public Vector2 MaxVelocity => new Vector2(_maxVelocity.x * MaxVelocityRateXState, _maxVelocity.y * MaxVelocityRateYState);
        #endregion

        #region メソッド
        public void SetVelocityForceX(float speed)
        {
            var vel = Velocity;
            vel.x = speed;
            Velocity = vel;
        }
        public void SetVelocityForceY(float speed)
        {
            var vel = Velocity;
            vel.y = speed;
            Velocity = vel;
        }

        public void SetVelocityForce(in Vector2 velocity)
        {
            Velocity = velocity;
        }
        #endregion

        #region Monobehavior の実装
        void Start()
        {
            // ステート開始時に初期化させる
            var stateMachine = GetComponent<StateMachine>();
            stateMachine.AddStateStartCallback(() =>
            {
                GravityRateState = 1.0f;
                DecalGravityRateState = 1.0f;
                IsUncontrollableState = false;
                MaxVelocityRateXState = 1.0f;
                MaxVelocityRateYState = 1.0f;
            });

            _uncontrollableTimer.ToLimitTime();

            // 地面への着地判定を強めるために、下方向に初速を与える
            Velocity = new Vector2(0.0f, -10.0f);
        }
        #endregion


        #region TadaLib.ProcSystem.IProcMove の実装
        /// <summary>
        /// 移動更新処理
        /// </summary>
        public void OnMove()
        {
            var deltaTime = gameObject.DeltaTime();

            var rigidbody = GetComponent<TadaRigidbody2D>();
            // 接地していたらY軸負の方向速度をなくす (永遠に速度が増えないようにするため)
            var isGround = rigidbody.IsGround;
            if (Velocity.y < 0.0f && isGround)
            {
                Velocity = new Vector2(Velocity.x, -0.4f);
            }

            // 天井にぶつかっていたら上向き速度をなくす
            var isTopCollide = rigidbody.IsTopCollide;
            if (Velocity.y > 0.0f && isTopCollide)
            {
                Velocity = new Vector2(Velocity.x, 0.0f);
            }

            // 壁にぶつかっていたら左右速度をなくす (ただし、小さすぎると地形判定の誤差に引っかかるので若干の速度はつける)
            if (Velocity.x > 0.0f && rigidbody.IsRightCollide)
            {
                Velocity = new Vector2(Mathf.Min(1.0f, Velocity.x), Velocity.y);
            }
            if (Velocity.x < 0.0f && rigidbody.IsLeftCollide)
            {
                Velocity = new Vector2(Mathf.Max(-1.0f, Velocity.x), Velocity.y);
            }

            var gravityRate = GravityRateState;
            // 空中にいるとき、頂点付近だと重力を小さくする (気持ちよさのため)
            if (!isGround && Mathf.Abs(Velocity.y) < 0.3f)
            {
                gravityRate *= 0.4f;
            }

            var newVel = Velocity;

            // 地面の傾斜を見てあれこれする
            if (isGround)
            {
                var groundInfo = rigidbody.GroundInfo;
            }

            // x軸の速度計算
            {
                var accelX = isGround ? _accelX : _accelAirX;
                var decelX = isGround ? _decelX : _decelAirX;
                var axisX = InputUtil.GetAxis(gameObject, AxisCode.Horizontal);
                if (IsUncontrollable || IsUncontrollableState)
                {
                    axisX = 0.0f;
                }
                var addVelX = axisX * accelX * deltaTime;

                newVel.x += addVelX;

                var maxVelocityX = _maxVelocity.x * MaxVelocityRateXState;
                // スティックの倒し具合も最高速度に反映させる
                maxVelocityX *= axisX;

                if (newVel.x * maxVelocityX >= 0.0f)
                {
                    // 同じ方向の時

                    if (Mathf.Abs(newVel.x) <= Mathf.Abs(maxVelocityX))
                    {
                        // 上限に達していないのでそのままでOK
                    }
                    else
                    {
                        // 上限を超えているので、最大で上限まで減速する
                        if (newVel.x < 0.0f)
                        {
                            newVel.x = Mathf.Min(newVel.x + decelX * deltaTime, maxVelocityX);
                        }
                        else
                        {
                            newVel.x = Mathf.Max(newVel.x - decelX * deltaTime, maxVelocityX);
                        }
                    }
                }
                else
                {
                    // 違う方向の時
                    // 絶対に上限を超えていない
                    // 減速度と加速度の大きいほうを足す
                    newVel.x -= addVelX;
                    newVel.x += Mathf.Sign(axisX) * Mathf.Max(accelX, decelX) * deltaTime;

                    // この加速で上限を超えないようにクランプ
                    if (maxVelocityX < 0.0f)
                    {
                        newVel.x = Mathf.Max(newVel.x, maxVelocityX);
                    }
                    else
                    {
                        newVel.x = Mathf.Min(newVel.x, maxVelocityX);
                    }
                }
            }

            // y軸の速度計算
            if (!isGround)
            {
                var addVelY = _gravity * gravityRate * deltaTime;
                newVel.y += addVelY;
                var maxVelocityY = _maxVelocity.y * MaxVelocityRateYState;

                if (newVel.y <= maxVelocityY && newVel.y >= -maxVelocityY)
                {
                    // 上限を超えていなければ何もしない
                }
                else
                {
                    // 上限を超えているので、最大で上限まで減速する
                    if (newVel.y > maxVelocityY)
                    {
                        // 重力で下がっているはずなので減速はなし
                    }
                    else
                    {
                        newVel.y = Mathf.Min(newVel.y + _decelGravity * DecalGravityRateState * deltaTime, -maxVelocityY);
                    }
                }
            }

            Velocity = newVel;
            var vel3 = new Vector3(Velocity.x * SpeedRateX, Velocity.y * SpeedRateY, 0.0f);
            transform.position += vel3 * deltaTime;
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        [SerializeField]
        Vector2 _maxVelocity = new Vector2(14.0f, 24.0f);
        [SerializeField]
        float _accelX = 36.0f;
        [SerializeField]
        float _decelX = 84.0f;
        [SerializeField]
        float _accelAirX = 24.0f;
        [SerializeField]
        float _decelAirX = 56.0f;
        [SerializeField]
        float _gravity = -98.0f;
        [SerializeField]
        float _decelGravity = 196.0f;

        TadaLib.Util.Timer _uncontrollableTimer = new TadaLib.Util.Timer(2.0f);
        #endregion
    }
}