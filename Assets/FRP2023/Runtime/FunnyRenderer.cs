using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// frp 的 pass 逻辑，主要函数是 setup，用于定于与添加需要渲染的所有 pass
    /// </summary>
    public sealed partial class FunnyRenderer : ScriptableRenderer {
        // 需要渲染 skybox pass
        DrawSkyboxPass m_DrawSkyboxPass;

        // 需要渲染 opaques
        DrawObjectsPass m_DrawOpaqueForwardPass;

        public FunnyRenderer(FunnyRendererData FunnyRendererData) : base(FunnyRendererData) {
            m_DrawSkyboxPass = new DrawSkyboxPass(RenderPassEvent.BeforeRenderingSkybox);
            m_DrawOpaqueForwardPass = new DrawObjectsPass(true, RenderPassEvent.BeforeRenderingOpaques);
        }

        void CreateCameraRenderTarget(ScriptableRenderContext renderContext, ref RenderTextureDescriptor descriptor, CommandBuffer commandBuffer, ref CameraData cameraData) {
            //ConfigureCameraColorTarget());
        }

        /// <summary>
        /// 由子类通过 rendering data 设置并插入渲染流程中的所有 passes
        /// </summary>
        public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData) {
            ref CameraData cameraData = ref renderingData.cameraData;
            Camera camera = cameraData.camera;
            RenderTextureDescriptor cameraRenderTextureDescriptor = cameraData.cameraRenderTextureDescriptor;
            //bool isOffscreenDepthTexture = cameraData
            EnqueuePass(m_DrawOpaqueForwardPass);
            EnqueuePass(m_DrawSkyboxPass);
        }
    }
}
