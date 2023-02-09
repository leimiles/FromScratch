using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// frp 的默认渲染逻辑
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
