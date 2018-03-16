#version 440

uniform float id;

layout(location = 0) out vec4 color;

void main()
{
    color = vec4(id);
    //color = vec4(1.0);
}