using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;
using TadaLib.Extension;

namespace App.Actor.Player
{
    /// <summary>
    /// Data保持
    /// </summary>
    public class DataHolder : BaseProc, IProcUpdate, IProcPostMove
    {
        #region プロパティ
        public bool IsDead { get; set; } = false;
        public Vector3 FaceVec { get; set; } = Vector3.right;
        public Vector2 Velocity { get; set; } = Vector3.zero;
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
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        #endregion
    }
}