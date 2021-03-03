Shader "Voxel Play/Voxels/Triangle/Cutout Cross"
{
	Properties
	{
		[HideInInspector] _MainTex ("Main Texture Array", Any) = "white" {}
	}

    SubShader {

        Tags { "RenderType" = "TransparentCutout" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" }
		Pass {
			Tags { "LightMode" = "UniversalForward" }
			AlphaToMask On
			Cull Off
			HLSLPROGRAM
			#pragma target 3.5
			#pragma vertex   vert
			#pragma fragment frag
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile _ VOXELPLAY_GLOBAL_USE_FOG
			#pragma multi_compile _ VOXELPLAY_USE_AA
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
			#define VP_CUTOUT
			#define FRESNEL_USES_CUSTOM_NORMAL
            #include "VPCommonURP.cginc"
            #include "VPCommonCore.cginc"
			#include "VPVoxelTriangleCutoutCrossPass.cginc"
			ENDHLSL
		}

		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
//			Cull Off // Commented out for performance; two sided shadows can be expensive for mass grass
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
			#pragma target 3.5
			#pragma vertex vert
			#pragma fragment frag
		    #pragma multi_compile_instancing
			#include "VPVoxelTriangleCutoutCrossShadowsURP.cginc"
			ENDHLSL
		}


	}

	SubShader {

		Tags { "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" "IgnoreProjector"="True"}
		Pass {
			AlphaToMask On
			Tags { "LightMode" = "ForwardBase" }
			Cull Off
			CGPROGRAM
			#pragma target 3.5
			#pragma vertex   vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase nolightmap nodynlightmap novertexlight nodirlightmap
			#pragma multi_compile _ VOXELPLAY_GLOBAL_USE_FOG
			#pragma multi_compile _ VOXELPLAY_USE_AA
			#define VP_CUTOUT
			#define FRESNEL_USES_CUSTOM_NORMAL
            #include "VPCommon.cginc"
			#include "VPVoxelTriangleCutoutCrossPass.cginc"
			ENDCG
		}

		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
//			Cull Off // Commented out for performance; two sided shadows can be expensive for mass grass
			CGPROGRAM
			#pragma target 3.5
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
            #include "VPCommon.cginc"
			#include "VPVoxelTriangleCutoutCrossShadows.cginc"
			ENDCG
		}
	}

	Fallback Off
}