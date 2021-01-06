Shader "SoFunny/Chicken-Terrain_v4" {
    Properties {
        _Control01("Splatmap01", 2D) = "white" {}
        _Control02("Splatmap02", 2D) = "white" {}

        _TexLayer01("Layer01", 2D) = "white" {}
        
        _TexLayer02("Layer02", 2D) = "white" {}
        _TexLayer03("Layer03", 2D) = "white" {}
        _TexLayer04("Layer04", 2D) = "white" {}
        _TexLayer05("Layer05", 2D) = "white" {}
        _TexLayer06("Layer06", 2D) = "white" {}
        _TexLayer07("Layer07", 2D) = "white" {}
        _TexLayer08("Layer08", 2D) = "white" {}
        
        _Color01("Color01", Color) = (1, 1, 1, 1)
        _Color02("Color02", Color) = (1, 1, 1, 1)
        _Color03("Color03", Color) = (1, 1, 1, 1)
        _Color04("Color04", Color) = (1, 1, 1, 1)
        _Color05("Color05", Color) = (1, 1, 1, 1)
        _Color06("Color06", Color) = (1, 1, 1, 1)
        _Color07("Color07", Color) = (1, 1, 1, 1)
        _Color08("Color08", Color) = (1, 1, 1, 1)
        _Weight01("Edge Control", Range(0.001, 1)) = 1
        _Edge02("Layer Edge02", Range(0.01, 0.99)) = 1
        _Edge03("Layer Edge03", Range(0.01, 0.99)) = 1
        _Edge04("Layer Edge04", Range(0.01, 0.99)) = 1
        _Edge05("Layer Edge05", Range(0.01, 0.99)) = 1
        _Edge06("Layer Edge06", Range(0.01, 0.99)) = 1
        _Edge07("Layer Edge07", Range(0.01, 0.99)) = 1
        _Edge08("Layer Edge08", Range(0.01, 0.99)) = 1
        //_Weight02("Edge Control02", Range(0.1, 1)) = 0
        _SpatLod("Splat LOD", Range(0, 10)) = 0
        _LayerLod("Layer LOD", Range(0, 10)) = 0


        [ToogleOff] _ReceiveShadows("Receive Shadows", Float) = 1.0
    }
    SubShader {
        Tags { "Queue" = "Geometry-100" "RenderType" = "Opaque" "RenderPipeline" = "LightweightPipeline" "IgnoreProjector" = "True"}
        
        Pass {
            Name "ForwardLit"
            Tags{"LightMode" = "LightweightForward"}
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _RECEIVE_SHADOWS_OFF

            // -------------------------------------
            // Lightweight Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag

        
            #include "ChickenTerrainInputs.hlsl"
            #include "ChickenTerrainPass.hlsl"

            ENDHLSL
            
            
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/ShadowCasterPass.hlsl"

            ENDHLSL


        }

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }

    }
    CustomEditor  "ChickenTerrainMaterialGUI"
}
