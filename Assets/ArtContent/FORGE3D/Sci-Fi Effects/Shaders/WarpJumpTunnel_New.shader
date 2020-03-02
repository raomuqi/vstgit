// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FORGE3D/WarpJumpTunnel_New"
{
	Properties
	{
		_TintColorA("Tint Color A", Color) = (0,0,0,0)
		_TintColorB("Tint Color B", Color) = (0,0,0,0)
		_Mult("Mult", Float) = 1
		_InvFade("InvFade", Float) = 0
		_MainTex("MainTex", 2D) = "white" {}
		_Twist("Twist", Float) = 0
		_Alpha("Alpha", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend One One
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 2.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float4 _TintColorA;
		uniform float _Mult;
		uniform float4 _TintColorB;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _Twist;
		uniform sampler2D _CameraDepthTexture;
		uniform float _InvFade;
		uniform float _Alpha;


		float2 TwistCustom33( float2 UV , float Twist )
		{
			float2 uv = UV - 0.5;
			  float dist = abs(length(uv));
				            float s = sin (Twist * dist);
				            float c = cos (Twist * dist);
				           
				            float2x2 rotationMatrix = float2x2( c, -s, s, c);
				            rotationMatrix *=0.5;
				            rotationMatrix +=0.5;
				            rotationMatrix = rotationMatrix * 2-1;
				            uv  = mul ( uv.xy, rotationMatrix );
				            uv.xy += 0.5;
			return uv;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 UV33 = uv_MainTex;
			float Twist33 = _Twist;
			float2 localTwistCustom33 = TwistCustom33( UV33 , Twist33 );
			float4 tex2DNode7 = tex2D( _MainTex, localTwistCustom33 );
			float4 lerpResult10 = lerp( ( _TintColorA * _TintColorA.a * _Mult ) , ( _TintColorB * _TintColorB.a * _Mult ) , ( _Mult * tex2DNode7 ).r);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth13 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth13 = abs( ( screenDepth13 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _InvFade ) );
			o.Emission = ( lerpResult10 * tex2DNode7 * i.vertexColor * i.vertexColor.a * saturate( distanceDepth13 ) * _Alpha ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15600
1927;29;1906;1124;318;696;1;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;19;-2324,-379;Float;True;Property;_MainTex;MainTex;5;0;Create;True;0;0;False;0;None;701de97950dcd70469736b201e292c6a;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2004,-10;Float;False;Property;_Twist;Twist;6;0;Create;True;0;0;False;0;0;-6.81;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-2029,-306;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;33;-1713,-288;Float;False;float2 uv = UV - 0.5@$  float dist = abs(length(uv))@$$	            float s = sin (Twist * dist)@$	            float c = cos (Twist * dist)@$	           $	            float2x2 rotationMatrix = float2x2( c, -s, s, c)@$	            rotationMatrix *=0.5@$	            rotationMatrix +=0.5@$	            rotationMatrix = rotationMatrix * 2-1@$	            uv  = mul ( uv.xy, rotationMatrix )@$	            uv.xy += 0.5@$return uv@;2;False;2;True;UV;FLOAT2;0,0;In;;True;Twist;FLOAT;0;In;;TwistCustom;True;False;0;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;4;-601,-74;Float;False;Property;_TintColorB;Tint Color B;2;0;Create;True;0;0;False;0;0,0,0,0;0.2499999,0.6896552,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-546,-413;Float;False;Property;_Mult;Mult;3;0;Create;True;0;0;False;0;1;2.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-925,-383;Float;True;Property;_MainTex_;MainTex_;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-684,-695;Float;False;Property;_TintColorA;Tint Color A;1;0;Create;True;0;0;False;0;0,0,0,0;0,0.5862141,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-124,-32;Float;False;Property;_InvFade;InvFade;4;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-446,-693;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-370,-67;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-327,-402;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;13;77,-25;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;10;-40,-489;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;16;291,-22;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;12;-87,-229;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;34;296,63;Float;False;Property;_Alpha;Alpha;7;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;532,-391;Float;False;6;6;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;961,-438;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;FORGE3D/WarpJumpTunnel_New;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;2;19;0
WireConnection;33;0;18;0
WireConnection;33;1;22;0
WireConnection;7;0;19;0
WireConnection;7;1;33;0
WireConnection;5;0;3;0
WireConnection;5;1;3;4
WireConnection;5;2;6;0
WireConnection;8;0;4;0
WireConnection;8;1;4;4
WireConnection;8;2;6;0
WireConnection;9;0;6;0
WireConnection;9;1;7;0
WireConnection;13;0;15;0
WireConnection;10;0;5;0
WireConnection;10;1;8;0
WireConnection;10;2;9;0
WireConnection;16;0;13;0
WireConnection;11;0;10;0
WireConnection;11;1;7;0
WireConnection;11;2;12;0
WireConnection;11;3;12;4
WireConnection;11;4;16;0
WireConnection;11;5;34;0
WireConnection;1;2;11;0
ASEEND*/
//CHKSM=83DC84D20E6C9F945ABBA54682E66B9C8F997C54