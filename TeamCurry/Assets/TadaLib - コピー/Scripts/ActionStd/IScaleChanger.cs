using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スケール変動用のインターフェース
/// </summary>
namespace TadaLib.ActionStd
{
    public interface IScaleChanger
    {
        #region プロパティ
        /// <summary>
        /// スケール倍率
        /// </summary>
        Vector3 ScaleRate { get; }
        /// <summary>
        /// 見た目のみのスケール倍率
        /// </summary>
        Vector3 ViewScaleRate { get; }
        #endregion
    }
}