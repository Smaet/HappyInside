Shader "Voxel Play/Skybox/SkyboxEarth" {
Properties {
	_StarBlockSize ("Star Block Size", Range(100,300)) = 200
	_StarAmount("Star Amount", Range(0.9,1)) = 0.997
	_SunFlare("Sun Flare", Range(0, 1.0)) = 0.4
    _SunLightColor("Sun Light Color", Color) = (1,1,1)
	_MoonFlare("Moon Flare", Range(0, 1.0)) = 0.1
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off Fog { Mode Off }

	Pass {

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#include "UnityCG.cginc"
		#include "Lighting.cginc"

		half3 _VPSkyTint;
		half _VPExposure;
		half _VPFogAmount;
		float _StarBlockSize,  _StarAmount;
		half _SunFlare;
		half _MoonFlare;
		half3 _SunLightColor;

		struct appdata
		{
			float4 vertex   : POSITION;
			float2 texcoord : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv: TEXCOORD0;
			float3 ray : TEXCOORD1;
			float4 scrPos : TEXCOORD2;
			float4 lightPos : TEXCOORD3;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f vert (appdata v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.ray = v.vertex.xyz;
			o.uv = v.texcoord;
			o.scrPos = o.pos; 
			o.lightPos = UnityObjectToClipPos(_WorldSpaceLightPos0.xyz);
			return o;
		}

		float rand2(float2 xy) {
			return frac(sin(dot(xy, float2(12.9898,78.233))) * 43758.5453);
		}

		float rand3(float3 xyz) {
			return frac(sin(dot(xyz, float3(12.9898,78.233,39.789))) * 43758.5453);
		}

		half4 frag (v2f i) : SV_Target
		{
			float3 ray    = i.ray;

			float3 delta  = _WorldSpaceLightPos0.xyz - ray;
			float dist    = dot(delta, delta);

			// base color
			half3 skyColor = _VPSkyTint;

			// fog
			float y = abs(ray.y);
			half fog = saturate(_VPFogAmount - y) / (1.0001 - _VPFogAmount);
			skyColor = lerp(skyColor, 1.0, fog);

			// sky tint
			float hy = abs(_WorldSpaceLightPos0.y) + y;
			half t = saturate( (0.4 - hy) * 2.2) / (1.0 + dist * 0.8);
			skyColor.r = lerp(skyColor.r, 1.0, t);
			skyColor.b = lerp(skyColor.b, 0.0, t);

			// daylight + obscure opposite side of sky
			half daylight = saturate(1.0 +_WorldSpaceLightPos0.y * 2.0 - dist * 0.03);
			skyColor *= daylight;

			// sun
			half sunFlare = _SunFlare / pow(1.0 + dist, 64.0);
			float2 scrDist = i.scrPos.xy/i.scrPos.w - i.lightPos.xy/i.lightPos.w;
			scrDist.x *= _ScreenParams.x/_ScreenParams.y;
			float2 scrDistVox = round(abs(scrDist) * 16.0) / 16.0;
			float distVox = max(scrDistVox.x, scrDistVox.y);
			half sunIntensity = (distVox + dist * 13.0 < 0.13);
			half3 sunColor = sunFlare + sunIntensity;

			// moon
			delta = -_WorldSpaceLightPos0.xyz - ray;
			dist = dot(delta, delta);
			half  moonFlare = _MoonFlare / pow(1.0 + dist, 64.0);
			half3 moonColor = moonFlare.xxx;
			if (distVox < 0.12) {
				scrDistVox = round(scrDist * 26.0) / 26.0;
				half3 moonTex = rand2(scrDistVox) + 0.2;
				moonColor += moonTex;
			}

			// stars
			float3 bray = round(ray * _StarBlockSize) / _StarBlockSize;
			float star = rand3(bray);
			float isStar = star > _StarAmount;
			float mag = isStar / pow(1.0 + daylight, 3.0);
			mag *= 1.0 - fog;
			star = frac(star * 10000.0);
			mag *= saturate(1.0 - saturate(frac(star + _Time.w) - 0.3) * 0.2 - moonFlare * 10.0);
			half3 starColor = mag * lerp(half3(1.0,0.3,0.1), half3(0.2,0.95,1.0), star);
			starColor = pow(starColor, 2);

			// final color
			half3 col = skyColor + (sunColor + moonColor + starColor) * saturate(ray.y + 0.2);

			// exposure
			col *= _VPExposure * _SunLightColor;

			// gamma
			#if defined(UNITY_COLORSPACE_GAMMA)
			col = sqrt(col);
			#endif

			return half4(col,1.0);
		}
		ENDCG
	}
}

Fallback Off
}


