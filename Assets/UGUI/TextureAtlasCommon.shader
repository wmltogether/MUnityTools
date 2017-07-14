Shader "Custom/TextureAtlasCommon" 
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Blend Color", Color) = (1,1,1,1)
		_Color2("Multiply Color", Color) = (1,1,1,1)
		_CellLeft("Cell Left", Float) = 0.0
		_CellTop("Cell Top", Float) = 0.0
		_CellWidth("Cell Width", Float) = 0.0
		_CellHeight("Cell Height", Float) = 0.0
		[MaterialToggle] ToggleBlend("Toggle Blend", Float) = 0
	}
	SubShader {
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}
		LOD 100
		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "CGLibTextureAltas.cginc"
			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float4 _MainTex_TexelSize;
			fixed4 _Color;
			fixed4 _Color2;
			float ToggleBlend;

			uniform fixed _CellLeft;
			uniform fixed _CellTop;
			uniform fixed _CellWidth;
			uniform fixed _CellHeight;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target 
			{
				i.uv = float2(i.uv.x , 1 - i.uv.y);

				float ux =  (_CellLeft / _MainTex_TexelSize.z);
				float uy =  (_CellTop / _MainTex_TexelSize.w);
				float u_width =  (_CellWidth / _MainTex_TexelSize.z);
				float u_height =  (_CellHeight / _MainTex_TexelSize.w);

				fixed4 framepixel = GetCurrentSprite(_MainTex, i.uv, ux, uy,u_width, u_height);
				if (ToggleBlend != 0) 
				{
					framepixel = GenerateBlendRGBA(framepixel, _Color, _Color2);
					return framepixel;
				}
				else
				{
					return framepixel * _Color;
				}
				
				
			}
		ENDCG
		}
	}
}
