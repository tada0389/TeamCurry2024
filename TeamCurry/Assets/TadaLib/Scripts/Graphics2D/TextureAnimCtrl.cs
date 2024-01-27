using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.Graphics2D
{
    /// <summary>
    /// TextureAnimCtrl
    /// </summary>
    public class TextureAnimCtrl
        : BaseProc
        , IProcPostMove
    {
        #region プロパティ
        #endregion

        #region メソッド
        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="updater"></param>
        public void RegisterUpdater(ITextureAnimUpdater updater)
        {
            _reservedRegisterUpdaters.Add(updater);
        }

        /// <summary>
        /// 登録解除
        /// </summary>
        /// <param name="updater"></param>
        public void UnregisterUpdater(ITextureAnimUpdater updater)
        {
            updater.OnEnd(_texture, _material);
            _updaters.Remove(updater);
        }
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            _texture = new Texture2D(_textureSize.x, _textureSize.y);
            if (_overwriteSprite != null)
            {
                _sprite = _overwriteSprite;
            }
            else
            {
                _sprite = Sprite.Create(
                    _texture,
                    new Rect(0.0f, 0.0f, _texture.width, _texture.height),
                    new Vector2(0.5f, 0.5f)
                    );
            }
            _material = GetComponent<SpriteRenderer>().material;
        }
        #endregion

        #region IProcPostMove の実装
        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            foreach (var updater in _reservedRegisterUpdaters)
            {
                updater.OnStart(_texture, _material);
                _updaters.Add(updater);
            }

            _reservedRegisterUpdaters.Clear();

            foreach (var updater in _updaters)
            {
                updater.OnUpdate(_texture, _material, gameObject.DeltaTime() * _timeRate);
            }

            if (GetComponent<SpriteRenderer>() is { } renderer)
            {
                renderer.sprite = _sprite;
            }

            //if (GetComponent<UnityEngine.UI.Image>() is { } image)
            //{
            //    image.sprite = _sprite;
            //}
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        Vector2Int _textureSize = new Vector2Int(400, 400);
        [SerializeField]
        float _timeRate = 1.0f;
        [SerializeField]
        Sprite _overwriteSprite;

        List<ITextureAnimUpdater> _updaters = new List<ITextureAnimUpdater>();
        List<ITextureAnimUpdater> _reservedRegisterUpdaters = new List<ITextureAnimUpdater>();
        Sprite _sprite;
        Texture2D _texture;
        Material _material;
        #endregion

        #region privateメソッド
        #endregion
    }
}