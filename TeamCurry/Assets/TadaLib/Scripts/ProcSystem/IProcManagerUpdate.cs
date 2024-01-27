using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.ProcSystem
{
    /// <summary>
    /// マネージャークラス更新用のインターフェース
    /// </summary>
    public interface IProcManagerUpdate
    {
        #region イベント関数
        /// <summary>
        /// 更新処理
        /// </summary>
        void OnUpdate();

        ///// <summary>
        ///// シーン終了処理
        ///// </summary>
        //void OnSceneEnd();
        #endregion
    }
}