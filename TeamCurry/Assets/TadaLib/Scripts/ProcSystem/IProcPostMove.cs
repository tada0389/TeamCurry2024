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
    /// 移動後の更新用インターフェース
    /// </summary>
    public interface IProcPostMove
    {
        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        void OnPostMove();
    }
}