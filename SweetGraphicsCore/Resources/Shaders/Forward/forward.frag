#version 440

const int MAX_POINT_LIGHTS = 10;
const int MAX_MATERIALS = 10;

struct PointLight {
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

layout (std140) uniform PointLightBlock
{
	PointLight pointLights[MAX_POINT_LIGHTS];
};
layout (std140) uniform MaterialBlock
{
	Material materials[MAX_MATERIALS];
};

uniform sampler2D mainTexture;
uniform sampler2D normalMap;
uniform sampler2D diffuseMap;
uniform sampler2D specularMap;

uniform int useMainTexture;
uniform int useNormalMap;
uniform int useDiffuseMap;
uniform int useSpecularMap;

in vec4 fPosition;
in vec4 fPreviousPosition;
in vec3 fNormal;
in vec3 fTangent;
in vec4 fColor;
in vec2 fUV;
flat in int fMaterialIndex;
in vec3 fCameraDirection;
in vec3 fLightDirections[MAX_POINT_LIGHTS];

layout(location = 0) out vec4 finalColor;
layout(location = 1) out vec2 velocity;

vec4 computeAmbientLight(vec3 ambient, vec3 lightColor)
{
    if (ambient == vec3(0.05, 0.05, 0.05))
    {
        return vec4(lightColor * ambient, 1.0);
    }
    else
    {
        return vec4(0, 0, 0, 1.0);
    }
}

vec4 computeDiffuseLight(vec3 diffuse, vec3 lightColor, float illuminance, vec3 unitNormal, vec3 unitLight)
{
    if (useDiffuseMap > 0)
    {
        return vec4(texture(diffuseMap, fUV).xyz, 1.0);
    }
    else
    {
        float diffuseFactor = dot(unitNormal, unitLight);
        if (diffuseFactor <= 0)
        {
            return vec4(0);
        }
        else
        {
            return vec4(illuminance * diffuseFactor * lightColor * diffuse, 1.0);
        }
    }
}

vec4 computeSpecularLight(vec3 specular, vec3 lightColor, float illuminance, vec3 unitNormal, vec3 unitLight, vec3 unitCamera, float specularExponent)
{
    if (useSpecularMap > 0)
    {
        return vec4(texture(specularMap, fUV).xyz, 1.0);
    }
    else
    {
        if (dot(unitNormal, unitLight) <= 0.0)
        {
            return vec4(0);
        }
        else
        {
            vec3 lightReflect = reflect(-unitLight, unitNormal);
            float specularFactor = dot(lightReflect, unitCamera);

            if (specularFactor <= 0.0)
            {
                return vec4(0);
            }
            else
            {
                return vec4(illuminance
			        * pow(specularFactor, specularExponent)
			        * lightColor
			        * specular, 1.0);
            }
        }
    }
}

float calculateIlluminance(vec3 lightDirection, float radius, float intensity)
{
	// Reference -> https://imdoingitwrong.wordpress.com/2011/01/31/light-attenuation/
    float distance = length(lightDirection);

    if (useNormalMap > 0)
    {
        vec4 normalDepth = 2.0 * texture(normalMap, fUV, -1.0) - 1.0;
        distance -= normalDepth.a;

        // Need to also consider that light direction is not just straight up...
        distance = min(distance, 0.0);
    }

    if (radius <= 0.0 || distance > radius)
    {
        return 0.0;
    }

    vec3 l = lightDirection / distance;
    float denom = max(distance - radius, 0.0) / radius + 1.0;
    float attenuation = 1.0 / (denom * denom);

	// Scale and bias attenuation such that a = 0 is the extent of max influence, and a = 1 when d = 0
	float cutOff = 0.001;
	attenuation = (attenuation - cutOff) / (1.0 - cutOff);
	attenuation = max(attenuation, 0);

	if (attenuation <= 0.0)
	{
		return 0.0;
	}
    else
    {
        return intensity / attenuation;
    }
}

vec3 calculateNormal()
{
    vec3 nNormal = normalize(fNormal);
    vec3 nTangent = normalize(fTangent);

    // Turn into an orthonormal basis by the Gramm-Schmidt process
    nTangent = normalize(nTangent - dot(nTangent, nNormal) * nNormal);
    
    vec3 nBitangent = cross(nTangent, nNormal);

    mat3 tbn = mat3(nTangent, nBitangent, nNormal);

    vec4 normalDepth = 2.0 * texture(normalMap, fUV, -1.0) - 1.0;
    return normalize(tbn * normalDepth.rgb);
}

void main()
{
	vec4 ambientColor = vec4(0.0, 0.0, 0.0, 1.0);
	vec4 diffuseColor = vec4(0.0, 0.0, 0.0, 1.0);
	vec4 specularColor = vec4(0.0, 0.0, 0.0, 1.0);

	vec3 unitNormal = (useNormalMap > 0) ? calculateNormal() : normalize(fNormal);
	vec3 unitCamera = normalize(fCameraDirection);

	for (int i = 0; i < MAX_POINT_LIGHTS; i++)
	{
        vec3 unitLight = normalize(fLightDirections[i]);
		float illuminance = calculateIlluminance(fLightDirections[i], pointLights[i].radius, pointLights[i].intensity);
		
		ambientColor += computeAmbientLight(materials[fMaterialIndex].ambient, pointLights[i].color);
		diffuseColor += computeDiffuseLight(materials[fMaterialIndex].diffuse, pointLights[i].color, illuminance, unitNormal, unitLight);
		specularColor += computeSpecularLight(materials[fMaterialIndex].specular, pointLights[i].color, illuminance, unitNormal, unitLight, unitCamera, materials[fMaterialIndex].specularExponent);
	}

	vec4 lightColor = ambientColor + diffuseColor + specularColor;
	vec4 textureColor = texture(mainTexture, fUV);

    finalColor = (useMainTexture > 0) ? textureColor : fColor;
	finalColor *= lightColor;//mix(lightColor, finalColor, 0.2);

    vec2 a = (fPosition.xy / fPosition.w) * 0.5 + 0.5;
    vec2 b = (fPreviousPosition.xy / fPreviousPosition.w) * 0.5 + 0.5;
    velocity = a - b;
}