#version 440

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
    float lightDistance = length(lightDirection);
    // TODO - Consider normal map distance in distance calculation as well?

    // Typical function is 1 / (1 + k * d ^ 2), where k is the base quadratic attenuation coefficient
    // Issue with this is that the attenuation never actually equals zero, so typically we have some cutoff (like .01)
    // Another approach is 1 - d ^ 2 / r ^ 2 -> For 0.01 -> (r ^ 2 - d ^ 2) / (r ^ 2 + 99 * d ^ 2), is clamped to range of 0 to 1
    if (lightRadius > 0.0 && lightDistance < lightRadius)
    {
        // Attenuation is the percentage of remaining light, from 0.0 to 1.0
        float attenuation = smoothstep(lightRadius, 0.0, lightDistance);

        //float denom = max(lightDistance - lightRadius, 0.0) / lightRadius + 1.0;
        //float attenuation = 1.0 / (denom * denom);
	    //attenuation = (attenuation - 0.001) / (1.0 - 0.001);

        //float r2 = lightRadius * lightRadius;
        //float d2 = lightDistance * lightDistance;
        //float attenuation = 1 - d2 / r2;
        //float attenuation = (r2 - d2) / (r2 + 99 * d2);

	    if (attenuation > 0.0)
	    {
		    return lightIntensity * attenuation;
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

    vec3 lightDirection = lightPosition - position;
    float illuminance = calculateIlluminance(lightDirection);
    vec3 unitCamera = normalize(cameraPosition - position);
    vec3 unitLight = normalize(lightDirection);

    vec4 diffuseLight = computeDiffuseLight(diffuse, illuminance, unitNormal, unitLight);
    vec4 specularLight = computeSpecularLight(specular.xyz, illuminance, unitNormal, unitLight, unitCamera, specular.z);

    finalColor = color * (diffuseLight + specularLight);

    // If these two vectors are parallel, color the pixel white
    // TODO - Remove this, it is purely for debugging purposes
    vec3 cameraToLight = lightPosition - cameraPosition;
    vec3 cameraToPosition = position - cameraPosition;

    float cosAngle = dot(cameraToLight, cameraToPosition) / (length(cameraToLight) * length(cameraToPosition));
    float angle = acos(cosAngle);

    if (angle <= 0.02)
    {
        finalColor = vec4(0, 1, 0, 1);
    }
}