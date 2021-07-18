#version 440

uniform sampler2D shadowMap;

in vec2 fUV;

out vec4 color;

void main()
{
	float depth = texture(shadowMap, fUV).x;
    depth = 1.0 - (1.0 - depth) * 25.0;

    color = vec4(depth);
}