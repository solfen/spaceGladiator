// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32450,y:32820|diff-27-RGB,normal-32-OUT,alpha-40-OUT,refract-24-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33732,y:32780,ptlb:Normalmap1,ptin:_Normalmap1,tex:93734f283ae9e94478cd41834805e825,ntxv:3,isnm:True|UVIN-10-OUT;n:type:ShaderForge.SFN_Time,id:3,x:34345,y:32630;n:type:ShaderForge.SFN_ValueProperty,id:4,x:34345,y:32789,ptlb:Speed,ptin:_Speed,glob:False,v1:0.01;n:type:ShaderForge.SFN_Multiply,id:5,x:34136,y:32678|A-3-T,B-4-OUT;n:type:ShaderForge.SFN_TexCoord,id:9,x:34136,y:32874,uv:0;n:type:ShaderForge.SFN_Add,id:10,x:33933,y:32780|A-5-OUT,B-9-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:14,x:33728,y:32989,ptlb:Normalmap2,ptin:_Normalmap2,tex:93734f283ae9e94478cd41834805e825,ntxv:3,isnm:True|UVIN-21-OUT;n:type:ShaderForge.SFN_Blend,id:16,x:33451,y:32899,blmd:10,clmp:True|SRC-2-RGB,DST-14-RGB;n:type:ShaderForge.SFN_ComponentMask,id:17,x:33256,y:32944,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-16-OUT;n:type:ShaderForge.SFN_Subtract,id:21,x:33909,y:32989|A-9-UVOUT,B-5-OUT;n:type:ShaderForge.SFN_Slider,id:23,x:33373,y:33157,ptlb:Deformation,ptin:_Deformation,min:0,cur:0,max:0.2;n:type:ShaderForge.SFN_Multiply,id:24,x:32803,y:33028|A-17-OUT,B-23-OUT;n:type:ShaderForge.SFN_Subtract,id:25,x:33021,y:32918|A-26-OUT,B-17-OUT;n:type:ShaderForge.SFN_Vector1,id:26,x:33260,y:32850,v1:1;n:type:ShaderForge.SFN_Cubemap,id:27,x:32804,y:32600,ptlb:Cubemap,ptin:_Cubemap;n:type:ShaderForge.SFN_Vector3,id:31,x:33364,y:32753,v1:0.5,v2:0.5,v3:1;n:type:ShaderForge.SFN_Lerp,id:32,x:33021,y:32708|A-31-OUT,B-16-OUT,T-23-OUT;n:type:ShaderForge.SFN_Fresnel,id:40,x:32803,y:33166;proporder:2-14-27-4-23;pass:END;sub:END;*/

Shader "Shader Forge/Water" {
    Properties {
        _Normalmap1 ("Normalmap1", 2D) = "bump" {}
        _Normalmap2 ("Normalmap2", 2D) = "bump" {}
        _Cubemap ("Cubemap", Cube) = "_Skybox" {}
        _Speed ("Speed", Float ) = 0.01
        _Deformation ("Deformation", Range(0, 0.2)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "ForwardBase"
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
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _Normalmap1; uniform float4 _Normalmap1_ST;
            uniform float _Speed;
            uniform sampler2D _Normalmap2; uniform float4 _Normalmap2_ST;
            uniform float _Deformation;
            uniform samplerCUBE _Cubemap;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = o.pos;
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float4 node_3 = _Time + _TimeEditor;
                float node_5 = (node_3.g*_Speed);
                float2 node_9 = i.uv0;
                float2 node_10 = (node_5+node_9.rg);
                float3 node_2 = UnpackNormal(tex2D(_Normalmap1,TRANSFORM_TEX(node_10, _Normalmap1)));
                float2 node_21 = (node_9.rg-node_5);
                float3 node_14 = UnpackNormal(tex2D(_Normalmap2,TRANSFORM_TEX(node_21, _Normalmap2)));
                float3 node_16 = saturate(( node_14.rgb > 0.5 ? (1.0-(1.0-2.0*(node_14.rgb-0.5))*(1.0-node_2.rgb)) : (2.0*node_14.rgb*node_2.rgb) ));
                float2 node_17 = node_16.rg;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_17*_Deformation);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalLocal = lerp(float3(0.5,0.5,1),node_16,_Deformation);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * texCUBE(_Cubemap,viewReflectDirection).rgb;
/// Final Color:
                return fixed4(lerp(sceneColor.rgb, finalColor,(1.0-max(0,dot(normalDirection, viewDirection)))),1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _Normalmap1; uniform float4 _Normalmap1_ST;
            uniform float _Speed;
            uniform sampler2D _Normalmap2; uniform float4 _Normalmap2_ST;
            uniform float _Deformation;
            uniform samplerCUBE _Cubemap;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float4 node_3 = _Time + _TimeEditor;
                float node_5 = (node_3.g*_Speed);
                float2 node_9 = i.uv0;
                float2 node_10 = (node_5+node_9.rg);
                float3 node_2 = UnpackNormal(tex2D(_Normalmap1,TRANSFORM_TEX(node_10, _Normalmap1)));
                float2 node_21 = (node_9.rg-node_5);
                float3 node_14 = UnpackNormal(tex2D(_Normalmap2,TRANSFORM_TEX(node_21, _Normalmap2)));
                float3 node_16 = saturate(( node_14.rgb > 0.5 ? (1.0-(1.0-2.0*(node_14.rgb-0.5))*(1.0-node_2.rgb)) : (2.0*node_14.rgb*node_2.rgb) ));
                float2 node_17 = node_16.rg;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_17*_Deformation);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalLocal = lerp(float3(0.5,0.5,1),node_16,_Deformation);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * texCUBE(_Cubemap,viewReflectDirection).rgb;
/// Final Color:
                return fixed4(finalColor * (1.0-max(0,dot(normalDirection, viewDirection))),0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
