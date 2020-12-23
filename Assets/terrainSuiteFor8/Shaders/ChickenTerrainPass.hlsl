#ifndef CHICKEN_TERRAIN_PASS_INCLUDED
#define CHICKEN_TERRAIN_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"

struct a2v
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
    float2 lightmapUV : TEXCOORD1;
    float3 normalOS : NORMAL;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f {
	float4 positionCS : SV_POSITION;
	float2 uv : TEXCOORD0;
	DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
	
	float4 uvL1L2 : TEXCOORD2;
	float4 uvL3L4 : TEXCOORD3;
	float4 uvL5L6 : TEXCOORD4;
	float4 uvL7L8 : TEXCOORD5;
	
	float4 positionWSAndFog : TEXCOORD6;
	float3 normalWS : TEXCOORD7;
	float3 viewDirWS : TEXCOORD8;
	#ifdef _MAIN_LIGHT_SHADOWS
		float4 shadowCoord : TEXCOORD9;
	#endif
	//half4 vertexLightAndFog : TEXCOORD10;		// xyz: vertexLight, w: fog
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};

void InitializeInputData(v2f input, out InputData inputData) {
	inputData.positionWS = input.positionWSAndFog.xyz;
	inputData.normalWS = input.normalWS;
	float3 viewDirWS = input.viewDirWS;
	#if SHADER_HINT_NICE_QUALITY
    	viewDirWS = SafeNormalize(viewDirWS);
	#endif
	inputData.viewDirectionWS = viewDirWS;
	inputData.normalWS = NormalizeNormalPerPixel(input.normalWS);
	#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
    	inputData.shadowCoord = input.shadowCoord;
	#else
    	inputData.shadowCoord = float4(0, 0, 0, 0);
	#endif
	inputData.fogCoord = input.positionWSAndFog.w;
	inputData.vertexLighting = half3(0, 0, 0);
	inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
}

// get control value which indicates how r g b a color mixes
half4 GetControlValue(half weight, half4 controlColor) {
	half4 controlValue;
	controlValue = controlColor;
	half4 maxControl = controlValue - (max(controlValue.r, max(controlValue.g, max(controlValue.b, controlValue.a))));
	half4 withWeight = max(maxControl + weight, half4(0, 0, 0, 0)) * controlColor;
	half4 finalValue = withWeight / (withWeight.r + withWeight.g + withWeight.b + withWeight.a);
	return saturate(finalValue);
	return finalValue;
}

// get control value which indicates how r g b a color mixes
void GetControlValueV3(half weight, half4 controlColor01, half4 controlColor02, out half4 splat01, out half4 splat02) {
    half4 controlValue01 = controlColor01;
    half4 controlValue02 = controlColor02;
    half4 mixedColor = controlColor01 + controlColor02;
    half maxChannel = max(controlValue01.r, max(controlValue01.g, max(controlValue01.b, max(controlValue01.a, max(controlValue02.r, max(controlValue02.g, max(controlValue02.b, controlValue02.a)))))));
    half4 maxControl01 = controlValue01 - maxChannel;
    half4 maxControl02 = controlValue02 - maxChannel;
    half4 withWeight01 = max(maxControl01 + weight, half4(0, 0, 0, 0)) * (controlColor01);
    half4 withWeight02 = max(maxControl02 + weight, half4(0, 0, 0, 0)) * (controlColor02);
    half4 finalValue01 = withWeight01 / (withWeight01.r + withWeight01.g + withWeight01.b + withWeight01.a + withWeight02.r + withWeight02.g + withWeight02.b + withWeight02.a);
    half4 finalValue02 = withWeight02 / (withWeight02.r + withWeight02.g + withWeight02.b + withWeight02.a + withWeight01.r + withWeight01.g + withWeight01.b + withWeight01.a);
    splat01 = saturate(finalValue01);
    splat02 = saturate(finalValue02);
}

