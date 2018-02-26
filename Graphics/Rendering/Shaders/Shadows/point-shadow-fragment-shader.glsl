#version 440 core

uniform vec3 lightPosition;
uniform float lightRadius;

in vec4 fPosition;

void main()
{
    vec3 lightToVertex = fPosition.xyz - lightPosition;
    float lightDistance = length(lightToVertex);
    gl_FragDepth = lightDistance / lightRadius;
}