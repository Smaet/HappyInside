#include "VPCommonIndirect.cginc"

struct appdata {
	float4 vertex   : POSITION;
	float3 normal   : NORMAL;
	fixed4 color    : COLOR;
    float4 uv       : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};


struct v2f {
	float4 pos    : SV_POSITION;
	VOXELPLAY_LIGHT_DATA(0,1)
	SHADOW_COORDS(2)
	float4 uv     : TEXCOORD3;
	#if defined(USE_TRIPLANAR) 
	    float3 wpos    : TEXCOORD4;
	    half3  worldNormal : TEXCOORD5;
	#endif
	fixed4 color  : COLOR;
	VOXELPLAY_FOG_DATA(6)
	UNITY_VERTEX_OUTPUT_STEREO
};


fixed4 _Color;

v2f vert (appdata v, uint instanceID : SV_InstanceID) {
	v2f o;

	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_INITIALIZE_OUTPUT(v2f, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	#if SHADER_TARGET >= 45
       	float4 position = _Positions[instanceID];
       	v.vertex.xyz *= position.w; // uniform scale
       	#if VOXELPLAY_USE_ROTATION
	    	float4 rotationQuaternion = _Rotations[instanceID];
       		VOXELPLAY_COMPUTE_WORLD_MATRIX(position, rotationQuaternion)
        	float3 wpos = mul(unity_ObjectToWorld_2, v.vertex).xyz;
	  	    float3 worldNormal = mul((float3x3)unity_ObjectToWorld_2, v.normal);
	  	#else
	  		float3 wpos = position.xyz + v.vertex.xyz;
	  		float3 worldNormal = v.normal;
	  	#endif
       	fixed4 colorsAndLight = _ColorsAndLight[instanceID];
    #else
	    float3 wpos = v.vertex.xyz;
       	fixed4 colorsAndLight = 1.0.xxxx;
	    float3 worldNormal = v.normal;
    #endif

	VOXELPLAY_MODIFY_WPOS(wpos)
	o.pos    = mul(UNITY_MATRIX_VP, float4(wpos, 1.0f));
	fixed4 color = v.color * _Color;
	color.rgb *= colorsAndLight.rgb;
	o.color = color;

    float4 uv = v.uv;
    uv.xy = TRANSFORM_TEX(uv.xy, _MainTex);
    uv.w = colorsAndLight.a;

	#if defined(USE_TRIPLANAR)
		o.wpos    = wpos;
		o.worldNormal = worldNormal;
	#endif

	VOXELPLAY_INITIALIZE_LIGHT_AND_FOG_NORMAL(uv, wpos, worldNormal);
	VOXELPLAY_SET_LIGHT(o, wpos, worldNormal);
    VOXELPLAY_OUTPUT_UV(uv, o);
	TRANSFER_SHADOW(o);
	return o;
}

fixed4 frag (v2f i) : SV_Target {
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	// Diffuse
	#if defined(USE_TEXTURE)
		fixed4 color = VOXELPLAY_GET_TEXEL_2D(i.uv) * i.color;
	#elif defined(USE_TRIPLANAR) // from bgolus' https://github.com/bgolus/Normal-Mapping-for-a-Triplanar-Shader/blob/master/TriplanarSwizzle.shader
		// triplanar blend
        half3 triblend = saturate(pow(i.worldNormal, 4));
        triblend /= max(dot(triblend, half3(1,1,1)), 0.0001);

        // triplanar uvs
        float2 uvX = i.wpos.zy * _MainTex_ST.xy + _MainTex_ST.zw;
        float2 uvY = i.wpos.xz * _MainTex_ST.xy + _MainTex_ST.zw;
        float2 uvZ = i.wpos.xy * _MainTex_ST.xy + _MainTex_ST.zw;

        // albedo textures
        fixed4 colX = VOXELPLAY_GET_TEXEL_2D(uvX);
        fixed4 colY = VOXELPLAY_GET_TEXEL_2D(uvY);
        fixed4 colZ = VOXELPLAY_GET_TEXEL_2D(uvZ);
        fixed4 color = colX * triblend.x + colY * triblend.y + colZ * triblend.z;

        half3 axisSign = i.worldNormal < 0 ? -1 : 1;

        // tangent space normal maps
	half3 tnormalX = UnpackNormal(_BumpMap.Sample(sampler_Point_Repeat, uvX));
	half3 tnormalY = UnpackNormal(_BumpMap.Sample(sampler_Point_Repeat, uvY));
	half3 tnormalZ = UnpackNormal(_BumpMap.Sample(sampler_Point_Repeat, uvZ));

        // flip normal maps' z axis to account for world surface normal facing
        tnormalX.z *= axisSign.x;
        tnormalY.z *= axisSign.y;
        tnormalZ.z *= axisSign.z;

        // swizzle tangent normals to match world orientation and blend together
        half3 worldNormal = normalize(tnormalX.zyx * triblend.x + tnormalY.xzy * triblend.y + tnormalZ.xyz * triblend.z);
        half ndotl = saturate(dot(worldNormal, _WorldSpaceLightPos0.xyz));
        color *= ndotl;
        color *= i.color;
	#else
		fixed4 color = i.color;
	#endif

    #if defined(VP_CUTOUT)
        clip(color.a - 0.5);
    #endif

	VOXELPLAY_APPLY_LIGHTING_AND_GI(color, i);
	VOXELPLAY_APPLY_FOG(color, i);

	return color;
}

