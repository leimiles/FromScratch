using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// this is the command buffer + culling + drawingGeo renderer
public class MilesRendererV4 {
    ScriptableRenderContext scriptableRenderContext;
    Camera camera;

    const string bufferName = "A Miles Buffer";
    CommandBuffer buffer = new CommandBuffer { name = bufferName };
    CullingResults cullingResults;

    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
   

    public void Render(ScriptableRenderContext scriptableRenderContext, Camera camera) {
        this.scriptableRenderContext = scriptableRenderContext;
        this.camera = camera;
        if(!Cull()) {
            return;
        }
       
        Setup();
        DrawGeometry();
        DrawSkybox();
        Submit();
    }

    void Setup() {
        scriptableRenderContext.SetupCameraProperties(camera);
        buffer.ClearRenderTarget(true, true, Color.clear);
        buffer.BeginSample(bufferName);
        ExecuteBuffer();
    }

    void DrawGeometry() {
        var sortingSettings = new SortingSettings(camera);
        var drawSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.all);
        scriptableRenderContext.DrawRenderers(cullingResults, ref drawSettings, ref filteringSettings);
    }

    void DrawSkybox() {
        scriptableRenderContext.DrawSkybox(camera);
    }

    void Submit() {
        buffer.EndSample(bufferName);
        ExecuteBuffer();
        scriptableRenderContext.Submit();
    }

    void ExecuteBuffer() {
        scriptableRenderContext.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    bool Cull() {
        if(camera.TryGetCullingParameters(out ScriptableCullingParameters p)) {
            cullingResults = scriptableRenderContext.Cull(ref p);
            return true;
        }
        return false;
    }
}
