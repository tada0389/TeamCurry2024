using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Actor.Player
{
    /// <summary>
    /// 足元エフェクト制御
    /// </summary>
    public class FootEffectCtrl : BaseProc, IProcPostMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region Monobehavior の実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        public void Start()
        {
            while (_isFootEffectStoppedList.Count < _isFootEffectStoppedList.Capacity)
            {
                _isFootEffectStoppedList.Add(false);
            }
        }
        #endregion

        #region TadaLib.ProcSystem.IProcPostMove の実装
        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            // 地形属性の取得
            _landAttribute = LandAttribute.Default; // 今はこれしかない

            for (int idx = 0; idx < (int)LandAttribute.TERM; ++idx)
            {
                if (idx >= _footEffects.Count)
                {
                    continue;
                }

                var effect = _footEffects[idx];

                if (effect == null)
                {
                    continue;
                }

                if (idx != (int)_landAttribute)
                {
                    // 属性が違うので無効にする
                    effect.Stop();
                    _isFootEffectStoppedList[idx] = true;
                    continue;
                }

                if (!GetComponent<TadaRigidbody2D>().IsGround)
                {
                    // 地面にいないので無効にする
                    effect.Stop();
                    _isFootEffectStoppedList[idx] = true;
                    continue;
                }

                // effect.isStoppedはstop呼び出しからの反映が遅いため、別変数を見る
                if (_isFootEffectStoppedList[idx])
                {
                    // 再生
                    effect.Play();
                    _isFootEffectStoppedList[idx] = false;
                }
            }
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        enum LandAttribute
        {
            Default,
            Ice,

            TERM,
        }

        [SerializeField, TadaLib.Attribute.EnumList(typeof(LandAttribute))]
        List<ParticleSystem> _footEffects;
        List<bool> _isFootEffectStoppedList = new List<bool>((int)LandAttribute.TERM);

        LandAttribute _landAttribute = LandAttribute.Default;
        #endregion
    }
}