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
    public class Timer
    {
        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitTimeSec">制限時間</param>
        public Timer(float limitTimeSec)
        {
            _time = 0.0f;
            _limitTimeSec = limitTimeSec;
        }
        #endregion

        #region 関数
        public void Advance(float deltaTime)
        {
            _time += deltaTime;
        }
        public void TimeReset()
        {
            _time = 0.0f;
        }
        public void TimeReset(float newLimitTimeSec)
        {
            _time = 0.0f;
            _limitTimeSec = newLimitTimeSec;
        }
        public void ToLimitTime()
        {
            _time = _limitTimeSec;
        }
        #endregion

        #region プロパティ
        public float TimeSec => _time;
        public float TimeRate01 => _limitTimeSec == 0 ? 1.0f : Mathf.Clamp(TimeSec / _limitTimeSec, 0.0f, 1.0f);
        public bool IsTimout => TimeSec >= _limitTimeSec;
        #endregion

        #region privateフィールド
        float _time;
        float _limitTimeSec;
        #endregion
    }
}