using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Extension;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Actor.Player
{
    /// <summary>
    /// Component処理
    /// </summary>
    public class CrouchCtrl
        : BaseProc
        , IProcMove
        , IScaleChanger
        , IViewOffsetChanger
    {
        #region プロパティ
        #endregion

        #region メソッド
        /// <summary>
        /// しゃがみ状態の開始 (ステート切り替えでリセット)
        /// </summary>
        public void StartCrouchModeState()
        {
            if (!_isCrouchMode)
            {
                _isCrouchMode = true;
                OnModeChanged();
            }
        }
        #endregion

        #region TadaLib.ProcSystem.IProcStart の実装
        void Start()
        {
            // ステート切り替えで状態リセット
            StateMachine.AddStateStartCallback(gameObject, () =>
            {
                if (_isCrouchMode)
                {
                    _isCrouchMode = false;
                    OnModeChanged();
                }
            });

            _startTimer = new TadaLib.Util.Timer(_expandDurationSec);
            _endTimer = new TadaLib.Util.Timer(_expandDurationSec * 0.5f);
        }
        #endregion

        #region TadaLib.ProcSystem.IProcMove の実装
        /// <summary>
        /// 移動更新処理
        /// </summary>
        public void OnMove()
        {
            if (_endTimer == null)
            {
                return;
            }

            var deltaTime = gameObject.DeltaTime();

            var scaleRate = Vector3.one;
            var viewScaleRate = Vector3.one;

            if (_isCrouchMode)
            {
                _startTimer.Advance(deltaTime);
                var rate = TadaLib.Util.Easing.Outback(_startTimer.TimeRate01);
                var scaleX = 1.0f + (_expandScale.x - 1.0f) * rate;
                var scaleY = 1.0f + (_expandScale.y - 1.0f) * rate;
                scaleRate = new Vector3(scaleX, scaleY, 1.0f);
            }
            else
            {
                _endTimer.Advance(deltaTime);
                var rate = TadaLib.Util.Easing.Outback(_endTimer.TimeRate01);
                var scaleX = _expandScale.x + (1.0f - _expandScale.x) * rate;
                var scaleY = _expandScale.y + (1.0f - _expandScale.y) * rate;
                scaleRate = new Vector3(scaleX, scaleY, 1.0f);
            }

            // 基準値を超えている場合は見た目だけのオフセットとする
            if (scaleRate.x > _expandScale.x)
            {
                var rate = scaleRate.x / _expandScale.x;
                scaleRate.x = _expandScale.x;
                viewScaleRate.x = rate;
            }

            if (scaleRate.x < 1.0f)
            {
                var rate = scaleRate.x / 1.0f;
                scaleRate.x = 1.0f;
                viewScaleRate.x = rate;
            }

            if (scaleRate.y > 1.0f)
            {
                var rate = scaleRate.y / 1.0f;
                scaleRate.y = 1.0f;
                viewScaleRate.y = rate;
            }
            if (scaleRate.y < _expandScale.y)
            {
                var rate = scaleRate.y / _expandScale.y;
                scaleRate.y = _expandScale.y;
                viewScaleRate.y = rate;
            }

            ScaleRate = scaleRate;
            ViewScaleRate = viewScaleRate;

            // 移動による振動
            if (Mathf.Abs(GetComponent<MoveCtrl>().Velocity.x) > 0.1f)
            {
                _moveTime += deltaTime;
                var theta = _moveTime * 2.0f * Mathf.PI / _moveVibPeriod;
                var t = Mathf.Sin(theta);
                t = TadaLib.Util.InterpUtil.Remap(t, _moveVibThr, 1.0f, 0.0f, _moveVibStrength);
                ViewOffset = Vector3.up * t;
            }
            else
            {
                ViewOffset = Vector3.zero;
            }
        }
        #endregion

        #region TadaLib.ProcSystem.IScaleChanger の実装
        public Vector3 ScaleRate { get; private set; } = Vector3.one;

        public Vector3 ViewScaleRate { get; private set; } = Vector3.one;
        #endregion

        #region TadaLib.ProcSystem.IViewOffsetChanger の実装
        public Vector3 ViewOffset { get; private set; } = Vector3.zero;
        #endregion

        #region privateフィールド
        [SerializeField]
        Vector2 _expandScale = new Vector2(4.0f, 0.6f);
        [SerializeField]
        float _expandDurationSec = 0.2f;
        [SerializeField]
        Chara.OutlineCtrl _outlineCtrl;

        bool _isCrouchMode = false;
        TadaLib.Util.Timer _startTimer;
        TadaLib.Util.Timer _endTimer;
        float _moveTime = 0.0f;

        [SerializeField]
        float _moveVibPeriod = 0.4f;
        [SerializeField]
        float _moveVibStrength = 0.2f;
        [SerializeField]
        float _moveVibThr = -0.4f;
        #endregion

        #region privateメソッド
        void OnModeChanged()
        {
            if (_isCrouchMode)
            {
                _startTimer.TimeReset();
                // レイヤーを変える
                gameObject.layer = TadaLib.Util.LayerUtil.LayerMaskToLayer(TadaLib.Sys.LayerMask.LandCollision);
                // アウトラインの色を変更する
                _outlineCtrl.OverwriteColorKind = Chara.ColorManager.ColorKind.StageOutline;
                GetComponent<DataHolder>().IsCrouch = true;
            }
            else
            {
                _endTimer.TimeReset();
                // レイヤーを変える
                gameObject.layer = TadaLib.Util.LayerUtil.LayerMaskToLayer(TadaLib.Sys.LayerMask.Player);
                // アウトラインの色を変更する
                _outlineCtrl.OverwriteColorKind = Chara.ColorManager.ColorKind.PlayerOutline;
                GetComponent<DataHolder>().IsCrouch = false;
            }
        }
        #endregion
    }
}