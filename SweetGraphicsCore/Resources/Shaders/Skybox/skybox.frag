#version 440

uniform samplerCube mainTexture;

in vec3 fUV;

layout(location = 0) out vec4 finalColor;

void main()
{
	finalColor = texture(mainTexture, fUV);
    //finalColor = vec4(finalColor / 30.0);
    //finalColor = vec4(1.0 - (1.0 - finalColor.x) * 50.0);
}