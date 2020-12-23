using UnityEngine;
using UnityEngine.Rendering;

// this is the simplest renderer
public class MilesRenderer {
    ScriptableRenderContext scriptableRenderContext;
    Camera camera;
    public void Render(ScriptableRenderContext scriptableRenderContext, Camera camera) {
        this.scriptableRenderContext = scriptableRenderContext;
        this.camera = camera;
        Setup();
        DrawSkybox();
        Submit();
    }

    void Setup() {
        scriptableRenderContext.SetupCameraProperties(camera);
    }

    void DrawSkybox() {
        scriptableRenderContext.DrawSkybox(camera);
    }

    void Submit() {
        scriptableRenderContext.Submit();
    }
}
