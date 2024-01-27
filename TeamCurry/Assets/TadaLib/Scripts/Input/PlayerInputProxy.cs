using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.Input
{
    /// <summary>
    /// プレイヤー入力の仲介者
    /// </summary>
    public class PlayerInputProxy
        : BaseProc
        , IInput
    {
        public bool ActionEnabled
        {
            get
            {
                return InputImpl.ActionEnabled;
            }
            set
            {
                InputImpl.ActionEnabled = value;
            }
        }

        public void ResetInput() => InputImpl.ResetInput();

        public bool GetButtonDown(ButtonCode code, float precedeSec)=> InputImpl.GetButtonDown(code, precedeSec);

        public bool GetButton(ButtonCode code, float precedeSec) => InputImpl.GetButton(code, precedeSec);

        public bool GetButtonUp(ButtonCode code, float precedeSec) => InputImpl.GetButtonUp(code, precedeSec);

        public void ForceFlagOnHistory(ButtonCode code) => InputImpl.ForceFlagOnHistory(code);

        public void ForceFlagOffHistory(ButtonCode code) => InputImpl.ForceFlagOffHistory(code);

        public float GetAxis(AxisCode code) => InputImpl.GetAxis(code);

        public IInput InputImpl => PlayerInputManager.Instance.GetInput(_number);

        [SerializeField]
        int _number = 0;
    }
}