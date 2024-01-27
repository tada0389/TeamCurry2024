using System;
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
    /// 処理
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SettingSo), menuName = nameof(SettingSo))]
    public class SettingSo : ScriptableObject
    {
        #region 定義
        [Serializable]
        struct BgmInfo
        {
            public int Bpm; // BPM
            public double TotalTimeSec; // 音楽時間
            public int TickCountPerMeasure; // 1小節に何拍あるか
        }
        #endregion

        #region プロパティ
        public double Bpm => _bgmInfo.Bpm;
        public int TickCountPerMeasure => _bgmInfo.TickCountPerMeasure;
        #endregion

        #region メソッド
        /// <summary>
        /// 経過時間からビート数を計算する
        /// </summary>
        /// <param name="timeSec"></param>
        /// <returns></returns>
        public int CalcBeatCount(int timeSec)
        {
            var bpm = _bgmInfo.Bpm;
            var sample = 44100 * 60 / bpm;
            var beatCount = (int)(timeSec / sample);
            return beatCount;
        }
        #endregion

        #region private フィールド
        [SerializeField]
        BgmInfo _bgmInfo;
        #endregion
    }
}