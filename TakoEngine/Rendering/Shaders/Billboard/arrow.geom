#version 400

const float LINE_WIDTH = 0.1;
const float BETWEEN_WIDTH = 0.05;

layout(points) in;
layout(triangle_strip, max_vertices = 45) out;

uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 cameraPosition;

out vec4 fColor;

void drawArrow(mat4 viewProjectionMatrix, vec3 position, vec4 color, vec3 direction, vec3 perpendicular)
{
    fColor = color;

    position -= perpendicular * (LINE_WIDTH / 2.0);
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    position += direction;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();
    
    position -= direction;
    position += perpendicular * LINE_WIDTH;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    position += direction;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    EndPrimitive();

    position -= perpendicular * 0.15;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    position += perpendicular * 0.2;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    position -= perpendicular * 0.1;
    position += direction * 0.15;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    EndPrimitive();
}

void drawBetween(mat4 viewProjectionMatrix, vec3 position, vec4 color, vec3 directionA, vec3 directionB)
{
    fColor = color;

    position += directionA * 0.25;
    //position += directionB * (LINE_WIDTH / 2.0);
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    position += directionB * 0.25;// - (LINE_WIDTH / 2.0);
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();
    
    position -= directionB * 0.25;// - (LINE_WIDTH / 2.0);
    position -= directionA * BETWEEN_WIDTH;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    position += directionB * 0.25;
    gl_Position = viewProjectionMatrix * vec4(position, 1.0);
    EmitVertex();

    EndPrimitive();
}

void main() 
{
    mat4 viewProjectionMatrix = projectionMatrix * viewMatrix;
	vec3 position = gl_in[0].gl_Position.xyz;

    vec3 toCamera = normalize(cameraPosition - position);

    vec3 xDirection = vec3(1.0, 0.0, 0.0);
    vec3 perpendicular = cross(toCamera, xDirection);
    vec4 color = vec4(1.0, 0.0, 0.0, 1.0);
    drawArrow(viewProjectionMatrix, position, color, xDirection, perpendicular);

    vec3 yDirection = vec3(0.0, 1.0, 0.0);
    perpendicular = cross(toCamera, yDirection);
    color = vec4(0.0, 1.0, 0.0, 1.0);
    drawArrow(viewProjectionMatrix, position, color, yDirection, perpendicular);

    vec3 zDirection = vec3(0.0, 0.0, 1.0);
    perpendicular = cross(toCamera, zDirection);
    color = vec4(0.0, 0.0, 1.0, 1.0);
    drawArrow(viewProjectionMatrix, position, color, zDirection, perpendicular);

    color = vec4(1.0, 1.0, 0.0, 1.0);
    drawBetween(viewProjectionMatrix, position, color, xDirection, yDirection);
    drawBetween(viewProjectionMatrix, position, color, yDirection, xDirection);

    color = vec4(0.0, 1.0, 1.0, 1.0);
    drawBetween(viewProjectionMatrix, position, color, yDirection, zDirection);
    drawBetween(viewProjectionMatrix, position, color, zDirection, yDirection);

    color = vec4(1.0, 0.0, 1.0, 1.0);
    drawBetween(viewProjectionMatrix, position, color, zDirection, xDirection);
    drawBetween(viewProjectionMatrix, position, color, xDirection, zDirection);
}