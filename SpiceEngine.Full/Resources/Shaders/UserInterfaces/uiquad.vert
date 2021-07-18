#version 440

uniform vec2 resolution;
uniform vec2 halfResolution;

layout(location = 0) in vec3 vPosition;
layout(location = 1) in float vBorderThickness;
layout(location = 2) in vec2 vSize;
layout(location = 3) in vec2 vCornerRadius;
layout(location = 4) in vec4 vColor;
layout(location = 5) in vec4 vBorderColor;

out vec3 gRight;
out vec3 gUp;
out vec2 gCornerRadius;
out vec2 gBorderThickness;
out vec4 gColor;
out vec4 gBorderColor;

void main()
{
	float x = (vPosition.x - halfResolution.x) / halfResolution.x;
	float y = (halfResolution.y - vPosition.y) / halfResolution.y;

	gl_Position = vec4(x, y, vPosition.z, 1.0);

	gRight = vec3(vSize.x / halfResolution.x, 0.0, 0.0);
	gUp = vec3(0.0, vSize.y / halfResolution.y, 0.0);
	gCornerRadius = vCornerRadius / vSize;
	gBorderThickness = vec2(vBorderThickness / vSize.x, vBorderThickness / vSize.y);
	gColor = vColor;
	gBorderColor = vBorderColor;
}