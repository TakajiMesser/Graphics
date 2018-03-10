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
    float a = length(gl_in[1].gl_Position.xyz - gl_in[2].gl_Position.xyz);
    float b = length(gl_in[2].gl_Position.xyz - gl_in[0].gl_Position.xyz);
    float c = length(gl_in[1].gl_Position.xyz - gl_in[0].gl_Position.xyz);

    float alpha = acos((b * b + c * c - a * a) / (2.0 * b * c));
    float beta = acos((a * a + c * c - b * b) / (2.0 * a * c));

    float ha = abs(c * sin(beta));
    float hb = abs(c * sin(alpha));
    float hc = abs(b * sin(alpha));

    gWireframeDistance = vec3(ha, 0, 0);
    gl_Position = gl_in[0].gl_Position;
    EmitVertex();

    gWireframeDistance = vec3(0, hb, 0);
    gl_Position = gl_in[1].gl_Position;
    EmitVertex();

    gWireframeDistance = vec3(0, 0, hc);
    gl_Position = gl_in[2].gl_Position;
    EmitVertex();

    EndPrimitive();

    /*vec4[3] clipPositions;
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

    EndPrimitive();*/
}