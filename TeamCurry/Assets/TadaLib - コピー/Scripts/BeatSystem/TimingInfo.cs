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
    /// 拍に関する情報
    /// </summary>
    public readonly struct TimingInfo
    {
        /// <summary>
        /// 何チック目か
        /// </summary>
        public readonly int CurrentTick;

        /// <summary>
        /// 1小節に何チックあるか
        /// </summary>
        public readonly int TickCountPerMeasure;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="currentTick">何チック目か</param>
        /// <param name="tickCountPerMeasure">1小節に何チックあるか</param>
        public TimingInfo(int currentTick, int tickCountPerMeasure)
        {
            CurrentTick = currentTick;
            TickCountPerMeasure = tickCountPerMeasure;
        }

        /// <summary>
        ///     現在のチックに<paramref name="offsetTick" />を加算したインスタンスを作成する。
        /// </summary>
        /// <param name="offsetTick">オフセットチック</param>
        /// <returns>作成したインスタンス</returns>
        public TimingInfo CloneWithOffset(int offsetTick)
        {
            return new TimingInfo(CurrentTick + offsetTick, TickCountPerMeasure);
        }

        /// <summary>
        ///     チックを上書きしたインスタンスを作成する
        /// </summary>
        /// <param name="tick">チック</param>
        /// <returns>作成したインスタンス</returns>
        public TimingInfo CloneWithOverridingTick(int tick)
        {
            return new TimingInfo(tick, TickCountPerMeasure);
        }

        /// <summary>
        ///     <paramref name="beat" />ビートのいずれか拍に合うかどうか
        /// </summary>
        /// <param name="beat">1小節に何拍あるか</param>
        /// <param name="offsetTick">オフセットチック</param>
        /// <returns>拍に合うかどうか</returns>
        public bool IsJustForAnyBeat(int beat, int offsetTick = 0)
        {
            return (CurrentTick + offsetTick) % (TickCountPerMeasure / beat) == 0;
        }

        /// <summary>
        ///     <paramref name="beat" />ビートで何拍目か返す
        /// </summary>
        /// <param name="beat">1小節に何拍あるか</param>
        /// <param name="offsetTick">オフセットチック</param>
        /// <returns>
        ///     拍に丁度合うときは何拍目か返す
        ///     拍に合わないときは-1を返す
        /// </returns>
        public int GetBeatCountBySpecificBeat(int beat, int offsetTick = 0)
        {
            if ((CurrentTick + offsetTick) % (TickCountPerMeasure / beat) != 0)
            {
                return -1;
            }

            return (CurrentTick + offsetTick) / (TickCountPerMeasure / beat);
        }

        /// <summary>
        ///     <paramref name="beat" />ビートの<paramref name="numerator" />拍目に合うかどうか
        /// </summary>
        /// <param name="numerator">特定の拍目</param>
        /// <param name="beat">1小節に何拍あるか</param>
        /// <param name="offsetTick">オフセットチック</param>
        /// <returns>拍に合うかどうか</returns>
        public bool IsJustForSpecificBeat(int numerator, int beat, int offsetTick = 0)
        {
            return (CurrentTick + offsetTick) % TickCountPerMeasure == numerator * TickCountPerMeasure / beat;
        }
    }
}