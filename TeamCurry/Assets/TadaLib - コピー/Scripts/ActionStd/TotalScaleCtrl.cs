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
    /// Scale制御
    /// </summary>
    public class TotalScaleCtrl
        : BaseProc
        , IProcUpdate
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            _scaleBase = transform.localScale;
        }
        #endregion

        #region TadaLib.ProcSystem.IProcUpdate の実装
        public void OnUpdate()
        {
            var scale = _scaleBase;
            var viewScale = Vector3.one;
            var scaleChangers = GetComponents<IScaleChanger>();
            foreach (var changer in scaleChangers)
            {
                scale = Vector3.Scale(scale, changer.ScaleRate);
                viewScale = Vector3.Scale(viewScale, changer.ViewScaleRate);
            }

            transform.localScale = scale;
            if (_transformForViewOffset != null)
            {
                _transformForViewOffset.localScale = viewScale;
            }
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        [SerializeField]
        Transform _transformForViewOffset = null;
        Vector3 _scaleBase;
        #endregion
    }
}