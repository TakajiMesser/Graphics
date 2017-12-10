#version 150
// vertex

const int MAX_LIGHTS = 10;
const int MAX_MATERIALS = 10;

struct Light {
	vec3 position;
	float radius;
	vec3 color;
	float intensity;
};

struct Material {
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	float specularExponent;
};

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
layout (std140) uniform LightBlock
{
	Light lights[MAX_LIGHTS];
};
layout (std140) uniform MaterialBlock
{
	Material materials[MAX_MATERIALS];
};

in vec3 vPosition;
in vec3 vNormal;
in vec2 vUV;
in vec4 vColor;
in int vMaterialIndex;

out vec3 fPosition;
out vec3 fNormal;
out vec3 fCameraDirection;
out vec3 fLightDirections[MAX_LIGHTS];
out vec2 fUV;
out vec4 fColor;
flat out int fMaterialIndex;

void main()
{
    gl_Position = modelMatrix * viewMatrix * projectionMatrix * vec4(vPosition, 1.0f);
    
	fPosition = (modelMatrix * vec4(vPosition, 1.0f)).xyz;
	fNormal = (viewMatrix * modelMatrix * vec4(vNormal, 0.0f)).xyz;
	fCameraDirection = vec3(0.0f, 0.0f, 0.0f) - (viewMatrix * modelMatrix * vec4(vPosition, 1.0f)).xyz;
	
	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		fLightDirections[i] = (viewMatrix * vec4(lights[i].position, 1.0f)).xyz + fCameraDirection;
	}
	
	fUV = vUV;
	fColor = vColor;
	fMaterialIndex = vMaterialIndex;
}