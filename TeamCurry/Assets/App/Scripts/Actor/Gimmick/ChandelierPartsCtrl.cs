using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;

namespace Actor.Gimmick
{
    /// <summary>
    /// ChandelierPartsCtrl
    /// </summary>
    public class ChandelierPartsCtrl
        : BaseProc
        , IProcMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        void Start()
        {
            _random = Random.Range(0.85f, 1.15f);
        }
        #endregion

        #region IProcMove の実装
        public void OnMove()
        {
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, _chandelier.AngularVelocity * _random * _swingPower);
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        ChandelierCtrl _chandelier;

        [SerializeField]
        float _swingPower = 1.0f;
        float _random = 1.0f;
        #endregion

        #region privateメソッド
        #endregion
    }
}