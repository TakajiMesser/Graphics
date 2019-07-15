#version 440

in vec4 fId;

layout(location = 0) out vec4 color;

void main()
{
    color = vec4(fId.xyz, 1.0);
    //color = vec4(0, 1, 0, 1);
}