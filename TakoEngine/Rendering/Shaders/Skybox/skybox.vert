#version 440

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

in vec3 vPosition;

out vec3 fUV;

void main()
{
    vec4 position = projectionMatrix * viewMatrix * modelMatrix * vec4(vPosition, 1.0);
    gl_Position = position.xyww;
    fUV = vPosition;
}