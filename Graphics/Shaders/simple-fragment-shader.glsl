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

uniform sampler2D textureSampler;
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

vec4 computeDiffuseLight(vec3 diffuse, vec3 lightColor, float illuminance, vec3 surfaceNormal, vec3 lightDirection)
{
	return vec4(illuminance * max(0.0, dot(surfaceNormal, lightDirection)) * lightColor * diffuse, 1.0);
}

vec4 computeSpecularLight(vec3 specular, vec3 lightColor, float illuminance, vec3 surfaceNormal, vec3 lightDirection, vec3 viewVector, float specularExponent)
{
	vec3 reflectionVector = reflect(-normalize(lightDirection), normalize(surfaceNormal));
	//mediump vec3 reflectionVector = 2.0 * dot(lightDirection, surfaceNormal) * surfaceNormal - lightDirection;

	//return pow(max(0.0, dot(reflectionVector, unitCamera)), specularExponent) * reflectivity * lightColor;
	return (dot(surfaceNormal, lightDirection) <= 0.0)
		? vec4(0.0, 0.0, 0.0, 0.0)
		: illuminance * lightColor * specular 
			* pow(max(0.0, dot(reflectionVector, viewVector)), specularExponent);
}

void main()
{
	vec4 ambientColor = vec4(0.0, 0.0, 0.0, 1.0);
	vec4 diffuseColor = vec4(0.0, 0.0, 0.0, 1.0);
	vec4 specularColor = vec4(0.0, 0.0, 0.0, 1.0);

	vec3 viewVector = normalize(fCameraDirection);
	//mediump vec3 viewVector = normalize(-surfacePosition.xyz);

	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		float distance = length(fLightDirections[i]);

		// Reference -> https://imdoingitwrong.wordpress.com/2011/01/31/light-attenuation/

		float attenuation = (lights[i].radius > 0.0)
			? 1 / pow(1 + max(distance - lights[i].radius, 0) / lights[i].radius, 2.0)
			: 0.0;

		float illuminance = (lights[i].radius > 0)
			? lights[i].intensity / attenuation
			: 0.0;

		ambientColor += computeAmbientLight(materials[fMaterialIndex].ambient, lights[i].color, illuminance);
		diffuseColor += computeDiffuseLight(materials[fMaterialIndex].diffuse, lights[i].color, illuminance, fNormal, fLightDirections[i]);
		specularColor += computeSpecularLight(materials[fMaterialIndex].specular, lights[i].color, illuminance, fNormal, fLightDirections[i], viewVector, materials[fMaterialIndex].specularExponent);
	}

	//vec4 textureColor = texture(textureSampler, fUV);

	//vec4 mixedColor = mix(textureColor, ambientColor + diffuseColor + specularColor, 0.3);
	vec4 mixedColor = ambientColor + diffuseColor + specularColor;
	//vec4 mixedColor = vec4(totalDiffuse, 1.0) * textureColor + vec4(totalSpecular, 1.0);

	/*color = (lights[0].position.x == 0.0 
		&& lights[0].position.y == 0.0 
		&& lights[0].position.z == -1.5
		&& lights[0].color.x == 1.0
		&& lights[0].color.y == 1.0
		&& lights[0].color.z == 1.0
		&& lights[0].attenuation.x == 5.0
		&& lights[0].attenuation.y == 5.0
		&& lights[0].attenuation.z == 5.0
		&& lights[0].intensity == 1.0)
		? mix(fColor, mixedColor, 0.5) 
		: fColor;*/
	color = mix(fColor, mixedColor, 0.5);
    //color = fColor;
}