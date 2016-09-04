uniform float4 _Color;
uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
uniform sampler2D _metalic_tex; uniform float4 _metalic_tex_ST;
uniform sampler2D _Emission; uniform float4 _Emission_ST;
uniform float _Emission_Power;
struct VertexInput {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
	float2 texcoord0 : TEXCOORD0;
	float2 texcoord1 : TEXCOORD1;
	float2 texcoord2 : TEXCOORD2;
};
struct VertexOutput {
	float4 pos : SV_POSITION;
	float2 uv0 : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float2 uv2 : TEXCOORD2;
	float4 posWorld : TEXCOORD3;
	float3 normalDir : TEXCOORD4;
	float3 tangentDir : TEXCOORD5;
	float3 bitangentDir : TEXCOORD6;
#if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
	float4 ambientOrLightmapUV : TEXCOORD7;
#endif
};
VertexOutput vert(VertexInput v) {
	VertexOutput o = (VertexOutput)0;
	o.uv0 = v.texcoord0;
	o.uv1 = v.texcoord1;
	o.uv2 = v.texcoord2;
#ifdef LIGHTMAP_ON
	o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	o.ambientOrLightmapUV.zw = 0;
#elif UNITY_SHOULD_SAMPLE_SH
#endif
#ifdef DYNAMICLIGHTMAP_ON
	o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif
	o.normalDir = UnityObjectToWorldNormal(v.normal);
	o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
	o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	return o;
}
void frag(
	VertexOutput i,
	out half4 outDiffuse : SV_Target0,
	out half4 outSpecSmoothness : SV_Target1,
	out half4 outNormal : SV_Target2,
	out half4 outEmission : SV_Target3)
{
	i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
	float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap, TRANSFORM_TEX(i.uv0, _BumpMap)));
	float3 normalLocal = _BumpMap_var.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
	float3 viewReflectDirection = reflect(-viewDirection, normalDirection);
	////// Lighting:
	float Pi = 3.141592654;
	float InvPi = 0.31830988618;
	///////// Gloss:
	float4 _metalic_tex_var = tex2D(_metalic_tex, TRANSFORM_TEX(i.uv0, _metalic_tex));
	float gloss = 1.0 - _metalic_tex_var.a; // Convert roughness to gloss
											/////// GI Data:
	UnityLight light; // Dummy light
	light.color = 0;
	light.dir = half3(0, 1, 0);
	light.ndotl = max(0, dot(normalDirection, light.dir));
	UnityGIInput d;
	d.light = light;
	d.worldPos = i.posWorld.xyz;
	d.worldViewDir = viewDirection;
	d.atten = 1;
#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
	d.ambient = 0;
	d.lightmapUV = i.ambientOrLightmapUV;
#else
	d.ambient = i.ambientOrLightmapUV;
#endif
	d.boxMax[0] = unity_SpecCube0_BoxMax;
	d.boxMin[0] = unity_SpecCube0_BoxMin;
	d.probePosition[0] = unity_SpecCube0_ProbePosition;
	d.probeHDR[0] = unity_SpecCube0_HDR;
	d.boxMax[1] = unity_SpecCube1_BoxMax;
	d.boxMin[1] = unity_SpecCube1_BoxMin;
	d.probePosition[1] = unity_SpecCube1_ProbePosition;
	d.probeHDR[1] = unity_SpecCube1_HDR;
	Unity_GlossyEnvironmentData ugls_en_data;
	ugls_en_data.roughness = 1.0 - gloss;
	ugls_en_data.reflUVW = viewReflectDirection;
	UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data);
	////// Specular:
	float3 specularColor = _metalic_tex_var.r;
	float specularMonochrome;
	float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
	float3 diffuseColor = (_MainTex_var.rgb*_Color.rgb); // Need this for specular when using metallic
	diffuseColor = DiffuseAndSpecularFromMetallic(diffuseColor, specularColor, specularColor, specularMonochrome);
	specularMonochrome = 1.0 - specularMonochrome;
	float NdotV = max(0.0, dot(normalDirection, viewDirection));
	half grazingTerm = saturate(gloss + specularMonochrome);
	float3 indirectSpecular = (gi.indirect.specular);
	indirectSpecular *= FresnelLerp(specularColor, grazingTerm, NdotV);
	/////// Diffuse:
	float3 indirectDiffuse = float3(0, 0, 0);
	indirectDiffuse += gi.indirect.diffuse;
	////// Emissive:
	float4 _Emission_var = tex2D(_Emission, TRANSFORM_TEX(i.uv0, _Emission));
	float3 emissive = (_Emission_var.rgb*_Emission_Power);
	/// Final Color:
	outDiffuse = half4(diffuseColor, 1);
	outSpecSmoothness = half4(specularColor, gloss);
	outNormal = half4(normalDirection * 0.5 + 0.5, 1);
	outEmission = half4((_Emission_var.rgb*_Emission_Power), 1);
	outEmission.rgb += indirectSpecular * 1;
	outEmission.rgb += indirectDiffuse * diffuseColor;

#ifndef UNITY_HDR_ON
	outEmission.rgb = exp2(-outEmission.rgb);
#endif
}