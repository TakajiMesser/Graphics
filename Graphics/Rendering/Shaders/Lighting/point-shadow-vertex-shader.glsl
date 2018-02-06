#version 440 core

uniform mat4 modelMatrix;

layout(location = 0) in vec3 vPosition;

void main()
{
	gl_Position = modelMatrix * vec4(vPosition, 1.0);
}