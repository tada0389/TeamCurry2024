using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using KanKikuchi.AudioManager;

namespace Actor.Gimmick
{
    /// <summary>
    /// VaseCtrl
    /// </summary>
    public class VaseCtrl
        : BaseProc
        , IProcPostMove
    {
        #region 定義
        enum State
        {
            Idle,
            Sway,
            Dropping,
            Dropped,
        }
        #endregion

        #region プロパティ
        #endregion

        #region メソッド
        public void Drop()
        {
            if (_vaseStage != null)
            {
                _vaseStage.StageOutcome = StageOutcome.Lose;
            }

            if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f)
            {
                SEManager.Instance.Play(SEPath.VASE_BREAK_1);
            }
            else
            {
                SEManager.Instance.Play(SEPath.VASE_BREAK_2);
            }
        }
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            _state = State.Idle;
        }
        #endregion

        #region IProcPostMove の実装
        public void OnPostMove()
        {
            if (_state != State.Dropped && _state != State.Dropping)
            {
                if (_owner.Owner.CollResultProxy.IsCollide)
                {
                    // do drop
                    // TODO: Decide if you want to drop in a hit or not

                    _state = State.Dropping;

                    // animation
                    if (_state == State.Dropping)
                    {
                        GetComponent<SimpleAnimation>().Play("Dropping");
                        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f)
                        {
                            SEManager.Instance.Play(SEPath.VASE_HIT_HEAVY_1);
                        }
                        else
                        {
                            SEManager.Instance.Play(SEPath.VASE_HIT_HEAVY_2);
                        }
                    }
                    else if (_state == State.Sway)
                    {

                    }
                }
            }
        }
        #endregion

        #region privateフィールド
        State _state = State.Idle;
        [SerializeField]
        TadaLib.HitSystem.OwnerCtrl _owner;

        [SerializeField] private VaseStage _vaseStage;

        #endregion

        #region privateメソッド

        #endregion
    }
}