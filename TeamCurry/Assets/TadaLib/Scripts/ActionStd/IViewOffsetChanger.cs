using UnityEngine;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// 見た目座標変動用のインターフェース
    /// </summary>
    public interface IViewOffsetChanger
    {
        #region プロパティ
        /// <summary>
        /// 見た目移動
        /// </summary>
        Vector3 ViewOffset { get; }
        #endregion
    }
}