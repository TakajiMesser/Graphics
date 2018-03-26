#version 400

layout(points) in;
layout(triangle_strip, max_vertices = 4) out;

uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 cameraPosition;

in vec4[] gColor;

out vec4 fColor;
out vec2 fUV;

void main() 
{
    mat4 viewProjectionMatrix = projectionMatrix * viewMatrix;
	vec3 position = gl_in[0].gl_Position.xyz;

    vec3 toCamera = normalize(cameraPosition - position);
    vec3 up = vec3(0.0, 0.0, -1.0);
    vec3 right = cross(toCamera, up);

    position -= (right * 0.5);
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    fColor = gColor[0];
    fUV = vec2(0.0, 0.0);
    EmitVertex();

    position.z -= 1.0;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    fColor = gColor[0];
    fUV = vec2(0.0, 1.0);
    EmitVertex();
    
    position.z += 1.0;
    position += right;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    fColor = gColor[0];
    fUV = vec2(1.0, 0.0);
    EmitVertex();

    position.z -= 1.0;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    fColor = gColor[0];
    fUV = vec2(1.0, 1.0);
    EmitVertex();

    EndPrimitive();
}