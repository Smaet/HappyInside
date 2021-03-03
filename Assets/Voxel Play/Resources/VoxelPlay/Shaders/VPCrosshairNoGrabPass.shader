Shader "Voxel Play/UI/Crosshair No GrabPass"
{
	Properties
	{
		_MainTex ("Texture (RGBA)", 2D) = "black" {}
		_Color ("Color", Color) = (1,1,1,1)
	}

SubShader
	{
		Tags { "Queue"="Transparent+100" "RenderType"="Transparent" "RenderPipeline" = "UniversalPipeline" }

    	Pass
		{
        	Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			ZTest Always

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
            #define NON_ARRAY_TEXTURE
            #include "VPCommonURP.cginc"
            #include "VPCommonMainTex.cginc"

			fixed4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert (appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.pos = TransformObjectToHClip(v.vertex.xyz);
				float4 cpos = TransformObjectToHClip(float3(0,0,0));
				o.pos.xy = cpos.xy + (o.pos.xy - cpos.xy) * o.pos.w;	// make it uniform size regardless of world position
				o.texcoord = v.texcoord;
				return o;
			}

			fixed getLuma(float3 rgb) {
				const fixed3 lum = float3(0.299, 0.587, 0.114);
				return dot(rgb, lum);
			}

			half4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.texcoord) * _Color;
				return color;
			}
			ENDHLSL
		}
	}

	SubShader
	{
		Tags { "Queue"="Overlay" "RenderType"="Transparent" }

		Pass
		{
        	Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			ZTest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _BackgroundTexture;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};


			v2f vert (appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.pos = UnityObjectToClipPos(v.vertex);
				float4 cpos = UnityObjectToClipPos(float4(0,0,0,1));
				o.pos.xy = cpos.xy + (o.pos.xy - cpos.xy) * o.pos.w;	// make it uniform size regardless of world position
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.texcoord) * _Color;
				return color;
			}
			ENDCG
		}
	}
}
