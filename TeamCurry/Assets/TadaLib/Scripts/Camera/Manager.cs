using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using System;

namespace TadaLib.Camera
{
    /// <summary>
    /// CameraManager
    /// </summary>
    public class Manager
        : BaseManagerProc<Manager>
        , IProcManagerUpdate
    {
        #region プロパティ
        #endregion

        #region static メソッド
        #endregion

        #region メソッド
        public void SwitchCamera(ICamera cameraNext, bool isSceneStartCamera)
        {
            _camera = cameraNext;
            _isSceneStartCamera = isSceneStartCamera;
        }

        public void SetPlayerObj(GameObject playerObj)
        {
            _data.PlayerObj = playerObj;
        }

        public void SetEditCamera(UnityEngine.Camera camera)
        {
            _data.EditCamera = camera;
        }
        #endregion

        #region IManagerProcUpdate の実装
        public void OnUpdate()
        {
            // UpdateData の更新
            {
                // カメラ、プレイヤーが null の場合はデフォルトのものを使う
                _data.EditCamera ??= UnityEngine.Camera.main;
                _data.PlayerObj ??= PlayerManager.TryGetMainPlayer();
                _data.DeltaTime = gameObject.DeltaTime();
            }


            if (_isSceneStartCamera)
            {
                _isSceneStartCamera = false;
                _camera?.OnSceneStartInitialize(_data);
            }
            _camera?.Update(_data);
        }
        #endregion

        #region private フィールド
        ICamera _camera = null;
        UpdateData _data;
        bool _isSceneStartCamera = false;
        #endregion

        #region private メソッド
        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            _camera?.DrawDebug(_data);
#endif
        }
        #endregion
    }
}