using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Actor.Player
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
        #endregion

        #region Monobehavior の実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        void Start()
        {
            UpdateRotate();
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
            var rotVec3 = transform.localEulerAngles;

            if (dataHolder.IsWall)
            {
                // 壁張り付き時
                var rigidbody = GetComponent<TadaRigidbody2D>();
                rotVec3.y = rigidbody.IsRightCollide ? 180.0f : 0.0f;
            }
            else
            {
                var velocityX = GetComponent<MoveCtrl>().Velocity.x;
                if (Mathf.Abs(velocityX) >= 1.0f)
                {
                    rotVec3.y = velocityX < 0.0f ? 180.0f : 0.0f;
                }
            }

            transform.localEulerAngles = rotVec3;
            dataHolder.FaceVec = transform.right;
        }
        #endregion

        #region privateフィールド
        #endregion
    }
}