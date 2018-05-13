#version 440

uniform float thickness;
uniform float length;

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

    float c = clamp(x + y, 0.0, 0.15);
    color = vec4(c, c, c, 1.0);
}