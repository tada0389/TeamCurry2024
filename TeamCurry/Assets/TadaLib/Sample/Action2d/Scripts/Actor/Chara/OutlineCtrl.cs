using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Actor.Chara
{
    /// <summary>
    /// Component処理
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class OutlineCtrl
        : BaseProc
        , IProcPostMove
    {
        #region プロパティ
        public ColorManager.ColorKind OverwriteColorKind { get; set; } = ColorManager.ColorKind.TERM;
        #endregion

        #region メソッド
        #endregion

        #region Monobehavior の実装
        void Start()
        {
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.loop = true;
        }
        #endregion

        #region TadaLib.ActorBase.IProcPostMove の実装
        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            if (_outlineCollider == null)
            {
                return;
            }

            var colorManager = ColorManager.Instance;

            Assert.IsTrue(colorManager != null);

            var colorKind = _colorKind;
            if (OverwriteColorKind != ColorManager.ColorKind.TERM)
            {
                colorKind = OverwriteColorKind;
            }
            var color = colorManager.GetColor(colorKind);

            // 親のスケールの逆数をかける
            if (_parent != null)
            {
                var scale = new Vector3(1.0f / _parent.localScale.x, 1.0f / _parent.localScale.y, 1.0f);
                if (_isScaleGlobal)
                {
                    scale = new Vector3(1.0f / _parent.lossyScale.x, 1.0f / _parent.lossyScale.y, 1.0f);
                }
                transform.localScale = scale;
            }

            var baseScale = (Vector2)_outlineCollider.transform.localScale;
            if (_scaleOffset != null)
            {
                if (_isScaleGlobal)
                {
                    baseScale.x *= _scaleOffset.lossyScale.x;
                    baseScale.y *= _scaleOffset.lossyScale.y;
                }
                else
                {
                    baseScale.x *= _scaleOffset.localScale.x;
                    baseScale.y *= _scaleOffset.localScale.y;
                }
            }
            baseScale.x = Mathf.Abs(baseScale.x);
            baseScale.y = Mathf.Abs(baseScale.y);

            var colliderSize = _outlineCollider.size * baseScale;
            var colliderOffset = _outlineCollider.offset * baseScale;
            var colliderPosition = colliderOffset + (Vector2)_outlineCollider.transform.position;
            if (_viewOffset != null)
            {
                colliderPosition += (Vector2)_viewOffset.localPosition;
            }
            var colliderAngle = _outlineCollider.transform.eulerAngles.z;

            Vector3[] positoins = new Vector3[4];
            positoins[0] = RotatePoint(colliderPosition + new Vector2(-colliderSize.x, -colliderSize.y) * 0.5f, colliderPosition, colliderAngle);
            positoins[1] = RotatePoint(colliderPosition + new Vector2(-colliderSize.x, colliderSize.y) * 0.5f, colliderPosition, colliderAngle);
            positoins[2] = RotatePoint(colliderPosition + new Vector2(colliderSize.x, colliderSize.y) * 0.5f, colliderPosition, colliderAngle);
            positoins[3] = RotatePoint(colliderPosition + new Vector2(colliderSize.x, -colliderSize.y) * 0.5f, colliderPosition, colliderAngle);

            var lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.positionCount = positoins.Length;
            lineRenderer.SetPositions(positoins);

            lineRenderer.startWidth = _lineWidth;
            lineRenderer.endWidth = _lineWidth;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        BoxCollider2D _outlineCollider;
        [SerializeField]
        Transform _scaleOffset;
        [SerializeField]
        Transform _viewOffset;
        [SerializeField]
        Transform _parent;
        [SerializeField]
        bool _isScaleGlobal = false;

        [SerializeField]
        float _lineWidth = 0.25f;

        [SerializeField]
        ColorManager.ColorKind _colorKind = ColorManager.ColorKind.PlayerOutline;
        #endregion

        #region privateメソッド
        // 回転補助
        Vector2 RotatePoint(in Vector2 point, in Vector2 pivot, float angle)
        {
            Vector2 dir = point - pivot;
            dir = Quaternion.Euler(0, 0, angle) * dir;
            return dir + pivot;
        }
        #endregion
    }
}