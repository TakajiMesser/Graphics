#version 440

//uniform sampler2D textureSampler;

//in vec3 fPosition;
in vec4 fColor;
in vec4 fId;
//in vec2 fUV;

out vec4 color;

void main()
{
	//color = texture(textureSampler, fUV);
    //color = vec4(1.0, 0.0, 0.0, 1.0);
	color = fColor;
}