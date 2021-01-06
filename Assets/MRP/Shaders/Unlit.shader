Shader "MRP/Unlit" {
	Properties {
		_BaseColor("BaseColor", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Pass {
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnlitPass.hlsl"
			ENDHLSL
		}
	}
}
