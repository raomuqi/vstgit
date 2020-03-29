Shader "Debug/DebugOverDraw"
{
  SubShader
	{
	  Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
      Fog { Mode Off }
      ZTest Always
      ZTest LEqual
      Blend One One
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			struct appdata
			{
				float4 vertex : POSITION;
			};
 
			struct v2f
			{
				float4 vertex : SV_POSITION;
			};
 
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
 
			fixed4 frag(v2f i) : SV_Target
			{
				 return fixed4(0.05, 0, 0, 0);
			}
			ENDCG
		}
	}
}