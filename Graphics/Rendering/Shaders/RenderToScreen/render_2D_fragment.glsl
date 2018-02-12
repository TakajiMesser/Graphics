#version 440

uniform sampler2D textureSampler;
uniform int channel;
uniform float gamma;

in vec2 fUV;
out vec4 color;

void main()
{
	vec4 tex = texture(textureSampler, fUV);
    
	color = vec4((channel < 0) ? tex : vec4(tex[channel]));
    //color = vec4(1.0 - (1.0 - tex.x) * 25.0);
    //color.rgb = pow(color.rgb, vec3(1.0 / gamma));
}