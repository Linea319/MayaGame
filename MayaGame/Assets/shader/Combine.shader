Shader "Shader Forge/Combine"
{ 
	Properties
	{
		//ここに必要なプロパティを各shaderファイルからコピーする
		 _MainTex("Base Color", 2D) = "white" {}
			_Color("Color", Color) = (1,1,1,1)
			_metalic_tex("metalic_tex", 2D) = "white" {}
			_BumpMap("Normal Map", 2D) = "bump" {}
			_Emission("Emission", 2D) = "white" {}
			_Emission_Power("Emission_Power", Float) = 0
			 _OutlineColor("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
			_outline_width("outline_width", Range(0, 0.1)) = 0
	}
	SubShader
	{
		Tags
		{
			//全体にかかるタグがあればここに
			"RenderType" = "Opaque"
		}

		Pass {
            Name "DEFERRED"
            Tags {
                "LightMode"="Deferred"
            }
		
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_DEFERRED
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile ___ UNITY_HDR_ON
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0

            #include "pbr_deferde.cginc"
            ENDCG
            }

		//以下、ひとつめのPASS

		Pass{
				Name "Outline-pre"

				Tags{
					}
				Stencil{
				Ref 10
				ZFail Replace
				}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#include "UnityPBSLighting.cginc"
				#include "UnityStandardBRDF.cginc"
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_shadowcaster
				#pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
				#pragma target 3.0
				#include "outline.cginc"
				ENDCG
			}
		


		Pass {
			Name "Outline"

			Tags{
			}
			//Cull Front
			ZTest Always
			Stencil{
			Ref 10
			Comp notequal
			}

				CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityPBSLighting.cginc"
			#include "UnityStandardBRDF.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcaster
			#pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
			#pragma target 3.0
			#include "outline.cginc"
			ENDCG
		}
	}
	// FallBackの指定を入れておく
	FallBack "Standard"

}