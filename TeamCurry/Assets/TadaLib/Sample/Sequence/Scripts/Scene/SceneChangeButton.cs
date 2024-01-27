using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;

namespace TadaLib.Sample.Sequence.Scene
{
    /// <summary>
    /// SceneChangeButton
    /// </summary>
    public class SceneChangeButton
        : MonoBehaviour
    {
        #region プロパティ
        #endregion

        #region メソッド
        public void ChangeScene(string nextScene)
        {
            TadaLib.Scene.TransitionManager.Instance.StartTransition(nextScene, 0.75f, 0.75f);
        }
        #endregion
    }
}