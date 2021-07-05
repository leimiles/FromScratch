using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering / Miles Render Pipeline Asset")]
public class MRPAsset : RenderPipelineAsset {
    protected override RenderPipeline CreatePipeline() {
        return new MilesRenderPipeline();
    }
}
