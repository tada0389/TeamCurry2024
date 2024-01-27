using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.HitSystem
{
    [System.Serializable]
    class NodeData
    {
        public Vector2 PosOffset;
        public float Radius;
    }

    public enum TagKind
    {
        Default,
        Damage,
        Player,
    }

    /// <summary>
    /// HitSystemのオーナー管理
    /// </summary>
    public class OwnerCtrl
        : MonoBehaviour
    {
        #region public static 関数
        public static bool TryGetCollResults(GameObject obj, out IReadOnlyCollection<CollResult> results)
        {
            results = null;

            var ownerCtrl = obj.GetComponent<OwnerCtrl>();
            Assert.IsTrue(ownerCtrl != null);
            var proxy = ownerCtrl.Owner.CollResultProxy;
            if (!proxy.IsCollide)
            {
                return false;
            }
            results = proxy.Results();
            return true;
        }
        #endregion

        #region プロパティ
        public Owner Owner { private set; get; }
        #endregion

        #region メソッド
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            // HitSystem用データの初期化
            Owner = Manager.Instance.CreateOwner(gameObject);
            if (_constraintTrans != null)
            {
                Owner.ConstraintTrans = _constraintTrans;
            }
            Owner.Tag = (int)_tag;

            // 初期生成用ノードに基づいてノード作成
            foreach (var data in _initCreatedNodes)
            {
                Owner.AddNode(data.PosOffset, data.Radius);
            }
        }
        #endregion

        #region privateフィールド

        #endregion

        #region privateメソッド
        [SerializeField]
        Transform _constraintTrans = null;
        [SerializeField]
        List<NodeData> _initCreatedNodes = new List<NodeData>();
        [SerializeField]
        TagKind _tag;
        #endregion

        #region MonoBehavior の実装
        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            foreach (var data in _initCreatedNodes)
            {
                var pos = _constraintTrans == null ? transform.position : _constraintTrans.position;
                Gizmos.DrawWireSphere(pos + (Vector3)data.PosOffset, data.Radius);
            }
#endif
        }
    }
    #endregion
}