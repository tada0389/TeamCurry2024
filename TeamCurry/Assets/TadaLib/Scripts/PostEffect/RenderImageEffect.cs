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
    /// ��ʃG�t�F�N�g
    /// �J�����ɃA�^�b�`���邱��
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
    //[ExecuteInEditMode, ImageEffectAllowedInSceneView]
    public class RenderImageEffect : MonoBehaviour
    {
        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, _material);
        }

        [SerializeField]
        Material _material;
    }
}