using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;
using System;

namespace TadaLib.BeatSystem
{
    /// <summary>
    /// ビート管理マネージャ
    /// </summary>
    public class Manager
        : BaseManagerProc<Manager>
        , IProcManagerUpdate
    {
        #region メソッド
        public float CalcBeatInterval()
        {
            return (float)(60.0 / _setting.Bpm);
        }
        #endregion

        #region プロパティ
        public IObservable<TimingInfo> OnBeat => _onBeatSubject;
        Subject<TimingInfo> _onBeatSubject = new Subject<TimingInfo>();

        public ReadOnlyReactiveProperty<int> BeatCountProperty => _beatCount.ToReadOnlyReactiveProperty();
        ReactiveProperty<int> _beatCount = new ReactiveProperty<int>(0);
        #endregion

        #region IManagerProc の実装
        public void OnUpdate()
        {
            //int curSamples = BgmManager.Instance.BgmSource.timeSamples;

            //var beatCountNext = _setting.CalcBeatCount(curSamples);
            //if(_beatCount.Value != beatCountNext)
            //{
            //    // 変わった
            //    _beatCount.Value = beatCountNext;
            //    var tickCountPerMeasure = _setting.TickCountPerMeasure;
            //    var info = new TimingInfo(_beatCount.Value % tickCountPerMeasure, tickCountPerMeasure);
            //    _onBeatSubject.OnNext(info);
            //}
        }
        #endregion

        #region private フィールド
        [SerializeField]
        SettingSo _setting;
        #endregion

        #region private メソッド
        #endregion
    }
}