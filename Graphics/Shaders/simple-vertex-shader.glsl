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
    gl_Position = modelMatrix * viewMatrix * projectionMatrix * vec4(vPosition, 1.0);
    
	fPosition = (modelMatrix * vec4(vPosition, 1.0)).xyz;
	//fNormal = (viewMatrix * modelMatrix * vec4(vNormal, 0.0)).xyz;
	fNormal = (modelMatrix * vec4(vNormal, 0.0)).xyz;

	// Model matrix maps vertices from "model coordinates" to "world coordinates"
	vec4 vertexPosition_world = modelMatrix * vec4(vPosition, 1.0);

	// This is the vector from the current vertex to the camera
	fCameraDirection = (inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - vertexPosition_world.xyz;
	//fCameraDirection = vec3(0,0,0) - (viewMatrix * modelMatrix * vec4(vPosition, 1.0)).xyz;

	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		// Wouldn't we want the direction that the light is coming from? This would be the current position MINUS the light source
		// This is the vector from the current vertex TO the light source
		fLightDirections[i] = lights[i].position - vertexPosition_world.xyz;

		//fLightDirections[i] = (viewMatrix * vec4(lights[i].position, 1.0)).xyz + fCameraDirection;
	}
	
	fUV = vUV;
	fColor = vColor;
	fMaterialIndex = vMaterialIndex;
}