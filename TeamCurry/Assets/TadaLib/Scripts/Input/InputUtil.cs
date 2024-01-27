using System.Collections;
using System.Collections.Generic;
using TadaLib.ActionStd;
using TadaLib.ProcSystem;
using UnityEngine;

namespace TadaLib.Input
{
    static class InputUtil
    {
        #region public static関数
        public static bool IsButton(GameObject obj, ButtonCode code, float precedeSec = 0.0f)
        {
            return TryGetInput(obj)?.GetButton(code, precedeSec) ?? false;
        }

        public static bool IsButtonDown(GameObject obj, ButtonCode code, float precedeSec = 0.0f)
        {
            return TryGetInput(obj)?.GetButtonDown(code, precedeSec) ?? false;
        }

        public static bool IsButtonUp(GameObject obj, ButtonCode code, float precedeSec = 0.0f)
        {
            return TryGetInput(obj)?.GetButtonUp(code, precedeSec) ?? false;
        }

        public static void ForceFlagOnHistory(GameObject obj, ButtonCode code)
        {
            TryGetInput(obj)?.ForceFlagOnHistory(code);
        }

        public static void ForceFlagOffHistory(GameObject obj, ButtonCode code)
        {
            TryGetInput(obj)?.ForceFlagOffHistory(code);
        }

        public static float GetAxis(GameObject obj, AxisCode code)
        {
            return TryGetInput(obj)?.GetAxis(code) ?? 0.0f;
        }

        public static Vector2 GetAxis(GameObject obj)
        {
            var input = TryGetInput(obj);

            if (input == null)
            {
                return Vector2.zero;
            }

            return new Vector2(input.GetAxis(AxisCode.Horizontal), input.GetAxis(AxisCode.Vertical));
        }

        /// <summary>
        /// 入力機能をすべて無効化する
        /// </summary>
        public static void DisablePlayerInput()
        {
            SetInputEnable(false);
        }

        /// <summary>
        /// 入力機能をすべて有効化する
        /// </summary>
        public static void EnablePlayerInput()
        {
            SetInputEnable(true);
        }
        #endregion

        #region private static関数
        static IInput TryGetInput(GameObject obj)
        {
            var inputs = obj.GetComponents<IInput>();

            // 一つしか持たない想定
            UnityEngine.Assertions.Assert.IsTrue(inputs.Length <= 1);

            return inputs.Length == 0 ? null : inputs[0];
        }

        static void SetInputEnable(bool isEnable)
        {
            // プレイヤーを探して有効無効を切り替える
            var player = PlayerManager.TryGetMainPlayer();
            if (player != null)
            {
                player.GetComponent<IInput>().ActionEnabled = isEnable;
            }
        }
        #endregion
    }
}
