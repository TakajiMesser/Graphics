#version 440

layout(location = 0) in vec3 position;

out vec3 ray;

uniform mat4 inverseViewPerspectiveMatrix;

const vec2 data[6] = vec2[]
(
  vec2(-1.0, -1.0),
  vec2( 1.0, -1.0),
  vec2(-1.0,  1.0),
  vec2(-1.0, 1.0),
  vec2(1.0, -1.0),
  vec2(1.0,  1.0)
);

void main()
{
	vec4 vPosition = vec4(data[gl_VertexID], 0, 1.0);
	vec4 finalPosition = inverseViewPerspectiveMatrix * vPosition;
	ray = finalPosition.xyz;

    gl_Position = vPosition;
}