using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.HitSystem
{
    /// <summary>
    /// 衝突判定マネージャ
    /// </summary>
    public class Manager : BaseManagerProc<Manager>, IProcManagerUpdate
    {
        #region メソッド
        /// <summary>
        /// Ownerの作成
        /// </summary>
        public Owner CreateOwner(GameObject obj)
        {
            var owner = new Owner(this, obj);
            _owners.Add(owner);
            return owner;
        }

        /// <summary>
        /// Nodeの作成
        /// </summary>
        public Node CreateNode(Owner owner, in Vector2 posOffset, float radius)
        {
            var node = new Node(owner, posOffset, radius);
            _nodes.Add(node);
            return node;
        }
        #endregion

        #region プロパティ

        #endregion

        #region IManagerProc の実装
        public void OnUpdate()
        {
            Cleanup();

            // 衝突判定
            CheckCollides();
        }
        #endregion

        #region private フィールド
        List<Owner> _owners = new List<Owner>();
        List<Node> _nodes = new List<Node>();
        #endregion

        #region private メソッド
        void Cleanup()
        {
            for(int idx = _owners.Count - 1; idx >= 0; --idx)
            {
                if (_owners[idx]?.IsDisposed ?? true)
                {
                    _owners.RemoveAt(idx);
                }
            }
        }

        void CheckCollides()
        {
            // 衝突情報のクリア
            foreach(var owner in _owners)
            {
                owner.CollResultProxy.Clear();
            }

            foreach(var lhs in _owners)
            {
                if (!lhs.IsEnabled)
                {
                    continue;
                }

                foreach(var rhs in _owners)
                {
                    if (!rhs.IsEnabled)
                    {
                        continue;
                    }

                    if(lhs == rhs)
                    {
                        continue;
                    }

                    CheckCollide(lhs, rhs);
                }
            }
        }

        void CheckCollide(Owner lhs, Owner rhs)
        {
            for(int lhsIdx = 0, lhsNodeCount = lhs.NodeCount; lhsIdx < lhsNodeCount; ++lhsIdx)
            {
                var lhsNode = lhs.Node(lhsIdx);

                if (!lhsNode.IsEnabled)
                {
                    continue;
                }

                for(int rhsIdx = 0, rhsNodeCount = rhs.NodeCount; rhsIdx < rhsNodeCount; ++rhsIdx)
                {
                    var rhsNode = rhs.Node(rhsIdx);

                    if (!rhsNode.IsEnabled)
                    {
                        continue;
                    }

                    // 衝突チェック
                    var diff = lhsNode.CenterPos - rhsNode.CenterPos;
                    var sqDistance = diff.x * diff.x + diff.y * diff.y;
                    var radius = lhsNode.Radius + rhsNode.Radius;

                    if(sqDistance <= radius * radius)
                    {
                        // 衝突
                        var resultForLhs = CollResult.Create(rhs.Obj, rhs.Tag);
                        var resultForRhs = CollResult.Create(lhs.Obj, lhs.Tag);

                        lhs.CollResultProxy.AddResult(resultForLhs);
                        rhs.CollResultProxy.AddResult(resultForRhs);
                    }
                }
            }
        }
        #endregion
    }
}