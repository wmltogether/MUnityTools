// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FX/MirrorReflection"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpTex ("Normal", 2D) = "bump" {}
		[HideInInspector] _ReflectionTex ("", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0,1)) = 0.02
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent"  "QUEUE" = "Transparent" }
		LOD 100
 
		Pass {
			Tags{ "RenderType" = "Transparent"  "QUEUE" = "Transparent" }
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 refl : TEXCOORD1;
				float2 uvbump : TEXCOORD2;
				float4 pos : SV_POSITION;
			};
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpTex;
			float _BumpAmt;
			float4 _BumpTex_ST;
			float  _Magnitude;
			sampler2D _ReflectionTex;

			v2f vert(float4 pos : POSITION, float2 uv : TEXCOORD0, float2 uvbump : TEXCOORD2)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (pos);
				o.uv = TRANSFORM_TEX(uv, _MainTex);
				o.refl = ComputeScreenPos (o.pos);
				o.uvbump = TRANSFORM_TEX(uvbump, _BumpTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 tex = tex2D(_MainTex, i.uv);
				half4 bump = tex2D(_BumpTex, i.uvbump);
				half2 distortion = UnpackNormal(bump).rg;
				i.refl.xy += (distortion * _Magnitude);
				fixed4 refl = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(i.refl));
				return tex.a * refl * _Color;
			}
			ENDCG
	    }
	}
}