#version 400

layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 previousViewMatrix;
uniform mat4 previousProjectionMatrix;
uniform vec3 cameraPosition;

uniform vec2 renderSize;

in vec3 gPosition[];
in vec3 gPreviousPosition[];
in vec3 gNormal[];
in vec3 gTangent[];
in vec4 gColor[];
in vec2 gUV[];
flat in int gMaterialIndex[];

out vec3 fPosition;
out vec3 fPreviousPosition;
out vec3 fNormal;
out vec3 fTangent;
out vec4 fColor;
out vec2 fUV;
flat out int fMaterialIndex;
out vec3 fCameraPosition;
out vec4 fClipPosition;
out vec4 fPreviousClipPosition;
noperspective out vec3 fWireframeDistance;

void main() 
{
	// Wireframe parts taken from 'Single-Pass Wireframe Rendering'
	vec4[3] viewPositions;
	vec4[3] clipPositions;	
	vec2[3] wireframePoints;

	for(int i = 0; i < 3; i++)
	{
		viewPositions[i] =  viewMatrix * gl_in[i].gl_Position;
		clipPositions[i] =  projectionMatrix * viewPositions[i];
	
		wireframePoints[i] = renderSize * clipPositions[i].xy / clipPositions[i].w;
	}

	vec2 v[3] = vec2[3](
		wireframePoints[2] - wireframePoints[1], 
		wireframePoints[2] - wireframePoints[0], 
		wireframePoints[1] - wireframePoints[0]
    );

	float area = abs(v[1].x*v[2].y - v[1].y * v[2].x);

	vec3 chooser[3] = vec3[3](vec3(1.0, 0.0, 0.0), vec3(0.0, 1.0, 0.0), vec3(0.0, 0.0, 1.0));

	// Output triangle
	for (int i = 0; i < 3; i++)
	{
		fPosition = gPosition[i];
		fPreviousPosition = gPreviousPosition[i];
        fNormal = gNormal[i];
        fTangent = gTangent[i];
        fColor = gColor[i];
		fUV = gUV[i];
		fMaterialIndex = gMaterialIndex[0];
        fCameraPosition = viewPositions[i].xyz;
        fWireframeDistance = vec3(area / length(v[i])) * chooser[i];

        fClipPosition = clipPositions[i];
		fPreviousClipPosition = previousProjectionMatrix * previousViewMatrix * vec4(gPreviousPosition[i], 1.0);

        gl_Position = clipPositions[i];
        EmitVertex();
	}
		
	EndPrimitive();
}