#version 440

uniform mat4 modelMatrix;
uniform vec2 halfResolution;

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec4 vColor;
layout(location = 2) in vec4 vId;

//out vec3 fPosition;
out vec4 fColor;
out vec4 fId;
//out vec2 fUV;

void main()
{
	vec4 position = vec4(vPosition, 1.0);
	vec4 clipPosition = modelMatrix * position;

	gl_Position = vec4((clipPosition.xy - halfResolution) / halfResolution, clipPosition.z, 1.0);
    //gl_Position = vec4((vPosition.xy - halfResolution) / halfResolution, vPosition.z, 1.0);
    
	fColor = vColor;
	fId = vId;

	//fUV = vec2((vPosition.x + 1.0) * 0.5, (vPosition.y + 1.0) * 0.5);
	//gl_Position = vec4(vPosition.x, vPosition.y, 0.0, 1.0);
}