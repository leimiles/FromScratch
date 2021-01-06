#ifndef MRP_UNLIT_PASS_INCLUDED
#define MRP_UNLIT_PASS_INCLUDED

#include "../ShaderLibrary/CommonFunction.hlsl"

//half4 _BaseColor;
CBUFFER_START(UnityPerMaterial)
half4 _BaseColor;
CBUFFER_END
float4 vert(float3 positionOS : POSITION) : SV_POSITION {
    //return float4(positionOS, 1.0);
    float3 positionWS = TransformObjectToWorld(positionOS).xyz;
    return TransformWorldToHClip(positionWS);
}

half4 frag() : SV_TARGET {
    return _BaseColor;
}

#endif