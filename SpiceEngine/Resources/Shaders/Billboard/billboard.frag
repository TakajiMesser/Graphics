#version 440

uniform vec4 overrideColor;
uniform sampler2D mainTexture; 

in vec2 fUV;

out vec4 color;

void main()
{
    color = texture(mainTexture, fUV);

    if (color.r == 1.0 && color.g == 1.0 && color.b == 1.0)
    {
        discard;
    }
	else if (overrideColor.a > 0.0)
	{
		color = overrideColor;
	}
	/*else
	{
		discard;
	}*/
}