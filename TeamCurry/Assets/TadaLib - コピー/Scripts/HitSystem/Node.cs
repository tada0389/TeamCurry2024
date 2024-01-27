using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.HitSystem
{
    /// <summary>
    /// 衝突判定のノード
    /// 円形状
    /// </summary>
    public class Node
    {
        #region コンストラクタ
        public Node(Owner owner, in Vector2 posOffset, float radius)
        {
            _owner = owner;
            _posOffset = posOffset;
            _radius = radius;
        }
        #endregion

        #region メソッド
        #endregion

        #region プロパティ
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 中心座標
        /// </summary>
        public Vector2 CenterPos => _owner.Pos + _posOffset;

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius => _owner.RadiusRate * _radius;

        /// <summary>
        /// 削除済みか
        /// </summary>
        public bool IsDisposed => _owner == null;
        #endregion

        #region private フィールド
        Owner _owner;
        Vector2 _posOffset;
        float _radius;
        #endregion

        #region private メソッド
        #endregion
    }
}