#version 400

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 previousViewMatrix;
uniform mat4 previousProjectionMatrix;
uniform vec3 cameraPosition;

uniform int useDisplacementTexture;
uniform int displacementTextureUnit;
uniform float displacementStrength = 0.1;
uniform vec2 renderSize;

/*layout(std140) uniform MaterialTextures
{
	sampler2D bindlessTexture[80];
};*/

uniform int useWireframe;

uniform vec3 diffuseColor;
uniform float emissionStrength;
uniform vec3 specularColor;
uniform float specularShininess;

uniform sampler2D mainTexture;
uniform sampler2D normalMap;
uniform sampler2D diffuseMap;
uniform sampler2D specularMap;
uniform sampler2D parallaxTexture;

uniform int useMainTexture;
uniform int useNormalMap;
uniform int useDiffuseMap;
uniform int useSpecularMap;
uniform int useParallaxTexture;

in vec3 fPosition;
in vec3 fPreviousPosition;
in vec3 fNormal;
in vec3 fTangent;
in vec4 fColor;
in vec2 fUV;
flat in int fMaterialIndex;
in vec3 fCameraPosition;
in vec4 fClipPosition;
in vec4 fPreviousClipPosition;
noperspective in vec3 fWireframeDistance;

out vec4 diffuseID;
out vec4 normalDepth;
out vec4 specular;
out vec2 velocity;

float calcLinearDepth(float depth, float near, float far)
{
	depth = depth * 2.0 - 1.0;
	float linearDepth = (2.0 * near * far) / (far + near - depth * (far - near));

	linearDepth = depth / (far - near);

	return linearDepth;
}

float calcRealDepth(float linearDepth, float near, float far)
{
	float depth = linearDepth * (far - near);
	return depth;
}

mat3 GetTangentMatrix()
{
    vec3 normal = normalize(fNormal);
    vec3 tangent = normalize(viewMatrix * modelMatrix * vec4(fTangent, 0.0)).xyz;
    vec3 bitangent = normalize(cross(normal, tangent));

    return mat3(
        tangent.x, bitangent.x, normal.x,
        tangent.y, bitangent.y, normal.y,
        tangent.z, bitangent.z, normal.z
    );
}

vec3 calcNormalMapping(sampler2D normal_texture, vec2 tex_coords, mat3 TBN)
{
	vec3 mapNormal = texture(normal_texture, tex_coords).xyz;
	mapNormal = 2.0 * mapNormal - vec3(1.0);

	vec3 finalNormal;
	finalNormal = TBN * mapNormal;
	return normalize(finalNormal);
}

vec2 calcParallaxMapping(sampler2D parallax_texture, vec2 tex_coords, mat3 TBN, vec3 camera_position, vec3 world_position)
{ 
	float height_scale = 0.02;

	mat3 tTBN = transpose(TBN);

	vec3 t_camPosition = tTBN * camera_position;
	vec3 t_worldPosition = tTBN * world_position;

	vec3 viewDir = normalize(-t_camPosition - t_worldPosition);

	// number of depth layers
	const float minLayers = 5;
	const float maxLayers = 20;
	float numLayers = mix(maxLayers, minLayers, abs(dot(vec3(0.0, 0.0, 1.0), viewDir)));  
	// calculate the size of each layer
	float layerDepth = 1.0 / numLayers;
	// depth of current layer
	float currentLayerDepth = 0.0;
	// the amount to shift the texture coordinates per layer (from vector P)
	vec2 P = viewDir.xy / viewDir.z * height_scale; 
	vec2 deltaTexCoords = P / numLayers;

	// get initial values
	vec2  currentTexCoords     = tex_coords;
	float currentDepthMapValue = 1.0 - texture(parallax_texture, currentTexCoords).r;

	while(currentLayerDepth < currentDepthMapValue)
	{
		// shift texture coordinates along direction of P
		currentTexCoords -= deltaTexCoords;
		// get depthmap value at current texture coordinates
		currentDepthMapValue = 1.0 - texture(parallax_texture, currentTexCoords).r;  
		// get depth of next layer
		currentLayerDepth += layerDepth;  
	}

	// -- parallax occlusion mapping interpolation from here on
	// get texture coordinates before collision (reverse operations)
	vec2 prevTexCoords = currentTexCoords + deltaTexCoords;

	// get depth after and before collision for linear interpolation
	float afterDepth  = currentDepthMapValue - currentLayerDepth;
	float beforeDepth = 1.0 - texture(parallax_texture, prevTexCoords).r - currentLayerDepth + layerDepth;

	// interpolation of texture coordinates
	float weight = afterDepth / (afterDepth - beforeDepth);
	vec2 finalTexCoords = prevTexCoords * weight + currentTexCoords * (1.0 - weight);

	return finalTexCoords;
}

