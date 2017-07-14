Shader "Custom/SpriteSheetAnimationCommon" 
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color0("Color", Color) = (1,1,1,1)
		_Color("Blend Color", Color) = (1,1,1,1)
		_Color2("Multiply Color", Color) = (1,1,1,1)
		_CurFrame("Current Frame", Int) = 0
		_CellWidth("Cell Width", Float) = 0.0
		_CellHeight("Cell Height", Float) = 0.0
		_CellRow("Cell Row Nums", Int) = 0
		_CellNums("Cell Total Nums", Int) = 0
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
			#include "CGLibTextureAltas.cginc"
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float4 _MainTex_TexelSize;
			fixed4 _Color0;
			fixed4 _Color;
			fixed4 _Color2;
			float ToggleBlend;

			uniform uint _CurFrame;
			uniform fixed _CellWidth;
			uniform fixed _CellHeight;
			uniform uint _CellRow;
			uniform uint _CellNums;

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
				int frames = _CellNums;
				float frame = fmod(_CurFrame, frames);
				int current = floor(frame);
				uint _CellCol = ((_CellNums + _CellRow - 1) & ~(_CellRow - 1)) / _CellRow;
				float ux =  (_CellWidth * _CellRow / _MainTex_TexelSize.z) / _CellRow;
				float uy =  (_CellHeight * _CellCol / _MainTex_TexelSize.w) / _CellCol;


				fixed4 framepixel = GetCurrentAnimationFrame(_MainTex, i.uv, ux, uy, current, _CellRow);
				if (ToggleBlend != 0) 
				{
					framepixel = GenerateBlendRGBA(framepixel, _Color, _Color2);
					return framepixel;
				}
				else
				{
					return framepixel * _Color0;
				}
			}

		ENDCG
		}
	}
}
