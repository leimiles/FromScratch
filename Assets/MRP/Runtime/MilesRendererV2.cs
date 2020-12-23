using UnityEngine;
using UnityEngine.Rendering;

// this is the simplest renderer
public class MilesRendererV2 {
    ScriptableRenderContext scriptableRenderContext;
    Camera camera;

    const string bufferName = "A Miles Buffer";
    CommandBuffer buffer = new CommandBuffer { name = bufferName };

    public void Render(ScriptableRenderContext scriptableRenderContext, Camera camera) {
        this.scriptableRenderContext = scriptableRenderContext;
        this.camera = camera;
        Setup();
        DrawSkybox();
        Submit();
    }

    void Setup() {
        buffer.BeginSample(bufferName);
        scriptableRenderContext.SetupCameraProperties(camera);
    }

    void DrawSkybox() {
        scriptableRenderContext.DrawSkybox(camera);
    }

    void Submit() {
        buffer.EndSample(bufferName);
        scriptableRenderContext.Submit();
    }
}
