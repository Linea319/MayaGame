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
	LIGHTING_COORDS(7, 8)
		UNITY_FOG_COORDS(9)
};
VertexOutput vert(VertexInput v) {
	VertexOutput o = (VertexOutput)0;
	o.uv0 = v.texcoord0;
	o.uv1 = v.texcoord1;
	o.uv2 = v.texcoord2;
	o.normalDir = UnityObjectToWorldNormal(v.normal);
	o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
	o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
	float3 lightColor = _LightColor0.rgb;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	UNITY_TRANSFER_FOG(o, o.pos);
	TRANSFER_VERTEX_TO_FRAGMENT(o)
		return o;
}
float4 frag(VertexOutput i) : COLOR{
	i.normalDir = normalize(i.normalDir);
float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
float3 normalLocal = _BumpMap_var.rgb;
float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
float3 lightColor = _LightColor0.rgb;
float3 halfDirection = normalize(viewDirection + lightDirection);
////// Lighting:
float attenuation = LIGHT_ATTENUATION(i);
float3 attenColor = attenuation * _LightColor0.xyz;
float Pi = 3.141592654;
float InvPi = 0.31830988618;
///////// Gloss:
float4 _metalic_tex_var = tex2D(_metalic_tex,TRANSFORM_TEX(i.uv0, _metalic_tex));
float gloss = _metalic_tex_var.a;
float specPow = exp2(gloss * 10.0 + 1.0);
////// Specular:
float NdotL = max(0, dot(normalDirection, lightDirection));
float LdotH = max(0.0,dot(lightDirection, halfDirection));
float3 specularColor = _metalic_tex_var.r;
float specularMonochrome;
float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
float3 diffuseColor = (_MainTex_var.rgb*_Color.rgb); // Need this for specular when using metallic
diffuseColor = DiffuseAndSpecularFromMetallic(diffuseColor, specularColor, specularColor, specularMonochrome);
specularMonochrome = 1.0 - specularMonochrome;
float NdotV = max(0.0,dot(normalDirection, viewDirection));
float NdotH = max(0.0,dot(normalDirection, halfDirection));
float VdotH = max(0.0,dot(viewDirection, halfDirection));
float visTerm = SmithJointGGXVisibilityTerm(NdotL, NdotV, 1.0 - gloss);
float normTerm = max(0.0, GGXTerm(NdotH, 1.0 - gloss));
float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
if (IsGammaSpace())
specularPBL = sqrt(max(1e-4h, specularPBL));
specularPBL = max(0, specularPBL * NdotL);
float3 directSpecular = attenColor*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
float3 specular = directSpecular;
/////// Diffuse:
NdotL = max(0.0,dot(normalDirection, lightDirection));
half fd90 = 0.5 + 2 * LdotH * LdotH * (1 - gloss);
float nlPow5 = Pow5(1 - NdotL);
float nvPow5 = Pow5(1 - NdotV);
float3 directDiffuse = ((1 + (fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
float3 finalColor = diffuse + specular;
fixed4 finalRGBA = fixed4(finalColor * 1,0);
UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
return finalRGBA;
}