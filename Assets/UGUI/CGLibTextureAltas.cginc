//Math.clamp(value ,min, max)
inline fixed Clamp(fixed value, fixed min, fixed max){
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
inline fixed Multiply(fixed p, fixed blend) {
    fixed value;
    value = p * blend;
    return value;
    }

inline fixed OverlayBlend(fixed p, fixed blend) {
    fixed value;
    if (p < 0.5) {
        value = Clamp(2.0 * p * blend, 0.0, 1.0);
    }
	else {
        value = Clamp((1.0 - 2.0 * (1.0 - p) * (1.0 - blend)), 0.0, 1.0);
    }
    return value;
}

//对通道颜色blend
inline fixed GenerateBlend(fixed p, fixed blend, fixed blend2) {
	fixed value = OverlayBlend(p, blend);
	value = Multiply(value, blend2);
	return value;
}

//对全部通道颜色blend
inline fixed4 GenerateBlendRGBA(fixed4 framepixel, fixed4 _Color, fixed4 _Color2) {
	framepixel.rgb *= framepixel.a;
	framepixel.r = GenerateBlend(framepixel.r, _Color.r, _Color2.r);
	framepixel.g = GenerateBlend(framepixel.g, _Color.g, _Color2.g);
	framepixel.b = GenerateBlend(framepixel.b, _Color.b, _Color2.b);
	return framepixel;
}


//获取当前动画帧的UV
inline fixed4 GetCurrentAnimationFrame(sampler2D tex, float2 uv, float ux, float uy, int frame, uint _CellRow)
{
	return tex2D(tex, float2(
		(uv.x * ux) + fmod(frame, _CellRow) * ux,
		(1 - ((uv.y * uy) + (frame / _CellRow) * uy))
		));
}


//从sampler2D图集中获取一个精灵
inline fixed4 GetCurrentSprite(sampler2D tex, float2 uv, float ux, float uy, float u_width, float u_height)
{
	return tex2D(tex, float2(
					(uv.x * u_width) + ux,
					1 - ((uv.y * u_height) + uy)
				));
}



inline fixed4 MaskAlpha(fixed4 col,float2 worldpos_uv, sampler2D Alphamask )
{
	fixed4 col2 = tex2D(Alphamask, worldpos_uv);
	return fixed4(col.r, col.g, col.b, col2.a * col.a);
}

inline fixed4 MaskDefault(fixed4 col,float2 uv, sampler2D mask, fixed _SpeedX)
{
	fixed4 colFlow = tex2D(mask, uv);
	fixed3 rgb = col.rgb;
	fixed a = 1- colFlow.r +  _SpeedX - 1;
	a = a + (1-colFlow.r);
	return fixed4(rgb.r, rgb.g, rgb.b, a);
}


