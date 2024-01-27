using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public sealed class ExampleRenderPass : ScriptableRenderPass
{
    private const string RenderPassName = nameof(ExampleRenderPass);
    private readonly Material _material;

    public ExampleRenderPass(Shader shader)
    {
        if (shader == null)
        {
            return;
        }

        _material = new Material(shader);
        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData data)
    {
        if (_material == null)
        {
            return;
        }

        var cmd = CommandBufferPool.Get(RenderPassName);

        // 一回Blitするだけでカラーバッファにマテリアルが適用される
        Blit(cmd, ref data, _material);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}