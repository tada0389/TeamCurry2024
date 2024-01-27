using System;
using System.Collections;
using System.Collections.Generic;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace TadaLib.Pool
{
    /// <summary>
    /// プール管理オブジェクト
    /// </summary>
    public class ObjectPool<T> where T : MonoBehaviour
    {
        #region コンストラクタ
        public ObjectPool(PoolAction<T> action)
        {
            _action = action;
            for (int idx = 0; idx < action.StartCount; ++idx)
            {
                var pbj = action.OnGenerate();
                action.OnReturn(pbj);
                _objs.Enqueue(pbj);
            }
        }
        #endregion

        #region public メソッド
        public T Rent()
        {
            if(!_objs.TryDequeue(out var result))
            {
                result = _action.OnGenerate();
            }

            _action.OnRent(result);
            return result;
        }

        public void Return(T obj)
        {
            _action.OnReturn(obj);
            _objs.Enqueue(obj);
        }
        #endregion

        #region private フィールド
        readonly PoolAction<T> _action;
        readonly Queue<T> _objs = new Queue<T>();
        #endregion
    }
}