void main()
{
	mat3 toTangentSpace = GetTangentMatrix();
	
	// Parallax Mapping
	vec2 textureCoords = fUV;
	if (useParallaxTexture == 1)
	{
		textureCoords = calcParallaxMapping(parallaxTexture, textureCoords, toTangentSpace, fCameraPosition, fPosition);
	}

	// Diffuse mapping + material ID
	vec4 diffuse_color_final = vec4(diffuseColor, 1.0);
	if (useDiffuseMap == 1)
	{
		diffuse_color_final = texture(diffuseMap, textureCoords);
	}

	int material_id = 0;
	if (emissionStrength > 0)
	{
		material_id = 1;
		diffuse_color_final.xyz *= (emissionStrength);
	}
	diffuseID = vec4(diffuse_color_final.xyz, material_id);

	if (useWireframe == 1)
	{
		float near_distance = min(min(fWireframeDistance[0], fWireframeDistance[1]), fWireframeDistance[2]);
		float line_size = 1.0;
		float edgeIntensity1 = exp2(-(1.0 / line_size) * near_distance * near_distance);
		line_size = 20.0;
		float edgeIntensity2 = exp2(-(1.0 / line_size) * near_distance * near_distance);

		vec3 lineColor_inner = (edgeIntensity1 * vec3(1.0)) + ((1.0 - edgeIntensity1) * vec3(0.0));
		vec3 lineColor_outer = (edgeIntensity2 * vec3(0.0)) + ((1.0 - edgeIntensity2) * diffuseID.xyz);

		diffuseID.xyz = lineColor_inner + lineColor_outer;
	}

	// Normal mapping + linear depth
	float depth = length(fCameraPosition);
	normalDepth = vec4(fNormal, depth);
	if (useNormalMap == 1)
	{	
		vec3 normal_map = calcNormalMapping(normalMap, textureCoords, toTangentSpace);
		normalDepth = vec4(normal_map, depth);
	}

    // Specular mapping
	vec3 specular_color_final = specularColor;
	float specular_shininess_final = max(0.05, 0.9 - (log2(specularShininess) / 9.0));
	if (useSpecularMap == 1)
	{
		specular_color_final = texture(specularMap, textureCoords).xyz;
	}
	specular = vec4(specular_color_final, specular_shininess_final);

	// Velocity mapping
	vec2 a = fClipPosition.xy / fClipPosition.w;
	vec2 b = fPreviousClipPosition.xy / fPreviousClipPosition.w;
	velocity = a - b;
}

/*out vec4 diffuseID;
out vec4 normalDepth;
out vec4 specular;
out vec2 velocity;

uniform vec3 diffuseColor;
uniform float emissionStrength;
uniform vec3 specularColor;
uniform float specularShininess;

uniform sampler2D mainTexture;
uniform sampler2D normalMap;
uniform sampler2D diffuseMap;
uniform sampler2D specularMap;
uniform sampler2D parallaxTexture;

uniform int useMainTexture;
uniform int useNormalMap;
uniform int useDiffuseMap;
uniform int useSpecularMap;
uniform int useParallaxTexture;

in vec3 fPosition;
in vec3 fPreviousPosition;
in vec3 fNormal;
in vec3 fTangent;
in vec4 fColor;
in vec2 fUV;
flat in int fMaterialIndex;
in vec3 fCameraPosition;
in vec4 fClipPosition;
in vec4 fPreviousClipPosition;
noperspective in vec3 fWireframeDistance;*/