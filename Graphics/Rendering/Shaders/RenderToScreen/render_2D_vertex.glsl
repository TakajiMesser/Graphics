#version 440

in vec3 vPosition;
//in vec2 vUV;
out vec2 fUV;

void main()
{
	//float x = -1.0 + float((gl_VertexID & 1) << 2);
    //float y = -1.0 + float((gl_VertexID & 2) << 1);
	//fUV = vec2((x + 1.0) * 0.5, (y + 1.0) * 0.5);
	//fUV = vec2(vPosition.x, vPosition.y);

	fUV = vec2((vPosition.x + 1.0) * 0.5, (vPosition.y + 1.0) * 0.5);
	gl_Position = vec4(vPosition.x, vPosition.y, vPosition.z, 1.0);

	/*if (gl_VertexID == 0)
	{
		fUV = vec2(1.0, 1.0);
		gl_Position = vec4(vPosition.x, vPosition.y, vPosition.z, 1.0);
	}
	else if (gl_VertexID == 1)
	{
		fUV = vec2(0.0, 1.0);
		gl_Position = vec4(vPosition.x, vPosition.y, vPosition.z, 1.0);
	}
	else if (gl_VertexID == 2)
	{
		fUV = vec2(0.0, 0.0);
		gl_Position = vec4(vPosition.x, vPosition.y, vPosition.z, 1.0);
	}
	else if (gl_VertexID == 3)
	{
		fUV = vec2(1.0, 0.0);
		gl_Position = vec4(vPosition.x, vPosition.y, vPosition.z, 1.0);
	}
	else
	{
		gl_Position = vec4(0.0, 0.0, 0.0, 1.0);
	}*/
	
	//fUV = vUV;
}