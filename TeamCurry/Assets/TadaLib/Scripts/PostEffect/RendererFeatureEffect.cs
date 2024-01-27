using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TadaLib;
using TadaLib.ProcSystem;
using TadaLib.Input;

namespace TadaLib.PostEffect
{
    /// <summary>
    /// 画面エフェクト
    /// </summary>
    public class RendererFeatureEffect
        : ScriptableRendererFeature
    {
        public override void Create()
        {
            if (_settings.material is { } material)
            {
                _scriptablePass = new GrayScalePass();
                _scriptablePass.postEffectMaterial = material;
                _scriptablePass.renderPassEvent = _settings.renderPassEvent;
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_scriptablePass != null && _scriptablePass.postEffectMaterial != null)
            {
                // レンダーキューに登録 (ポストエフェクト実行)
                renderer.EnqueuePass(_scriptablePass);
            }
        }


        [System.Serializable]
        public class GrayscaleSetting
        {
            // ポストエフェクトに使用するマテリアル
            public Material material;

            // レンダリングの実行タイミング
            public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        /// <summary>
        /// Grayscale実行Pass
        /// </summary>
        class GrayScalePass : ScriptableRenderPass
        {
            private readonly string profilerTag = "GrayScale Pass";

            public Material postEffectMaterial; // グレースケール計算用マテリアル

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var cameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;

                // コマンドバッファ
                var cmd = CommandBufferPool.Get(profilerTag);

                int id = Shader.PropertyToID("");
                var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;

                cmd.GetTemporaryRT(id, tempTargetDescriptor);

                // マテリアル実行
                //cmd.Blit(cameraColorTarget, cameraColorTarget, postEffectMaterial);
                cmd.Blit(cameraColorTarget, cameraColorTarget, postEffectMaterial);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }

        [SerializeField]
        GrayscaleSetting _settings = new GrayscaleSetting();
        GrayScalePass _scriptablePass;
    }
}