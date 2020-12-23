using UnityEngine;
using UnityEngine.Rendering;

public class MilesRenderPipeline : RenderPipeline {
    MilesRenderer milesRendererV2 = new MilesRenderer();
    protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
        foreach(Camera camera in cameras) {
            milesRendererV2.Render(context, camera);
        }
    }
}
