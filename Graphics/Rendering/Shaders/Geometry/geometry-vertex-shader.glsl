#version 150

uniform mat4 modelMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

uniform int useSkinning;
uniform mat4[32] boneMatrices;

in vec3 vPosition;
in vec3 vNormal;
in vec3 vTangent;
in vec4 vColor;
in vec2 vUV;
in int vMaterialIndex;
in vec4 vBoneIDs;
in vec4 vBoneWeights;

out vec3 cPosition;
out vec3 cPreviousPosition;
out vec3 cNormal;
out vec3 cTangent;
out vec4 cColor;
out vec2 cUV;
flat out int cMaterialIndex;

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

    cPosition = toTangentSpace * (modelMatrix * vec4(vPosition, 0.0)).xyz;
    cPreviousPosition = toTangentSpace * (previousModelMatrix * vec4(vPosition, 0.0)).xyz;
	cNormal = toTangentSpace * (modelMatrix * vec4(vNormal, 0.0)).xyz;
    cColor = vColor;
    cUV = vUV;
    cMaterialIndex = vMaterialIndex;
}