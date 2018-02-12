#version 440

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat4 previousModelMatrix;
uniform mat4 previousViewMatrix;
uniform mat4 previousProjectionMatrix;

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec3 vNormal;
layout(location = 2) in vec3 vTangent;
layout(location = 3) in vec4 vColor;
layout(location = 4) in vec2 vUV;

out vec3 fPosition;
out vec4 fClipPosition;
out vec4 fPreviousClipPosition;
out vec3 fNormal;
out vec3 fTangent;
out vec4 fColor;
out vec2 fUV;

void main()
{
    mat4 mvp = projectionMatrix * viewMatrix * modelMatrix;
    vec4 position = vec4(vPosition, 1.0);

    fPosition = (modelMatrix * position).xyz;
    fClipPosition = mvp * position;
    fPreviousClipPosition = previousProjectionMatrix * previousViewMatrix * previousModelMatrix * position;
	fNormal = (modelMatrix * vec4(vNormal, 0.0)).xyz;
    fTangent = (modelMatrix * vec4(vTangent, 0.0)).xyz;
    fColor = vColor;
    fUV = vUV;

    gl_Position = fClipPosition;
}