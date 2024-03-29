﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using InputSystem;

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
            var vel = Mathf.Clamp(-(joyconOrientation.y - 90.0f + effect) / 45.0f, -1.0f, 1.0f);

            var erp = 0.5f;
            if (JoyconInput.Instance.IsKeybord)
            {
                var axisX = JoyconInput.Instance.GetAxis(AxisCode.Horizontal);
                vel = axisX * 0.5f;
                erp = 0.2f;
            }

            _angle = TadaLib.Util.InterpUtil.Linier(_angle, vel * _swingPower, erp, Time.deltaTime);
            _moveX = TadaLib.Util.InterpUtil.Linier(_moveX, vel * _movePower, erp, Time.deltaTime);
            transform.localPosition = new Vector3(vel, transform.localPosition.y, transform.localPosition.z);
            transform.localEulerAngles = new Vector3(_movePower, 0.0f, _angle);
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        SpriteRenderer _body;

        [SerializeField]
        float _swingPower = 1.0f;

        [SerializeField]
        float _movePower = 1.0f;

        float _angle = 0.0f;
        float _moveX = 0.0f;
        #endregion

        #region privateメソッド
        #endregion
    }
}