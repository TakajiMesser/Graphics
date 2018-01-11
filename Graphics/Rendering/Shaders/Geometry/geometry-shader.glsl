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
in vec3 vTangent;
in vec4 vColor;
in vec2 vUV;
in int vMaterialIndex;

out vec3 fNormal;
out vec4 fColor;
out vec2 fUV;
flat out int fMaterialIndex;
out vec3 fCameraDirection;
out vec3 fLightDirections[MAX_LIGHTS];

mat3 GetTangentMatrix()
{
    vec3 normal = normalize(vNormal);
    vec3 tangent = normalize(viewMatrix * modelMatrix * vec4(vTangent, 0.0)).xyz;
    vec3 bitangent = normalize(cross(normal, tangent));

    return mat3(
        tangent.x, bitangent.x, normal.x,
        tangent.y, bitangent.y, normal.y,
        tangent.z, bitangent.z, normal.z
    );
}

void main()
{
	gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(vPosition, 1.0);

    mat3 toTangentSpace = GetTangentMatrix();

	fNormal = toTangentSpace * (modelMatrix * vec4(vNormal, 0.0)).xyz;
    fColor = vColor;
    fUV = vUV;
    fMaterialIndex = vMaterialIndex;

	// Model matrix maps vertices from "model coordinates" to "world coordinates"
	vec4 vertexPosition_world = modelMatrix * vec4(vPosition, 1.0);

	// This is the vector from the current vertex to the camera
	fCameraDirection = toTangentSpace * ((inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - vertexPosition_world.xyz);

	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		// Wouldn't we want the direction that the light is coming from? This would be the current position MINUS the light source
		// This is the vector from the current vertex TO the light source
		fLightDirections[i] = toTangentSpace * (lights[i].position - vertexPosition_world.xyz);
	}
}