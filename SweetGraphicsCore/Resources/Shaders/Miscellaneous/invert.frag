#version 440

uniform sampler2D sceneTexture;

in vec2 fUV;
out vec4 color;

void main()
{
	vec4 tex = texture(sceneTexture, fUV);
	color = vec4(1.0 - tex.r, 1.0 - tex.g, 1.0 - tex.b, tex.a);
}
