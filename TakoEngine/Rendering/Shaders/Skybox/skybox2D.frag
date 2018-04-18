#version 440

uniform sampler2D mainTexture;

in vec2 fUV;

layout(location = 0) out vec4 finalColor;

void main()
{
	finalColor = texture(mainTexture, fUV);
    //finalColor = vec4(1.0);
}