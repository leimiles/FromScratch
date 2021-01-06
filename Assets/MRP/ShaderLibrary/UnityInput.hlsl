#ifndef MRP_UNITY_INPUT_INCLUDED
#define MRP_UNITY_INPUT_INCLUDED

// the following values are set by GPU once per draw
CBUFFER_START(UnityPerDraw)
float4x4 unity_ObjectToWorld;
float4x4 unity_WorldToObject;
float4 unity_LODFade;
real4 unity_WorldTransformParams;
CBUFFER_END

float4x4 unity_MatrixVP;
float4x4 unity_MatrixV;
float4x4 glstate_matrix_projection;

#endif