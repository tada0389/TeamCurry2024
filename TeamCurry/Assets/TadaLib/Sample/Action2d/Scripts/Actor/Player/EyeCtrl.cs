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
    /// Component処理
    /// </summary>
    public class EyeCtrl
        : BaseProc
        , IProcPostMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region Monobehavior の実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        void Start()
        {
            _eyeScaleLocalLeft = _eyeLeft.transform.localScale;
            _eyeScaleLocalRight = _eyeRight.transform.localScale;
        }
        #endregion

        #region TadaLib.ProcSystem.IProcPostMove の実装
        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            // 目的スケールになるように調整する
            void AdjustScale(Transform eye, List<Transform> parents, Vector3 targetScale)
            {
                var scale = targetScale;
                foreach (var parent in parents)
                {
                    scale.x /= parent.transform.localScale.x;
                    scale.y /= parent.transform.localScale.y;
                    scale.z /= parent.transform.localScale.z;
                }
                eye.transform.localScale = scale;
            }

            AdjustScale(_eyeLeft, _parents, _eyeScaleLocalLeft);
            AdjustScale(_eyeRight, _parents, _eyeScaleLocalRight);

            var useSprite = (_dataHolder.IsCrouch || _dataHolder.IsDeadShrink) ? _crouchSprite : _normalSprite;
            _eyeLeft.GetComponent<SpriteRenderer>().sprite = useSprite;
            _eyeRight.GetComponent<SpriteRenderer>().sprite = useSprite;

            // 両目の間隔を合わせる
            var sign = Mathf.Sign(_dataHolder.FaceVec.x);
            _eyeLeft.position = _eyeRight.position + Vector3.left * (_eyeDistanceX * sign);
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        Transform _eyeLeft;
        [SerializeField]
        Transform _eyeRight;
        [SerializeField]
        List<Transform> _parents;

        [SerializeField]
        DataHolder _dataHolder;

        [SerializeField]
        Sprite _normalSprite;
        [SerializeField]
        Sprite _crouchSprite;

        [SerializeField]
        float _eyeDistanceX = 0.4f;

        Vector3 _eyeScaleLocalLeft;
        Vector3 _eyeScaleLocalRight;
        #endregion

        #region privateメソッド
        #endregion
    }
}