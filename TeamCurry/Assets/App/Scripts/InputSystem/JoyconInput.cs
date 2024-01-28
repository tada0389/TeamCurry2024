using System;
using System.Collections;
using System.Collections.Generic;
using TadaLib.Extension;
using UnityEngine;
using UnityEngine.Assertions;

namespace InputSystem
{
    public enum ButtonCode
    {
        Jump,
        UpArrow,
        DownArrow,
        LeftArrow,
        RightArrow,
        Cancel
    }

    public enum AxisCode
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// JoyconInput
    /// </summary>
    public class JoyconInput : TadaLib.Util.SingletonMonoBehaviour<JoyconInput>
    {
        public static bool actionEnabled;

        public delegate bool Button(ButtonCode code);
        public delegate float Axis(AxisCode code);
        public delegate Vector3 JoyconSensor();
        public delegate float[] JoyconStick();
        public Button GetButtonDown { get; private set; }
        public Button GetButton { get; private set; }
        public Button GetButtonUp { get; private set; }

        public Axis GetAxis { get; private set; }
        public JoyconSensor GetJoyconVector { get; private set; }
        public JoyconSensor GetJoyconGyro { get; private set; }
        public JoyconSensor GetJoyconAccel { get; private set; }
        public JoyconStick GetJoyconStick { get; private set; }

        public bool IsAnyButtonDown
        {
            get
            {
                return GetButtonDown(ButtonCode.Jump) || GetButton(ButtonCode.Cancel) || GetButtonDown(ButtonCode.DownArrow) || GetButtonDown(ButtonCode.LeftArrow) || GetButtonDown(ButtonCode.RightArrow) || GetButtonDown(ButtonCode.UpArrow);
            }
        }            

        static float joyconAngle = 180;

        /// <summary>
        /// InputDeadzone
        /// </summary>
        public const float DeadZone = 0.5f;

        void Start()
        {
            actionEnabled = true;

            foreach (ButtonCode button in Enum.GetValues(typeof(ButtonCode)))
            {
                _buttonCur.Add(button, false);
                _buttonPrev.Add(button, false);
            }

            //Debug.Log("コントローラーにキーボードが設定されました");
            SetControllerKeyboard();
            SetSensorKeyboard();

            //Debug.Log("ActionInoput.Awake()");
            var joycons = JoyconManager.Instance.j;
            _stickJoycon = joycons.Find(c => c.isLeft);
            if (_stickJoycon == null)
            {
                _stickJoycon = joycons.Find(c => !c.isLeft);
                if (_stickJoycon != null)
                {
                    Debug.Log("コントローラーにジョイコンが設定されました");
                    SetControllerJoycon(_stickJoycon);
                }
            }
            else
            {
                SetControllerJoycon(_stickJoycon);
            }

            _gyroJoycon = joycons.Find(c => !c.isLeft);
            if (_gyroJoycon == null)
            {
                _gyroJoycon = joycons.Find(c => c.isLeft);
                if (_gyroJoycon != null)
                {
                    SetSensorJoycon(_gyroJoycon);
                }
            }
            else
            {
                SetSensorJoycon(_gyroJoycon);
            }

        }

        void Update()
        {
            foreach (Joycon joycon in JoyconManager.Instance.j)
            {
                if (joycon.GetButton(Joycon.Button.SL) || joycon.GetButton(Joycon.Button.SR) || joycon.GetButton(Joycon.Button.SHOULDER_1) || joycon.GetButton(Joycon.Button.SHOULDER_2))
                {
                    _stickJoycon = joycon;
                    SetControllerJoycon(_stickJoycon);

                    _gyroJoycon = joycon;
                    SetSensorJoycon(_gyroJoycon);
                }
            }

            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                SetControllerKeyboard();
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                SetSensorKeyboard();
            }

            //ジョイコンの補正
            if (_gyroJoycon != null)
            {
                if (_gyroJoycon.GetButtonDown(Joycon.Button.SHOULDER_2))
                {
                    _gyroJoycon.Recenter();
                }
            }

            //キーボード用
            if (Input.GetKey(KeyCode.D))
            {
                joyconAngle -= Time.deltaTime * 180;
            }
            if (Input.GetKey(KeyCode.A))
            {
                joyconAngle += Time.deltaTime * 180;
            }
        }

        protected virtual void LateUpdate()
        {
            //1f前のボタンを入れる
            foreach (ButtonCode button in Enum.GetValues(typeof(ButtonCode)))
            {
                _buttonPrev[button] = GetButton(button);
                //Debug.Log(button + GetButton(button).ToString());
            }

            if (_stickJoycon != null)
            {
                _beforeHorizontalValue = _stickJoycon.GetStick()[1];
                _beforeVerticalValue = _stickJoycon.GetStick()[0];
            }
        }

        public bool IsLeftJoycon()
        {
            if (_gyroJoycon != null)
            {
                return _gyroJoycon.isLeft;
            }
            else if (_stickJoycon != null)
            {
                return _stickJoycon.isLeft;
            }

            return false;
        }

