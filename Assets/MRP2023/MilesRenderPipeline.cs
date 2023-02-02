using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Miles.Rendering {
    // 渲染管线主逻辑 partial
    public sealed partial class MilesRenderPipeline : RenderPipeline {
        // 用于保存渲染管线 asset 配置文件当前的内容
        private readonly MilesRenderPipelineAsset milesRenderPipelineAsset;
        public MilesRenderPipeline(MilesRenderPipelineAsset milesRenderPipelineAsset) {
            this.milesRenderPipelineAsset = milesRenderPipelineAsset;
        }
        // 将摄影机 array 转换为摄影机 list
        protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras) {
            // 渲染管线起点
            Render(renderContext, new List<Camera>(cameras));
        }

        // 遍历摄影机对象，进行渲染，使用 foreach 代替 for
        protected override void Render(ScriptableRenderContext renderContext, List<Camera> cameras) {
            foreach (Camera camera in cameras) {
                // 判断是否是 game 窗口
                if (IsGameCamera(camera)) {
                    RenderCameraStack(renderContext, camera);
                }
            }
        }


        // 以每个摄影机的 camera stack 为单位进行渲染
        static void RenderCameraStack(ScriptableRenderContext renderContext, Camera baseCamera) {

        }

    }
}
