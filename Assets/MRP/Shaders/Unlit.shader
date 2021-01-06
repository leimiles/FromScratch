Shader "MRP/Unlit" {
	Properties {
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
