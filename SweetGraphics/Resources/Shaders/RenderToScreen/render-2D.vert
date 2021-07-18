#version 440

in vec2 vPosition;

out vec2 fUV;

void main()
{
    fUV = vec2((vPosition.x + 1.0) * 0.5, (vPosition.y + 1.0) * 0.5);
	gl_Position = vec4(vPosition.x, vPosition.y, 0.0, 1.0);
}