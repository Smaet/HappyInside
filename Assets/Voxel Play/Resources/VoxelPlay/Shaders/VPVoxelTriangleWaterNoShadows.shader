Shader "Voxel Play/Voxels/Triangle/Water No Shadows"
{
	Properties
	{
		[HideInInspector] _MainTex ("Main Texture Array", Any) = "white" {}
	}
	SubShader {
	    // Note: needs transparent+1 so when rendering other transparent voxels, water remains on top
		Tags { "Queue" = "Transparent+1" "RenderType" = "Transparent" }
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			CGPROGRAM
			#pragma target 3.5
			#pragma vertex   vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile _ VOXELPLAY_GLOBAL_USE_FOG
			#pragma multi_compile _ VOXELPLAY_USE_NORMAL
			#pragma multi_compile _ VOXELPLAY_USE_AA VOXELPLAY_USE_PARALLAX
			#pragma multi_compile _ VOXELPLAY_PIXEL_LIGHTS
			#define USE_SPECULAR
			#include "VPVoxelTriangleWaterPass.cginc"
			ENDCG
		}
	}
	Fallback Off
}