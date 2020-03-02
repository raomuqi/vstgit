// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FORGE3D/WarpTunnel"
{
	Properties
	{
		_TintA("TintA", Color) = (1,1,1,1)
		_TintB("TintB", Color) = (1,1,1,1)
		_UTile("UTile", Float) = 0
		_VTile("VTile", Float) = 0
		_Mask("Mask", 2D) = "white" {}
		_Boost("Boost", Float) = 0.04
		_Exp("Exp", Float) = 0.04
		_FadeMin("FadeMin", Float) = 0
		_FadeMax("FadeMax", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog noforwardadd 
		struct Input
		{
			half2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldPos;
		};

		uniform half4 _TintA;
		uniform half4 _TintB;
		uniform sampler2D _Mask;
		uniform half _UTile;
		uniform half _VTile;
		uniform float4 _Mask_ST;
		uniform half _Exp;
		uniform half _Boost;
		uniform half _FadeMin;
		uniform half _FadeMax;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult14 = (half2(_UTile , _VTile));
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float2 panner7 = ( _Time.y * appendResult14 + uv_Mask);
			half4 tex2DNode17 = tex2D( _Mask, panner7 );
			float4 lerpResult18 = lerp( ( _TintA * ( 100.0 * _TintA.a ) ) , ( _TintB * ( 100.0 * _TintB.a ) ) , tex2DNode17.r);
			half4 temp_cast_1 = (_Exp).xxxx;
			float3 ase_worldPos = i.worldPos;
			float smoothstepResult40 = smoothstep( 0.0 , 1.0 , ( ( distance( _WorldSpaceCameraPos , ase_worldPos ) - _FadeMin ) / ( _FadeMax - _FadeMin ) ));
			o.Emission = ( max( float4( 0,0,0,0 ) , pow( lerpResult18 , temp_cast_1 ) ) * i.vertexColor * i.vertexColor.a * tex2DNode17 * _Boost * ( 1.0 - smoothstepResult40 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
}
/*ASEBEGIN
Version=15600
1927;29;1906;1124;325.9942;947.7028;1.3;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;16;-1704,-122;Float;True;Property;_Mask;Mask;5;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1255,-188;Float;False;Property;_UTile;UTile;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1238,-52;Float;False;Property;_VTile;VTile;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;-671,-336;Float;False;Property;_TintB;TintB;2;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceCameraPos;33;-213.6328,41.84901;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;47;-136.7218,218.5916;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1412,26;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;8;-1238,135;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-639.5,-524.25;Float;False;Property;_TintA;TintA;1;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;14;-1037,-205;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DistanceOpNode;38;138.667,43.14901;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;131.2787,241.7917;Float;False;Property;_FadeMax;FadeMax;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;133.7674,144.649;Float;False;Property;_FadeMin;FadeMin;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-369,-473;Float;False;2;2;0;FLOAT;100;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-362,-259;Float;False;2;2;0;FLOAT;100;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;7;-932,-15;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;348.2791,242.7917;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;328.2791,41.79155;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-682,-113;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-192,-389;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-186,-526;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;42;536.2789,79.79162;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-88,-687;Float;False;Property;_Exp;Exp;7;0;Create;True;0;0;False;0;0.04;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;80,-446;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;40;721.279,73.7916;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;26;316,-577;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;44;916.1786,65.0916;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;1;444.5,-463.25;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;487.0671,-261.0545;Float;False;Property;_Boost;Boost;6;0;Create;True;0;0;False;0;0.04;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;29;516,-601;Float;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;700.5,-608.25;Float;False;6;6;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;1782.3,-633.4002;Half;False;True;0;Half;;0;0;Unlit;FORGE3D/WarpTunnel;False;False;False;False;True;True;True;True;True;True;False;True;False;False;True;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;2;16;0
WireConnection;14;0;11;0
WireConnection;14;1;12;0
WireConnection;38;0;33;0
WireConnection;38;1;47;0
WireConnection;23;1;3;4
WireConnection;24;1;19;4
WireConnection;7;0;15;0
WireConnection;7;2;14;0
WireConnection;7;1;8;2
WireConnection;46;0;45;0
WireConnection;46;1;39;0
WireConnection;41;0;38;0
WireConnection;41;1;39;0
WireConnection;17;0;16;0
WireConnection;17;1;7;0
WireConnection;21;0;19;0
WireConnection;21;1;24;0
WireConnection;25;0;3;0
WireConnection;25;1;23;0
WireConnection;42;0;41;0
WireConnection;42;1;46;0
WireConnection;18;0;25;0
WireConnection;18;1;21;0
WireConnection;18;2;17;0
WireConnection;40;0;42;0
WireConnection;26;0;18;0
WireConnection;26;1;27;0
WireConnection;44;0;40;0
WireConnection;29;1;26;0
WireConnection;4;0;29;0
WireConnection;4;1;1;0
WireConnection;4;2;1;4
WireConnection;4;3;17;0
WireConnection;4;4;30;0
WireConnection;4;5;44;0
WireConnection;6;2;4;0
ASEEND*/
//CHKSM=F7D19A721A9D563CA7E1CC834AA0DEE2806410D7