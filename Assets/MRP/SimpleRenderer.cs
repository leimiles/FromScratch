using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleRenderer  {
    ScriptableRenderContext context;
    Camera mainCamera;
    public void Render(ScriptableRenderContext context, Camera camera) {
        this.context = context;
        this.mainCamera = camera;
        Setup();
        DrawSkybox();
        Submit();
    }

    void Setup() {
        this.context.SetupCameraProperties(this.mainCamera);
    }

    void DrawSkybox() {
        // set drawsky command to pipeline, but not executed
        this.context.DrawSkybox(this.mainCamera);
    }

    void Submit() {
        this.context.Submit();
    }

}
