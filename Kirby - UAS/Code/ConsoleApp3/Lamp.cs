using LearnOpenTK.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4; //buat GL.ClearColor
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Grafkom
{
    class Lamp
    {
        string startupPath = System.IO.Path.GetFullPath(@"..\..\..\");

        // Lamp / Light
        float[] _vertices =
        {
            // Position
            -0.5f, -0.5f, -0.5f, // Front face
             0.5f, -0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f, -0.5f,  0.5f, // Back face
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,

            -0.5f,  0.5f,  0.5f, // Left face
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,

             0.5f,  0.5f,  0.5f, // Right face
             0.5f,  0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,

            -0.5f, -0.5f, -0.5f, // Bottom face
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f,  0.5f, -0.5f, // Top face
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f
        };

        int _vertexBufferLightObject;
        int _vaoLamp;
        Shader _lampShader;
        Vector3 _lightPos = new Vector3(0.0f, 10.0f, 0.0f);

        public Lamp()
        {

        }

        public Lamp(float x, float y, float z)
        {
            _lightPos.X = x;
            _lightPos.Y = y;
            _lightPos.Z = z;
        }

        public Lamp(Vector3 lightPos)
        {
            _lightPos.X = lightPos.X;
            _lightPos.Y = lightPos.Y;
            _lightPos.Z = lightPos.Z;
        }

        public void load()
        {
            //Initialize Light Object
            _vertexBufferLightObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferLightObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _lampShader = new Shader(startupPath + "/Shader/Shader.vert",
                startupPath + "/Shader/Shader.frag");

            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        public void render(Matrix4 camera_view, Matrix4 camera_projection) {
            _lampShader.Use();
            Matrix4 lampMatrix = Matrix4.CreateScale(0.0001f);
            lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

            _lampShader.SetMatrix4("transform", lampMatrix);
            _lampShader.SetMatrix4("view", camera_view);
            _lampShader.SetMatrix4("projection", camera_projection);

            GL.BindVertexArray(_vaoLamp);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}
