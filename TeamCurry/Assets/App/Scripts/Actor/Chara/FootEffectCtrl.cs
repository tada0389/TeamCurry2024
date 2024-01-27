using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using KanKikuchi.AudioManager;

namespace App.Actor.Chara
{
    /// <summary>
    /// FootEffectCtrl
    /// </summary>
    public class FootEffectCtrl
        : BaseProc
        , IProcPostMove
    {
        #region プロパティ
        public bool IsEnableState { get; set; } = false;
        #endregion

        #region メソッド
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            _seDist = _soundEffectInvervalPerDist;
            _veDist = _visualEffectInvervalPerDist;

            // ステート開始時に初期化させる
            var stateMachine = GetComponent<StateMachine>();
            stateMachine.AddStateStartCallback(() =>
            {
                IsEnableState = false;
            });

            _posPrev = transform.position;
        }
        #endregion

        #region IProcPostMove の実装
        public void OnPostMove()
        {
            //if (!IsEnableState)
            //{
            //    _posPrev = transform.position;
            //    return;
            //}

            //var curPos = transform.position;
            //var dist = (curPos - _posPrev).magnitude;

            //_seDist -= dist;
            //_veDist -= dist;

            //if (_seDist < 0.0f)
            //{
            //    _seDist += _soundEffectInvervalPerDist;

            //    SEManager.Instance.Play(SEPath.A02_HUMAN_RUN, volumeRate: 0.3f);
            //}

            //if (_veDist < 0.0f)
            //{
            //    _veDist += _visualEffectInvervalPerDist;

            //    var sandSmoke = SandEffectProvider.Instance.Rent();
            //    sandSmoke.transform.position = transform.position - Vector3.up * 2.0f;
            //    sandSmoke.Init((_posPrev - curPos).normalized);
            //}

            //_posPrev = curPos;
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        float _soundEffectInvervalPerDist = 1.0f;

        [SerializeField]
        float _visualEffectInvervalPerDist = 1.0f;

        float _seDist;
        float _veDist;

        Vector3 _posPrev;
        #endregion

        #region privateメソッド
        #endregion
    }
}