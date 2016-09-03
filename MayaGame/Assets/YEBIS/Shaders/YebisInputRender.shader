Shader "Yebis/YebisInputRender" {
	Properties{
		vDofFactorScaleOffset("vDofFactorScaleOffset", Vector) = (1.0, 0.0, 0.0, 0.0)
		fDofFocusDistance("fDofFocusDistance", Float) = 0.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			float4 vDofFactorScaleOffset;
			float fDofFocusDistance;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv:TEXCOORD1;
			};

			//Vertex Shader
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = ComputeScreenPos(o.pos);
				// Vertical flip
				//o.uv.y = 1 - o.uv.y;
				return o;
			}

			//Fragment Shader
			float4 frag(v2f i) : COLOR
			{
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv.xy);
				// Z buffer to linear 0..1 depth (0 at eye, 1 at far plane)
				float linearDepth = Linear01Depth(depth);
				// camera scace z = linearDepth * far
				float z = linearDepth * _ProjectionParams.z;

				float4 outColor;
				outColor.rgb = tex2D(_MainTex, i.uv.xy).rgb;
				outColor.a = ((z-fDofFocusDistance)/z)*vDofFactorScaleOffset.x + vDofFactorScaleOffset.y;

				return outColor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}