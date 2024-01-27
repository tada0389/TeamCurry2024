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
    /// プールイベント時のアクション
    /// </summary>
    [System.Serializable]
    public class PoolAction<T> where T : MonoBehaviour
    {
        #region public メソッド
        public T OnGenerateDefault()
        {
            return UnityEngine.Object.Instantiate(_prefab, _parent);
        }

        public void OnRentDefault(T pool)
        {
            pool.gameObject.SetActive(true);
            if (_parent != null)
            {
                pool.transform.SetParent(_parent);
            }
        }

        public void OnReturnDefault(T pool)
        {
            if (_parent != null)
            {
                pool.transform.SetParent(_parent);
            }
            pool.gameObject.SetActive(false);
        }
        #endregion

        #region public フィールド
        /// <summary>
        /// 生成の処理
        /// 代入でカスタマイズ可能
        /// </summary>
        public Func<T> OnGenerate
        {
            get
            {
                if (_onGenerate is null)
                {
                    _onGenerate = OnGenerateDefault;
                }
                return _onGenerate;
            }
            set
            {
                _onGenerate = value;
            }
        }

        /// <summary>
        /// 貸し出しの処理
        /// 代入でカスタマイズ可能
        /// </summary>
        public Action<T> OnRent
        {
            get
            {
                if (_onRent is null)
                {
                    _onRent = OnRentDefault;
                }
                return _onRent;
            }
            set
            {
                _onRent = value;
            }
        }

        /// <summary>
        /// 返却の処理
        /// 代入でカスタマイズ可能
        /// </summary>
        public Action<T> OnReturn
        {
            get
            {
                if (_onReturn is null)
                {
                    _onReturn = OnReturnDefault;
                }
                return _onReturn;
            }
            set
            {
                _onReturn = value;
            }
        }

        /// <summary>
        /// プールの生成数
        /// 代入でカスタマイズ可能
        /// </summary>
        public int StartCount = 10;
        #endregion

        #region private フィールド
        [SerializeField]
        T _prefab;
        [SerializeField]
        Transform _parent;

        Func<T> _onGenerate = null;
        Action<T> _onRent = null;
        Action<T> _onReturn = null;
        #endregion
    }
}