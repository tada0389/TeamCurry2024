using UnityEngine;

namespace TadaLib.Util
{
    public static class LayerUtil
    {
        /// <summary>
        /// レイヤーマスクからレイヤーの値を取得する
        /// ex) LandCollision -> 9 (LandCollision = 1 << 9)
        ///     ThrougLandCollision -> 10 (ThrougLandCollision = 1 << 10)
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static int LayerMaskToLayer(Sys.LayerMask mask)
        {
            return Mathf.RoundToInt(Mathf.Log((float)mask, 2.0f));
        }
    }
}
