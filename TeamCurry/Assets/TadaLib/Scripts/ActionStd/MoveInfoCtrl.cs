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
    /// 移動情報制御
    /// </summary>
    public class MoveInfoCtrl : BaseProc, IProcUpdate
    {
        #region プロパティ
        public Vector2 MoveDiff => (Vector2)transform.position - _PosPrev;
        #endregion

        #region メソッド
        public void RegisterRidedFrame(GameObject obj)
        {
            _rideObjects.Add(obj);
        }

        public bool IsRidedByPlayer()
        {
            var player = PlayerManager.TryGetMainPlayer();
            foreach (var obj in _rideObjects)
            {
                if (obj == player)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region TadaLib.ProcSystem.IProcUpdate の実装
        /// <summary>
        /// 移動前の更新処理
        /// </summary>
        public void OnUpdate()
        {
            _PosPrev = transform.position;
            _rideObjects.Clear();
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        Vector2 _PosPrev; // 更新前の座標
        List<GameObject> _rideObjects = new List<GameObject>();
        #endregion
    }
}