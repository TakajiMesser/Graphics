#version 440

uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

uniform vec3 cameraPosition;

in vec2 vPosition;

void main()
{
	gl_Position = vec4(vPosition.x, vPosition.y, 0.0, 1.0);
}