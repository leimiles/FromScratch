using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Miles.Rendering {
    // 渲染管线主逻辑 partial
    public sealed partial class MilesRenderPipeline : RenderPipeline {
        /// <summary>
        /// 用于保存渲染管线 asset 配置文件当前的内容
        /// </summary>
        private readonly MilesRenderPipelineAsset milesRenderPipelineAsset;
        public MilesRenderPipeline(MilesRenderPipelineAsset milesRenderPipelineAsset) {
            this.milesRenderPipelineAsset = milesRenderPipelineAsset;
        }

        /// <summary>
        /// 将摄影机 array 转换为摄影机 list
        /// </summary>
        protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras) {
            // 渲染管线起点
            Render(renderContext, new List<Camera>(cameras));
        }

        /// <summary>
        /// 遍历摄影机对象，进行渲染，使用 foreach 代替 for
        /// </summary>
        protected override void Render(ScriptableRenderContext renderContext, List<Camera> cameras) {
            foreach (Camera camera in cameras) {
                // 判断是否是 game 窗口
                if (IsGameCamera(camera)) {
                    RenderCameraStack(renderContext, camera);
                }
            }
        }

        /// <summary>
        /// 渲染开始，以每个摄影机的 camera stack 为单位进行渲染
        /// </summary>
        static void RenderCameraStack(ScriptableRenderContext renderContext, Camera baseCamera) {
            // 获取摄影机的 camerdaAddtional 数据，该类扩展了摄影机属性
            baseCamera.TryGetComponent<MilesAdditionalCameraData>(out var baseCameraAdditionalData);
            if (baseCameraAdditionalData != null && baseCameraAdditionalData.cameraRenderType == CameraRenderType.Overlay) {
                return;
            }

            int lastActiveOverlayCameraIndex = -1;
            // 判断是否是 stack rendering, 只有但摄影机的情况不属于 stack rendering
            bool isStackedRendering = lastActiveOverlayCameraIndex != -1;
            // 初始化 cameraData
            InitializeCameraData(baseCamera, baseCameraAdditionalData, !isStackedRendering, out var baseCameraData);
            // 基于 cameraData 渲染场景
            RenderSingleCamera(renderContext, ref baseCameraData);
        }

        /// <summary>
        /// 以一个独立摄影机为单位开始渲染
        /// </summary>
        static void RenderSingleCamera(ScriptableRenderContext renderContext, ref CameraData cameraData) {
            Camera camera = cameraData.camera;
            var renderer = cameraData.scriptableRenderer;
            if (renderer == null) {
                Debug.LogWarning("Can't find renderer for camera, quit rendering");
            } else {
                Debug.Log("found renderer");
            }
        }

        /// <summary>
        /// 初始化摄影机数据 cameraData
        /// </summary>
        static void InitializeCameraData(Camera camera, MilesAdditionalCameraData additionalCameraData, bool resolveFinalTarget, out CameraData cameraData) {
            cameraData = new CameraData();
        }

    }
}
