using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public sealed class ExampleRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader _shader;

    private ExampleRenderPass _postProcessPass;

    public override void Create()
    {
        _postProcessPass = new ExampleRenderPass(_shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_postProcessPass != null)
        {
            renderer.EnqueuePass(_postProcessPass);
        }
    }
}