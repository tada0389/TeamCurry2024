using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// アクターに関するUtilクラス
    /// </summary>
    public static class ActorUtil
    {
        // アニメーションを再生する
        public static void TryToPlayAnim(GameObject obj, string animName)
        {
            if (obj.TryGetComponent<SimpleAnimation>(out var animator))
            {
                if (animator.GetState(animName) != null)
                {
                    animator.Play(animName);
                }
            }
        }

        public static bool IsAnimEnd(GameObject obj)
        {
            if(obj.TryGetComponent<SimpleAnimation>(out var animator))
            {
                return !animator.isPlaying;
            }
            return true;
        }

        /// <summary>
        /// オブジェクトの中心座標を取得する
        /// コリジョン情報を持っているときのみ有効
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Vector3 GetCenterPosIfHasCollision(GameObject obj)
        {
            // @todo: BoxCollider2D以外も対応する
            if (obj.TryGetComponent<BoxCollider2D>(out var hitBox))
            {
                var scale = obj.transform.localScale;
                var offset = hitBox.offset * scale;
                return obj.transform.position + (Vector3)offset;
            }

            return obj.transform.position;
        }
    }
}