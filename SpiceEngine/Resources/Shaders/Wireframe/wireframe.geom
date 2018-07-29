#version 440

layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

smooth out vec3 gEdgeDistance;

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
}