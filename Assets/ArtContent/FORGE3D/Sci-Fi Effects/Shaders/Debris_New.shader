// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FORGE3D/Debris New"
{
	Properties
	{
		_Color("Color", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_FadeInAInBOutAOutB("Fade[InA,InB,OutA,OutB]", Vector) = (0,0,0,0)
		_TintRGBAmbientAMult("Tint [RGB:Ambient, A:Mult]", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha noshadow novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _TintRGBAmbientAMult;
		uniform sampler2D _Color;
		uniform float4 _Color_ST;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float4 _FadeInAInBOutAOutB;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Color = i.uv_texcoord * _Color_ST.xy + _Color_ST.zw;
			o.Albedo = ( _TintRGBAmbientAMult + ( tex2D( _Color, uv_Color ) * _TintRGBAmbientAMult.a * 10.0 ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float3 ase_worldPos = i.worldPos;
			float temp_output_5_0_g2 = distance( ase_worldPos , _WorldSpaceCameraPos );
			float temp_output_4_0_g2 = _FadeInAInBOutAOutB.z;
			float smoothstepResult13_g2 = smoothstep( 0.0 , 1.0 , ( 1.0 - ( ( temp_output_5_0_g2 - temp_output_4_0_g2 ) / ( _FadeInAInBOutAOutB.w - temp_output_4_0_g2 ) ) ));
			float temp_output_1_0_g2 = _FadeInAInBOutAOutB.x;
			float smoothstepResult25_g2 = smoothstep( 0.0 , 1.0 , ( 1.0 - ( ( temp_output_5_0_g2 - temp_output_1_0_g2 ) / ( _FadeInAInBOutAOutB.y - temp_output_1_0_g2 ) ) ));
			o.Alpha = ( smoothstepResult13_g2 - smoothstepResult25_g2 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15600
1927;29;1906;1124;1337.829;522.037;1;True;False
Node;AmplifyShaderEditor.WorldSpaceCameraPos;8;-1282.829,575.963;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;13;-1005.829,-333.037;Float;False;Property;_TintRGBAmbientAMult;Tint [RGB:Ambient, A:Mult];6;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0.091;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-1205.829,427.963;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;2;-1009,-134;Float;True;Property;_Color;Color;1;0;Create;True;0;0;False;0;None;9e69f8e597471ca4195f2e5c72dccde2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-708.829,-23.03699;Float;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;10;-931.8289,500.963;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-507.829,-91.03699;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;6;-745.8289,496.963;Float;False;Property;_FadeInAInBOutAOutB;Fade[InA,InB,OutA,OutB];5;0;Create;True;0;0;False;0;0,0,0,0;1,5,10,15;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;7;-390.8289,500.963;Float;False;Fade;-1;;2;26f683f6237cc0645a089d8f3513ef33;0;5;5;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;18
Node;AmplifyShaderEditor.RangedFloatNode;12;-428.829,338.963;Float;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;0;0.569;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-308.829,-126.037;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-429.829,224.963;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;0;0.837;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-798,114;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;None;c4e2f822f704cfb4282740a2ee220565;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;258,7;Float;False;True;0;Float;ASEMaterialInspector;0;0;Standard;FORGE3D/Debris New;False;False;False;False;False;True;True;True;True;False;True;True;False;False;True;True;True;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;9;0
WireConnection;10;1;8;0
WireConnection;15;0;2;0
WireConnection;15;1;13;4
WireConnection;15;2;16;0
WireConnection;7;5;10;0
WireConnection;7;1;6;1
WireConnection;7;2;6;2
WireConnection;7;4;6;3
WireConnection;7;3;6;4
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;0;0;14;0
WireConnection;0;1;3;0
WireConnection;0;3;11;0
WireConnection;0;4;12;0
WireConnection;0;9;7;18
ASEEND*/
//CHKSM=D7815D8B16711B71C665F83AC1D207C75530A398