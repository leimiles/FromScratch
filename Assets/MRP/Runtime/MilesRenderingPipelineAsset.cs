using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace mrp {
    [CreateAssetMenu(menuName = "Rendering/Miles Render Pipeline")]
    public class MilesRenderingPipelineAsset : RenderPipelineAsset {
        protected override RenderPipeline CreatePipeline() {
            return new MilesRenderingPipeline();
        }
    }
}
