using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;
using TadaLib.Extension;

namespace TadaLib.Sample.Action2d.Actor.Player
{
    /// <summary>
    /// Data保持
    /// </summary>
    public class DataHolder : BaseProc, IProcUpdate, IProcPostMove
    {
        #region プロパティ
        public float NoGroundDurationSec { get; private set; } = 0.0f;
        public bool IsJumpStartFrame { get; set; } = false;
        public bool IsSpringJumpStartFrame { get; set; } = false;
        /// <summary>
        /// 壁張り付き状態
        /// </summary>
        public bool IsWall { get; set; } = false;
        public bool IsDead { get; set; } = false;
        public bool IsDeadShrink { get; set; } = false;
        public bool IsDeadByShockwave { get; set; } = false;
        public bool IsCrouch { get; set; } = false;
        public Vector3 LastLandingPos { get; set; } = Vector3.zero;
        public Vector3 FaceVec { get; set; } = Vector3.right;
        public Vector2 Velocity { get; set; } = Vector3.zero;
        #endregion

        #region メソッド
        #endregion

        public void OnUpdate()
        {
            IsSpringJumpStartFrame = false;
        }

        #region TadaLib.ProcSystem.IProcPostMove の実装
        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            if (GetComponent<TadaRigidbody2D>().IsGround)
            {
                NoGroundDurationSec = 0.0f;
                LastLandingPos = transform.position;
            }
            else
            {
                NoGroundDurationSec += gameObject.DeltaTime();
            }

            Velocity = GetComponent<MoveCtrl>().Velocity;

            IsJumpStartFrame = false;
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        #endregion
    }
}