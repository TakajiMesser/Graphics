#version 440

uniform float unit;
uniform float length;
uniform float thickness;
uniform vec4 lineUnitColor;
uniform vec4 lineAxisColor;
uniform vec4 line5Color;
uniform vec4 line10Color;

in vec2 fPosition;
in vec2 fUV;

layout(location = 0) out vec4 color;

void main() {
    float x = fract(fUV.x * (1.0f / unit) * length);
    x = min(x, 1.0 - x);
    float xdelta = fwidth(x);
    x = smoothstep(x - xdelta, x + xdelta, thickness);

    float y = fract(fUV.y * (1.0f / unit) * length);
    y = min(y, 1.0 - y);
    float ydelta = fwidth(y);
    y = smoothstep(y - ydelta, y + ydelta, thickness);

	float xy = x + y;

	if (xy <= 0.0)
	{
		discard;
	}
	else
	{
		float alpha = clamp(xy, 0.0, 1.0);

		int xDiv5 = int(round(fPosition.x)) % 5;
		int xDiv10 = int(round(fPosition.x)) % 10;
		float xFrac = fract(fPosition.x);

		int yDiv5 = int(round(fPosition.y)) % 5;
		int yDiv10 = int(round(fPosition.y)) % 10;
		float yFrac = fract(fPosition.y);

		vec4 lineColor;

		if ((int(round(fPosition.x)) == 0 && min(xFrac, 1.0 - xFrac) < thickness) || (int(round(fPosition.y)) == 0 && min(yFrac, 1.0 - yFrac) < thickness))
		{
			lineColor = lineAxisColor;
		}
		else if ((xDiv10 == 0 && min(xFrac, 1.0 - xFrac) < thickness) || (yDiv10 == 0 && min(yFrac, 1.0 - yFrac) < thickness))
		{
			lineColor = line10Color;
		}
		else if ((xDiv5 == 0 && min(xFrac, 1.0 - xFrac) < thickness) || (yDiv5 == 0 && min(yFrac, 1.0 - yFrac) < thickness))
		{
			lineColor = line5Color;
		}
		else
		{
			lineColor = lineUnitColor;
		}

		color = vec4(lineColor.xyz, lineColor.w * alpha);
	}
}
