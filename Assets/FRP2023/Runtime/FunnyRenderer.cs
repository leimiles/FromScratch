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
        DrawSkyboxPass drawSkyboxPass;

        public FunnyRenderer(FunnyRendererData FunnyRendererData) : base(FunnyRendererData) {
            drawSkyboxPass = new DrawSkyboxPass(RenderPassEvent.BeforeRenderingSkybox);
        }

        /// <summary>
        /// 由子类通过 rendering data 设置并插入渲染流程中的所有 passes
        /// </summary>
        public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData) {
            EnqueuePass(drawSkyboxPass);
        }
    }
}
