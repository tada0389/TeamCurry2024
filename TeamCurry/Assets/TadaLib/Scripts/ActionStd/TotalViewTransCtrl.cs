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
    /// 見た目座標制御
    /// </summary>
    public class TotalViewTransCtrl : BaseProc, IProcUpdate
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region TadaLib.ProcSystem.IProcUpdate の実装
        /// <summary>
        /// 移動前の更新処理
        /// </summary>
        public void OnUpdate()
        {
            var viewOffset = Vector3.zero;
            var viewOffsetChangers = GetComponents<IViewOffsetChanger>();
            foreach (var changer in viewOffsetChangers)
            {
                viewOffset += changer.ViewOffset;
            }

            if (_transformForViewOffset != null)
            {
                _transformForViewOffset.localPosition = viewOffset;
            }
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        [SerializeField]
        Transform _transformForViewOffset = null;
        #endregion
    }
}