using TadaLib.ActionStd;
using UnityEngine;

namespace TadaLib.Extension
{
    /// <summary>
    /// UnityEngine.GameObject の拡張メソッド
    /// <see cref="GameObject"/>
    /// </summary>
    public static class ExGameObject
    {
        /// <summary>
        /// GameObject の DeltaTime を取得
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static float DeltaTime(this  GameObject obj)
        {
            var timeScale = 1.0f;
            if(obj.TryGetComponent<DeltaTimeCtrl>(out var comp))
            {
                timeScale *= comp.TimeScale;
            }
            return Time.deltaTime * timeScale;
        }
    }
}