        public void SetControllerKeyboard()
        {
            GetButtonDown = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return Input.GetKeyDown(KeyCode.Space);
                    case ButtonCode.UpArrow:
                        return Input.GetKeyDown(KeyCode.UpArrow);
                    case ButtonCode.DownArrow:
                        return Input.GetKeyDown(KeyCode.DownArrow);
                    case ButtonCode.LeftArrow:
                        return Input.GetKeyDown(KeyCode.LeftArrow);
                    case ButtonCode.RightArrow:
                        return Input.GetKeyDown(KeyCode.RightArrow);
                    case ButtonCode.Cancel:
                        return Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete);
                }
                return false;
            };

            GetButton = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return Input.GetKey(KeyCode.Space);
                    case ButtonCode.UpArrow:
                        return Input.GetKey(KeyCode.UpArrow);
                    case ButtonCode.DownArrow:
                        return Input.GetKey(KeyCode.DownArrow);
                    case ButtonCode.LeftArrow:
                        return Input.GetKey(KeyCode.LeftArrow);
                    case ButtonCode.RightArrow:
                        return Input.GetKey(KeyCode.RightArrow);
                    case ButtonCode.Cancel:
                        return Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Delete);
                }
                return false;
            };

            GetButtonUp = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return Input.GetKeyUp(KeyCode.Space);
                    case ButtonCode.UpArrow:
                        return Input.GetKeyUp(KeyCode.UpArrow);
                    case ButtonCode.DownArrow:
                        return Input.GetKeyUp(KeyCode.DownArrow);
                    case ButtonCode.LeftArrow:
                        return Input.GetKeyUp(KeyCode.LeftArrow);
                    case ButtonCode.RightArrow:
                        return Input.GetKeyUp(KeyCode.RightArrow);
                    case ButtonCode.Cancel:
                        return Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Delete);
                }
                return false;
            };

            GetAxis = (code) =>
            {
                if (!actionEnabled) return 0;
                switch (code)
                {
                    case AxisCode.Horizontal:
                        return -GetJoyconStick()[1];
                    //return Input.GetAxis("Horizontal");
                    case AxisCode.Vertical:
                        return GetJoyconStick()[0];
                        //return Input.GetAxis("Vertical");
                }
                return 0;
            };
        }

        public void SetSensorKeyboard()
        {
            if (GetJoyconVector != null)
            {
                joyconAngle = GetJoyconVector().y;
            }
            GetJoyconVector = () =>
            {
                return new Vector3(0, joyconAngle, 0);
            };

            GetJoyconGyro = () =>
            {
                if (Input.GetKey(KeyCode.D)) return new Vector3(0, 0, -5);
                if (Input.GetKey(KeyCode.A)) return new Vector3(0, 0, 5);
                return Vector3.zero;
            };

            GetJoyconAccel = () =>
            {
                return new Vector3(Input.GetAxis("Vertical") * 10, 1, 0);
            };

            GetJoyconStick = () =>
            {
                return new float[] { Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal") };
            };
        }

        public void SetControllerJoycon(Joycon joycon)
        {
            if (joycon.isLeft)
            {
                GetButtonDown = (code) =>
                {
                    if (!actionEnabled) return false;
                    switch (code)
                    {
                        case ButtonCode.Jump:
                            return joycon.GetButtonDown(Joycon.Button.DPAD_LEFT);
                        case ButtonCode.UpArrow:
                            if (_beforeVerticalValue <= 0)
                            {
                                return joycon.GetStick()[0] > 0;
                            }
                            return false;
                        case ButtonCode.DownArrow:
                            if (_beforeVerticalValue >= 0)
                            {
                                return joycon.GetStick()[0] < 0;
                            }
                            return false;
                        case ButtonCode.LeftArrow:
                            if (_beforeHorizontalValue <= 0)
                            {
                                return joycon.GetStick()[1] > 0;
                            }
                            return false;
                        case ButtonCode.RightArrow:
                            if (_beforeHorizontalValue >= 0)
                            {
                                return joycon.GetStick()[1] < 0;
                            }
                            return false;
                        case ButtonCode.Cancel:
                            return joycon.GetButtonDown(Joycon.Button.DPAD_DOWN);
                    }
                    return false;
                };

                GetButton = (code) =>
                {
                    if (!actionEnabled) return false;
                    switch (code)
                    {
                        case ButtonCode.Jump:
                            return joycon.GetButton(Joycon.Button.DPAD_LEFT);
                        case ButtonCode.UpArrow:
                            return joycon.GetStick()[0] > DeadZone;
                        case ButtonCode.DownArrow:
                            return joycon.GetStick()[0] < -DeadZone;
                        case ButtonCode.LeftArrow:
                            return joycon.GetStick()[1] > DeadZone;
                        case ButtonCode.RightArrow:
                            return joycon.GetStick()[1] < -DeadZone;
                        case ButtonCode.Cancel:
                            return joycon.GetButton(Joycon.Button.DPAD_DOWN);
                    }
                    return false;
                };

                GetButtonUp = (code) =>
                {
                    if (!actionEnabled) return false;
                    switch (code)
                    {
                        case ButtonCode.Jump:
                            return joycon.GetButtonUp(Joycon.Button.DPAD_LEFT);
                        case ButtonCode.UpArrow:
                            if (_beforeVerticalValue > 0)
                            {
                                return joycon.GetStick()[0] <= 0;
                            }
                            return false;
                        case ButtonCode.DownArrow:
                            if (_beforeVerticalValue < 0)
                            {
                                return joycon.GetStick()[0] >= 0;
                            }
                            return false;
                        case ButtonCode.LeftArrow:
                            if (_beforeHorizontalValue > 0)
                            {
                                return joycon.GetStick()[1] <= 0;
                            }
                            return false;
                        case ButtonCode.RightArrow:
                            if (_beforeHorizontalValue < 0)
                            {
                                return joycon.GetStick()[1] >= 0;
                            }
                            return false;
                        case ButtonCode.Cancel:
                            return joycon.GetButtonUp(Joycon.Button.DPAD_DOWN);
                    }
                    return false;
                };
            }
            else
            {
                GetButtonDown = (code) =>
                {
                    if (!actionEnabled) return false;
                    switch (code)
                    {
                        case ButtonCode.Jump:
                            return joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT);
                        case ButtonCode.UpArrow:
                            if (_beforeVerticalValue >= 0)
                            {
                                return joycon.GetStick()[0] < 0;
                            }
                            return false;
                        case ButtonCode.DownArrow:
                            if (_beforeVerticalValue <= 0)
                            {
                                return joycon.GetStick()[0] > 0;
                            }
                            return false;
                        case ButtonCode.LeftArrow:
                            if (_beforeHorizontalValue >= 0)
                            {
                                return joycon.GetStick()[1] < 0;
                            }
                            return false;
                        case ButtonCode.RightArrow:
                            if (_beforeHorizontalValue <= 0)
                            {
                                return joycon.GetStick()[1] > 0;
                            }
                            return false;
                        case ButtonCode.Cancel:
                            return joycon.GetButtonDown(Joycon.Button.DPAD_UP);
                    }
                    return false;
                };

                GetButton = (code) =>
                {
                    if (!actionEnabled) return false;
                    switch (code)
                    {
                        case ButtonCode.Jump:
                            return joycon.GetButton(Joycon.Button.DPAD_RIGHT);
                        case ButtonCode.UpArrow:
                            return joycon.GetStick()[0] < -DeadZone;
                        case ButtonCode.DownArrow:
                            return joycon.GetStick()[0] > DeadZone;
                        case ButtonCode.LeftArrow:
                            return joycon.GetStick()[1] < -DeadZone;
                        case ButtonCode.RightArrow:
                            return joycon.GetStick()[1] > DeadZone;
                        case ButtonCode.Cancel:
                            return joycon.GetButton(Joycon.Button.DPAD_UP);
                    }
                    return false;
                };

                GetButtonUp = (code) =>
                {
                    if (!actionEnabled) return false;
                    switch (code)
                    {
                        case ButtonCode.Jump:
                            return joycon.GetButtonUp(Joycon.Button.DPAD_RIGHT);
                        case ButtonCode.UpArrow:
                            if (_beforeVerticalValue < 0)
                            {
                                return joycon.GetStick()[0] >= 0;
                            }
                            return false;
                        case ButtonCode.DownArrow:
                            if (_beforeVerticalValue > 0)
                            {
                                return joycon.GetStick()[0] <= 0;
                            }
                            return false;
                        case ButtonCode.LeftArrow:
                            if (_beforeHorizontalValue < 0)
                            {
                                return joycon.GetStick()[1] >= 0;
                            }
                            return false;
                        case ButtonCode.RightArrow:
                            if (_beforeHorizontalValue > 0)
                            {
                                return joycon.GetStick()[1] <= 0;
                            }
                            return false;
                        case ButtonCode.Cancel:
                            return joycon.GetButtonUp(Joycon.Button.DPAD_UP);
                    }
                    return false;
                };
            }
        }

        public void SetSensorJoycon(Joycon joycon)
        {
            GetJoyconVector = () =>
            {
                return joycon.GetVector().eulerAngles;
            };

            GetJoyconGyro = joycon.GetGyro;

            GetJoyconAccel = joycon.GetAccel;

            GetJoyconStick = joycon.GetStick;
        }

        #region private フィールド
        Joycon _stickJoycon;
        Joycon _gyroJoycon;
        static float _beforeHorizontalValue;
        static float _beforeVerticalValue;
        Dictionary<ButtonCode, bool> _buttonCur = new Dictionary<ButtonCode, bool>();
        Dictionary<ButtonCode, bool> _buttonPrev = new Dictionary<ButtonCode, bool>();
        #endregion
    }
}