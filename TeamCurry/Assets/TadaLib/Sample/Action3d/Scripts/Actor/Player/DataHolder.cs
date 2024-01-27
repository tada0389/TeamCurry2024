using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;
using TadaLib.Extension;

namespace TadaLib.Sample.Action3d.Actor.Player
{
    /// <summary>
    /// Data保持
    /// </summary>
    public class DataHolder 
        : BaseProc
        , IDataHolder
        , IProcUpdate
        , IProcPostMove
    {
        #region IDataHolder の実装
        public bool IsDead => GetComponent<StateMachine>().StateInfo.GetFlag(StateInfoKind.IsDead);
        public bool IsWall => GetComponent<StateMachine>().StateInfo.GetFlag(StateInfoKind.IsWall);
        public bool IsJumpStartFrame { get; set; } = false;
        public Vector3 LastLandingPos { private set; get; } = Vector3.zero;
        public Vector3 Velocity => GetComponent<MoveCtrl>().Velocity;
        public Vector3 MoveVelocityRate
        {
            get
            {
                var moveCtrl = GetComponent<MoveCtrl>();
                var vel = moveCtrl.Velocity;
                var maxVel = moveCtrl.MaxVelocity;
                var x = Mathf.Clamp(vel.x / maxVel.x, -1.0f, 1.0f);
                var y = Mathf.Clamp(vel.y / maxVel.y, -1.0f, 1.0f);
                return new Vector3(x, y, 0.0f);
            }
        }
        public Vector3 FaceVec => transform.right;
        public float NoGroundDurationSec { private set; get; } = 0.0f;
        #endregion

        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        public void OnUpdate()
        {
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

            IsJumpStartFrame = false;
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        #endregion
    }
}