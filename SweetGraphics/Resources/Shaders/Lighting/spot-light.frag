#version 440

uniform sampler2D positionMap;
uniform sampler2D colorMap;
uniform sampler2D normalMap;
uniform sampler2D diffuseMaterial;
uniform sampler2D specularMap;
uniform sampler2D shadowMap;

uniform mat4 lightMatrix;

uniform vec3 cameraPosition;
uniform vec3 lightPosition;
uniform float lightRadius;
uniform float lightHeight;
uniform vec3 lightVector;
uniform float lightCutoffAngle;
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

float calculateShadowFactor(vec3 position)
{
    // Convert world position to light-space
    vec4 lightSpace = lightMatrix * vec4(position, 1.0);

    // Convert coordinate to NDC space (-1 to 1)
    vec3 projCoords = lightSpace.xyz / lightSpace.w;

    // Convert range to [0 to 1]
    vec3 uvCoords = projCoords * vec3(0.5) + vec3(0.5);

    float depth = texture(shadowMap, uvCoords.xy).x;
    float z = uvCoords.z;

    // If the depth from the shadow map is LESS than the depth of the current pixel,
    // then this pixel is in shadow
    const float bias = 0.00001;
    return (depth < z - bias)
        ? 0.5
        : 1.0;
}

float calculateIlluminance(vec3 lightToPixel, vec3 position)
{
    float spotFactor = dot(normalize(lightToPixel), normalize(lightVector));

    if (spotFactor > lightCutoffAngle)
    {
        float attenuation = 1.0 - (1.0 - spotFactor) * 1.0 / (1.0 - lightCutoffAngle);
        if (attenuation > 0.0)
	    {
            float shadowFactor = calculateShadowFactor(position);
		    return lightIntensity * attenuation * shadowFactor;
	    }
    }
    
    return 0.0;
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

    // Get diffuse/material
    vec4 diffuseAndMaterial = texture(diffuseMaterial, uv);
    vec3 diffuse = diffuseAndMaterial.rgb;
    float materialIndex = diffuseAndMaterial.a;

    // Get specular properties
    vec4 specular = texture(specularMap, uv);

    vec3 lightToPixel = position - lightPosition;
    float illuminance = calculateIlluminance(lightToPixel, position);

    vec3 unitLight = normalize(lightPosition - position);
    vec3 unitCamera = normalize(cameraPosition - position);

    vec4 diffuseLight = computeDiffuseLight(diffuse, illuminance, unitNormal, unitLight);
    vec4 specularLight = computeSpecularLight(specular.xyz, illuminance, unitNormal, unitLight, unitCamera, specular.z);

    finalColor = color * (diffuseLight + specularLight);

    // If these two vectors are parallel, color the pixel white
    // TODO - Remove this, it is purely for debugging purposes
    /*vec3 cameraToLight = lightPosition - cameraPosition;
    vec3 cameraToPosition = position - cameraPosition;

    float cosAngle = dot(cameraToLight, cameraToPosition) / (length(cameraToLight) * length(cameraToPosition));
    float angle = acos(cosAngle);

    if (angle <= 0.02)
    {
        finalColor = vec4(1, 0, 0, 1);
    }*/
}