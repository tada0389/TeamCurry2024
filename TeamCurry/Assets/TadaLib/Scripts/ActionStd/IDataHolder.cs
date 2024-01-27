using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// 他のコンポーネントから参照されるデータ群
    /// </summary>
    public interface IDataHolder
    {
        /// <summary>
        /// 死亡状態か
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// 壁張り付き状態か
        /// </summary>
        bool IsWall { get; }

        /// <summary>
        /// ジャンプ開始時のフレームか
        /// </summary>
        bool IsJumpStartFrame { get; }

        /// <summary>
        /// 最終着地地点
        /// </summary>
        public Vector3 LastLandingPos { get; }

        /// <summary>
        /// 速度
        /// </summary>
        public Vector3 Velocity { get; }

        /// <summary>
        /// 速度の大きさ倍率。[-1, 1]
        /// </summary>
        public Vector3 MoveVelocityRate { get; }

        /// <summary>
        /// 向いている方向
        /// </summary>
        public Vector3 FaceVec { get; }

        /// <summary>
        /// 地上にいない時間
        /// </summary>
        public float NoGroundDurationSec { get; }
    }
}