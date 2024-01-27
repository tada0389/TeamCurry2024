using System.Collections;
using System.Collections.Generic;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace TadaLib.BeatSystem
{
    /// <summary>
    /// ビートに反応する人
    /// </summary>
    public interface IObserver
    {
        void OnBeat(in TimingInfo info);
    }
}