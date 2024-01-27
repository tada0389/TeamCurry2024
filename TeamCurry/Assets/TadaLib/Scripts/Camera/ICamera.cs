using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;

namespace TadaLib.Camera
{
    /// <summary>
    /// ICamera
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// シーン開始時の初期化処理
        /// </summary>
        void OnSceneStartInitialize(in UpdateData data);

        /// <summary>
        /// カメラを更新する
        /// </summary>
        void Update(in UpdateData data);

#if UNITY_EDITOR
        /// <summary>
        /// デバッグ描画
        /// </summary>
        void DrawDebug(in UpdateData data);
#endif
    }
}