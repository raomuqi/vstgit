// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FORGE3D/BurnoutV2"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_NormalScale("NormalScale", Float) = 1
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_MaskTex("MaskTex", 2D) = "white" {}
		_LoopTex("LoopTex", 2D) = "white" {}
		_LoopTexMult("LoopTexMult", Float) = 100
		_LoopTexPanSpeed("LoopTexPanSpeed", Vector) = (0.1,0,0,0)
		_BurnoutMask("BurnoutMask", 2D) = "white" {}
		_WipeEm("WipeEm", 2D) = "white" {}
		_WipeOp("WipeOp", 2D) = "white" {}
		_FresnelScale("FresnelScale", Float) = 0
		_FresnelBias("FresnelBias", Float) = 0
		_FresnelExp("FresnelExp", Float) = 0
		_BurnColor("BurnColor", Color) = (0,0,0,0)
		_Burnout("Burnout", Range( 0 , 1)) = 0
		_BurnUVOffset("BurnUVOffset", Float) = 0
		_Cutoff( "Mask Clip Value", Float ) = 0.33
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 2.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float _NormalScale;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _LoopTex;
		uniform float2 _LoopTexPanSpeed;
		uniform float4 _LoopTex_ST;
		uniform float _LoopTexMult;
		uniform sampler2D _MaskTex;
		uniform float4 _MaskTex_ST;
		uniform sampler2D _BurnoutMask;
		uniform float4 _BurnoutMask_ST;
		uniform sampler2D _WipeEm;
		uniform float _Burnout;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelExp;
		uniform float4 _BurnColor;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform sampler2D _WipeOp;
		uniform float _BurnUVOffset;
		uniform float _Cutoff = 0.33;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode13 = UnpackScaleNormal( tex2D( _Normal, uv_Normal ) ,_NormalScale );
			o.Normal = tex2DNode13;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float2 uv_LoopTex = i.uv_texcoord * _LoopTex_ST.xy + _LoopTex_ST.zw;
			float2 panner13_g44 = ( 1.0 * _Time.y * _LoopTexPanSpeed + uv_LoopTex);
			float4 LoopTex33_g44 = ( tex2D( _LoopTex, panner13_g44 ) * _LoopTexMult );
			float2 uv_MaskTex = i.uv_texcoord * _MaskTex_ST.xy + _MaskTex_ST.zw;
			float4 tex2DNode4_g44 = tex2D( _MaskTex, uv_MaskTex );
			float4 _Vector2 = float4(15,1,0,0);
			float2 uv_BurnoutMask = i.uv_texcoord * _BurnoutMask_ST.xy + _BurnoutMask_ST.zw;
			float dotResult63_g44 = dot( tex2D( _BurnoutMask, uv_BurnoutMask ) , float4( float3(0.22,0.707,0.071) , 0.0 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV22_g44 = dot( normalize( normalize( WorldNormalVector( i , tex2DNode13 ) ) ), ase_worldViewDir );
			float fresnelNode22_g44 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV22_g44, _FresnelExp ) );
			float Fresnel31_g44 = saturate( fresnelNode22_g44 );
			float2 temp_output_39_0_g44 = ( float2( 0,1.2 ) - ( ( float2( 0,1 ) * _Burnout ) + Fresnel31_g44 ) );
			float4 wipe49_g44 = tex2D( _WipeEm, temp_output_39_0_g44 );
			o.Emission = ( ( LoopTex33_g44 * ( tex2DNode4_g44.g * _Vector2 ) ) + ( tex2DNode4_g44.b * _Vector2 ) + ( dotResult63_g44 * wipe49_g44 * _BurnColor * ( _BurnColor.a * 300.0 ) ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			float4 tex2DNode2_g44 = tex2D( _WipeOp, ( 1.0 - ( temp_output_39_0_g44 + _BurnUVOffset ) ) );
			float wipeMask56_g44 = ( tex2DNode2_g44.r * tex2DNode4_g44.r );
			clip( wipeMask56_g44 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1927;29;1906;1124;942.4622;1174.502;1.254136;True;False
Node;AmplifyShaderEditor.RangedFloatNode;45;-448.3327,-518.5889;Float;False;Property;_NormalScale;NormalScale;2;0;Create;True;0;0;False;0;1;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-212.0171,-563.9171;Float;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;None;4316d53818a14334b954fa305b364f1c;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-197.0171,-843.9171;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;60;146.6016,-526.7086;Float;False;F3DBurnout;5;;44;87cfee5b5269ad846b54547661f27860;0;1;24;FLOAT3;0,0,0;False;2;COLOR;11;FLOAT;12
Node;AmplifyShaderEditor.SamplerNode;12;-216.2233,-765.2726;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;ddbaefd8f70b0f14d9a9cc658b7724a2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-197.0171,-919.9171;Float;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;0;0.078;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;484.8862,-705.8973;Float;False;True;0;Float;ASEMaterialInspector;0;0;Standard;FORGE3D/BurnoutV2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.33;True;True;0;True;Opaque;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;20;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;5;45;0
WireConnection;60;24;13;0
WireConnection;0;0;12;0
WireConnection;0;1;13;0
WireConnection;0;2;60;11
WireConnection;0;3;14;0
WireConnection;0;4;15;0
WireConnection;0;10;60;12
ASEEND*/
//CHKSM=5A291FCCC6524018C4508E01DD1E9009A5885711