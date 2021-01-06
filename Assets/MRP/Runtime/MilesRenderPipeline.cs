using UnityEngine;
using UnityEngine.Rendering;

public class MilesRenderPipeline : RenderPipeline {
    MilesRendererV4 milesRenderer = new MilesRendererV4();

    // 为 RP 定义一个构造方法，并在构造方法中启用 SRP Batcher
    public MilesRenderPipeline() {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
        foreach(Camera camera in cameras) {
            milesRenderer.Render(context, camera);
        }
    }
}
