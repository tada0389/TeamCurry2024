using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.Util
{
    /// <summary>
    /// 時間計測
    /// </summary>
    public class TimerUnscaled
    {
        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitTimeSec">制限時間</param>
        public TimerUnscaled(float limitTimeSec)
        {
            _limitTimeSec = limitTimeSec;
            _startTime = Time.unscaledTime;
        }
        #endregion

        #region 関数
        public void TimeReset()
        {
            _startTime = Time.unscaledTime;
        }

        public void TimeReset(float limitTimeSec)
        {
            _limitTimeSec = limitTimeSec;
            _startTime = Time.unscaledTime;
        }
        #endregion

        #region プロパティ
        public float TimeSec => Time.unscaledTime - _startTime;
        public float TimeRate01 => _limitTimeSec == 0.0f ? 1.0f : Mathf.Clamp(TimeSec / _limitTimeSec, 0.0f, 1.0f);
        public bool IsTimout => TimeSec >= _limitTimeSec;


        #endregion

        #region privateフィールド
        float _startTime;
        float _limitTimeSec;
        #endregion
    }
}