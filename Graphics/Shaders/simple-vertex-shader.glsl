#version 130

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 lightPosition;

in vec3 vPosition;
in vec3 vNormal;
in vec2 vUV;
in vec4 vColor;

out vec3 fPosition;
out vec3 fNormal;
out vec3 fEyeDirection;
out vec3 fLightDirection;
out vec2 fUV;
out vec4 fColor;

void main()
{
    gl_Position = modelMatrix * viewMatrix * projectionMatrix * vec4(vPosition, 1.0f);
    
	fPosition = (modelMatrix * vec4(vPosition, 1.0f)).xyz;
	fNormal = (viewMatrix * modelMatrix * vec4(vNormal, 0.0f)).xyz;
	fEyeDirection = vec3(0.0f, 0.0f, 0.0f) - (viewMatrix * modelMatrix * vec4(vPosition, 1.0f)).xyz;
	fLightDirection = (viewMatrix * vec4(lightPosition, 1.0f)).xyz + fEyeDirection;
	fUV = vUV;
	fColor = vColor;
}