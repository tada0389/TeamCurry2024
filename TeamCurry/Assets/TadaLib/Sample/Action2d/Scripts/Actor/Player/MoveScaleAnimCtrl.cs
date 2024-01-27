using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;
using TadaLib.Util;
using TadaLib.Extension;

namespace TadaLib.Sample.Action2d.Actor.Player
{
    /// <summary>
    /// 移動によるスケール変動
    /// 見た目のスケールだけが変わる
    /// </summary>
    public class MoveScaleAnimCtrl 
        : BaseProc
        , IProcUpdate
        , IScaleChanger
    {
        #region プロパティ
        public bool IsDisableFastFallAnim { get; set; } = false;
        #endregion

        #region メソッド
        #endregion

        #region TadaLib.ProcSystem.IProcの実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        public void OnStart()
        {
            _landingAnimTimer.TimeReset(_landingAnimDurationSec);
            _landingAnimTimer.ToLimitTime();

            _jumpAnimTimer.TimeReset(_jumpAnimDurationSec);
            _jumpAnimTimer.ToLimitTime();
        }

        /// <summary>
        /// 移動前の更新処理
        /// </summary>
        public void OnUpdate()
        {
            // まずスケールの初期化
            ScaleRate = Vector3.one;
            ViewScaleRate = Vector3.one;

            // アニメーション用のデータ更新
            _isGroundPrev = _isGround;
            _isGround = GetComponent<TadaRigidbody2D>().IsGround;

            var deltaTime = gameObject.DeltaTime();

            UpdateLandingAnim(deltaTime);
            UpdateJumpStartAnim(deltaTime);
            if (!IsDisableFastFallAnim)
            {
                UpdateFastFallAnim(deltaTime);
            }
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 着地時のスケールアニメーション
        /// </summary>
        void UpdateLandingAnim(float deltaTime)
        {
            _landingAnimTimer.Advance(deltaTime);

            if (_isGround && !_isGroundPrev)
            {
                // 着地開始
                _landingAnimTimer.TimeReset(_landingAnimDurationSec);

                // アニメの強さ計算
                var moveCtrl = GetComponent<MoveCtrl>();
                var strengthRate = 4.0f * Mathf.Clamp(-moveCtrl.Velocity.y / moveCtrl.MaxVelocityIgnoreRate.y, 0.0f, 1.0f);
                _landingAnimStrength = _landingAnimStrengthBase * strengthRate;
            }

            if (_landingAnimTimer.IsTimout)
            {
                return;
            }

            var rate = _landingAnimTimer.TimeRate01;
            rate = 1.0f - rate;// Easing.InOutBack(rate);
            var scaleX = 1.0f + _landingAnimStrength * rate;
            var scaleY = 1.0f / scaleX;

            ViewScaleRate = Vector3.Scale(ViewScaleRate, new Vector3(scaleX, scaleY, 1.0f));
        }

        /// <summary>
        /// ジャンプ開始時のスケールアニメーション
        /// </summary>
        void UpdateJumpStartAnim(float deltaTime)
        {
            _jumpAnimTimer.Advance(deltaTime);

            if (GetComponent<DataHolder>().IsJumpStartFrame)
            {
                // ジャンプ開始
                _jumpAnimTimer.TimeReset(_jumpAnimDurationSec);

                // アニメの強さ計算
                var moveCtrl = GetComponent<MoveCtrl>();
                var strengthRate = 4.0f * Mathf.Clamp(moveCtrl.Velocity.y / moveCtrl.MaxVelocityIgnoreRate.y, 0.0f, 1.0f);
                _jumpAnimStrength = _jumpAnimStrengthBase * strengthRate;
            }

            if (_jumpAnimTimer.IsTimout)
            {
                return;
            }

            var rate = _jumpAnimTimer.TimeRate01;
            rate = 1.0f - rate;// Easing.InOutBack(rate);
            var scaleY = 1.0f + _jumpAnimStrength * rate;
            var scaleX = 1.0f / scaleY;

            ViewScaleRate = Vector3.Scale(ViewScaleRate, new Vector3(scaleX, scaleY, 1.0f));
        }

        /// <summary>
        /// 高速落下時のスケールアニメーション
        /// </summary>
        void UpdateFastFallAnim(float deltaTime)
        {
            // Y軸の落下速度がデフォルトの上限を超えていた時に拡縮する

            var moveCtrl = GetComponent<MoveCtrl>();
            var velY = moveCtrl.Velocity.y;

            var rate = 0.0f;

            if (velY < 0.0f && -velY > moveCtrl.MaxVelocityIgnoreRate.y)
            {
                var min = moveCtrl.MaxVelocityIgnoreRate.y;
                var max = moveCtrl.MaxVelocityIgnoreRate.y * 1.8f;
                var cur = -velY;
                rate = Mathf.Clamp01((cur - min) / (max - min));
            }

            //rate = Easing.InOutBack(rate);
            var scaleY = 1.0f + _fastFallAnimMaxStrength * rate;
            var scaleX = 1.0f / scaleY;

            ViewScaleRate = Vector3.Scale(ViewScaleRate, new Vector3(scaleX, scaleY, 1.0f));
        }
        #endregion

        #region IScaleChangerの実装
        public Vector3 ScaleRate { private set; get; } = Vector3.one;

        public Vector3 ViewScaleRate { private set; get; } = Vector3.one;
        #endregion

        #region privateフィールド
        [SerializeField, Range(0.0f, 0.2f)]
        float _landingAnimStrengthBase = 0.2f;
        [SerializeField, Range(0.1f, 1.0f)]
        float _landingAnimDurationSec = 0.5f;

        [SerializeField, Range(0.0f, 0.2f)]
        float _jumpAnimStrengthBase = 0.2f;
        [SerializeField, Range(0.1f, 1.0f)]
        float _jumpAnimDurationSec = 0.5f;

        [SerializeField, Range(0.0f, 0.8f)]
        float _fastFallAnimMaxStrength = 0.2f;

        Timer _jumpAnimTimer = new TadaLib.Util.Timer(1.0f);
        float _jumpAnimStrength = 0.0f;

        Timer _landingAnimTimer = new TadaLib.Util.Timer(1.0f);
        float _landingAnimStrength = 0.0f;

        bool _isGroundPrev = false;
        bool _isGround = false;
        #endregion
    }
}