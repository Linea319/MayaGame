// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32959,y:32693,varname:node_3138,prsc:2|emission-8691-OUT,alpha-2141-R;n:type:ShaderForge.SFN_TexCoord,id:7222,x:31428,y:32975,varname:node_7222,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:2141,x:32643,y:32682,ptovrint:False,ptlb:node_2141,ptin:_node_2141,varname:node_2141,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:672c9fb2b933b444f863297de0e0bcd1,ntxv:0,isnm:False|UVIN-917-OUT;n:type:ShaderForge.SFN_Multiply,id:917,x:32450,y:32727,varname:node_917,prsc:2|A-7541-OUT,B-152-OUT;n:type:ShaderForge.SFN_Vector1,id:3343,x:31677,y:32756,varname:node_3343,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector2,id:1556,x:31515,y:32855,varname:node_1556,prsc:2,v1:2,v2:3;n:type:ShaderForge.SFN_Divide,id:152,x:31840,y:32756,varname:node_152,prsc:2|A-3343-OUT,B-1556-OUT;n:type:ShaderForge.SFN_Color,id:8739,x:32646,y:32487,ptovrint:False,ptlb:node_8739,ptin:_node_8739,varname:node_8739,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.751724,c3:1,c4:1;n:type:ShaderForge.SFN_Add,id:8691,x:32795,y:32588,varname:node_8691,prsc:2|A-8739-RGB,B-2141-RGB;n:type:ShaderForge.SFN_Time,id:9428,x:31538,y:33154,varname:node_9428,prsc:2;n:type:ShaderForge.SFN_Add,id:7541,x:32328,y:32781,varname:node_7541,prsc:2|A-7222-UVOUT,B-9540-OUT;n:type:ShaderForge.SFN_Floor,id:3495,x:31727,y:33199,varname:node_3495,prsc:2|IN-9428-TDB;n:type:ShaderForge.SFN_Round,id:6603,x:31906,y:33225,varname:node_6603,prsc:2|IN-3495-OUT;n:type:ShaderForge.SFN_ComponentMask,id:1959,x:31906,y:33019,varname:node_1959,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-152-OUT;n:type:ShaderForge.SFN_Divide,id:7121,x:32159,y:33099,varname:node_7121,prsc:2|A-1959-G,B-6603-OUT;n:type:ShaderForge.SFN_Floor,id:7420,x:32326,y:33099,varname:node_7420,prsc:2|IN-7121-OUT;n:type:ShaderForge.SFN_Append,id:9540,x:32159,y:32904,varname:node_9540,prsc:2|A-1959-R,B-7420-OUT;proporder:2141-8739;pass:END;sub:END;*/

Shader "Shader Forge/test2" {
    Properties {
        _node_2141 ("node_2141", 2D) = "white" {}
        _node_8739 ("node_8739", Color) = (0,0.751724,1,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_2141; uniform float4 _node_2141_ST;
            uniform float4 _node_8739;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_152 = (1.0/float2(2,3));
                float2 node_1959 = node_152.rg;
                float4 node_9428 = _Time + _TimeEditor;
                float2 node_917 = ((i.uv0+float2(node_1959.r,floor((node_1959.g/round(floor(node_9428.b))))))*node_152);
                float4 _node_2141_var = tex2D(_node_2141,TRANSFORM_TEX(node_917, _node_2141));
                float3 emissive = (_node_8739.rgb+_node_2141_var.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,_node_2141_var.r);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
