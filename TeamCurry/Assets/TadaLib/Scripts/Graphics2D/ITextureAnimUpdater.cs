using UnityEngine;

namespace TadaLib.Graphics2D
{
    /// <summary>
    /// ITextureAnimUpdater
    /// </summary>
    public interface ITextureAnimUpdater
    {
        /// <summary>
        /// アニメ開始処理
        /// </summary>
        void OnStart(Texture2D texture, Material mat);

        /// <summary>
        /// アニメ終了処理
        /// </summary>
        void OnEnd(Texture2D texture, Material mat);
        
        /// <summary>
        /// アニメ更新処理
        /// </summary>
        void OnUpdate(Texture2D texture, Material mat, float deltaTime);
    }
}