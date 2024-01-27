using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using UnityEditor;

namespace TadaLib.Scene
{
    /// <summary>
    /// TransitionEffect
    /// @memo: ITransitionEffect のようにしたかったが、interface は Inspector で表示できない
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class TransitionEffect
        : MonoBehaviour
    {
        #region 定義
        enum EffectType
        {
            Material,
            RuleImage
        }
        #endregion

        #region プロパティ
        #endregion

        #region メソッド
        public void SetProgress(float progress01)
        {
            switch (_effectType)
            {
                case EffectType.Material:
                    {
                        _image.material.SetFloat("_Progress", progress01);
                    }
                    break;
                case EffectType.RuleImage:
                    {
                        _image.material.SetFloat("_Progress", progress01);
                        //_image.color = _image.color.SetAlpha(progress01);
                    }
                    break;
            }

            // 共通処理
            // 処理不可削減のために、0 のときは描画をオフにする
            _image.enabled = (progress01 > 1e-4);
        }

        public void ChangeMaterial(Material material)
        {
            if (_effectType != EffectType.Material)
            {
                Debug.LogWarning("[TransitionEffect] Materialモードではありません。");
            }
            _image.material = material;
        }

        public void ChangeRuleImage(Sprite ruleImage)
        {
            if (_effectType != EffectType.RuleImage)
            {
                Debug.LogWarning("[TransitionEffect] RuleImageモードではありません。");
            }
            _image.sprite = ruleImage;
        }

        public void ChangeColor(in Color color)
        {
            _image.material.SetColor("Color", color);
        }
        #endregion

        #region Monobehavior の実装
        void Start()
        {
            _image = GetComponent<UnityEngine.UI.Image>();
            _image.enabled = false;
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        EffectType _effectType = EffectType.RuleImage;

        UnityEngine.UI.Image _image;
        #endregion

        #region privateメソッド
        #endregion

#if UNITY_EDITOR
        //#region inspector 拡張
        //[CustomEditor(typeof(TransitionEffect))]
        //public class CustomInspector : Editor
        //{
        //    public override void OnInspectorGUI()
        //    {
        //        // 対象のインスペクターターゲットを取得
        //        TransitionEffect transitionEffect = target as TransitionEffect;

        //        // エフェクトタイプを表示
        //        transitionEffect._effectType = (EffectType)EditorGUILayout.EnumPopup("EffectType", transitionEffect._effectType);

        //        // エフェクトタイプがMaterialの場合にMaterialフィールドを表示
        //        if (transitionEffect._effectType == EffectType.Material)
        //        {
        //            transitionEffect._material = (Material)EditorGUILayout.ObjectField("Material", transitionEffect._material, typeof(Material), false);
        //        }
        //        // エフェクトタイプがRuleImageの場合にRuleImageフィールドを表示
        //        if (transitionEffect._effectType == EffectType.RuleImage)
        //        {
        //            transitionEffect._ruleImage = (Sprite)EditorGUILayout.ObjectField("RuleImage", transitionEffect._ruleImage, typeof(Sprite), false);
        //        }

        //        // インスペクターを更新
        //        serializedObject.ApplyModifiedProperties();
        //    }
        //}
        //#endregion
#endif
    }
}