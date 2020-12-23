using UnityEngine;
using UnityEngine.Rendering;

public class MilesRenderPipeline : RenderPipeline {
    MilesRendererV4 milesRenderer = new MilesRendererV4();
    protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
        foreach(Camera camera in cameras) {
            milesRenderer.Render(context, camera);
        }
    }
}
