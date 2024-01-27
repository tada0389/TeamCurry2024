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
    /// 衝突結果の仲介者
    /// </summary>
    public class CollResultProxy
    {
        #region コンストラクタ
        public CollResultProxy()
        {
        }
        #endregion

        #region メソッド
        public void Clear()
        {
            _result.Clear();
        }

        public void AddResult(in CollResult result)
        {
            _result.Add(result);
        }

        [System.Obsolete("Resuls関数使ってね")]
        public CollResult Result(int idx)
        {
            Assert.IsTrue(idx >= 0);
            Assert.IsTrue(idx < ResultCount);

            return _result[idx];
        }

        public IReadOnlyCollection<CollResult> Results()
        {
            return _result.AsReadOnly();
        }
        #endregion

        #region プロパティ
        public int ResultCount => _result.Count;
        public bool IsCollide => ResultCount != 0;
        #endregion

        #region private フィールド
        List<CollResult> _result = new List<CollResult>();
        #endregion

        #region private メソッド
        #endregion
    }
}