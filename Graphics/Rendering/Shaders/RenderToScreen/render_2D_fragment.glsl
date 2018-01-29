﻿#version 440

uniform sampler2D textureSampler;
uniform int channel;

in vec2 fUV;
out vec4 color;

void main()
{
	vec4 tex = texture(textureSampler, fUV);
	//color = vec4(1.0 - tex.r, 1.0 - tex.g, 1.0 - tex.b, tex.a);
	color = vec4((channel < 0) ? tex : vec4(tex[channel]));
}