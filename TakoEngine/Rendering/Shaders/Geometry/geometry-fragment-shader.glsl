#version 440

uniform vec3 ambientColor;
uniform vec3 diffuseColor;
uniform vec3 specularColor;
uniform float specularExponent;

uniform sampler2D diffuseMap;
uniform sampler2D normalMap;
uniform sampler2D specularMap;
uniform sampler2D parallaxMap;

uniform int useDiffuseMap;
uniform int useNormalMap;
uniform int useSpecularMap;
uniform int useParallaxMap;

uniform vec3 cameraPosition;

in vec3 fPosition;
in vec4 fClipPosition;
in vec4 fPreviousClipPosition;
in vec3 fNormal;
in vec3 fTangent;
in vec4 fColor;
in vec2 fUV;

layout(location = 0) out vec3 position;
layout(location = 1) out vec4 color;
layout(location = 2) out vec3 normal;
layout(location = 3) out vec3 diffuse;
layout(location = 4) out vec4 specular;
layout(location = 5) out vec2 velocity;
layout(location = 6) out vec4 finalColor;

vec3 calculateNormal(vec2 texCoords)
{
    vec3 nNormal = normalize(fNormal);
    vec3 nTangent = normalize(fTangent);

    // Turn into an orthonormal basis by the Gramm-Schmidt process
    nTangent = normalize(nTangent - dot(nTangent, nNormal) * nNormal);
    
    vec3 nBitangent = cross(nTangent, nNormal);
    mat3 tbn = mat3(nTangent, nBitangent, nNormal);

    vec4 bumpedNormal = 2.0 * texture(normalMap, texCoords, -1.0) - 1.0;
    return normalize(tbn * bumpedNormal.rgb);
}

vec2 calculateParallaxMap(vec2 texCoords)
{
    vec3 nNormal = normalize(fNormal);
    vec3 nTangent = normalize(fTangent);

    // Turn into an orthonormal basis by the Gramm-Schmidt process
    nTangent = normalize(nTangent - dot(nTangent, nNormal) * nNormal);
    
    vec3 nBitangent = cross(nTangent, nNormal);
    mat3 tbn = mat3(nTangent, nBitangent, nNormal);

    vec3 tangentViewPosition = tbn * cameraPosition;
    vec3 tangentFragPosition = tbn * fPosition;
    vec3 viewDirection = normalize(tangentViewPosition - tangentFragPosition);

    float height = texture(parallaxMap, texCoords).r;
    float heightScale = 0.02;
    vec2 p = viewDirection.xy / viewDirection.z * (height * heightScale);

    vec2 displacedTexCoords = texCoords - p;
    if (displacedTexCoords.x > 1.0 || displacedTexCoords.x < 0.0 || displacedTexCoords.y > 1.0 || displacedTexCoords.y < 0.0)
    {
        discard;
    }

    return displacedTexCoords;
}

void main()
{
    position = fPosition;

    vec2 texCoords = (useParallaxMap > 0) ? calculateParallaxMap(fUV) : fUV;

    color = (useDiffuseMap > 0) ? texture(diffuseMap, texCoords) : fColor;
	normal = (useNormalMap > 0) ? calculateNormal(texCoords) : normalize(fNormal);

    diffuse = diffuseColor;
    specular = (useSpecularMap > 0)
        ? vec4(texture(specularMap, texCoords))
        : vec4(specularColor, specularExponent);

    vec2 a = (fClipPosition.xy / fClipPosition.w) * 0.5 + 0.5;
    vec2 b = (fPreviousClipPosition.xy / fPreviousClipPosition.w) * 0.5 + 0.5;
    velocity = a - b;

    finalColor = color * vec4(ambientColor, 1.0);
}