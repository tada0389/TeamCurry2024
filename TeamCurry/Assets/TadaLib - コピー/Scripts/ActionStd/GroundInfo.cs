using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// 地面情報
    /// </summary>
    public struct GroundInfo
    {
        /// <summary>
        /// 地面の座標
        /// </summary>
        public Vector2 Pos;
        /// <summary>
        /// 地面の法線
        /// </summary>
        public Vector2 Normal;
        /// <summary>
        /// 地面の摩擦力
        /// </summary>
        public float Friction;
    }
}