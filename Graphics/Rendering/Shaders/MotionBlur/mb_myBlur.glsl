#version 440

const int MAX_SAMPLES = 50;

uniform sampler2D sceneTexture;
uniform sampler2D velocityTexture;

//uniform float fps_scaler;

in vec2 fUV;
out vec4 color;

void main(void)
{
    vec2 velocity = texture(velocityTexture, fUV).rg;
    color = texture(sceneTexture, fUV);

    for (int i = 1; i < MAX_SAMPLES; i++)
    {
        vec2 offset = velocity * (float(i) / float(MAX_SAMPLES - 1) - 0.5);
        color += texture(sceneTexture, fUV + offset);
    }
    color /= float(MAX_SAMPLES);
}
