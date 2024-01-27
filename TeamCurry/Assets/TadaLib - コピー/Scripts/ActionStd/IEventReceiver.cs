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
    /// イベント受信者
    /// </summary>
    public interface IEventReceiver
    {
        /// <summary>
        /// イベント受信時の処理
        /// </summary>
        void OnEventReceived();
    }
}