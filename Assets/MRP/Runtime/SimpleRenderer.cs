using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleRenderer  {
    CommandBuffer commandBuffer = new CommandBuffer {
        name = "MilesCB"
    };
    static ShaderTagId unlitShaderTagID = new ShaderTagId("SRPDefaultUnlit");
    static ShaderTagId[] legacyShaderTagIDs = { 
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBass"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM"),
    };
    public void Render(ScriptableRenderContext context, Camera camera) {


        // culling 
        CullingResults cullingResults;
        ScriptableCullingParameters p;
        if(camera.TryGetCullingParameters(out p)) {
            cullingResults = context.Cull(ref p);
        } else {
            return;
        }

        // setup camera
        context.SetupCameraProperties(camera);
        // a sample under buffer name
        commandBuffer.BeginSample("JustClear");
        // use commandbuffer to draw clear
        commandBuffer.ClearRenderTarget(true, true, Color.clear);
        // execute here
        context.ExecuteCommandBuffer(commandBuffer);
        // clear buffer
        commandBuffer.Clear();

        // draw opaques
        var filterSettings = new FilteringSettings(RenderQueueRange.opaque);
        var sortingSettings = new SortingSettings(camera) {
            criteria = SortingCriteria.CommonOpaque
        };
        var drawingSettings = new DrawingSettings(unlitShaderTagID, sortingSettings);
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filterSettings);
        
        // use context to draw sky
        context.DrawSkybox(camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filterSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filterSettings);

        // sample ends here
        commandBuffer.EndSample("JustClear");

        context.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();



        // draw unsupportedShaders
        sortingSettings.criteria = SortingCriteria.CommonOpaque;
        drawingSettings = new DrawingSettings(legacyShaderTagIDs[0], sortingSettings);
        for(int i = 1; i < legacyShaderTagIDs.Length; i++) {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIDs[i]);
        }
        filterSettings = FilteringSettings.defaultValue;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filterSettings);

        context.Submit();
    }



}
