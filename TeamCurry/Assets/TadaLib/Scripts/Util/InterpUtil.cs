using UnityEngine;

namespace TadaLib.Util
{
    /// <summary>
    /// 補間ユーティリティ
    /// </summary>
    public static class InterpUtil
    {
        /// <summary>
        /// 想定された FPS
        /// deltaTime を考慮した補間計算に用いる
        /// </summary>
        static float AssumptFps = 60.0f;
        static float AssumptDeltaTime = 1.0f / AssumptFps;

        /// <summary>
        /// from → to までの補間
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Linier(float from, float to, float t)
        {
            return from + (to - from) * t;
        }

        /// <summary>
        /// from → to までの補間
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static float Linier(float from, float to, float t, float deltaTime)
        {
            var t2 = ConvertInterpRate(t, deltaTime);
            return Linier(from, to, t2);
        }

        /// <summary>
        /// org から変換
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromOrg"></param>
        /// <param name="toOrg"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Remap(float value, float fromOrg, float toOrg, float from, float to)
        {
            value = Mathf.Clamp(value, fromOrg, toOrg);
            var t = (value - fromOrg) / (toOrg - fromOrg);
            return Linier(from, to, t);
        }

        static float ConvertInterpRate(float t, float deltaTime)
        {
            return 1.0f - Mathf.Pow(1.0f - t, deltaTime / AssumptDeltaTime);
        }
    }
}
