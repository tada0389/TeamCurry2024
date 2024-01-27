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
    /// 衝突判定のノードの保持者
    /// </summary>
    public class Owner
    {
        #region コンストラクタ
        public Owner(Manager manager, GameObject obj)
        {
            _manager = manager;
            Obj = obj;
        }
        #endregion

        #region メソッド
        /// <summary>
        /// ノード追加
        /// </summary>
        public void AddNode(in Vector2 posOffset, float radius)
        {
            _nodes.Add(_manager.CreateNode(this, posOffset, radius));
        }

        /// <summary>
        /// ノード削除
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(Node node)
        {
            _nodes.Remove(node);
        }

        public Node Node(int idx)
        {
            Assert.IsTrue(idx >= 0);
            Assert.IsTrue(idx < NodeCount);

            return _nodes[idx];
        }

        public void NotifyResult(CollResultProxy resultProxy)
        {
            CollResultProxy = resultProxy;
        }
        #endregion

        #region プロパティ
        public bool IsEnabled { get; set; } = true;

        public GameObject Obj { get; }

        public Transform ConstraintTrans { get; set; } = null;
        public int Tag { get; set; } = 0;

        /// <summary>
        /// ノード数
        /// </summary>
        public int NodeCount => _nodes.Count;

        /// <summary>
        /// 座標
        /// </summary>
        public Vector2 Pos => (ConstraintTrans == null) ? Obj.transform.position : ConstraintTrans.position;

        public float RadiusRate => (ConstraintTrans == null) ? 1.0f : 1.0f;// ConstraintTrans.localScale.x;

        public CollResultProxy CollResultProxy { get; private set; } = new CollResultProxy();

        /// <summary>
        /// 破棄されたか
        /// </summary>
        public bool IsDisposed => Obj == null;
        #endregion

        #region private フィールド
        Manager _manager;
        List<Node> _nodes = new List<Node>();
        #endregion

        #region private メソッド
        #endregion
    }
}