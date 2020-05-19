#version 440

uniform sampler2D mainTexture;

in vec4 fColor;
in vec2 fUV;

out vec4 color;

void main()
{
    vec4 textureColor = texture(mainTexture, fUV);

    if (textureColor.r == 1.0 && textureColor.g == 1.0 && textureColor.b == 1.0)
    {
        discard;
    }
    else
    {
        //color = vec4(1, 0, 0, 1);
        color = fColor;
    }
}