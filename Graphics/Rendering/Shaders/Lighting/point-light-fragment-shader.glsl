#version 440

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

uniform sampler2D positionMap;
uniform sampler2D colorMap;
uniform sampler2D normalDepth;
uniform sampler2D diffuseMaterial;
uniform sampler2D specularMap;

uniform vec3 lightPosition;
uniform float lightRadius;
uniform vec3 lightColor;
uniform float lightIntensity;
uniform vec3 cameraPosition;

layout(location = 0) out vec4 finalColor;

vec4 computeDiffuseLight(vec3 diffuse, float illuminance, vec3 unitNormal, vec3 unitLight)
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

vec4 computeSpecularLight(vec3 specular, float illuminance, vec3 unitNormal, vec3 unitLight, vec3 unitCamera, float specularExponent)
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

float calculateIlluminance(vec3 lightDirection)
{
	// Reference -> https://imdoingitwrong.wordpress.com/2011/01/31/light-attenuation/
    float distance = length(lightDirection);

    /*if (useNormalMap > 0)
    {
        vec4 normalDepth = 2.0 * texture(normalMap, fUV, -1.0) - 1.0;
        distance -= normalDepth.a;

        // Need to also consider that light direction is not just straight up...
        distance = min(distance, 0.0);
    }*/

    if (lightRadius <= 0.0 || distance > lightRadius)
    {
        return 0.0;
    }

    vec3 l = lightDirection / distance;
    float denom = max(distance - lightRadius, 0.0) / lightRadius + 1.0;
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
        return lightIntensity / attenuation;
    }
}

void main()
{
    // Easy to calculate texture coordinates in here
    vec2 resolution = textureSize(colorMap, 0);
	vec2 uv = gl_FragCoord.xy / resolution;

    vec3 position = texture(positionMap, uv).xyz;

    // Get color
    vec4 color = texture(colorMap, uv);

    // Get normal/depth
    vec4 normalAndDepth = texture(normalDepth, uv);
    vec3 unitNormal = normalAndDepth.xyz;
    //unitNormal = vec3(0, 0, 1);
    float depth = normalAndDepth.w;

    // Get diffuse/material
    vec4 diffuseAndMaterial = texture(diffuseMaterial, uv);
    vec3 diffuse = diffuseAndMaterial.rgb;
    float materialIndex = diffuseAndMaterial.a;

    // Get specular properties
    vec4 specular = texture(specularMap, uv);

    // vec3 lightDirection
    float illuminance = calculateIlluminance(lightPosition - position);

    // Get unit light vector (light)
    // Get unit camera vector (camera)
    vec3 unitCamera = normalize(cameraPosition - position);
    vec3 unitLight = normalize(lightPosition - position);

    vec4 diffuseLight = computeDiffuseLight(diffuse, illuminance, unitNormal, unitLight);
    vec4 specularLight = computeSpecularLight(specular.xyz, illuminance, unitNormal, unitLight, unitCamera, specular.z);

    finalColor = color * (diffuseLight + specularLight);
    /*finalColor = (illuminance > 0.0)
        ? vec4(1)
        : vec4(0);*/
}