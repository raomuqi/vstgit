// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FORGE3D/Holographic_New"
{
	Properties
	{
		_MainTex("Interlace Mask", 2D) = "white" {}
		_bLayerColorA("Tint Color A", Color) = (0,0,0,0)
		_bLayerColorB("Tint Color B", Color) = (0,0,0,0)
		_bLayerColorC("Tint Color C", Color) = (0,0,0,0)
		_Inter("Interlace scale: Back, X, Y | UV Speed", Vector) = (0,0,0,0)
		_FresPowOut("Edge Factor", Float) = 0
		_FresMultOut("Edge Mult", Float) = 0
		_FresPow("Surface Factor", Float) = 0
		_FresMult("Surface Mult", Float) = 0
		_InvFade("Soft Fade Factor", Float) = 0
		_Fade("Fade Factor", Range( 0 , 1)) = 1
		_Flicker("Flicker", Range( 0 , 1)) = 0.9
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend OneMinusDstColor One
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 2.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float3 viewDir;
			float3 worldNormal;
			float4 screenPos;
		};

		uniform float4 _bLayerColorA;
		uniform float4 _bLayerColorB;
		uniform float _FresPow;
		uniform float _FresMult;
		uniform float4 _bLayerColorC;
		uniform float _FresPowOut;
		uniform float _FresMultOut;
		uniform sampler2D _MainTex;
		uniform float4 _Inter;
		uniform float _Fade;
		uniform sampler2D _CameraDepthTexture;
		uniform float _InvFade;
		uniform float _Flicker;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldNormal = i.worldNormal;
			float dotResult70 = dot( i.viewDir , ase_worldNormal );
			float temp_output_72_0 = abs( dotResult70 );
			float4 lerpResult78 = lerp( _bLayerColorA , _bLayerColorB , ( pow( temp_output_72_0 , _FresPow ) * _FresMult ));
			float4 temp_output_79_0 = ( _bLayerColorC * ( pow( ( 1.0 - temp_output_72_0 ) , _FresPowOut ) * _FresMultOut ) );
			float4 temp_output_80_0 = ( lerpResult78 + temp_output_79_0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float2 appendResult29 = (float2(ase_screenPos.xy));
			float2 temp_output_31_0 = ( distance( float4( _WorldSpaceCameraPos , 0.0 ) , mul( unity_ObjectToWorld, float4(0,0,0,1) ) ) * ( appendResult29 / ase_screenPos.w ) * _Inter.x );
			float2 appendResult38 = (float2(0.0 , ( _Time.x * _Inter.w )));
			float2 break45 = temp_output_31_0;
			float2 appendResult51 = (float2(( _Inter.y * break45.x ) , ( break45.y * _Inter.z )));
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth99 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth99 = abs( ( screenDepth99 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _InvFade ) );
			float mulTime107 = _Time.y * 155.0;
			o.Emission = saturate( ( saturate( ( ( temp_output_80_0 * tex2D( _MainTex, ( temp_output_31_0 + appendResult38 ) ) ) + ( temp_output_80_0 * tex2D( _MainTex, ( appendResult51 + appendResult38 ) ) ) + lerpResult78 + temp_output_79_0 ) ) * _Fade * saturate( distanceDepth99 ) * saturate( ( saturate( sin( ( mulTime107 * 55.0 ) ) ) + ( 1.0 - _Flicker ) ) ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1927;29;1906;1124;-1906.936;-1002.533;1;True;False
Node;AmplifyShaderEditor.Vector4Node;21;-382.2997,818.724;Float;False;Constant;_Vector1;Vector 1;12;0;Create;True;0;0;False;0;0,0,0,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;27;-308.1765,991.7634;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldMatrixNode;20;-404.8576,733.0048;Float;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.WorldNormalVector;68;721.4227,1500.118;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;66;726.4227,1339.118;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;17;-431.9247,489.3829;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-171.7598,763.082;Float;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;29;-51.17665,995.7634;Float;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;70;984.4214,1410.118;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;30;100.3885,996.1981;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;6;-139.2359,264.2175;Float;False;Property;_Inter;Interlace scale: Back, X, Y | UV Speed;5;0;Create;False;0;0;False;0;0,0,0,0;6.65,200,4,55;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;18;46.30011,632.2473;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;72;1123.42,1411.118;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;324.4599,813.15;Float;False;3;3;0;FLOAT;0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;95;1293.226,1409.683;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;1311.88,1132.437;Float;False;Property;_FresPow;Surface Factor;8;0;Create;False;0;0;False;0;0;7.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;45;494.6636,645.7797;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;10;1259.806,1493.462;Float;False;Property;_FresPowOut;Edge Factor;6;0;Create;False;0;0;False;0;0;3.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;48;715.702,596.7739;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;34;301.7234,1002.839;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;1505.38,1040.733;Float;False;Property;_FresMult;Surface Mult;9;0;Create;False;0;0;False;0;0;1.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;74;1478.019,1416.018;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;1475.106,1515.661;Float;False;Property;_FresMultOut;Edge Mult;7;0;Create;False;0;0;False;0;0;5.88;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;96;1533.727,1120.282;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;827.7186,669.7835;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;573.2687,1004.339;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;832.2192,525.7597;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;1673.017,1415.018;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;107;2111.629,1426.886;Float;False;1;0;FLOAT;155;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;1728.725,1119.282;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;51;1048.833,587.4506;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;4;1673.23,908.2739;Float;False;Property;_bLayerColorB;Tint Color B;3;0;Create;False;0;0;False;0;0,0,0,0;1,0.6492902,0.3308823,0.5019608;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;38;739.7955,983.3356;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;5;1771.116,1236.729;Float;False;Property;_bLayerColorC;Tint Color C;4;0;Create;False;0;0;False;0;0,0,0,0;0.6176471,0.4176947,0.2361591,0.5019608;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;1673.23,735.2739;Float;False;Property;_bLayerColorA;Tint Color A;2;0;Create;False;0;0;False;0;0,0,0,0;0.3455882,0.1983801,0.04065742,0.5019608;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;39;1086.352,812.3077;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;42;625.1849,206.2067;Float;True;Property;_MainTex;Interlace Mask;0;0;Create;False;0;0;False;0;None;2331a5a0008d09b428d80d3f554b6ef6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;1216.861,588.951;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;2305.629,1394.886;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;55;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;2037.556,1239.298;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;78;2041.079,812.7753;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;113;2241.629,1506.886;Float;False;Property;_Flicker;Flicker;12;0;Create;True;0;0;False;0;0.9;0.29;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;52;1369.885,560.4459;Float;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;43;1255.289,782.3019;Float;True;Property;_TextureSample0;Texture Sample 0;13;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;110;2453.629,1406.886;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;2340.843,1098.327;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;2511.143,1082.728;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;2509.841,1204.928;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;2180.5,1695.101;Float;False;Property;_InvFade;Soft Fade Factor;10;0;Create;False;0;0;False;0;0;8.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;114;2537.629,1515.886;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;112;2594.629,1412.886;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;2751.643,1121.728;Float;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;99;2431.629,1691.886;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;111;2744.629,1487.886;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;84;2911.543,1123.028;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;103;2906.689,1359.786;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;2766.56,1262.001;Float;False;Property;_Fade;Fade Factor;11;0;Create;False;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;100;2680.629,1682.886;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;3065.689,1227.786;Float;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;101;3221.689,1229.786;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-287.8635,76.18321;Float;False;Constant;_Float6;Float 6;12;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3398.919,1185.084;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;FORGE3D/Holographic_New;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;True;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;5;4;False;-1;1;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;20;0
WireConnection;22;1;21;0
WireConnection;29;0;27;0
WireConnection;70;0;66;0
WireConnection;70;1;68;0
WireConnection;30;0;29;0
WireConnection;30;1;27;4
WireConnection;18;0;17;0
WireConnection;18;1;22;0
WireConnection;72;0;70;0
WireConnection;31;0;18;0
WireConnection;31;1;30;0
WireConnection;31;2;6;1
WireConnection;95;0;72;0
WireConnection;45;0;31;0
WireConnection;48;0;6;3
WireConnection;74;0;95;0
WireConnection;74;1;10;0
WireConnection;96;0;72;0
WireConnection;96;1;8;0
WireConnection;47;0;45;1
WireConnection;47;1;48;0
WireConnection;35;0;34;1
WireConnection;35;1;6;4
WireConnection;46;0;6;2
WireConnection;46;1;45;0
WireConnection;75;0;74;0
WireConnection;75;1;11;0
WireConnection;97;0;96;0
WireConnection;97;1;9;0
WireConnection;51;0;46;0
WireConnection;51;1;47;0
WireConnection;38;1;35;0
WireConnection;39;0;31;0
WireConnection;39;1;38;0
WireConnection;54;0;51;0
WireConnection;54;1;38;0
WireConnection;109;0;107;0
WireConnection;79;0;5;0
WireConnection;79;1;75;0
WireConnection;78;0;1;0
WireConnection;78;1;4;0
WireConnection;78;2;97;0
WireConnection;52;0;42;0
WireConnection;52;1;54;0
WireConnection;43;0;42;0
WireConnection;43;1;39;0
WireConnection;110;0;109;0
WireConnection;80;0;78;0
WireConnection;80;1;79;0
WireConnection;81;0;80;0
WireConnection;81;1;43;0
WireConnection;82;0;80;0
WireConnection;82;1;52;0
WireConnection;114;0;113;0
WireConnection;112;0;110;0
WireConnection;83;0;81;0
WireConnection;83;1;82;0
WireConnection;83;2;78;0
WireConnection;83;3;79;0
WireConnection;99;0;12;0
WireConnection;111;0;112;0
WireConnection;111;1;114;0
WireConnection;84;0;83;0
WireConnection;103;0;111;0
WireConnection;100;0;99;0
WireConnection;98;0;84;0
WireConnection;98;1;13;0
WireConnection;98;2;100;0
WireConnection;98;3;103;0
WireConnection;101;0;98;0
WireConnection;0;2;101;0
ASEEND*/
//CHKSM=31AFBBCA45FE3F4D235CDDFA8F25B691943C0EBB