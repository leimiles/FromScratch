using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MilesRenderingPipeline {
    public sealed partial class MilesRenderingPipeline : RenderPipeline {
        protected override void Render(ScriptableRenderContext context, List<Camera> cameras) {
            BeginContextRendering(context, cameras);
            SortCameras(cameras);
            for (int i = 0; i < cameras.Count; ++i) {
                var camera = cameras[i];
            }

        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
            throw new System.NotImplementedException();
        }

        public MilesRenderingPipeline(MilesRenderingPipelineAsset milesRenderingPipelineAsset) {
        }
    }
}
