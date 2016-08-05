// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/hologram" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_Emission("emission rate",Float) = 0.0
		_Size("Size",Range(0.1,3)) = 1.0
		_Height("Height", Range(-50, 50)) = 1
		_Parallax("Parallax",Range(0.005,1)) = 0.02
		_ParallaxMap("Base (RGB) RefStrength (A)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 500

		CGPROGRAM
#pragma surface surf Lambert alpha

		sampler2D _MainTex;
	sampler2D _ParallaxMap;
	float4 _Color;
	float _Height;
	float _Parallax;
	float _Emission;
	float _Size;

	struct Input {
		float2 uv_ParallaxMap;
		float2 uv_BumpMap;
		float3 viewDir;
		float3 worldPos;
		float3 worldNormal;
		float4 screenPos;
		INTERNAL_DATA
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half h = tex2D(_ParallaxMap, IN.uv_BumpMap).w;
		float4 pos;
		pos.xyz = normalize(_WorldSpaceCameraPos - IN.worldPos);
		pos.w = h;
		float3 dir = normalize(_WorldSpaceCameraPos - IN.worldPos);
		dir = mul(unity_WorldToObject, pos).xyz;
		
		float2 offset = ParallaxOffset(h, _Height, dir);
		
		float2 posOffset = float2(0.1, _Height*1.18);
		IN.uv_ParallaxMap -= offset+ posOffset;
		IN.uv_BumpMap -= offset+ posOffset;
		IN.uv_ParallaxMap.x *= 1.5;
		float sizeOffset = -(1 - _Size)/2;
		IN.uv_ParallaxMap = IN.uv_ParallaxMap*_Size- float2(sizeOffset, sizeOffset);
		IN.uv_BumpMap = IN.uv_BumpMap*_Size - float2(sizeOffset, sizeOffset);
		


		/*
		IN.uv_ParallaxMap += offset;
		IN.uv_BumpMap += offset;
		float sizeOffset = -(1 - _Size)/2;
		IN.uv_ParallaxMap = IN.uv_ParallaxMap*_Size- float2(sizeOffset, sizeOffset);
		IN.uv_BumpMap = IN.uv_BumpMap*_Size - float2(sizeOffset, sizeOffset);
		IN.uv_ParallaxMap.y = IN.uv_ParallaxMap.y*(1-_Height*_Parallax)+(_Height*_Parallax)/2;
		*/

		fixed4 tex = tex2D(_ParallaxMap, IN.uv_ParallaxMap);
		fixed4 c = tex * _Color;
		o.Albedo = c.rgb;
		o.Emission = c.rgb*_Emission;
		o.Alpha = c.a;

	}
	ENDCG
	}

		FallBack "Diffuse"
}