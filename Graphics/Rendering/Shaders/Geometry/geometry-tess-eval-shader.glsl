#version 400

layout(triangles, fractional_odd_spacing, ccw) in;

uniform mat4 modelMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 cameraPosition;

uniform int useDisplacementTexture;
uniform int displacementTextureUnit;
uniform float displacementStrength = 0.1;

/*layout(std140) uniform MaterialTextures
{
	sampler2D bindlessTexture[80];
};*/

in vec3 ePosition[];
in vec3 ePreviousPosition[];
in vec3 eNormal[];
in vec3 eTangent[];
in vec4 eColor[];
in vec2 eUV[];

out vec3 gPosition;
out vec3 gPreviousPosition;
out vec3 gNormal;
out vec3 gTangent;
out vec4 gColor;
out vec2 gUV;

vec2 interpolate2DTriangle(vec2 v0, vec2 v1, vec2 v2)
{
   	return vec2(gl_TessCoord.x) * v0 + vec2(gl_TessCoord.y) * v1 + vec2(gl_TessCoord.z) * v2;
}

vec3 interpolate3DTriangle(vec3 v0, vec3 v1, vec3 v2)
{
   	return vec3(gl_TessCoord.x) * v0 + vec3(gl_TessCoord.y) * v1 + vec3(gl_TessCoord.z) * v2;
}

vec4 interpolate3DTriangle(vec4 v0, vec4 v1, vec4 v2)
{
   	return vec4(gl_TessCoord.x) * v0 + vec4(gl_TessCoord.y) * v1 + vec4(gl_TessCoord.z) * v2;
}

vec2 interpolate2DQuad(vec2 v0, vec2 v1, vec2 v2, vec2 v3)
{
	float u = gl_TessCoord.x;
	float v = gl_TessCoord.y;

	vec2 a = mix(v0, v1, u);
	vec2 b = mix(v2, v3, u);
	return mix(a, b, v);
}

vec3 interpolate3DQuad(vec3 v0, vec3 v1, vec3 v2, vec3 v3)
{
	float u = gl_TessCoord.x;
	float v = gl_TessCoord.y;

	vec3 a = mix(v0, v1, u);
	vec3 b = mix(v2, v3, u);

	return mix(a, b, v);
}

void main()
{
	gPosition = interpolate3DTriangle(ePosition[0], ePosition[1], ePosition[2]);
	gPreviousPosition = interpolate3DTriangle(ePreviousPosition[0], ePreviousPosition[1], ePreviousPosition[2]);
    gNormal = normalize(interpolate3DTriangle(eNormal[0], eNormal[1], eNormal[2]));
	gTangent = normalize(interpolate3DTriangle(eTangent[0], eTangent[1], eTangent[2]));
    gColor = normalize(interpolate3DTriangle(eColor[0], eColor[1], eColor[2]));
	gUV = interpolate2DTriangle(eUV[0], eUV[1], eUV[2]);

	if(useDisplacementTexture == 1)
	{	
		//float displacement = texture(bindlessTexture[displacementTextureUnit], gUV).r * displacementStrength;
		//vec3 displacementMod = (gNormal * displacement);
		//gPosition += displacementMod;
		//gPreviousPosition = gPreviousPosition + (displacementMod);
	}

	gl_Position = vec4(gPosition, 1.0);
}