using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using UnityEngine.UIElements;

namespace Actor.Gimmick
{
    /// <summary>
    /// ChandelierCtrl
    /// </summary>
    public class ChandelierCtrl
        : BaseProc
        , IProcMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            _angle = _initialSwingDeg * Mathf.Deg2Rad;
            _angularVelocity = _initialAngularVelocity;
        }
        #endregion

        #region IProcMove の実装
        public void OnMove()
        {
            var gravity = 9.81f;

            var acceleration = -gravity / _length * (float)Mathf.Sin(_angle);

            _angularVelocity += acceleration * Time.deltaTime;
            _angle += _angularVelocity * Time.deltaTime;

            transform.localEulerAngles = new Vector3(0.0f, 0.0f, _angle * Mathf.Rad2Deg);
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        float _initialSwingDeg = 70.0f;
        [SerializeField]
        float _initialAngularVelocity = 0.0f;
        [SerializeField]
        float _length;

        float _angle;
        float _angularVelocity;
        #endregion

        #region privateメソッド
        #endregion
    }
}