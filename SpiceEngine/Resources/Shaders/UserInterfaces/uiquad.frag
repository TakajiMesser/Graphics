#version 440

#define PI 3.14159274
#define HALF_PI = 1.57079637;

//uniform sampler2D textureSampler;

in vec4 fColor;
in vec4 fId;
in vec2 fCornerRadius;
in vec2 fUV;

out vec4 color;

void main()
{
	//float maxY = fCornerRadius.y * sin(acos(fUV.x / fCornerRadius.x));

	if (fUV.x < fCornerRadius.x && fUV.y < fCornerRadius.y) {
		// Bottom Left -> X and Y are BOTH flipped
		float x = fCornerRadius.x - fUV.x;
		float minY = fCornerRadius.y - fCornerRadius.y * sin(acos(x / fCornerRadius.x));

		if (fUV.y < minY) {
			discard;
		}
	}
	else if (fUV.x < fCornerRadius.x && fUV.y > 1.0 - fCornerRadius.y) {
		// Top Left -> X is flipped
		float x = fCornerRadius.x - fUV.x;
		float maxY = 1.0 - fCornerRadius.y + fCornerRadius.y * sin(acos(x / fCornerRadius.x));

		if (fUV.y > maxY) {
			discard;
		}
	}
	else if (fUV.x > 1.0 - fCornerRadius.x && fUV.y < fCornerRadius.y) {
		// Bottom Right -> Y is flipped
		float x = fUV.x + fCornerRadius.x - 1.0;
		float minY = fCornerRadius.y - fCornerRadius.y * sin(acos(x / fCornerRadius.x));

		if (fUV.y < minY) {
			discard;
		}
	}
	else if (fUV.x > 1.0 - fCornerRadius.x && fUV.y > 1.0 - fCornerRadius.y) {
		// Top Right -> Neither X nor Y are flipped
		float x = fUV.x + fCornerRadius.x - 1.0;
		float maxY = 1.0 - fCornerRadius.y + fCornerRadius.y * sin(acos(x / fCornerRadius.x));

		if (fUV.y > maxY) {
			discard;
		}
	}

	//color = texture(textureSampler, fUV);
	//color = vec4(1.0, 0.0, 0.0, 1.0);
	//color = vec4(1.0, 1.0, 0.0, fColor.w);
	color = fColor;
}