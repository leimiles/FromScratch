using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MilesRenderingPipeline {
    public class MilesRenderer : ScriptableRenderer {
        private struct RenderPassInputSummary {
            internal bool requiresColorTexture;
            internal bool requiresDepthTexture;
            internal bool requiresDepthPrepass;
            internal bool requiresNormalsTexture;
            internal bool requiresMotionVectors;
        }
        public MilesRenderer(MilesRendererData data) : base(data) {
        }

        public override void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData) {
            base.SetupCullingParameters(ref cullingParameters, ref cameraData);
            Debug.Log("milesrender setup culling parameters");
        }

        public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData) {
            // to avoid copy of rendering data, we need reference here
            ref CameraData cameraData = ref renderingData.cameraData;
            Camera camera = cameraData.camera;
            // rt info for creating render texture
            RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;

            // for not-game views, disable all render passes
            if (cameraData.cameraType != CameraType.Game) {
                useRenderPassEnabled = false;
            }

            // add render passes
            isCameraColorTargetValid = true;
            AddRenderPasses(ref renderingData);
            isCameraColorTargetValid = false;
        }
    }
}
