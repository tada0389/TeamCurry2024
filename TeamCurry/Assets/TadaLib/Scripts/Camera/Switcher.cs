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
    /// カメラ切り替え者
    /// </summary>
    public static class Switcher
    {
        #region static 関数
        /// <summary>
        /// カメラを切り替える
        /// </summary>
        /// <param name="cameraNext"></param>
        public static void SwitchCamera(ICamera cameraNext, bool isSceneStartCamera = false)
        {
            Assert.IsNotNull(Manager.Instance);
            Manager.Instance.SwitchCamera(cameraNext, isSceneStartCamera);
        }
        #endregion
    }
}