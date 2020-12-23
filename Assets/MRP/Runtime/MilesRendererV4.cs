using UnityEngine;
using UnityEngine.Rendering;

// this is the command buffer + culling + drawingGeo renderer
public class MilesRendererV4 {
    ScriptableRenderContext scriptableRenderContext;
    Camera camera;

    const string bufferName = "A Miles Buffer";
    CommandBuffer buffer = new CommandBuffer { name = bufferName };
    CullingResults cullingResults;

    public void Render(ScriptableRenderContext scriptableRenderContext, Camera camera) {
        this.scriptableRenderContext = scriptableRenderContext;
        this.camera = camera;
        if(!Cull()) {
            return;
        }
       
        Setup();
        DrawSkybox();
        Submit();
    }

    void Setup() {
        scriptableRenderContext.SetupCameraProperties(camera);
        buffer.ClearRenderTarget(true, true, Color.clear);
        buffer.BeginSample(bufferName);
        ExecuteBuffer();
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
