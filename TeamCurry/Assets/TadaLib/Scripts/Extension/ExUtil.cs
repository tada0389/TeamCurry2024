using TadaLib.ProcSystem;
using UnityEngine;

namespace TadaLib.Extension
{
    /// <summary>
    /// 便利拡張メソッド
    /// </summary>
    public static class ExUtil
    {
        #region ログ出力
        public static T Log<T>(this T val)
        {
            Debug.Log(val);
            return val;
        }

        public static T LogW<T>(this T val)
        {
            Debug.LogWarning(val);
            return val;
        }

        public static T LogE<T>(this T val)
        {
            Debug.LogError(val);
            return val;
        }
        #endregion

        #region Color
        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
        #endregion
    }
}