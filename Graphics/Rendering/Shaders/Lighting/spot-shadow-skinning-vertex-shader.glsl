﻿#version 440

const int MAX_JOINTS = 100;
const int MAX_WEIGHTS = 4;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat4[MAX_JOINTS] jointTransforms;

layout(location = 0) in vec3 vPosition;
layout(location = 5) in vec4 vBoneIDs;
layout(location = 6) in vec4 vBoneWeights;

out vec2 fUV;

void main()
{
    vec4 position = vec4(vPosition, 1.0);

    mat4 jointTransform = mat4(0.0);
    for (int i = 0; i < MAX_WEIGHTS; i++)
    {
        jointTransform += jointTransforms[int(vBoneIDs[i])] * vBoneWeights[i];
    }

    position = jointTransform * position;

	gl_Position = projectionMatrix * viewMatrix * modelMatrix * position;
    //fUV = vUV;
    fUV = vec2((position.x + 1.0) * 0.5, (position.y + 1.0) * 0.5);
}