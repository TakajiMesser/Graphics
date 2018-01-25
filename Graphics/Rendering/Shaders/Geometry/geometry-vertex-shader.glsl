#version 440

const int MAX_JOINTS = 32;
const int MAX_WEIGHTS = 4;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 previousViewMatrix;
uniform mat4 previousProjectionMatrix;

uniform int useSkinning;
uniform mat4[MAX_JOINTS] jointTransforms;

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec3 vNormal;
layout(location = 2) in vec3 vTangent;
layout(location = 3) in vec4 vColor;
layout(location = 4) in vec2 vUV;
layout(location = 5) in int vMaterialIndex;
layout(location = 6) in vec4 vBoneIDs;
layout(location = 7) in vec4 vBoneWeights;

out vec3 fPosition;
out vec4 fClipPosition;
out vec4 fPreviousClipPosition;
out vec3 fNormal;
out vec3 fTangent;
out vec4 fColor;
out vec2 fUV;
flat out int fMaterialIndex;
out vec3 fCameraDirection;

void main()
{
    mat4 mvp = projectionMatrix * viewMatrix * modelMatrix;

    vec4 position = vec4(vPosition, 1.0);
    vec4 normal = vec4(vNormal, 1.0);
    vec4 tangent = vec4(vTangent, 1.0);

    if (useSkinning == 1)
    {
        mat4 jointTransform = mat4(0);
        for (int i = 0; i < MAX_WEIGHTS; i++)
        {
            jointTransform += jointTransforms[0] * vBoneWeights[i];
        }

        position = jointTransform * position;
        normal = jointTransform * normal;
        tangent = jointTransform * tangent;
    }

    fPosition = (modelMatrix * position).xyz;
    fClipPosition = mvp * position;
    fPreviousClipPosition = previousProjectionMatrix * previousViewMatrix * previousModelMatrix * position;
	fNormal = (modelMatrix * normal).xyz;
    fTangent = (modelMatrix * tangent).xyz;
    fColor = vColor;
    fUV = vUV;
    fMaterialIndex = vMaterialIndex;

    gl_Position = fClipPosition;

    // Model matrix maps vertices from "model coordinates" to "world coordinates"
	vec4 worldPosition = modelMatrix * vec4(vPosition, 1.0);

	// This is the vector from the current vertex to the camera
	fCameraDirection = (inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - worldPosition.xyz;
}