using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 所有 pass 都需要按照指定的 native event 节点来执行渲染
    /// </summary>
    public enum RenderPassEvent {
        /// <summary>
        /// 渲染开始前，摄影机矩阵未设置，用来渲染与摄影机无关的 texture，例如 LUT
        /// </summary>
        BeforeRendering = 0,

        /// <summary>
        /// 阴影渲染开始前，摄影机矩阵未设置
        /// </summary>
        BeforeRenderingShadows = 50,

        /// <summary>
        /// 阴影渲染结束后，摄影机矩阵未设置
        /// </summary>
        AfterRenderingShadows = 100,

        /// <summary>
        /// 预渲染 pass 开始前，例如 depth prepass，摄影机矩阵已设置
        /// </summary>
        BeforeRenderingPrePasses = 150,

        /// <summary>
        /// 预渲染 pass 结束后，摄影机矩阵已设置
        /// </summary>
        AfterRenderingPrePasses = 200,

        /// <summary>
        /// G buffer pass 开始前
        /// </summary>
        BeforeRenderingGbuffer = 210,

        /// <summary>
        /// G buffer pass 结束后
        /// </summary>
        AfterRenderingGbuffer = 220,

        /// <summary>
        /// defferred pass 开始前
        /// </summary>
        BeforeRenderingDeferredLights = 230,

        /// <summary>
        /// defferred pass 结束后
        /// </summary>
        AfterRenderingDeferredLights = 240,

        /// <summary>
        /// opaque pass 开始前
        /// </summary>
        BeforeRenderingOpaques = 250,

        /// <summary>
        /// opaque pass 结束后
        /// </summary>
        AfterRenderingOpaques = 300,

        /// <summary>
        /// skybox pass 开始前
        /// </summary>
        BeforeRenderingSkybox = 350,

        /// <summary>
        /// skybox pass 结束后
        /// </summary>
        AfterRenderingSkybox = 400,

        /// <summary>
        /// transparent pass 开始前
        /// </summary>
        BeforeRenderingTransparents = 450,

        /// <summary>
        /// transparent pass 结束后
        /// </summary>
        AfterRenderingTransparents = 500,

        /// <summary>
        /// post processing pass 开始前
        /// </summary>
        BeforeRenderingPostProcessing = 550,

        /// <summary>
        /// post processing pass 结束后，但是在 final blit，post-procssing AA，color grading 开始前
        /// </summary>
        AfterRenderingPostProcessing = 600,

        /// <summary>
        /// 渲染结束后
        /// </summary>
        AfterRendering = 1000,
    }
    public abstract class ScriptableRenderPass {
        public RenderPassEvent renderPassEvent { get; set; }
        public abstract void Execute(ScriptableRenderContext context, ref RenderingData renderingData);
    }
}