v2f vert(a2v v) {
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	VertexPositionInputs vpi = GetVertexPositionInputs(v.positionOS.xyz);
	o.positionCS = vpi.positionCS;
	VertexNormalInputs vni = GetVertexNormalInputs(v.normalOS);
	o.normalWS = vni.normalWS;
	o.uv = v.uv;
	float3 viewDirWS = GetCameraPositionWS() - vpi.positionWS;
	#if !SHADER_HINT_NICE_QUALITY
    	viewDirWS = SafeNormalize(viewDirWS);
	#endif
	o.viewDirWS = viewDirWS;
	//float3 vertexLight = VertexLighting(vpi.positionWS, vni.normalWS);
	float fogFactor = ComputeFogFactor(vpi.positionCS.z);
	o.positionWSAndFog.xyz = vpi.positionWS;
	o.positionWSAndFog.w = fogFactor;

	o.uvL1L2.xy = TRANSFORM_TEX(v.uv, _TexLayer01);
	o.uvL1L2.zw = TRANSFORM_TEX(v.uv, _TexLayer02);
	o.uvL3L4.xy = TRANSFORM_TEX(v.uv, _TexLayer03);
	o.uvL3L4.zw = TRANSFORM_TEX(v.uv, _TexLayer04);
	o.uvL5L6.xy = TRANSFORM_TEX(v.uv, _TexLayer05);
	o.uvL5L6.zw = TRANSFORM_TEX(v.uv, _TexLayer06);
	o.uvL7L8.xy = TRANSFORM_TEX(v.uv, _TexLayer07);
	o.uvL7L8.zw = TRANSFORM_TEX(v.uv, _TexLayer08);

	OUTPUT_LIGHTMAP_UV(v.lightmapUV, unity_LightmapST, o.lightmapUV);
    OUTPUT_SH(o.normalWS.xyz, o.vertexSH);
	#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
    	o.shadowCoord = GetShadowCoord(vpi);
	#endif
	return o;
}

half3 SimpleBlinnPhong(InputData inputData, half3 diffuse) {
	Light mainLight = GetMainLight(inputData.shadowCoord);
	MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));		// shadowmask instead
	half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
	half3 diffuseColor = inputData.bakedGI + LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS);
	return diffuseColor * diffuse;
}

half4 frag(v2f i) : SV_TARGET {
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	InputData inputData;
	InitializeInputData(i, inputData);

    half4 controlColor01 = SAMPLE_TEXTURE2D_LOD(_Control01, sampler_Control01, i.uv, _SpatLod);
    half4 controlColor02 = SAMPLE_TEXTURE2D_LOD(_Control02, sampler_Control02, i.uv, _SpatLod);
    half4 splat01;
    half4 splat02;
    GetControlValueV3(_Weight01, controlColor01, controlColor02, splat01, splat02);

    half4 layer01 = SAMPLE_TEXTURE2D_LOD(_TexLayer01, sampler_TexLayer01, i.uvL1L2.xy, _LayerLod) * _Color01;
    layer01 *= splat01.r;
    half4 layer02 = SAMPLE_TEXTURE2D_LOD(_TexLayer02, sampler_TexLayer02, i.uvL1L2.zw, _LayerLod) * _Color02;
    layer02 *= splat01.g;
    half4 layer03 = SAMPLE_TEXTURE2D_LOD(_TexLayer03, sampler_TexLayer03, i.uvL3L4.xy, _LayerLod) * _Color03;
    layer03 *= splat01.b;
    half4 layer04 = SAMPLE_TEXTURE2D_LOD(_TexLayer04, sampler_TexLayer04, i.uvL3L4.zw, _LayerLod) * _Color04;
    layer04 *= splat01.a;
	half3 firstPass = layer01.rgb + layer02.rgb + layer03.rgb + layer04.rgb;

    half4 layer05 = SAMPLE_TEXTURE2D_LOD(_TexLayer05, sampler_TexLayer05, i.uvL5L6.xy, _LayerLod) * _Color05;
    layer05 *= splat02.r;
    half4 layer06 = SAMPLE_TEXTURE2D_LOD(_TexLayer06, sampler_TexLayer06, i.uvL5L6.zw, _LayerLod) * _Color06;
    layer06 *= splat02.g;
    half4 layer07 = SAMPLE_TEXTURE2D_LOD(_TexLayer07, sampler_TexLayer07, i.uvL7L8.xy, _LayerLod) * _Color07;
    layer07 *= splat02.b;
    half4 layer08 = SAMPLE_TEXTURE2D_LOD(_TexLayer08, sampler_TexLayer08, i.uvL5L6.zw, _LayerLod) * _Color08;
    layer08 *= splat02.a;
	half3 secondPass = layer05.rgb + layer06.rgb + layer07.rgb + layer08.rgb;
	
    half3 diffuseColor = SimpleBlinnPhong(inputData, firstPass + secondPass);
    return half4(diffuseColor, 1);


}


#endif