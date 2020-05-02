#version 440

uniform sampler2D textureSampler;
uniform vec4 color;

in vec2 fUV;
out vec4 finalColor;

void main()
{
	//color = texture(textureSampler, fUV);
    
	vec4 textureColor = texture(textureSampler, fUV);

	if (textureColor.w == 1.0 && (textureColor.x > 0.0 || textureColor.y > 0.0 || textureColor.z > 0.0)) {
		finalColor = color;
	}
	else {
		finalColor = vec4(0.0, 0.0, 0.0, 0.0);
	}

	//color = vec4(1.0, 0.0, 0.0, 1.0);
}