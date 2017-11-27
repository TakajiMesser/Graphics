#version 130

// a projection transformation to apply to the vertex' position
uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

// attributes of our vertex
in vec3 vPosition;
in vec3 vNormal;
in vec4 vColor;

// must match name in fragment shader
out vec4 fColor;

void main()
{
    // gl_Position is a special variable of OpenGL that must be set
    gl_Position = modelMatrix * viewMatrix * projectionMatrix * vec4(vPosition, 1.0f);
    fColor = vColor;
}