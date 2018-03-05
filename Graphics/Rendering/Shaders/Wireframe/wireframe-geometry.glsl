#version 440

layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

//layout(location = 0) in vec3 vPosition;

noperspective out vec3 gWireframeDistance;

void main()
{
    vec4[3] clipPositions;
    vec2[3] wireframePoints;

    for (int i = 0; i < 3; i++)
    {
        vec4 viewPosition = viewMatrix * gl_in[i].gl_Position;
        clipPositions[i] = projectionMatrix * viewPosition;
        wireframePoints[i] = clipPositions[i].xy / clipPositions[i].w;
    }
    
    vec2 v0 = wireframePoints[2] - wireframePoints[1];
    vec2 v1 = wireframePoints[2] - wireframePoints[0];
    vec2 v2 = wireframePoints[1] - wireframePoints[0];

    float area = abs(v1.x * v2.y - v1.y * v2.x);

    gWireframeDistance = vec3(area / length(v0)) * vec3(1.0, 0.0, 0.0);
    gl_Position = clipPositions[0];
    EmitVertex();

    gWireframeDistance = vec3(area / length(v1)) * vec3(0.0, 1.0, 0.0);
    gl_Position = clipPositions[1];
    EmitVertex();

    gWireframeDistance = vec3(area / length(v2)) * vec3(0.0, 0.0, 1.0);
    gl_Position = clipPositions[2];
    EmitVertex();

    EndPrimitive();
}