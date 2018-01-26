#version 440

const int MAX_MATERIALS = 10;

struct Material {
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	float specularExponent;
};

uniform sampler2D mainTexture;
uniform sampler2D normalMap;
uniform sampler2D diffuseMap;
uniform sampler2D specularMap;

uniform int useMainTexture;
uniform int useNormalMap;
uniform int useDiffuseMap;
uniform int useSpecularMap;

layout (std140) uniform MaterialBlock
{
	Material materials[MAX_MATERIALS];
};

in vec3 fPosition;
in vec4 fClipPosition;
in vec4 fPreviousClipPosition;
in vec3 fNormal;
in vec3 fTangent;
in vec4 fColor;
in vec2 fUV;
flat in int fMaterialIndex;

layout(location = 0) out vec3 position;
layout(location = 1) out vec4 color;
layout(location = 2) out vec3 normal;
layout(location = 3) out vec4 diffuseMaterial;
layout(location = 4) out vec4 specular;
layout(location = 5) out vec2 velocity;
layout(location = 6) out vec4 finalColor;

vec3 calculateNormal()
{
    vec3 nNormal = normalize(fNormal);
    vec3 nTangent = normalize(fTangent);

    // Turn into an orthonormal basis by the Gramm-Schmidt process
    nTangent = normalize(nTangent - dot(nTangent, nNormal) * nNormal);
    
    vec3 nBitangent = cross(nTangent, nNormal);
    mat3 tbn = mat3(nTangent, nBitangent, nNormal);

    vec4 bumpedNormal = 2.0 * texture(normalMap, fUV, -1.0) - 1.0;
    return normalize(tbn * bumpedNormal.rgb);
}

void main()
{
    position = fPosition;

    color = (useMainTexture > 0) ? texture(mainTexture, fUV) : fColor;

	normal = (useNormalMap > 0) ? calculateNormal() : normalize(fNormal);

    diffuseMaterial = (useDiffuseMap > 0)
        ? vec4(texture(diffuseMap, fUV).xyz, fMaterialIndex)
        : vec4(materials[fMaterialIndex].diffuse, fMaterialIndex);

    specular = (useSpecularMap > 0)
        ? vec4(texture(specularMap, fUV))
        : vec4(materials[fMaterialIndex].specular, materials[fMaterialIndex].specularExponent);

    vec2 a = (fClipPosition.xy / fClipPosition.w) * 0.5 + 0.5;
    vec2 b = (fPreviousClipPosition.xy / fPreviousClipPosition.w) * 0.5 + 0.5;
    velocity = a - b;

    finalColor = color * vec4(materials[fMaterialIndex].ambient, 1.0);
}