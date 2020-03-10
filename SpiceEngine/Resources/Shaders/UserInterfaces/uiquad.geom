#version 400

uniform vec2 halfResolution;

layout(points) in;
layout(triangle_strip, max_vertices = 4) out;

in vec3 gRight[];
in vec3 gUp[];
in vec2 gCornerRadius[];
in vec4 gColor[];
in vec4 gId[];

out vec4 fColor;
out vec4 fId;
out vec2 fCornerRadius;
out vec2 fUV;

void main()
{
    vec3 position = gl_in[0].gl_Position.xyz;

    fColor = gColor[0];
    fId = gId[0];
    fCornerRadius = gCornerRadius[0];

    vec3 right = gRight[0];
    vec3 up = gUp[0];

    gl_Position = vec4(position, 1.0);
    fUV = vec2(0.0, 1.0);
    EmitVertex();

    position -= up;
    gl_Position = vec4(position, 1.0);
    fUV = vec2(0.0, 0.0);
    EmitVertex();

    position += right + up;
    gl_Position = vec4(position, 1.0);
    fUV = vec2(1.0, 1.0);
    EmitVertex();

    position -= up;
    gl_Position = vec4(position, 1.0);
    fUV = vec2(1.0, 0.0);
    EmitVertex();

    EndPrimitive();
}