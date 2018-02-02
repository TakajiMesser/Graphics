#version 440

uniform vec3 lightPosition;

in vec3 fPosition;

out float color;

void main()
{
    vec3 lightToVertex = fPosition - lightPosition;
    color = length(lightToVertex);
}