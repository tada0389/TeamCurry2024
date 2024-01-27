using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// ヒットストップ処理
    /// </summary>
    public class HitStopCtrl : BaseProc, IProcUpdate
    {
        #region プロパティ
        #endregion

        #region メソッド
        public void StartStop(float durationSec, float timeScale = 0.0f)
        {
            _timer = new Util.TimerUnscaled(durationSec);
            _timeScale = timeScale;
            _isStopping = true;
        }
        #endregion

        #region TadaLib.ProcSystem.IProcUpdate の実装
        /// <summary>
        /// 移動前の更新処理
        /// </summary>
        public void OnUpdate()
        {
            if (!_isStopping)
            {
                return;
            }

            var deltaTimeCtrl = GetComponent<DeltaTimeCtrl>();
            if (_timer.IsTimout)
            {
                // 終了
                _isStopping = false;
                deltaTimeCtrl.TimeScale = 1.0f;
            }
            else
            {
                deltaTimeCtrl.TimeScale = Mathf.Min(deltaTimeCtrl.TimeScale, _timeScale);
            }
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        Util.TimerUnscaled _timer = null;
        float _timeScale = 1.0f;
        bool _isStopping = false;
        #endregion
    }
}