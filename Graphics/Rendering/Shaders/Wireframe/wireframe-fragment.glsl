#version 440

noperspective in vec3 gWireframeDistance;

layout(location = 0) out vec4 color;

void main()
{
    float nearDistance = min(gWireframeDistance[0], gWireframeDistance[1]);
    nearDistance = min(nearDistance, gWireframeDistance[2]);

    float innerEdgeIntensity = exp2(-1.0 * nearDistance * nearDistance);
    float outerEdgeIntensity = exp2(-1.0 / 20.0 * nearDistance * nearDistance);

    vec3 innerLineColor = innerEdgeIntensity * vec3(1.0);
    vec3 outerLineColor = (1.0 - outerEdgeIntensity) * vec3(1.0);

    color.xyz = innerLineColor + outerLineColor;
    color.w = 1.0;
	color = vec4(1.0);
}