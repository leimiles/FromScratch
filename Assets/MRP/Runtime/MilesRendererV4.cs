using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// this is the command buffer + culling + drawingGeo renderer
public partial class MilesRendererV4 {
    ScriptableRenderContext scriptableRenderContext;
    Camera camera;

    const string bufferName = "A Miles Buffer";
    CommandBuffer buffer = new CommandBuffer { name = bufferName };
    CullingResults cullingResults;

    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
   
    public void Render(ScriptableRenderContext scriptableRenderContext, Camera camera) {
        this.scriptableRenderContext = scriptableRenderContext;
        this.camera = camera;
        PrepareBuffer();
        PrepareForSceneWindow();

        if(!Cull()) {
            return;
        }
       
        Setup();
        DrawGeometryOpaque();
        DrawLegacyShaders();
        DrawSkybox();
        DrawGeometryTransparent();
        DrawGizmo();
        Submit();
    }

    void Setup() {
        scriptableRenderContext.SetupCameraProperties(camera);
        CameraClearFlags flags = camera.clearFlags;
        // if clear flag enum less than depth (2, means skybox, solid color, or depth only), clear all detph
        buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth, flags == CameraClearFlags.Color, flags == CameraClearFlags.Color? camera.backgroundColor.linear : Color.clear);
        buffer.BeginSample(SampleName);
        ExecuteBuffer();
    }

    void DrawGeometryOpaque() {
        // 保证渲染排序按照不透明的渲染方式
        var sortingSettings = new SortingSettings(camera) { 
            criteria = SortingCriteria.CommonOpaque
        };
        // 保证特定的 shader id 的 pass 会被渲染，按照预设的排序顺序
        var drawSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        // 在 DrawRender 的时候，用 Filter 可以得到处于特定 Render Queue 的对象
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        scriptableRenderContext.DrawRenderers(cullingResults, ref drawSettings, ref filteringSettings);
    }

    void DrawGeometryTransparent() {
        var sortingSettings = new SortingSettings(camera) {
            criteria = SortingCriteria.CommonTransparent
        };
        var drawSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        var filterSettings = new FilteringSettings(RenderQueueRange.transparent);
        scriptableRenderContext.DrawRenderers(cullingResults, ref drawSettings, ref filterSettings);
    }

    void DrawSkybox() {
        scriptableRenderContext.DrawSkybox(camera);
    }

    void Submit() {
        buffer.EndSample(SampleName);
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
