using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;

namespace Actor.Player
{
    /// <summary>
    /// CatchChandelier
    /// </summary>
    public class CatchChandelier
        : BaseProc
        , IProcMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region IProcMove の実装
        public void OnMove()
        {
            var joyconOrientation = InputSystem.JoyconInput.Instance.GetJoyconVector();
            var effect = InputSystem.JoyconInput.Instance.IsLeftJoycon() ? -180.0f : 0.0f;
            var vel = Mathf.Clamp(-(joyconOrientation.y - 90.0f + effect) / 45.0f, -1.0f, 1.0f) * _swingPower;

            _angle = TadaLib.Util.InterpUtil.Linier(_angle, vel, 0.2f, Time.deltaTime);
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, _angle);
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        SpriteRenderer _body;

        [SerializeField]
        float _swingPower = 1.0f;

        float _angle = 0.0f;
        #endregion

        #region privateメソッド
        #endregion
    }
}