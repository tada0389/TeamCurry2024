using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// 残像処理
    /// </summary>
    public class AfterImageCtrl
        : BaseProc
        , IProcPostMove
    {
        #region プロパティ
        public bool IsActiveState { get; set; } = true;
        #endregion

        #region メソッド
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            GetComponent<StateMachine>()?.AddStateStartCallback(() =>
            {
                IsActiveState = false;
                _timer = 0.0f;
            });
        }
        #endregion

        #region TadaLib.ProcSystem.IProcPostMove の実装
        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            var deltaTime = gameObject.DeltaTime();

            // クリーンアップは非アクティブの時も行う
            for (int idx = _imageObjects.Count - 1; idx >= 0; idx--)
            {
                var obj = _imageObjects[idx];
                if (obj.AdvanceTrigger(deltaTime))
                {
                    // 消去
                    obj.Dispose();
                    _imageObjects.RemoveAt(idx);
                }
            }

            if (!IsActiveState)
            {
                return;
            }

            _timer += deltaTime;


            if (_timer > _imageInterval)
            {
                while (_timer > _imageInterval)
                {
                    // 同フレームに複数体生成しても無駄なので、時間消費にだけ while 使う
                    _timer -= _imageInterval;
                }

                _imageObjects.Add(CreateImage());
            }
        }
        #endregion

        #region privateフィールド
        class ImageObject
        {
            public ImageObject(Transform imageTarget, float lifetime, in Color color)
            {
                _color = color;
                _lifetimeDefault = lifetime;
                _lifetime = _lifetimeDefault;

                _obj = CreateObj(imageTarget);
            }

            public void Init()
            {

            }

            public void Dispose()
            {
                Destroy(_obj);
            }


            public bool AdvanceTrigger(float deltaTime)
            {
                _lifetime -= deltaTime;
                if (_lifetime <= 0.0f)
                {
                    return true;
                }

                // 寿命に応じて色を変える
                var sprites = _obj.GetComponentsInChildren<SpriteRenderer>();
                foreach (var sprite in sprites)
                {
                    var color = _color;
                    color.a = LifeTimeRate01;
                    sprite.color = color;
                }

                return false;
            }

            float LifeTimeRate01 => _lifetime / _lifetimeDefault;

            GameObject CreateObj(Transform imageTarget)
            {
                // @todo: オブジェクトだけ最初に作って、座標反映だけさせる

                var obj = new GameObject("AfterImage");
                //obj.transform.localPosition = imageTarget.position;
                //obj.transform.localRotation = imageTarget.rotation;
                //obj.transform.localScale = imageTarget.lossyScale;

                // 子供にある SpriteRenderer をすべて持つ
                var sprites = imageTarget.GetComponentsInChildren<SpriteRenderer>();
                foreach (var sprite in sprites)
                {
                    var childObj = new GameObject(sprite.name, typeof(SpriteRenderer));

                    childObj.transform.parent = obj.transform;

                    //// 2DAnimation を使っているため、ボーン座標も考慮する必要がある
                    //var boneTransform = sprite.GetComponent<UnityEngine.U2D.Animation.SpriteSkin>().rootBone;
                    //childObj.transform.position = boneTransform.position;
                    //childObj.transform.rotation = boneTransform.rotation;
                    //childObj.transform.localScale = boneTransform.lossyScale;

                    childObj.transform.localPosition = sprite.transform.localPosition;
                    childObj.transform.localRotation = sprite.transform.localRotation;
                    childObj.transform.localScale = sprite.transform.localScale;

                    // スプライトをコピー
                    var spriteRenderer = childObj.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprite.sprite;
                    spriteRenderer.color = _color;
                    // @todo: マテリアルを変える (カラースケールを入れたもの)
                    spriteRenderer.material = spriteRenderer.material;

                }

                return obj;
            }

            void ReflectTransform()
            {

            }

            GameObject _obj;
            Color _color;
            float _lifetime;
            float _lifetimeDefault;
        }

        [SerializeField]
        Transform _imageTarget;

        [SerializeField]
        float _imageLifetime = 0.3f;

        [SerializeField]
        float _imageInterval = 0.1f;

        [SerializeField]
        Color _imageColor = Color.blue;

        List<ImageObject> _imageObjects = new List<ImageObject>();
        float _timer = 0.0f;
        #endregion

        #region privateメソッド
        ImageObject CreateImage()
        {
            return new ImageObject(_imageTarget, _imageLifetime, _imageColor);
        }
        #endregion
    }
}