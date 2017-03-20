Shader "Unlit/DisplaymentShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DispTex("Texture", 2D) = "white" {}
		_DispValue("Displayment Value", float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _DispTex;
			float4 _MainTex_ST;
			float _DispValue;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				/* You are not allow to do below in vertex shader */
				/* o.vertex += float4(0, tex2D(_DispTex, v.uv).r , 0, 0); */

				// So we use text2Dlod instead
				float4 dispColor = tex2Dlod(_DispTex, float4(v.uv, 0, 0));
				o.vertex -= float4(v.normal, 0) * dispColor.r * _DispValue;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
