#version 150
// fragment

const int MAX_LIGHTS = 10;
const int MAX_MATERIALS = 10;

struct Light {
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

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
layout (std140) uniform LightBlock
{
	Light lights[MAX_LIGHTS];
};
layout (std140) uniform MaterialBlock
{
	Material materials[MAX_MATERIALS];
};

uniform sampler2D mainTexture;
uniform int useMainTexture;
uniform sampler2D normalMap;
uniform int useNormalMap;
//uniform float shine;
//uniform float reflectivity;

in vec3 fPosition;
in vec3 fNormal;
in vec3 fCameraDirection;
in vec3 fLightDirections[MAX_LIGHTS];
in vec2 fUV;
in vec4 fColor;
flat in int fMaterialIndex;

out vec4 color;

vec4 computeAmbientLight(vec3 ambient, vec3 lightColor, float illuminance)
{
	return vec4(illuminance * lightColor * ambient, 1.0);
}

vec4 computeDiffuseLight(vec3 diffuse, vec3 lightColor, float illuminance, vec3 unitNormal, vec3 unitLight)
{
	return vec4(illuminance * max(0.0, dot(unitNormal, unitLight)) * lightColor * diffuse, 1.0);
}

vec4 computeSpecularLight(vec3 specular, vec3 lightColor, float illuminance, vec3 unitNormal, vec3 unitLight, vec3 unitCamera, float specularExponent)
{
	return (dot(unitNormal, unitLight) <= 0.0)
		? vec4(0.0, 0.0, 0.0, 0.0)
		: vec4(illuminance
			* pow(max(0.0, dot(reflect(-unitLight, unitNormal), unitCamera)), specularExponent)
			* lightColor
			* specular, 1.0);
}

void main()
{
	vec4 ambientColor = vec4(0.0, 0.0, 0.0, 1.0);
	vec4 diffuseColor = vec4(0.0, 0.0, 0.0, 1.0);
	vec4 specularColor = vec4(0.0, 0.0, 0.0, 1.0);

	vec3 unitNormal = normalize(fNormal);
	vec3 unitCamera = normalize(fCameraDirection);

	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		float distance = length(fLightDirections[i].xy);//length(fLightDirections[i]);
		vec3 unitLight = normalize(fLightDirections[i]);

		// Reference -> https://imdoingitwrong.wordpress.com/2011/01/31/light-attenuation/
		float attenuation = (lights[i].radius > 0.0)
			? 1.0 / pow(1.0 + max(distance - lights[i].radius, 0.0) / lights[i].radius, 2.0)
			: 0.0;

		// Scale and bias attenuation such that a = 0 is the extent of max influence, and a = 1 when d = 0
		float cutOff = 0.001;
		attenuation = (attenuation - cutOff) / (1.0 - cutOff);
		attenuation = max(attenuation, 0);

		if (distance > lights[i].radius)
		{
			attenuation = 0.0;
		}

		float illuminance = (attenuation > 0.0)
			? lights[i].intensity / attenuation
			: 0.0;
		
		ambientColor += computeAmbientLight(materials[fMaterialIndex].ambient, lights[i].color, illuminance);
		diffuseColor += computeDiffuseLight(materials[fMaterialIndex].diffuse, lights[i].color, illuminance, unitNormal, unitLight);
		specularColor += computeSpecularLight(materials[fMaterialIndex].specular, lights[i].color, illuminance, unitNormal, unitLight, unitCamera, materials[fMaterialIndex].specularExponent);
	}

	vec4 lightColor = ambientColor + diffuseColor + specularColor;
	vec4 textureColor = texture(mainTexture, fUV);

	if (useMainTexture > 0)
	{
		color = mix(textureColor, fColor, 0.2);
	}
	else
	{
		color = fColor;
	}
	
	color = mix(lightColor, color, 0.1);
}