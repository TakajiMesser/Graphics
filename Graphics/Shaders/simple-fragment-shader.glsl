#version 130

uniform sampler2D textureSampler;
uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 lightPosition;

in vec3 fPosition;
in vec3 fNormal;
in vec3 fEyeDirection;
in vec3 fLightDirection;
in vec2 fUV;
in vec4 fColor;

out vec4 color;

void main()
{
    color = fColor;
}