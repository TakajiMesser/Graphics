#version 400

layout(vertices = 3) out;

uniform mat4 modelMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 cameraPosition;

uniform int useDisplacementTexture;
uniform vec2 renderSize;

in vec3 cPosition[];
in vec3 cPreviousPosition[];
in vec3 cNormal[];
in vec3 cTangent[];
in vec4 cColor[];
in vec2 cUV[];

out vec3 ePosition[];
out vec3 ePreviousPosition[];
out vec3 eNormal[];
out vec3 eTangent[];
out vec4 eColor[];
out vec2 eUV[];

float screenSphereSize(vec4 e1, vec4 e2)
{
	vec4 p1 = (e1 + e2) * 0.5;
	vec4 p2 = p1;
	p2.y += distance(e1, e2);

	p1 = p1 / p1.w;
	//p1 = p1 * 0.5 + 0.5;
	p2 = p2 / p2.w;
	//p2 = p2 * 0.5 + 0.5;

	float l = length((p1.xy - p2.xy) * renderSize * 0.5);

	return (clamp(l / 15.0, 1.0, 64.0));
}

bool edgeInFrustum(vec4 p, vec4 q)
{
	return !((p.x < -p.w && q.x < -q.w) 
        || (p.x > p.w && q.x > q.w) 
		|| (p.z < -p.w && q.z < -q.w)
        || (p.z > p.w && q.z > q.w));
}

bool frustumCullTest(vec4 vertexPosition[3])
{
	return edgeInFrustum(vertexPosition[1], vertexPosition[0])
        || edgeInFrustum(vertexPosition[2], vertexPosition[0])
        || edgeInFrustum(vertexPosition[2], vertexPosition[1]);
}

bool backfaceCullTest(vec3 worldPosition, vec3 eyePosition, vec3 normal)
{
	vec3 L = normalize(eyePosition - worldPosition);
	float angle_of_inc = dot(L, normal);
	return angle_of_inc > 0;
}

void controlTessellation()
{
	float tessLevel = 1.0;
	vec4 vertexPosition[3];

	for (int i = 0; i < 3; i++)
	{
		vertexPosition[i] = projectionMatrix * viewMatrix * vec4(cPosition[i], 1.0);
	}

	if (frustumCullTest(vertexPosition) && backfaceCullTest(cPosition[gl_InvocationID], -cameraPosition, cNormal[gl_InvocationID]))
	{
		if (useDisplacementTexture == 0)
		{
			tessLevel = 1.0;
			gl_TessLevelOuter[2] = tessLevel;
			gl_TessLevelOuter[1] = tessLevel;
			gl_TessLevelOuter[0] = tessLevel;
			gl_TessLevelInner[0] = tessLevel;
		}
		else
		{
			// Calculate the tessellation levels
			gl_TessLevelOuter[2] = screenSphereSize(vertexPosition[1], vertexPosition[0]);
			gl_TessLevelOuter[1] = screenSphereSize(vertexPosition[2], vertexPosition[0]);
			gl_TessLevelOuter[0] = screenSphereSize(vertexPosition[2], vertexPosition[1]);
			gl_TessLevelInner[0] = max(gl_TessLevelOuter[1], max(gl_TessLevelOuter[0], gl_TessLevelOuter[2]));
		}
	}
	else
	{
		gl_TessLevelOuter[0] = gl_TessLevelOuter[1] = gl_TessLevelOuter[2] = 0.0;
		gl_TessLevelInner[0] = 0.0;
	}
}

void main()
{
    ePosition[gl_InvocationID] = cPosition[gl_InvocationID];
    ePreviousPosition[gl_InvocationID] = cPreviousPosition[gl_InvocationID];
	eNormal[gl_InvocationID] = cNormal[gl_InvocationID];
	eTangent[gl_InvocationID] = cTangent[gl_InvocationID];
    eColor[gl_InvocationID] = cColor[gl_InvocationID];
    eUV[gl_InvocationID] = cUV[gl_InvocationID];

	controlTessellation();
}