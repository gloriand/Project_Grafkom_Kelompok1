﻿#version 330

out vec4 outputColor;

//N3
//in vec4 vertexColor;
//N4
//uniform vec4 ourColor;
//N5
//in vec3 ourColor;
void main(){
	outputColor = vec4(0.0, 0.48627450980392156, 0.792156862745098, 1.0);
	//N3
	//outputColor = vertexColor;
	//N4
	//outputColor = ourColor;
	//N5
	//outputColor = vec4(ourColor,1.0);
}