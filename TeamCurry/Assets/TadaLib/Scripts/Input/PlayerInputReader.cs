using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TadaLib.Input
{
    /// <summary>
    /// Component処理
    /// </summary>
    public class PlayerInputReader
        : ProcSystem.BaseProc
        , ProcSystem.IProcUpdate
        , IInput
    {
        #region プロパティ
        public bool ActionEnabled { get; set; } = true;
        #endregion

        #region メソッド
        #endregion

        #region TadaLib.Input.IInputの実装
        // 入力状態をリセットする
        public void ResetInput()
        {
            foreach (ButtonCode code in System.Enum.GetValues(typeof(ButtonCode)))
            {
                _buttonDict[code].Clear();
                _buttonDict[code].AddFirst(new ButtonData(false, Time.unscaledTime));
            }

            foreach (AxisCode code in System.Enum.GetValues(typeof(AxisCode)))
            {
                _axisDict[code] = 0.0f;
            }
        }

        /// <summary>
        /// 指定したボタンが入力されたかを取得する
        /// </summary>
        /// <param name="code">ボタン</param>
        /// <returns></returns>
        public bool GetButtonDown(ButtonCode code, float precedeSec = 0.0f)
        {
            // 先行入力を考慮する
            var buff = _buttonDict[code];
            var prev = buff.First.Value.IsPushed;
            foreach (var data in buff)
            {
                if (Time.unscaledTime - data.InputTime > precedeSec)
                {
                    if (prev && !data.IsPushed)
                    {
                        return true;
                    }
                    break;
                }

                if (prev && !data.IsPushed)
                {
                    return true;
                }

                prev = data.IsPushed;
            }

            return false;
        }

        /// <summary>
        /// 指定したボタンが入力されているかを取得する
        /// </summary>
        /// <param name="code">ボタン</param>
        /// <returns></returns>
        public bool GetButton(ButtonCode code, float precedeSec = 0.0f)
        {
            // 先行入力を考慮する
            var buff = _buttonDict[code];

            if (buff.First.Value.IsPushed)
            {
                return true;
            }

            foreach (var data in buff)
            {
                if (Time.unscaledTime - data.InputTime > precedeSec)
                {
                    break;
                }

                if (data.IsPushed)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 指定したボタンの入力が離されたかを取得する
        /// </summary>
        /// <param name="code">ボタン</param>
        /// <returns></returns>
        public bool GetButtonUp(ButtonCode code, float precedeSec = 0.0f)
        {
            // 先行入力を考慮する
            var buff = _buttonDict[code];
            var prev = buff.First.Value.IsPushed;
            foreach (var data in buff)
            {
                if (Time.unscaledTime - data.InputTime > precedeSec)
                {
                    if (!prev && data.IsPushed)
                    {
                        return true;
                    }
                    break;
                }

                if (!prev && data.IsPushed)
                {
                    return true;
                }

                prev = data.IsPushed;
            }

            return false;
        }

        /// <summary>
        /// 過去の入力フラグを全て立てる
        /// </summary>
        /// <param name="code"></param>
        public void ForceFlagOnHistory(ButtonCode code)
        {
            var buff = _buttonDict[code];

            foreach (var node in buff)
            {
                node.ForceSetIsPushed(true);
            }
        }
        /// <summary>
        /// 過去の入力フラグを全て降ろす
        /// </summary>
        /// <param name="code"></param>
        public void ForceFlagOffHistory(ButtonCode code)
        {
            var buff = _buttonDict[code];
            foreach (var node in buff)
            {
                node.ForceSetIsPushed(false);
            }
        }

        /// <summary>
        /// 指定したボタンが入力されているかを取得する
        /// </summary>
        /// <param name="code">ボタン</param>
        /// <returns></returns>
        public float GetAxis(AxisCode code)
        {
            if (_axisDict == null || _axisDict.Count == 0)
            {
                // エラー対策
                return 0.0f;
            }
            return _axisDict[code];
        }
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            //_inputSystemInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();

            // 初期化
            foreach (ButtonCode code in System.Enum.GetValues(typeof(ButtonCode)))
            {
                _buttonDict.Add(code, new LinkedList<ButtonData>());
                _buttonDict[code].AddFirst(new ButtonData(false, Time.unscaledTime));
            }

            foreach (AxisCode code in System.Enum.GetValues(typeof(AxisCode)))
            {
                _axisDict.Add(code, 0.0f);
            }
        }
        #endregion

        #region TadaLib.ProcSystem.IProcUpdate の実装
        /// <summary>
        /// 移動前の更新処理
        /// </summary>
        public void OnUpdate()
        {
            if (!ActionEnabled)
            {
                ResetInput();

                return;
            }

            //    foreach (ButtonCode code in System.Enum.GetValues(typeof(ButtonCode)))
            //    {
            //        var list = _buttonDict[code];

            //        list.AddFirst(new ButtonData(_inputSystemInput.actions[code.ToString()].IsPressed(), Time.unscaledTime));

            //        while (list.Count >= 2)
            //        {
            //            var duration = Time.unscaledTime - list.Last.Value.InputTime;
            //            if (duration < MaxBuffSec)
            //            {
            //                break;
            //            }
            //            list.RemoveLast();
            //        }
            //    }


            //    var moveVec2 = _inputSystemInput.actions["Move"].ReadValue<Vector2>();
            //    _axisDict[AxisCode.Horizontal] = AdjustAxis(moveVec2.x);
            //    _axisDict[AxisCode.Vertical] = AdjustAxis(moveVec2.y);
        }
        #endregion

        #region privateメソッド
        float AdjustAxis(float rawAxis)
        {
            var deadZone = 0.1f;
            if (Mathf.Abs(rawAxis) <= deadZone)
            {
                return 0.0f;
            }

            // [deadZone, 1.0] → [0.0, 1.0] に補正
            var ret = Mathf.Sign(rawAxis) * (Mathf.Abs(rawAxis) - deadZone) / (1.0f - deadZone);

            // 斜め上にスティックを倒しても(1, 1)にできるよう補正
            ret = Mathf.Clamp(ret * _root2, -1.0f, 1.0f);

            return ret;
        }
        #endregion

        #region privateフィールド
        class ButtonData
        {
            public bool IsPushed { get; private set; }
            public float InputTime { get; }

            public ButtonData(bool isPushed, float inputTime)
            {
                IsPushed = isPushed;
                InputTime = inputTime;
            }

            public void ForceSetIsPushed(bool isPushed)
            {
                IsPushed = isPushed;
            }
        }

        const float MaxBuffSec = 0.5f;
        //UnityEngine.InputSystem.PlayerInput _inputSystemInput = null;
        Dictionary<ButtonCode, LinkedList<ButtonData>> _buttonDict = new Dictionary<ButtonCode, LinkedList<ButtonData>>();
        Dictionary<AxisCode, float> _axisDict = new Dictionary<AxisCode, float>();

        static readonly float _root2 = Mathf.Sqrt(2.0f);
        #endregion
    }
}