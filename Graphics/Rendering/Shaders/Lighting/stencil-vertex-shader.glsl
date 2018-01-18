#version 440

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 previousViewMatrix;
uniform mat4 previousProjectionMatrix;

in vec3 vPosition;
in vec3 vNormal;
in vec3 vTangent;
in vec4 vColor;
in vec2 vUV;
in int vMaterialIndex;

out vec4 fPosition;
out vec3 fNormal;
out vec4 fColor;
out vec2 fUV;
flat out int fMaterialIndex;
out vec3 fCameraDirection;
out vec3 fLightDirections[MAX_LIGHTS];

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

    fPosition = projectionMatrix * viewMatrix * modelMatrix * position;
    fPreviousPosition = previousProjectionMatrix * previousViewMatrix * previousModelMatrix * position;
	fNormal = toTangentSpace * (modelMatrix * normal).xyz;
    fColor = vColor;
    fUV = vUV;
    fMaterialIndex = vMaterialIndex;

    gl_Position = fPosition;

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