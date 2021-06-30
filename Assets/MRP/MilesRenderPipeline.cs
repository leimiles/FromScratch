using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MilesRenderPipeline : RenderPipeline {
    SimpleRenderer simpleRenderer = new SimpleRenderer();
    protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
        foreach(Camera camera in cameras) {
            simpleRenderer.Render(context, camera);
        }
    }
}
