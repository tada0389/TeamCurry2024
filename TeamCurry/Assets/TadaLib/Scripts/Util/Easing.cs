using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.Util
{
    /// <summary>
    /// イージング関数群
    /// 参考：https://easings.net/ja
    /// </summary>
    public static class Easing
    {
        #region public static 関数
        public static float InOutBack(float x)
        {
            var c1 = 1.70158f;
            var c2 = c1 * 1.525f;

            return x < 0.5f ?
                (Mathf.Pow(2.0f * x, 2.0f) * ((c2 + 1.0f) * 2.0f * x - c2)) * 0.5f :
                (Mathf.Pow(2.0f * x - 2.0f, 2.0f) * ((c2 + 1) * (x * 2.0f - 2.0f) + c2) + 2.0f) * 0.5f;
        }

        public static float Outback(float x)
        {
            var c1 = 1.70158f;
            var c2 = c1 + 1.0f;

            return 1.0f + c2 * Mathf.Pow(x - 1.0f, 3.0f) + c1 * Mathf.Pow(x - 1.0f, 2.0f);
        }
        #endregion
    }
}