using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;
using Cysharp.Threading.Tasks;

namespace TadaLib.Camera
{
    /// <summary>
    /// CameraShaker
    /// </summary>
    public class CameraShaker
        : BaseManagerProc<CameraShaker>
    {
        #region プロパティ
        #endregion

        #region static メソッド
        #endregion

        #region メソッド
        public void ShakeStart(float strength = 1.0f)
        {
            _shakeStength = strength;
            if (_isShaking)
            {
                return;
            }
            _isShaking = true;
            Shake().Forget();
        }
        #endregion

        #region private フィールド
        [SerializeField]
        float _durationRate = 1.0f;

        float _shakeStength = 0.0f;
        bool _isShaking = false;
        #endregion

        #region private メソッド
        async UniTask Shake()
        {
            while (_shakeStength > 0.0f)
            {
                transform.position = new Vector3(Random.Range(-0.1f, 0.1f) * _shakeStength * 10.0f, 0.0f, -10.0f);
                await UniTask.Delay(System.TimeSpan.FromSeconds(0.03f));
                _shakeStength -= Time.deltaTime * _durationRate;
            }

            transform.position = new Vector3(0.0f, 0.0f, -10.0f);

            _isShaking = false;
        }
        #endregion
    }
}