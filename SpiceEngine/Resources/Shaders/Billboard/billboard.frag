#version 440

uniform sampler2D mainTexture;

in vec2 fUV;

out vec4 color;

void main()
{
    color = texture2D(mainTexture, fUV);

    if (color.r == 1.0 && color.g == 1.0 && color.b == 1.0)
    {
        discard;
    }
}