#version 440

uniform float lineThickness;
uniform vec4 lineColor;
uniform float selectedLineThickness;
uniform vec4 selectedLineColor;

smooth in vec3 gEdgeDistance;
in vec4 fId;

layout(location = 0) out vec4 color;

void main()
{
    // For this triangle, find the minimum distance of any corner from the edge
    float distance = min(gEdgeDistance[0], min(gEdgeDistance[1], gEdgeDistance[2]));

    // Discard any fragments that are not close to the edge
	float thickness = fId.w < 1.0
		? selectedLineThickness
		: lineThickness;
	
	vec4 lColor = fId.w < 1.0
		? selectedLineColor
		: lineColor;

    if (distance > thickness)
    {
        discard;
    }
    else
    {
		//float innerEdgeIntensity = exp2(-1.0 * distance * distance);
        //float outerEdgeIntensity = exp2(-1.0 / 20.0 * distance * distance);

		//float alpha = innerEdgeIntensity + (1.0 - outerEdgeIntensity);

        //color = vec4(lColor.xyz, alpha);
        color = lColor;
    }
}