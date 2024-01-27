using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace App.Actor.Player
{
    /// <summary>
    /// 向き制御処理
    /// </summary>
    public class RotateCtrl
        : BaseProc
        , IProcPostMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        public bool IsFlipState { get; set; } = false;
        #endregion

        #region Monobehavior の実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        void Start()
        {
            // ステート開始時に初期化させる
            var stateMachine = GetComponent<StateMachine>();
            stateMachine.AddStateStartCallback(() =>
            {
                IsFlipState = false;
            });

            UpdateRotate();

            _eulerAngles = transform.localEulerAngles;
        }

        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            UpdateRotate();
        }
        #endregion

        #region privateメソッド
        void UpdateRotate()
        {
            var dataHolder = GetComponent<DataHolder>();

            var velocityX = dataHolder.Velocity.x;
            if (Mathf.Abs(velocityX) >= 1.0f)
            {
                _eulerAngles.y = velocityX < 0.0f ? 180.0f : 0.0f;
            }

            var eullerAngle = _eulerAngles;
            if (IsFlipState)
            {
                eullerAngle.y = _eulerAngles.y > 90.0f ? 0.0f : 180.0f;
            }

            transform.localEulerAngles = eullerAngle;
            dataHolder.FaceVec = transform.right;
        }
        #endregion

        #region privateフィールド
        Vector3 _eulerAngles = Vector3.zero;
        #endregion
    }
}