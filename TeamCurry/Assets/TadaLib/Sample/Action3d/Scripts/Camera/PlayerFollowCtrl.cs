using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;
using UniRx;

namespace TadaLib.Sample.Action3d.Camera
{
    /// <summary>
    /// カメラのメイン処理
    /// </summary>
    public class PlayerFollowCtrl : MonoBehaviour
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region Monobehavior の実装
        void Start()
        {
            TadaLib.ActionStd.Camera.PlayerFollowCamera.SwitchCamera(_initSetting, isSceneStartCamera: true);

            var cameraManager = TadaLib.Camera.Manager.Instance;

            PlayerManager.Instance.GetMainPlayerAsync
                .Subscribe(playerObj => cameraManager.SetPlayerObj(playerObj));
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        [SerializeField]
        TadaLib.ActionStd.Camera.PlayerFllowCameraSetting _initSetting;
        #endregion
    }
}