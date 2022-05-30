using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MilesRenderingPipeline {
    public sealed partial class MilesRenderingPipeline : RenderPipeline {
        // use for profiling
        private static class Profiling {
            public static class Pipeline {
            }

        }

        // render camra list
        protected override void Render(ScriptableRenderContext context, List<Camera> cameras) {

            SortCameras(cameras);
            for (int i = 0; i < cameras.Count; ++i) {
                var camera = cameras[i];
                RenderSingleCamera(context, camera);
            }


        }

        // single Camera rendering
        public static void RenderSingleCamera(ScriptableRenderContext context, Camera camera) {
            InitializeCameraData(camera, null, true, out var cameraData);
            RenderSingleCamera(context, cameraData, cameraData.postProcessEnabled);
        }

        // as it says
        static void InitializeCameraData(Camera camera, MilesAdditionalCameraData milesAdditionalCameraData, bool resolveFinalTarget, out CameraData cameraData) {
            cameraData = new CameraData();
            InitializeAdditionalCameraData(camera, milesAdditionalCameraData, resolveFinalTarget, ref cameraData);
        }

        static void InitializeAdditionalCameraData(Camera camera, MilesAdditionalCameraData milesAdditionalCameraData, bool resolveFinalTarget, ref CameraData cameraData) {
            cameraData.camera = camera;
            cameraData.renderer = asset.scriptableRenderer;
        }

        // single camera rendering happens here
        static void RenderSingleCamera(ScriptableRenderContext context, CameraData cameraData, bool anyPostProcessingEnabled) {
            Camera camera = cameraData.camera;
            var renderer = cameraData.renderer;
            if (renderer == null) {
                Debug.LogWarning(string.Format("Trying to render {0} with an invalid renderer. Camera rendering will be skipped.", camera.name));
                return;
            }
            if (!TryGetCullingParameters(cameraData, out var cullingParameters)) {
                return;
            }

            ScriptableRenderer.current = renderer;
            bool isSceneViewCamera = cameraData.isSceneViewCamera;
            CommandBuffer cmd = CommandBufferPool.Get();
            renderer.ClearTarget(cameraData.renderType);
            renderer.OnPreCullRenderPasses(in cameraData);
            renderer.SetupCullingParameters(ref cullingParameters, ref cameraData);
            context.ExecuteCommandBuffer(cmd);
            /*
#if UNITY_EDITOR
            // enable scene view UI
            if (isSceneViewCamera) {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
#endif
            */
            var cullResults = context.Cull(ref cullingParameters);
            InitializeRenderingData(asset, ref cameraData, ref cullResults, anyPostProcessingEnabled, out var renderingData);
            renderer.Setup(context, ref renderingData);
            context.Submit();
        }

        // the old way entry point for rendering camera list
        protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
            Render(context, new List<Camera>(cameras));
        }

        static bool TryGetCullingParameters(CameraData cameraData, out ScriptableCullingParameters cullingParameters) {
            return cameraData.camera.TryGetCullingParameters(false, out cullingParameters);
        }

        static void InitializeRenderingData(MilesRenderingPipelineAsset asset, ref CameraData cameraData, ref CullingResults cullingResults, bool anyPostProcessingEnabled, out RenderingData renderingData) {
            renderingData.cullingResults = cullingResults;
            renderingData.cameraData = cameraData;
        }

        // constructor, init via mrp asset
        public MilesRenderingPipeline(MilesRenderingPipelineAsset milesRenderingPipelineAsset) {
        }

    }
}
