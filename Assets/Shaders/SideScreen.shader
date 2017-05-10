// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Side Screen"
{
	Properties
	{
		_MainTex ("Content", 2D) = "clear" {}
		_MainTexMask ("Content Mask", 2D) = "white" {}
		_MainTexBrightness ("Content Brightness", Range(0.0, 1.0)) = 1.0
		_MainTexMinBrightness ("Content Minimum Brightness", Range(0.0, 1.0)) = 0.0
		_MainTexMipBias ("Content Mip Bias", float) = 0.0
		_TexA ("TextureA", 2D) = "black" {}
		_TexABrightness ("Brightness", Range(0.0, 1.0)) = 1.0
		_TexAMipBias ("Mip Bias", float) = 0.0
		_TexB ("TextureB", 2D) = "black" {}
		_TexBBrightness ("Brightness", Range(0.0, 1.0)) = 1.0
		_BlendTex ("Blending", Range(0.0, 1.0)) = 0.0
		_AdditiveMask ("Additive Mask", 2D) = "white" {}
		_ColorReference ("Color Reference", 2D) = "black" {}
		_ColorRefMipLevel ("Color Ref Mip Level", int) = 0
		_Intensity ("Intensity", Range(0.0, 1.0)) = 0.5
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 colorRef1 : COLOR0;
				fixed4 colorRef2 : COLOR1;
				fixed4 colorRef3 : COLOR2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _MainTexMask;
			float _MainTexBrightness;
			float _MainTexMinBrightness;
			float _MainTexMipBias;
			sampler2D _TexA;
			float _TexABrightness;
			float _TexAMipBias;
			sampler2D _TexB;
			float _TexBBrightness;
			float _BlendTex;
			sampler2D _AdditiveMask;
			sampler2D _ColorReference;
			int _ColorRefMipLevel;
			float _Intensity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.colorRef1 = tex2Dlod(_ColorReference, fixed4(0.5, 0.5, 0, _ColorRefMipLevel));
				o.colorRef2 = tex2Dlod(_ColorReference, fixed4(1.0, 0.5, 0, _ColorRefMipLevel));
				o.colorRef3 = tex2Dlod(_ColorReference, fixed4(0.0, 0.5, 0, _ColorRefMipLevel));
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 texCol = (1.0 - _BlendTex) * _TexABrightness * tex2Dbias(_TexA, float4(i.uv, 0, _TexAMipBias)) + _BlendTex * _TexBBrightness * tex2D(_TexB, i.uv);
				fixed4 contentCol = tex2Dbias(_MainTex, float4(i.uv, 0, _MainTexMipBias)) * 
									tex2D(_MainTexMask, i.uv) * _MainTexBrightness;
				fixed3 contentColRGB = fixed3(max(contentCol.r, _MainTexMinBrightness) * contentCol.a,
										      max(contentCol.g, _MainTexMinBrightness) * contentCol.a,
										      max(contentCol.b, _MainTexMinBrightness) * contentCol.a);
			    float contentColProjA = max(length(fixed3(contentCol.rgb)) / 1.73205, _MainTexMinBrightness);
				
				fixed4 mask = tex2D(_AdditiveMask, i.uv);
				fixed4 additiveColor = (i.colorRef1 * mask.r + i.colorRef2 * mask.g + i.colorRef3 * mask.b) * _Intensity;
				return (1.0 - contentColProjA) * (texCol + additiveColor) + fixed4(contentColRGB, contentColProjA);
			}
			ENDCG
		}
	}
}
