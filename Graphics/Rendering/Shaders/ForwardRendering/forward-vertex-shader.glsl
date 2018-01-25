#version 440

const int MAX_POINT_LIGHTS = 10;
const int MAX_JOINTS = 32;
const int MAX_WEIGHTS = 4;

struct PointLight {
	vec3 position;
	float radius;
	vec3 color;
	float intensity;
};

layout (std140) uniform PointLightBlock
{
	PointLight pointLights[MAX_POINT_LIGHTS];
};

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 previousViewMatrix;
uniform mat4 previousProjectionMatrix;

uniform int useSkinning;
uniform mat4[MAX_JOINTS] jointTransforms;

in vec3 vPosition;
in vec3 vNormal;
in vec3 vTangent;
in vec4 vColor;
in vec2 vUV;
in int vMaterialIndex;
in ivec4 vBoneIDs;
in vec4 vBoneWeights;

out vec4 fPosition;
out vec4 fPreviousPosition;
out vec3 fNormal;
out vec3 fTangent;
out vec4 fColor;
out vec2 fUV;
flat out int fMaterialIndex;
out vec3 fCameraDirection;
out vec3 fLightDirections[MAX_POINT_LIGHTS];

void main()
{
    mat4 mvp = projectionMatrix * viewMatrix * modelMatrix;

    vec4 position = vec4(vPosition, 1.0);
    vec4 normal = vec4(vNormal, 0.0);
    vec4 tangent = vec4(vTangent, 0.0);

    if (useSkinning == 1)
    {
        mat4 jointTransform = mat4(0);
        for (int i = 0; i < MAX_WEIGHTS; i++)
        {
            jointTransform += jointTransforms[vBoneIDs[i]] * vBoneWeights[i];
        }

        position = jointTransform * position;
        normal = jointTransform * normal;
        tangent = jointTransform * tangent;
    }

    fPosition = mvp * position;
    fPreviousPosition = previousProjectionMatrix * previousViewMatrix * previousModelMatrix * position;
	fNormal = (modelMatrix * normal).xyz;
    fTangent = (modelMatrix * tangent).xyz;
    fColor = vColor;
    fUV = vUV;
    fMaterialIndex = vMaterialIndex;

    gl_Position = fPosition;

	// Model matrix maps vertices from "model coordinates" to "world coordinates"
	vec4 worldPosition = modelMatrix * vec4(vPosition, 1.0);

	// This is the vector from the current vertex to the camera
	fCameraDirection = (inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - worldPosition.xyz;

	for (int i = 0; i < MAX_POINT_LIGHTS; i++)
	{
		// Wouldn't we want the direction that the light is coming from? This would be the current position MINUS the light source
		// This is the vector from the current vertex TO the light source
		fLightDirections[i] = pointLights[i].position - worldPosition.xyz;
	}
}