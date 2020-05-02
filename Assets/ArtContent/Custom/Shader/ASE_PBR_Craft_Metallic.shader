// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/ASE_PBR_Craft_Metallic"
{
	Properties
	{
		_Color("Color", Color) = (0.5588235,0.5588235,0.5588235,1)
		_MainTex("Albedo", 2D) = "white" {}
		_MetallicGlossMap("Metallic", 2D) = "white" {}
		_GlossMapScale("Smoothness", Range( 0 , 1)) = 0
		_BumpMap("NormalMap", 2D) = "bump" {}
		_BumpScale("NormalIntensity", Float) = 1
		_OcclusionMap("Occlusion", 2D) = "white" {}
		[HDR]_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}
		[HDR]_StateColor("StateColor", Color) = (1,1,1,0)
		_StateIntensity("StateIntensity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _BumpScale;
		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _StateColor;
		uniform float _StateIntensity;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionMap_ST;
		uniform float4 _EmissionColor;
		uniform sampler2D _MetallicGlossMap;
		uniform float4 _MetallicGlossMap_ST;
		uniform float _GlossMapScale;
		uniform sampler2D _OcclusionMap;
		uniform float4 _OcclusionMap_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float3 normalMap234 = UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap ), _BumpScale );
			o.Normal = normalMap234;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			o.Albedo = ( _Color * tex2D( _MainTex, uv_MainTex ) ).rgb;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float4 emissionMap236 = ( tex2D( _EmissionMap, uv_EmissionMap ) * _EmissionColor );
			o.Emission = ( ( _StateColor * _StateIntensity ) + emissionMap236 ).rgb;
			float2 uv_MetallicGlossMap = i.uv_texcoord * _MetallicGlossMap_ST.xy + _MetallicGlossMap_ST.zw;
			o.Smoothness = ( tex2D( _MetallicGlossMap, uv_MetallicGlossMap ).a * _GlossMapScale );
			float2 uv_OcclusionMap = i.uv_texcoord * _OcclusionMap_ST.xy + _OcclusionMap_ST.zw;
			o.Occlusion = tex2D( _OcclusionMap, uv_OcclusionMap ).g;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
390;139;1541;970;1862.802;442.4601;1.502271;True;False
Node;AmplifyShaderEditor.CommentaryNode;65;-2785.703,-881.2858;Inherit;False;914.83;811.3738;Emission;7;236;63;69;228;234;2;230;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;69;-2710.188,-521.8348;Inherit;True;Property;_EmissionMap;Emission;8;0;Create;False;0;0;False;0;-1;None;9bedc177fb360174ca84798db67c59b1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;228;-2612.785,-298.0354;Float;False;Property;_EmissionColor;EmissionColor;7;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.408377,0.8952881,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;253;-2166.442,237.6651;Inherit;False;1397.554;585.5229;Outline;8;292;277;260;258;259;261;254;252;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-2366.558,-382.363;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;252;-1741.949,296.7745;Inherit;False;Property;_StateColor;StateColor;10;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;230;-2731.397,-723.9238;Float;False;Property;_BumpScale;NormalIntensity;5;0;Create;False;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2504.008,-769.3459;Inherit;True;Property;_BumpMap;NormalMap;4;0;Create;False;0;0;False;0;-1;None;af9b234176f6de644a46d55265ac9b18;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;254;-1761.16,505.8141;Inherit;False;Property;_StateIntensity;StateIntensity;11;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;236;-2203.463,-388.5828;Inherit;False;emissionMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RelayNode;291;-1148.526,-56.15056;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;289;-952.171,-58.94863;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-406.4144,-310.9285;Inherit;True;Property;_MainTex;Albedo;1;0;Create;False;0;0;False;0;-1;None;124643223406b9e43a74db821d1b2bfe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;276;-2727.847,850.3014;Inherit;False;1053.588;495.8953;gameObject to camera distance;7;275;272;270;273;267;268;266;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;237;-923.4503,50.35842;Inherit;False;236;emissionMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;224;-359.6898,-532.8564;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;0.5588235,0.5588235,0.5588235,1;0.5330188,1,0.9766741,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;234;-2165.048,-768.9377;Inherit;False;normalMap;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;233;-388.859,119.4305;Inherit;True;Property;_MetallicGlossMap;Metallic;2;0;Create;False;0;0;False;0;-1;None;9d769823e9aa3ac489a9421b45cb3e7b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-372.7396,323.759;Float;False;Property;_GlossMapScale;Smoothness;3;0;Create;False;0;0;False;0;0;0.938;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;267;-2404.14,1083.202;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;275;-1817.564,1129.524;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;235;-26.21724,-201.5045;Inherit;False;234;normalMap;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;-1410.875,426.6042;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-62.54572,250.8037;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;268;-2349.45,948.0303;Inherit;False;True;True;True;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-38.4257,-411.7667;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;292;-1003.559,501.3296;Inherit;False;LocalVertexOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;259;-2049.603,593.7531;Inherit;False;Property;_LineWidth;LineWidth;12;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;266;-2647.474,950.9654;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;273;-2143.921,1206.331;Inherit;False;Property;_MaxDistance;MaxDistance;13;0;Create;True;0;0;False;0;500;500;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;261;-1410.416,601.722;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;229;-339.0996,455.2946;Inherit;True;Property;_OcclusionMap;Occlusion;6;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;270;-2130.541,1006.566;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OutlineNode;258;-1235.565,502.1559;Inherit;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;277;-1669.633,676.7661;Inherit;False;Constant;_Float0;Float 0;14;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;272;-1950.259,1126.908;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;279;-683.8322,-57.70797;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;370,-107;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/ASE_PBR_Craft_Metallic;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.51;True;True;0;True;Opaque;;AlphaTest;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;9;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;63;0;69;0
WireConnection;63;1;228;0
WireConnection;2;5;230;0
WireConnection;236;0;63;0
WireConnection;291;0;252;0
WireConnection;289;0;291;0
WireConnection;289;1;254;0
WireConnection;234;0;2;0
WireConnection;275;0;272;0
WireConnection;260;0;252;0
WireConnection;260;1;254;0
WireConnection;7;0;233;4
WireConnection;7;1;6;0
WireConnection;268;0;266;0
WireConnection;72;0;224;0
WireConnection;72;1;1;0
WireConnection;292;0;258;0
WireConnection;261;0;259;0
WireConnection;261;1;277;0
WireConnection;261;2;275;0
WireConnection;270;0;268;0
WireConnection;270;1;267;0
WireConnection;258;0;260;0
WireConnection;258;1;261;0
WireConnection;272;0;270;0
WireConnection;272;1;273;0
WireConnection;279;0;289;0
WireConnection;279;1;237;0
WireConnection;0;0;72;0
WireConnection;0;1;235;0
WireConnection;0;2;279;0
WireConnection;0;4;7;0
WireConnection;0;5;229;2
ASEEND*/
//CHKSM=BBBC10AC0D7B7CB9B7731DD2226F9299A2B1FE79