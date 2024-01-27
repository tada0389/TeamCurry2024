using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// 簡単な移動制御処理
    /// </summary>
    [RequireComponent(typeof(MoveInfoCtrl))]
    public class SimpleMover : BaseProc, IProcMove, IProcPostMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            _fromPos = transform.position;
            _toPos = _fromPos + _moveDistance;
            UpdateIsTrigger();
        }
        #endregion

        #region TadaLib.ProcSystem.IProcMove の実装
        /// <summary>
        /// 移動更新処理
        /// </summary>
        public void OnMove()
        {
            if (!_isTriggered)
            {
                return;
            }

            _time += gameObject.DeltaTime();
            var rate = (1.0f -Mathf.Cos(_time * Mathf.PI / _moveNeedSec)) * 0.5f;
            _ratePeak = Mathf.Max(rate, _ratePeak);
            if (_isOneWay)
            {
                rate = _ratePeak;
            }
            transform.position = _fromPos + (_toPos - _fromPos) * rate;
        }
        #endregion

        #region TadaLib.ProcSystem.IProcPostMove の実装
        /// <summary>
        /// 移動更新処理
        /// </summary>
        public void OnPostMove()
        {
            UpdateIsTrigger();
        }
        #endregion

        #region Gizmos描画
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (_fromPos == _toPos)
            {
                Gizmos.DrawLine(transform.position, transform.position + _moveDistance);
            }
            else
            {
                Gizmos.DrawLine(_fromPos, _toPos);
            }
        }
#endif
        #endregion

        #region privateメソッド
        void UpdateIsTrigger()
        {
            // トリガー更新

            if (_isTriggered)
            {
                return;
            }

            switch (_triggerType)
            {
                case TriggerType.Always:
                    _isTriggered = true;
                    break;
                case TriggerType.OnRidden:
                    {
                        var moveInfoCtrl = GetComponent<MoveInfoCtrl>();
                        if (moveInfoCtrl.IsRidedByPlayer())
                        {
                            _isTriggered = true;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region privateフィールド
        enum TriggerType
        {
            Always,
            OnRidden,
        }

        [SerializeField]
        float _moveNeedSec = 3.0f;
        [SerializeField]
        Vector3 _moveDistance;
        [SerializeField]
        DG.Tweening.Ease _ease = DG.Tweening.Ease.Linear;
        [SerializeField]
        TriggerType _triggerType;
        [SerializeField]
        bool _isOneWay = false;

        Vector3 _fromPos = Vector3.zero;
        Vector3 _toPos = Vector3.zero;
        float _time = 0.0f;
        float _ratePeak = 0.0f;
        bool _isTriggered = false;
        #endregion
    }
}