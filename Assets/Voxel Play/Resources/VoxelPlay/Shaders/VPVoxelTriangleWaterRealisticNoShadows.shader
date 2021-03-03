Shader "Voxel Play/Voxels/Triangle/Water Realistic No Shadows"
{
	Properties
	{
        [HideInInspector] _MainTex ("Main Texture Array", Any) = "white" {}
        [NoScaleOffset] _ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
        [NoScaleOffset] _BumpMap ("Normalmap", 2D) = "bump" {}
        [NoScaleOffset] _FoamTex ("Foam texture", 2D) = "" {}    
        [NoScaleOffset] _FoamGradient ("Foam gradient ", 2D) = "white" {}
        _WaterColor ("Water Color", Color) = (0.23,0.70,0.82,0.31)
        _FoamColor ("Foam Color", Color) = (1,1,1)
        _UnderWaterFogColor("UnderWater Fog Color", Color) = (0.39,0.42,0.53,0.235)
        _WaveScale ("Wave scale", Range (0.02,0.15)) = 0.05
        _WaveSpeed ("Wave speed", Float) = 0.2
        _WaveAmplitude ("Wave Amplitude", Float) = 1.0
        _SpecularIntensity ("Specular Intensity", Float) = 2.0
        _SpecularPower ("Specular Power", Float) = 64
        _Fresnel("Fresnel", Float) = 0.1
        _NormalStrength("Normal Strength", Float) = 2.0
	    _OceanWave("Ocean Wave Data", Vector) = (0.512, 12, 0)
	}
	SubShader {

		Tags { "Queue" = "Transparent+1" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
			HLSLPROGRAM
			#pragma target 3.5
			#pragma vertex   vert
			#pragma fragment frag
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma multi_compile _ VOXELPLAY_GLOBAL_USE_FOG
			#pragma multi_compile _ VOXELPLAY_PIXEL_LIGHTS
            #define NO_SHADOWS
            #include "VPCommonURP.cginc"
			#include "VPVoxelTriangleWaterRealisticPass.cginc"
			ENDHLSL
		}

	}

	SubShader {

		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		Pass {
			Tags { "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			CGPROGRAM
			#pragma target 3.5
			#pragma vertex   vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile _ VOXELPLAY_GLOBAL_USE_FOG
			#pragma multi_compile _ VOXELPLAY_PIXEL_LIGHTS
            #include "VPCommon.cginc"
			#include "VPVoxelTriangleWaterRealisticPass.cginc"
			ENDCG
		}

	}

	Fallback Off
}