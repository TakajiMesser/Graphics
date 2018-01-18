#version 150

const int MAX_JOINTS = 32;
const int MAX_WEIGHTS = 4;

uniform mat4 modelMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

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

out vec3 cPosition;
out vec3 cPreviousPosition;
out vec3 cNormal;
out vec3 cTangent;
out vec4 cColor;
out vec2 cUV;
flat out int cMaterialIndex;

mat3 GetTangentMatrix(vec4 normal, vec4 tangent)
{
    vec4 nNormal = normalize(normal);
    vec4 nTangent = normalize(viewMatrix * modelMatrix * tangent);
    vec3 nBitangent = normalize(cross(nNormal.xyz, nTangent.xyz));

    return mat3(
        nTangent.x, nBitangent.x, nNormal.x,
        nTangent.y, nBitangent.y, nNormal.y,
        nTangent.z, nBitangent.z, nNormal.z
    );
}

void main()
{
	//gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(vPosition, 1.0);
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

    mat3 toTangentSpace = GetTangentMatrix(normal, tangent);

    cPosition = toTangentSpace * (modelMatrix * position).xyz;
    cPreviousPosition = toTangentSpace * (previousModelMatrix * position).xyz;
	cNormal = toTangentSpace * (modelMatrix * normal).xyz;
    cColor = vColor;
    cUV = vUV;
    cMaterialIndex = vMaterialIndex;
}