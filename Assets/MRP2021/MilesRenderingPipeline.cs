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
        }

        // single camera rendering happens here
        static void RenderSingleCamera(ScriptableRenderContext context, CameraData cameraData, bool anyPostProcessingEnabled) {
            CommandBuffer cmd = CommandBufferPool.Get();
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
            context.Submit();

        }

        // the old way entry point for rendering camera list
        protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
            Render(context, new List<Camera>(cameras));
        }

        // init via mrp asset
        public MilesRenderingPipeline(MilesRenderingPipelineAsset milesRenderingPipelineAsset) {
        }
    }
}
