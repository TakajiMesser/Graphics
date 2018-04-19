#version 440

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

in vec3 vPosition;

out vec2 fUV;

void main()
{
    vec4 position = projectionMatrix * viewMatrix * modelMatrix * vec4(vPosition, 1.0);

    gl_Position = position.xyww;
    fUV = vec2((vPosition.x + 1.0) * 0.5, (vPosition.y + 1.0) * 0.5);
}