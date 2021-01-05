using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

// this renderer is for editor renderer
partial class MilesRendererV4 {
    // partial method for drawing unsupported shaders
    partial void DrawLegacyShaders();
    // partial method for drawing gizmo
    partial void DrawGizmo();
    // partial method for drawing ui in scene view as a world geometry
    partial void PrepareForSceneWindow();
    // partial method for drawing two cameras with its own scope
    partial void PrepareBuffer();
#if UNITY_EDITOR
    static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
    static Material errorMaterial;

    partial void DrawLegacyShaders() {
        // for unsupported shader, display pink for error
        if(errorMaterial == null) {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }
        var sortingSettings = new SortingSettings(camera);
        var drawSettings = new DrawingSettings(legacyShaderTagIds[0], sortingSettings);
        drawSettings.overrideMaterial = errorMaterial;
        // 使用 SetShaderPassName 这个方法，还是可以 ShaderPass 都初始化对应的值, 0 -> always, 1 -> forwardBase ...
        for(int i = 1; i < legacyShaderTagIds.Length; i++) {
            drawSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        var filteringSettings = FilteringSettings.defaultValue;
        scriptableRenderContext.DrawRenderers(cullingResults, ref drawSettings, ref filteringSettings);
    }

    partial void DrawGizmo() {
        if(Handles.ShouldRenderGizmos()) {
            scriptableRenderContext.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            scriptableRenderContext.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }

    partial void PrepareForSceneWindow() {
        if(camera.cameraType == CameraType.SceneView) {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
    }

    partial void PrepareBuffer() {
        buffer.name = camera.name;
    }
#endif
}
