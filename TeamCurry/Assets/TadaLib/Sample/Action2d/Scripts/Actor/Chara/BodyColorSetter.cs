using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib;
using TadaLib.ProcSystem;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Actor.Chara
{
    /// <summary>
    /// Component処理
    /// </summary>
    public class BodyColorSetter
        : BaseProc
        , IProcPostMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region TadaLib.ActorBase.IProcPostMove の実装
        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            var colorManager = ColorManager.Instance;
            Assert.IsTrue(colorManager != null);

            var color = colorManager.GetColor(_colorKind);

            if (_mesh != null)
            {
                _mesh.color = color;
            }
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        ColorManager.ColorKind _colorKind = ColorManager.ColorKind.PlayerBody;
        [SerializeField]
        SpriteRenderer _mesh;
        #endregion

        #region privateメソッド
        #endregion
    }
}