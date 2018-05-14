#version 440

uniform float thickness;
uniform float length;

in vec2 fPosition;
in vec2 fUV;

layout(location = 0) out vec4 color;

void main() {
    float x = fract(fUV.x * length);
    x = min(x, 1.0 - x);
    float xdelta = fwidth(x);
    x = smoothstep(x - xdelta, x + xdelta, thickness);

    float y = fract(fUV.y * length);
    y = min(y, 1.0 - y);
    float ydelta = fwidth(y);
    y = smoothstep(y - ydelta, y + ydelta, thickness);

	float brightness = 0.2;

	int xDiv = int(round(fPosition.x)) % 5;
	float xFrac = fract(fPosition.x);

	int yDiv = int(round(fPosition.y)) % 5;
	float yFrac = fract(fPosition.y);

	if ((xDiv == 0 && min(xFrac, 1.0 - xFrac) < thickness) || (yDiv == 0 && min(yFrac, 1.0 - yFrac) < thickness))
	{
		brightness = 0.8;
	}

    float c = clamp(x + y, 0.0, brightness);
	if (c > 0.0)
	{
		color = vec4(c, c, c, 1.0);
	}
	else
	{
		discard;
	}
}