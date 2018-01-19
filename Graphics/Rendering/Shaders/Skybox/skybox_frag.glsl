#version 440

uniform samplerCube mainTexture;

in vec3 fUV;

layout(location = 0) out vec4 finalColor;

void main()
{
	finalColor = texture(mainTexture, fUV);
}