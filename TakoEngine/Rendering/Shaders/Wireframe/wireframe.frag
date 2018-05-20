#version 440

uniform float lineThickness;
uniform vec4 lineColor;

smooth in vec3 gEdgeDistance;

layout(location = 0) out vec4 color;

void main()
{
    // For this triangle, find the minimum distance of any corner from the edge
    float distance = min(gEdgeDistance[0], min(gEdgeDistance[1], gEdgeDistance[2]));

    // Discard any fragments that are not close to the edge
    if (distance > lineThickness)
    {
        discard;
    }
    else
    {
		float innerEdgeIntensity = exp2(-1.0 * distance * distance);
        float outerEdgeIntensity = exp2(-1.0 / 20.0 * distance * distance);

		float alpha = innerEdgeIntensity + (1.0 - outerEdgeIntensity);

        color = vec4(lineColor.xyz, alpha);
		//color = lineColor;
    }
}