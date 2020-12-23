using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Miles Render Pipeline", fileName = "MilesRenderPipelineAsset")]
public class MilesRenderPipelineAsset : RenderPipelineAsset {
    protected override RenderPipeline CreatePipeline() {
        return new MilesRenderPipeline();
    }
}
