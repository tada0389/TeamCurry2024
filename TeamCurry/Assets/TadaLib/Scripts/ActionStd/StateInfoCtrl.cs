using System.Collections;
using System.Collections.Generic;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// ステート情報。
    /// この数が多くてもそれほど重くならないので、ゲームで使えそうな kind は一通り用意する。
    /// (ゲームごとに StateInfoCtrl を作らないで済むように)
    /// </summary>
    [System.Flags]
    public enum StateInfoKind
    {
        IsDead = 0x001,
        IsWall = 0x002,
        IsIdle = 0x004,
        IsRun = 0x008,
        /// <summary>
        /// 拘束されている
        /// </summary>
        IsBinded = 0x010,
    }

    /// <summary>
    /// ステートパラメータの管理
    /// </summary>
    public class StateInfoCtrl
    {
        #region メソッド
        /// <summary>
        /// フラグ操作
        /// </summary>
        public void SetFlag(StateInfoKind kind, bool isEnable)
        {
            if (isEnable)
            {
                _bitFlag |= kind;
            }
            else
            {
                _bitFlag &= ~kind;
            }
        }

        /// <summary>
        /// フラグが立っているか
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public bool GetFlag(StateInfoKind kind)
        {
            return _bitFlag.HasFlag(kind);
        }

        /// <summary>
        /// ステート切り替え時の処理
        /// </summary>
        public void OnStateChanged()
        {
            ResetFlags();
        }
        #endregion

        #region private フィールド
        StateInfoKind _bitFlag = 0;
        #endregion

        #region private メソッド
        void ResetFlags()
        {
            _bitFlag = 0;
        }
        #endregion
    }
}