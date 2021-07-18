#version 440

uniform sampler2D filterTexture;
uniform vec2 texelStep;
uniform float maxThreshold;
uniform float mulReduction;
uniform float minReduction;
uniform float maxSpan;

in vec2 fUV;

out vec4 color;

void main()
{
    vec3 rgbTL = textureOffset(filterTexture, fUV, ivec2(-1, 1)).rgb;
    vec3 rgbTR = textureOffset(filterTexture, fUV, ivec2(1, 1)).rgb;
    vec3 rgbBL = textureOffset(filterTexture, fUV, ivec2(-1, -1)).rgb;
    vec3 rgbBR = textureOffset(filterTexture, fUV, ivec2(1, -1)).rgb;
    vec3 rgbM = texture(filterTexture, fUV).rgb;

    vec3 luma = vec3(0.299, 0.587, 0.114);

    float lumaTL = dot(rgbTL, luma);
    float lumaTR = dot(rgbTR, luma);
    float lumaBL = dot(rgbBL, luma);
    float lumaBR = dot(rgbBR, luma);
    float lumaM = dot(rgbM, luma);

    float lumaMin = min(lumaTL, min(lumaTR, min(lumaBL, min(lumaBR, lumaM))));
    float lumaMax = max(lumaTL, max(lumaTR, max(lumaBL, max(lumaBR, lumaM))));

    if (lumaMax - lumaMin < lumaMax * maxThreshold)
    {
        color = vec4(rgbM, 1.0);
    }
    else
    {
        vec2 direction;
        direction.x = -((lumaTL + lumaTR) - (lumaBL + lumaBR));
        direction.y = (lumaTL + lumaBL) - (lumaTR + lumaBR);

        float reducedDirection = max((lumaTL + lumaTR + lumaBL + lumaBR) * 0.25 * mulReduction, minReduction);
        float minDirectionFactor = 1.0 / (min(abs(direction.x), abs(direction.y)) + reducedDirection);

        direction = clamp(direction * minDirectionFactor, vec2(-maxSpan, -maxSpan), vec2(maxSpan, maxSpan)) * texelStep;
    
        vec3 negativeInnerRGB = texture(filterTexture, fUV + direction * (1.0 / 3.0 - 0.5)).rgb;
        vec3 positiveInnerRGB = texture(filterTexture, fUV + direction * (2.0 / 3.0 - 0.5)).rgb;
    
        vec3 negativeOuterRGB = texture(filterTexture, fUV + direction * -0.5).rgb;
        vec3 positiveOuterRGB = texture(filterTexture, fUV + direction * 0.5).rgb;
    
        vec3 rgbTwoSamples = (negativeInnerRGB + positiveInnerRGB) * 0.5;
        vec3 rgbFourSamples = (negativeOuterRGB + positiveOuterRGB) * 0.25 + rgbTwoSamples * 0.5;
    
        float lumaFourSamples = dot(rgbFourSamples, luma);

        if (lumaFourSamples < lumaMin || lumaFourSamples > lumaMax)
        {
            color = vec4(rgbTwoSamples, 1.0);
        }
        else
        {
            color = vec4(rgbFourSamples, 1.0);
        }
    }
}