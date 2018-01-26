#version 440

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

uniform sampler2D positionMap;
uniform sampler2D colorMap;
uniform sampler2D normalMap;
uniform sampler2D diffuseMaterial;
uniform sampler2D specularMap;

uniform vec3 cameraPosition;

uniform vec3 lightPosition;
uniform float lightRadius;
uniform vec3 lightColor;
uniform float lightIntensity;

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
    if (dot(unitNormal, unitLight) > 0.0)
    {
        vec3 lightReflect = normalize(reflect(-unitLight, unitNormal));
        float specularFactor = dot(unitCamera, lightReflect);

        if (specularFactor > 0.0)
        {
            return vec4(illuminance * lightColor * specular
			    * pow(specularFactor, specularExponent), 1.0);
        }
        else
        {
            return vec4(0);
        }
    }
    else
    {
        return vec4(0);
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

    // Typical function is 1 / (1 + k * d ^ 2), where k is the base quadratic attenuation coefficient
    // Issue with this is that the attenuation never actually equals zero, so typically we have some cutoff (like .01)
    // Another approach is 1 - d ^ 2 / r ^ 2 -> For 0.01 -> (r ^ 2 - d ^ 2) / (r ^ 2 + 99 * d ^ 2), is clamped to range of 0 to 1

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
    vec2 resolution = textureSize(positionMap, 0);
	vec2 uv = gl_FragCoord.xy / resolution;

    vec3 position = texture(positionMap, uv).xyz;

    // Get color
    vec4 color = texture(colorMap, uv);

    // Get normal/depth
    vec4 normal = texture(normalMap, uv);
    vec3 unitNormal = normalize(normal.xyz);
    //unitNormal = vec3(0, 0, 1);

    // Get diffuse/material
    vec4 diffuseAndMaterial = texture(diffuseMaterial, uv);
    vec3 diffuse = diffuseAndMaterial.rgb;
    float materialIndex = diffuseAndMaterial.a;

    // Get specular properties
    vec4 specular = texture(specularMap, uv);

    vec3 lightDirection = lightPosition - position;
    float illuminance = calculateIlluminance(lightDirection);
    vec3 unitCamera = normalize(cameraPosition - position);
    vec3 unitLight = normalize(lightDirection);

    vec4 diffuseLight = computeDiffuseLight(diffuse, illuminance, unitNormal, unitLight);
    vec4 specularLight = computeSpecularLight(specular.xyz, illuminance, unitNormal, unitLight, unitCamera, specular.z);

    finalColor = color * (diffuseLight + specularLight);

    /*if (unitNormal == vec3(0, 0, 1))
    {
        finalColor = vec4(1.0);
    }
    finalColor = vec4(unitNormal, 1.0);*/

    /*finalColor = (length(lightDirection) < lightRadius)
        ? color * vec4(1.0 - length(lightDirection) / lightRadius)
        : color;*/

    // If these two vectors are parallel, color the pixel white
    vec3 cameraToLight = lightPosition - cameraPosition;
    vec3 cameraToPosition = position - cameraPosition;

    float cosAngle = dot(cameraToLight, cameraToPosition) / (length(cameraToLight) * length(cameraToPosition));
    float angle = acos(cosAngle);

    if (angle <= 0.02)
    {
        finalColor = vec4(1.0);
    }
}