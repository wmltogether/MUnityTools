Shader "CameraEffect/OldMachineShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_RampTex("Base (RGB)", 2D) = "grayscaleRamp" {}
	}
	SubShader
	{
		// 关掉zwrite culling 
		ZTest Always Cull Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _RampTex;
			uniform half _RampOffset;
			half4 _MainTex_ST;

			fixed _mr;
			fixed _mg;
			fixed _mb;

			fixed FixGamma(fixed inPixel)
			{
				//原始Luminance之后的灰度值不太好，尝试降低gamma
				return inPixel - 0.05 ;
			}
			fixed Clamp(fixed value, fixed min, fixed max) 
			{
				fixed dst;
				if (value > max) 
				{
					dst = max;
				}
				else if (value < min) 
				{
					dst = min;
				}
				else {
					dst = value;
				}
				
				return dst;
			}

			fixed OverlayBlendMode(fixed basePixel, fixed blendPixel) {

				fixed value;
				if (basePixel < 0.5){
					value = Clamp(2.0 * basePixel * blendPixel, 0.0, 1.0);
				}

				
				else {
					value = Clamp((1.0 - 2.0 * (1.0 - basePixel) * (1.0 - blendPixel)), 0.0, 1.0);
				}
				
				return value;
				
			}

			fixed4 frag(v2f_img i) : SV_Target
			{
				fixed4 original = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));

				//把tex2D转成灰度显示,如果灰度效果不好，需要再修改gamma
				fixed grayscale = FixGamma(Luminance(original.rgb));

				half2 remap = half2 (grayscale + _RampOffset, .5);

				fixed4 output = tex2D(_RampTex, remap);
				//对rgb分量做一个正片叠底(multiply)
				output.r = OverlayBlendMode(output.r, (_mr / 255.0));
				output.g = OverlayBlendMode(output.g, (_mg / 255.0));
				output.b = OverlayBlendMode(output.b, (_mb / 255.0));
				output.a = original.a;

				return output;
			}
		ENDCG
		}
	}
	Fallback off
}
