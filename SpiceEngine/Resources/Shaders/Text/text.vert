#version 440

uniform vec2 halfResolution;

in vec2 vPosition;
in vec2 vUV;

out vec2 fUV;

void main()
{
    //vec2 clipSpacePosition = vPosition - vec2();
    gl_Position = vec4((vPosition - halfResolution) / halfResolution, 0.0, 1.0);
    fUV = vUV;
}