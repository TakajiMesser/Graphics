#version 440

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

layout(location = 0) in vec3 vPosition;

out vec2 fUV;

void main()
{
    fUV = vec2((vPosition.x + 1.0) * 0.5, (vPosition.y + 1.0) * 0.5);
	gl_Position = vec4(vPosition.x, vPosition.y, vPosition.z, 1.0);
}