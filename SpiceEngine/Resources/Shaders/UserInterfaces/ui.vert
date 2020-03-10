#version 440

uniform mat4 modelMatrix;
uniform vec2 resolution;
uniform vec2 halfResolution;

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec4 vColor;
layout(location = 2) in vec4 vId;

//out vec3 fPosition;
out vec4 fColor;
out vec4 fId;
//out vec2 fUV;

void main()
{
	vec4 position = vec4(vPosition.x, /*resolution.y - */vPosition.y, vPosition.z, 1.0);
	vec4 clipPosition = modelMatrix * position;

	// TODO - This will convert the position to being relative from the bottom-left of the screen, BUT we want it from the top-left!
	// gl_Position = [-1, 1]

	// gl_Position comes out to y = -0.25 -> user supplied y = resolution.y * (3/8)
	// SO we actually want this to instead be gl_Position for y = 0.25

	float x = (clipPosition.x - halfResolution.x) / halfResolution.x;
	float y = /*-1.0 * */((clipPosition.y - halfResolution.y) / halfResolution.y);

	gl_Position = vec4(x, y, clipPosition.z, 1.0);

	//gl_Position = vec4((clipPosition.xy - halfResolution) / halfResolution, clipPosition.z, 1.0);
    //gl_Position = vec4((vPosition.xy - halfResolution) / halfResolution, vPosition.z, 1.0);
    
	fColor = vColor;
	fId = vId;

	//fUV = vec2((vPosition.x + 1.0) * 0.5, (vPosition.y + 1.0) * 0.5);
	//gl_Position = vec4(vPosition.x, vPosition.y, 0.0, 1.0);
}