#version 440

uniform vec4 id;

layout(location = 0) out vec4 color;

void main()
{
    color = id;
    //color = vec4(id);
    //color = vec4(1.0);
}