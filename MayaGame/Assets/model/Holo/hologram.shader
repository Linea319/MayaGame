Shader "Custom/hologram" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_Emission("emission rate",Float) = 0.0
		_Size("Size",Range(0.1,3)) = 1.0
		_Parallax("Height", Range(0.005, 50)) = 0.02
		_MainTex("Base (RGB) RefStrength (A)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 500

		CGPROGRAM
#pragma surface surf Lambert alpha

		sampler2D _MainTex;
	sampler2D _ParallaxMap;
	float4 _Color;
	float _Parallax;
	float _Emission;
	float _Size;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 viewDir;
		float3 worldPos;
		float3 worldNormal;
		float4 screenPos;
		INTERNAL_DATA
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half h = tex2D(_MainTex, IN.uv_BumpMap).w;
		float4 pos;
		pos.xyz = normalize(_WorldSpaceCameraPos - IN.worldPos);
		pos.w = h;
		float3 dir = normalize(_WorldSpaceCameraPos - IN.worldPos);
		dir = mul(_World2Object, pos).xyz;
		float2 offset = ParallaxOffset(h, _Parallax, dir);
		float2 posOffset = float2(0.1, _Parallax*1.18);
		IN.uv_MainTex -= offset+ posOffset;
		IN.uv_BumpMap -= offset+ posOffset;
		IN.uv_MainTex.x *= 1.5;
		float sizeOffset = -(1 - _Size)/2;
		IN.uv_MainTex = IN.uv_MainTex*_Size- float2(sizeOffset, sizeOffset);
		IN.uv_BumpMap = IN.uv_BumpMap*_Size - float2(sizeOffset, sizeOffset);
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 c = tex * _Color;
		o.Albedo = c.rgb;
		o.Emission = c.rgb*_Emission;
		o.Alpha = c.a;

	}
	ENDCG
	}

		FallBack "Diffuse"
}