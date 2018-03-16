﻿#version 440

layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
//uniform mat4 viewportMatrix;

smooth out vec3 gEdgeDistance;

void main()
{
    //vec p0 = vec3(viewportMatrix * (gl_in[0].gl_Position / gl_in[0].gl_Position.w));
    //vec p1 = vec3(viewportMatrix * (gl_in[1].gl_Position / gl_in[1].gl_Position.w));
    //vec p2 = vec3(viewportMatrix * (gl_in[2].gl_Position / gl_in[2].gl_Position.w));

    //float a = length(p1 - p2);
    //float b = length(p2 - p0);
    //float c = length(p1 - p0);

    float a = length(gl_in[1].gl_Position.xyz - gl_in[2].gl_Position.xyz);
    float b = length(gl_in[2].gl_Position.xyz - gl_in[0].gl_Position.xyz);
    float c = length(gl_in[1].gl_Position.xyz - gl_in[0].gl_Position.xyz);

    float alpha = acos((b * b + c * c - a * a) / (2.0 * b * c));
    float beta = acos((a * a + c * c - b * b) / (2.0 * a * c));

    float ha = abs(c * sin(beta));
    float hb = abs(c * sin(alpha));
    float hc = abs(b * sin(alpha));

    gEdgeDistance = vec3(ha, 0, 0);
    gl_Position = gl_in[0].gl_Position;
    EmitVertex();

    gEdgeDistance = vec3(0, hb, 0);
    gl_Position = gl_in[1].gl_Position;
    EmitVertex();

    gEdgeDistance = vec3(0, 0, hc);
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
    
    float a = length(wireframePoints[1] - wireframePoints[2]);
    float b = length(wireframePoints[2] - wireframePoints[0]);
    float c = length(wireframePoints[1] - wireframePoints[0]);

    vec2 v0 = wireframePoints[1] - wireframePoints[2];
    vec2 v1 = wireframePoints[2] - wireframePoints[0];
    vec2 v2 = wireframePoints[1] - wireframePoints[0];

    float area = abs(v1.x * v2.y - v1.y * v2.x);

    float ha = area / a;
    float hb = area / b;
    float hc = area / c;

    gEdgeDistance = vec3(ha, 0, 0);
    //gl_Position = gl_in[0].gl_Position;
    gl_Position = clipPositions[0];
    EmitVertex();

    gEdgeDistance = vec3(0, hb, 0);
    //gl_Position = gl_in[1].gl_Position;
    gl_Position = clipPositions[1];
    EmitVertex();

    gEdgeDistance = vec3(0, 0, hc);
    //gl_Position = gl_in[2].gl_Position;
    gl_Position = clipPositions[2];
    EmitVertex();

    EndPrimitive();*/
}