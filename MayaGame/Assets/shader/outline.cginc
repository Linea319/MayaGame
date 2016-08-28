uniform float4 _OutlineColor;
uniform float _outline_width;
struct VertexInput {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
struct VertexOutput {
	float4 pos : SV_POSITION;
};
VertexOutput vert(VertexInput v) {
	VertexOutput o = (VertexOutput)0;
	o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal*_outline_width, 1));
	return o;
}
float4 frag(VertexOutput i) : COLOR{
	return fixed4(_OutlineColor.rgb,0);
}