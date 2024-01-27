using System.Collections;
using System.Collections.Generic;
using TadaLib.ProcSystem;
using TadaLib.HitSystem;
using TadaLib.Extension;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace TadaLib.BeatSystem
{
    /// <summary>
    /// ビートに反応する人を制御する
    /// IObserver を継承したコンポーネントは、このコンポーネントも付与する想定
    /// </summary>
    public class ObserverCtrl
        : MonoBehaviour
    {
        #region MonoBehavior の実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        void Start()
        {
            var observers = GetComponents<IObserver>();
            // 登録
            foreach (var observer in observers)
            {
                _subject.Subscribe(x => observer.OnBeat(x));
            }

            // マネージャーへ登録
            Manager.Instance.OnBeat
                .Where(_ => gameObject.activeSelf)
                .Subscribe(_subject)
                .AddTo(gameObject);
        }
        #endregion

        #region private フィールド
        Subject<TimingInfo> _subject= new Subject<TimingInfo>();
        #endregion
    }
}