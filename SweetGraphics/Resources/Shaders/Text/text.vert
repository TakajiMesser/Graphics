#version 440

uniform vec2 halfResolution;

in vec2 vPosition;
in vec2 vUV;

out vec2 fUV;

void main()
{
    float x = (vPosition.x - halfResolution.x) / halfResolution.x;
	float y = (halfResolution.y - vPosition.y) / halfResolution.y;
    //float y = (vPosition.y - halfResolution.y) / halfResolution.y;

	gl_Position = vec4(x, y, 0.0, 1.0);

    //vec2 clipSpacePosition = vPosition - vec2();
    //gl_Position = vec4((vPosition - halfResolution) / halfResolution, 0.0, 1.0);
    fUV = vUV;
}