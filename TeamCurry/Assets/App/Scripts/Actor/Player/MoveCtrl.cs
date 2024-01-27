using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;

namespace App.Actor.Player
{
    /// <summary>
    /// MoveCtrl
    /// </summary>
    public class MoveCtrl
        : BaseProc
        , IProcMove
    {
        #region プロパティ
        public bool IsEnableState { get; set; } = true;

        public bool IsUncontrollable { get; set; } = false;
        public bool IsUncontrollableState { get; set; } = false;

        public bool IsDisableGoOutState { get; set; } = false;

        public Vector2 Velocity { get; private set; } = Vector2.zero;

        public float SpeedRateX { get; set; } = 1.0f;
        public float SpeedRateY { get; set; } = 1.0f;
        public float MaxVelocityRateXState { get; set; } = 1.0f;
        public float MaxVelocityRateYState { get; set; } = 1.0f;
        public Vector2 MaxVelocityIgnoreRate => _maxVelocity;
        public Vector2 MaxVelocity => new Vector2(_maxVelocity.x * MaxVelocityRateXState, _maxVelocity.y * MaxVelocityRateYState);
        #endregion

        #region メソッド
        #endregion

        #region Monobehavior の実装
        void Start()
        {
            // ステート開始時に初期化させる
            var stateMachine = GetComponent<StateMachine>();
            stateMachine.AddStateStartCallback(() =>
            {
                IsEnableState = true;
                IsUncontrollableState = false;
                MaxVelocityRateXState = 1.0f;
                MaxVelocityRateYState = 1.0f;
                IsDisableGoOutState = false;
            });

            _uncontrollableTimer.ToLimitTime();

            // 地面への着地判定を強めるために、下方向に初速を与える
            Velocity = new Vector2(0.0f, 0.0f);
        }
        #endregion

        #region TadaLib.ProcSystem.IProcMove の実装
        /// <summary>
        /// 移動更新処理
        /// </summary>
        public void OnMove()
        {
            if (!IsEnableState)
            {
                return;
            }

            var deltaTime = gameObject.DeltaTime();

            if (deltaTime > 0.2f)
            {
                // 以上に高い場合は前回のを使う
                // 不具合の暫定対処
                deltaTime = _deltaTimePrev;
            }
            _deltaTimePrev = deltaTime;

            var newVel = Velocity;

            var axis = new Vector2(InputSystem.JoyconInput.Instance.GetAxis(InputSystem.AxisCode.Horizontal), InputSystem.JoyconInput.Instance.GetAxis(InputSystem.AxisCode.Vertical));


            // x軸の速度計算
            {
                var accelX = _accelX;
                var decelX = _decelX;
                var axisX = axis.x;
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
            {
                var accelY = _accelY;
                var decelY = _decelY;
                var axisY = axis.y;
                if (IsUncontrollable || IsUncontrollableState)
                {
                    axisY = 0.0f;
                }
                var addVelY = axisY * accelY * deltaTime;

                newVel.y += addVelY;

                var maxVelocityY = _maxVelocity.y * MaxVelocityRateYState;
                // スティックの倒し具合も最高速度に反映させる
                maxVelocityY *= axisY;

                if (newVel.y * maxVelocityY >= 0.0f)
                {
                    // 同じ方向の時

                    if (Mathf.Abs(newVel.y) <= Mathf.Abs(maxVelocityY))
                    {
                        // 上限に達していないのでそのままでOK
                    }
                    else
                    {
                        // 上限を超えているので、最大で上限まで減速する
                        if (newVel.y < 0.0f)
                        {
                            newVel.y = Mathf.Min(newVel.y + decelY * deltaTime, maxVelocityY);
                        }
                        else
                        {
                            newVel.y = Mathf.Max(newVel.y - decelY * deltaTime, maxVelocityY);
                        }
                    }
                }
                else
                {
                    // 違う方向の時
                    // 絶対に上限を超えていない
                    // 減速度と加速度の大きいほうを足す
                    newVel.y -= addVelY;
                    newVel.y += Mathf.Sign(axisY) * Mathf.Max(accelY, decelY) * deltaTime;

                    // この加速で上限を超えないようにクランプ
                    if (maxVelocityY < 0.0f)
                    {
                        newVel.y = Mathf.Max(newVel.y, maxVelocityY);
                    }
                    else
                    {
                        newVel.y = Mathf.Min(newVel.y, maxVelocityY);
                    }
                }
            }

            Velocity = newVel;
            var vel3 = new Vector3(Velocity.x * SpeedRateX, Velocity.y * SpeedRateY, 0.0f);
            transform.position += vel3 * deltaTime;

            GetComponent<DataHolder>().Velocity = Velocity;

            if (IsDisableGoOutState)
            {
                // 画面内に抑える
                var x = transform.position.x;
                var y = transform.position.y;

                var newPos = transform.position;
                if (x > _borderX)
                {
                    newPos.x = _borderX;
                }
                else if (x < -_borderX - 2.0f)
                {
                    newPos.x = -_borderX - 2.0f;
                }

                if (y > _borderY)
                {
                    newPos.y = _borderY;
                }
                else if (y < -_borderY)
                {
                    newPos.y = -_borderY;
                }

                transform.position = newPos;
            }

            //deltaTime.Log();
            //(vel3 * deltaTime).Log();
        }
        #endregion

        #region privateフィールド
        #endregion

        #region privateメソッド
        [SerializeField]
        Vector2 _maxVelocity = new Vector2(14.0f, 10.0f);
        [SerializeField]
        float _accelX = 36.0f;
        [SerializeField]
        float _decelX = 84.0f;
        [SerializeField]
        float _accelY = 36.0f;
        [SerializeField]
        float _decelY = 84.0f;

        [SerializeField]
        float _borderX = 25.0f;
        [SerializeField]
        float _borderY = 16.0f;

        float _deltaTimePrev = 0.015f;

        TadaLib.Util.Timer _uncontrollableTimer = new TadaLib.Util.Timer(2.0f);
        #endregion
    }
}