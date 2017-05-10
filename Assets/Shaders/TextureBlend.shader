// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Texture Blending (with additive unicolor mask)"
{
	Properties
	{
		_TexA ("TextureA", 2D) = "black" {}
		_TexABrightness ("Brightness", Range(0.0, 1.0)) = 1.0
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
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				fixed4 colorRef1 : COLOR0;
				fixed4 colorRef2 : COLOR1;
				fixed4 colorRef3 : COLOR2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _TexA;
			float _TexABrightness;
			float4 _TexA_ST;
			sampler2D _TexB;
			float _TexBBrightness;
			float _BlendTex;
			sampler2D _AdditiveMask;
			float4 _AdditiveMask_ST;
			sampler2D _ColorReference;
			float4 _ColorReference_ST;
			int _ColorRefMipLevel;
			float _Intensity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _TexA);
				o.uv2 = TRANSFORM_TEX(v.uv2, _AdditiveMask);
				o.colorRef1 = tex2Dlod(_ColorReference, fixed4(0.5, 0.5, 0, _ColorRefMipLevel));
				o.colorRef2 = tex2Dlod(_ColorReference, fixed4(1.0, 0.5, 0, _ColorRefMipLevel));
				o.colorRef3 = tex2Dlod(_ColorReference, fixed4(0.0, 0.5, 0, _ColorRefMipLevel));
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = (1.0 - _BlendTex) * _TexABrightness * tex2D(_TexA, i.uv) + _BlendTex * _TexBBrightness * tex2D(_TexB, i.uv);
				fixed4 mask = tex2D(_AdditiveMask, i.uv);
				fixed4 additiveColor = (i.colorRef1 * mask.r + i.colorRef2 * mask.g + i.colorRef3 * mask.b) * _Intensity;
				
				return col + additiveColor;
			}
			ENDCG
		}
	}
}
