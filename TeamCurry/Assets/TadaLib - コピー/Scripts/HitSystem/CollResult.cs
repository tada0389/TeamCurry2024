using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.HitSystem
{
    /// <summary>
    /// 衝突結果
    /// </summary>
    public struct CollResult
    {
        #region static関数
        public static CollResult Create(GameObject opponentObj, int tag)
        {
            return new CollResult()
            {
                OpponentObj = opponentObj,
                Tag = tag,
            };
        }
        #endregion

        #region フィールド
        public GameObject OpponentObj;
        public int Tag;
        #endregion
    }
}