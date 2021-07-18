#version 440

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

uniform float length;

in vec3 vPosition;

out vec2 fPosition;
out vec2 fUV;

void main()
{
	gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(vPosition, 1.0);
	
	fPosition = vPosition.xy * length;
	fUV = vPosition.xy;
}