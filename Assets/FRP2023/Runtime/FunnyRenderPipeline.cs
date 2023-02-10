using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    // 渲染管线主逻辑，主要用于遍历摄影机，准备和初始化各种渲染资源和数据
    public sealed partial class FunnyRenderPipeline : RenderPipeline {
        /// <summary>
        /// 用于保存渲染管线 asset 配置文件当前的内容
        /// </summary>
        private readonly FunnyRenderPipelineAsset FunnyRenderPipelineAsset;

        /// <summary>
        /// 以当前图形设置面板中的 asset 设置文件返回
        /// </summary>
        public static FunnyRenderPipelineAsset currentPipelineAsset {
            get => GraphicsSettings.currentRenderPipeline as FunnyRenderPipelineAsset;
        }

        public FunnyRenderPipeline(FunnyRenderPipelineAsset FunnyRenderPipelineAsset) {
            this.FunnyRenderPipelineAsset = FunnyRenderPipelineAsset;

            // init RTHandle System
            RTHandles.Initialize(Screen.width, Screen.height);
        }

        /// <summary>
        /// 将摄影机 array 转换为摄影机 list，注意遍历的摄影机也包含 sceneView, preView 这些摄影机
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
                } else {
                    RenderSingleCameraInternal(renderContext, camera);
                }
            }
        }

        /// <summary>
        /// Game View 渲染开始，以每个摄影机的 camera stack 为单位进行渲染
        /// </summary>
        static void RenderCameraStack(ScriptableRenderContext renderContext, Camera baseCamera) {
            // 获取摄影机的 camerdaAddtional 数据，该类扩展了摄影机属性
            baseCamera.TryGetComponent<FunnyAdditionalCameraData>(out var baseCameraAdditionalData);
            if (baseCameraAdditionalData != null && baseCameraAdditionalData.cameraRenderType == CameraRenderType.Overlay) {
                return;
            }
            int lastActiveOverlayCameraIndex = -1;
            // 判断是否是 stack rendering, 只有单摄影机的情况不属于 stack rendering
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
                return;
            }

            // 渲染开始前需要进行 cullingResult 设置
            if (!TryGetCullingParameters(cameraData, out var cullingParameters)) {
                return;
            }

            ScriptableRenderer.currentRenderer = renderer;

            // 第一个 commandbuffer
            CommandBuffer cmd = CommandBufferPool.Get();

#if UNITY_EDITOR
            if (cameraData.isSceneViewCamera) {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
#endif

            var cullResults = renderContext.Cull(ref cullingParameters);

            // 需要初始化 rendering data 作为所有被渲染对象的渲染设置
            InitializeRenderingData(FunnyRenderPipeline.currentPipelineAsset, ref cameraData, ref cullResults, cmd, out var renderingData);

            // pass 开始前，设置 RTHandle System 的最大引用尺寸
            RTHandles.SetReferenceSize(cameraData.cameraRenderTextureDescriptor.width, cameraData.cameraRenderTextureDescriptor.height);

            // 插入 render feature
            renderer.AddRenderPasses(ref renderingData);

            // 配置渲染场景的所有 passes
            renderer.Setup(renderContext, ref renderingData);

            // 执行所有 passes 的渲染命令
            renderer.Execute(renderContext, ref renderingData);

            renderContext.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            // 最终提交渲染命令
            renderContext.Submit();
        }

        /// <summary>
        /// 对于非 game 窗口，用此方法渲染
        /// </summary>
        internal static void RenderSingleCameraInternal(ScriptableRenderContext renderContext, Camera camera) {
            FunnyAdditionalCameraData additionalCameraData = null;
            InitializeCameraData(camera, additionalCameraData, true, out var cameraData);
            RenderSingleCamera(renderContext, ref cameraData);

        }

        /// <summary>
        /// 初始化摄影机数据 camera data
        /// </summary>
        static void InitializeCameraData(Camera camera, FunnyAdditionalCameraData additionalCameraData, bool resolveFinalTarget, out CameraData cameraData) {
            cameraData = new CameraData();
            InitializeStackedCameraData(camera, additionalCameraData, ref cameraData);
            InitializeAdditionalCameraData(camera, additionalCameraData, resolveFinalTarget, ref cameraData);
            cameraData.cameraRenderTextureDescriptor = CreateRenderTextureDescriptor(camera);
        }

        /// <summary>
        /// 通过摄影机的基本信息初始化 camera data
        /// </summary>
        static void InitializeStackedCameraData(Camera camera, FunnyAdditionalCameraData additionalCameraData, ref CameraData cameraData) {
            cameraData.cameraTargetTexture = camera.targetTexture;
            cameraData.cameraType = camera.cameraType;
        }

        /// <summary>
        /// 通过摄影机的 additional data 初始化 camera data
        /// </summary>
        static void InitializeAdditionalCameraData(Camera camera, FunnyAdditionalCameraData additionalCameraData, bool resolveFinalTarget, ref CameraData cameraData) {
            cameraData.camera = camera;
            cameraData.scriptableRenderer = currentPipelineAsset.scriptableRenderer;

            if (cameraData.isSceneViewCamera) {
                cameraData.cameraRenderType = CameraRenderType.Base;

            } else if (additionalCameraData != null) {
                cameraData.cameraRenderType = additionalCameraData.cameraRenderType;

            } else {
                cameraData.cameraRenderType = CameraRenderType.Base;
            }

            cameraData.SetViewAndProjectionMatrix(camera.worldToCameraMatrix, camera.projectionMatrix);
        }

        /// <summary>
        /// 初始化所有渲染数据 rendering data
        /// </summary>
        static void InitializeRenderingData(FunnyRenderPipelineAsset asset, ref CameraData cameraData, ref CullingResults cullingResults, CommandBuffer cmd, out RenderingData renderingData) {
            renderingData.cullingResults = cullingResults;
            renderingData.cameraData = cameraData;
            renderingData.commandBuffer = cmd;
        }

        /// <summary>
        /// 获得视锥剔除数据的参数 culling results
        /// </summary>
        static bool TryGetCullingParameters(CameraData cameraData, out ScriptableCullingParameters cullingParameters) {
            return cameraData.camera.TryGetCullingParameters(false, out cullingParameters);
        }

        static RenderTextureDescriptor CreateRenderTextureDescriptor(Camera camera) {
            RenderTextureDescriptor descriptor;

            if (camera.targetTexture == null) {
                descriptor = new RenderTextureDescriptor(camera.pixelWidth, camera.pixelHeight);

            } else {
                descriptor = camera.targetTexture.descriptor;
            }

            return descriptor;
        }

        /// <summary>
        /// 用于判断是否是 game 窗口
        /// </summary>
        public static bool IsGameCamera(Camera camera) {
            if (camera == null) {
                throw new ArgumentNullException("camera null error");
            } else {
                return camera.cameraType == CameraType.Game;
            }
        }
    }
}
