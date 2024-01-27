using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.Dbg
{
    /// <summary>
    /// FPS計測
    /// </summary>
    public class FpsCounter : MonoBehaviour
    {
        #region MonoBehaviorの実装
        void Start()
        {
            DebugTextManager.Display(() => $"FPS: {_fps:F2}({Time.unscaledDeltaTime * 1000:00}ms)\n", 0);
        }

        void Update()
        {
            if (Time.unscaledDeltaTime == 0.0f)
            {
                return;
            }

            _timeRemain -= Time.unscaledDeltaTime;
            _accum += 1.0f / Time.unscaledDeltaTime;
            ++_frames;

            if (_timeRemain > 0.0f)
            {
                return;
            }

            // FPSの更新と初期化
            _fps = _accum / _frames;
            _timeRemain = _updateInterval;
            _accum = 0.0f;
            _frames = 0.0f;
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        float _updateInterval = 0.5f;
        float _accum;
        float _frames;
        float _timeRemain;
        float _fps;
        #endregion
    }
}