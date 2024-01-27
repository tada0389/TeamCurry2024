using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using System.Text;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// DebugInfoCtrl
    /// </summary>
    public class DebugInfoCtrl
        : BaseProc
        , IProcPostMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        public override string ToString()
        {
            return CreateDebugStr();
        }
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
#if UNITY_EDITOR
            TadaLib.Dbg.DebugBoxManager.Display(this).SetSize(new Vector2(500.0f, 400.0f));
            if (_isUseDebugText)
            {
                TadaLib.Dbg.DebugTextManager.Display(() => CreateDebugStr(), 0);
            }
#endif
        }
        #endregion

        #region IProcPostMove の実装
        public void OnPostMove()
        {
            // ステートログの更新
            if (TryGetComponent<StateMachine>(out var stateMachine))
            {
                var stateCur = stateMachine.CurrentStateName;
                if (stateCur != _statePrev)
                {
                    _stateLog.Enqueue(stateCur);
                    while (_stateLog.Count > MaxStateLogCount)
                    {
                        _stateLog.Dequeue();
                    }
                    _statePrev = stateCur;
                    ++_stateNoForLog;
                }
            }
        }
        #endregion

        #region privateフィールド
        const int MaxStateLogCount = 6;

        [SerializeField]
        bool _isUseDebugText = false;
        Queue<string> _stateLog = new Queue<string>();
        string _statePrev = "";
        int _stateNoForLog = 0;
        #endregion

        #region privateメソッド
        string CreateDebugStr()
        {
            StringBuilder sb = new StringBuilder();

            if (TryGetComponent<StateMachine>(out var stateComp))
            {
                sb.AppendLine(stateComp.CurrentStateName);
            }

            if (TryGetComponent<MoveCtrl>(out var moveComp))
            {
                var vel = moveComp.Velocity;
                sb.AppendLine($"({vel.x:F2}, {vel.y:F2})");
            }

            if (TryGetComponent<TadaRigidbody2D>(out var rigidbody))
            {
                sb.AppendLine($"IsGround       : {rigidbody.IsGround}");
                sb.AppendLine($"IsTopCollide   : {rigidbody.IsTopCollide}");
                sb.AppendLine($"IsRightCollide : {rigidbody.IsRightCollide}");
                sb.AppendLine($"IsLeftCollide  : {rigidbody.IsLeftCollide}");
                sb.AppendLine($"IsRightNearCol : {rigidbody.IsRightNearCollide}");
                sb.AppendLine($"IsLeftNearCol  : {rigidbody.IsLeftNearCollide}");
            }

            if (_stateLog.Count > 0)
            {
                sb.AppendLine("StateLog: ");
                var no = _stateNoForLog - _stateLog.Count;
                foreach(var stateStr in _stateLog)
                {
                    sb.AppendLine($"{string.Format("{0, -3}", no++)}: {stateStr}");
                }
            }

            return sb.ToString();
        }
        #endregion
    }
}