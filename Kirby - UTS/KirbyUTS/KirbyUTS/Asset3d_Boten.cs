using System;
using System.Collections.Generic;
using System.IO;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace KirbyUTS
{
    class Asset3d_Boten
    {
        string startupPath = System.IO.Path.GetFullPath(@"..\..\..\");

        List<Vector3> _vertices = new List<Vector3>();
        List<Vector3> _textureVertices = new List<Vector3>();
        List<Vector3> _normals = new List<Vector3>();

        List<uint> _indices = new List<uint>();
        int _vertexBufferObject;//buffer obj (handle variabel vertex spy bs di vgacard
        int _vertexArrayObject;//VAO  mengurus terkait array vertex yg kita kirim
        int _elementBufferObject;
        Shader _shader; //mengurus apa yg ditampilkan ke layar kita.
        int[] _pascal = { };
        Matrix4 transform = Matrix4.Identity;
        public Vector3 _centerPosition = new Vector3(0, 0, 0);
        public List<Vector3> _euler = new List<Vector3>();
        Matrix4 _view;
        Matrix4 _projection;
        public List<Asset3d_Boten> Child = new List<Asset3d_Boten>();


        public Asset3d_Boten()
        {
            _euler.Add(new Vector3(1, 0, 0)); //sumbu X
            _euler.Add(new Vector3(0, 1, 0)); //sumbu Y
            _euler.Add(new Vector3(0, 0, 1)); //sumbu Z
        }

        public void load(int size_x, int size_y)
        {
            //SETTINGAN BUFFER
            _vertexBufferObject = GL.GenBuffer(); //create buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject); //setting target dari buffer yang dituju
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);
            //SETTINGAN VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            //SETINGAN CARA BACA BINARY
            //DEFAULT
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //Menyalakan var index[0] (layout location=0) yg ada pd shader.vert DEFAULT
            GL.EnableVertexAttribArray(0);

            if (_indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), size_x / (float)size_y, 0.1f, 100f);

            foreach (var item in Child)
            {
                item.load(size_x, size_y);
            }
        }
        public void shaderType(int color)
        {
            //SETINGAN SHADER
            switch (color)
            {
                case 1:
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Body_Boten.frag");
                    break;
                case 2:
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Thorn.frag");
                    break;
                case 3:
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Eyes_Black.frag");
                    break;
                case 4:
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Bristle.frag");
                    break;
                case 5:
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Ribbon1.frag");
                    break;
                case 6:
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Ribbon2.frag");
                    break;
            }
            _shader.Use();
        }
        public void render(int _lines)
        {
            //STEP menggambar sebuah objek
            //1 enable shader(default)
            //transform = transform * Matrix4.CreateTranslation(0.0001f, 0f, 0f);
            //transform = transform * Matrix4.CreateScale(1.001f);
            //transform = transform * Matrix4.CreateRotationY(0.01f);

            _shader.Use();
            _shader.SetMatrix4("transform", transform);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            //2 panggil bind VAO(default)
            GL.BindVertexArray(_vertexArrayObject);

            if (_indices.Count != 0)
            {
                //3.1 DRAW EBO ============ menggambar PERSEGI dari 2 SEGITIGA
                GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                if (_lines == 0) //gambar dengan isi DEFAULT
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count);
                }
                else if (_lines == 1) //gambar garis tpi gak sampai semua
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, _vertices.Count);
                }
                else if (_lines == 2) // gambar tanpa isi
                {
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, _vertices.Count);
                }
                else if (_lines == 3)
                {
                    //lingkaran line
                    //GL.DrawArrays(PrimitiveType.LineLoop, 0, _vertices.Count);

                    //lingkaran berisi
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, _vertices.Count);
                }
            }
            foreach (var item in Child)
            {
                item.render(_lines);
            }
        } //end of render
        public void setVertices(List<Vector3> vertices)
        {
            _vertices = vertices;
        }
        public bool getVerticesLength()
        {
            if (_vertices.Count == 0)
            {
                return false;
            }
            if (_vertices.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void createBoxVertices(float _x, float _y, float _z, float length, int color)//ttik pusat dari box dimana
        {
            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;
            //length panjang dari titik kubus
            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = _x - length / 2.0f;
            temp_vector.Y = _y + length / 2.0f;
            temp_vector.Z = _z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = _x + length / 2.0f;
            temp_vector.Y = _y + length / 2.0f;
            temp_vector.Z = _z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = _x - length / 2.0f;
            temp_vector.Y = _y - length / 2.0f;
            temp_vector.Z = _z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = _x + length / 2.0f;
            temp_vector.Y = _y - length / 2.0f;
            temp_vector.Z = _z - length / 2.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = _x - length / 2.0f;
            temp_vector.Y = _y + length / 2.0f;
            temp_vector.Z = _z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = _x + length / 2.0f;
            temp_vector.Y = _y + length / 2.0f;
            temp_vector.Z = _z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = _x - length / 2.0f;
            temp_vector.Y = _y - length / 2.0f;
            temp_vector.Z = _z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = _x + length / 2.0f;
            temp_vector.Y = _y - length / 2.0f;
            temp_vector.Z = _z + length / 2.0f;
            _vertices.Add(temp_vector);

            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
            shaderType(color);
        }
        public void createEllipsoid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = _x + x * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = _y + y * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = _z + z;
                    _vertices.Add(temp_vector);
                }
            }
            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
            shaderType(color);
        }
        public void createEllipticParaboloid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (float i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * StackAngle;
                y = radiusY * StackAngle;
                z = radiusZ * StackAngle * StackAngle;

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = _x + x * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = _y + y * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = _z + z;
                    _vertices.Add(temp_vector);
                }
            }
            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
            shaderType(color);
        }
        public void createEllips(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle) * 1.5f;

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.Z = _x + x * (float)Math.Cos(sectorAngle);
                    temp_vector.X = _y + y * (float)Math.Sin(sectorAngle);
                    temp_vector.Y = _z + z;
                    _vertices.Add(temp_vector);
                }
            }
            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
            shaderType(color);
        }
        public void createLong(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle) * 5.5f;

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = _x + x * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = _y + y * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = _z + z;
                    _vertices.Add(temp_vector);
                }
            }
            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
            shaderType(color);
        }
        public void createEllipticCone(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (float i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * StackAngle;
                y = radiusY * StackAngle;
                z = radiusZ * StackAngle;

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = _x + x * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = _y + y * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = _z + z;
                    _vertices.Add(temp_vector);
                }
            }
            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
            shaderType(color);
        }
        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            //pivot -> mau rotate di titik mana            
            //vector -> mau rotate di sumbu apa? (x,y,z)            
            //angle -> rotatenya berapa derajat?
            var real_angle = angle;
            angle = MathHelper.DegreesToRadians(angle);
            //mulai ngerotasi           
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = getRotationResult(pivot, vector, angle, _vertices[i]);
            }
            //rotate the euler direction           
            for (int i = 0; i < 3; i++)
            {
                _euler[i] = getRotationResult(pivot, vector, angle, _euler[i], true);
                //NORMALIZE                
                //LANGKAH - LANGKAH                
                //length = akar(x^2+y^2+z^2)               
                float length = (float)Math.Pow(Math.Pow(_euler[i].X, 2.0f) + Math.Pow(_euler[i].Y, 2.0f) + Math.Pow(_euler[i].Z, 2.0f), 0.5f);
                Vector3 temporary = new Vector3(0, 0, 0);
                temporary.X = _euler[i].X / length;
                temporary.Y = _euler[i].Y / length;
                temporary.Z = _euler[i].Z / length;
                _euler[i] = temporary;
            }
            _centerPosition = getRotationResult(pivot, vector, angle, _centerPosition);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);
            foreach (var item in Child)
            {
                item.rotate(pivot, vector, real_angle);
            }
        }
        Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;
            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }
            newPosition.X =
                (float)temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                (float)temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                (float)temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));
            newPosition.Y =
                (float)temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                (float)temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                (float)temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));
            newPosition.Z = (float)temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                (float)temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                (float)temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }
        public void resetEuler()
        {
            _euler[0] = new Vector3(1, 0, 0);
            _euler[1] = new Vector3(0, 1, 0);
            _euler[2] = new Vector3(0, 0, 1);
        }
        public void move(int choice)
        {
            if (choice == 0)
            {
                transform = transform * Matrix4.CreateTranslation(new Vector3(0.005f, 0f, 0f));
            }
            if (choice == 1)
            {
                transform = transform * Matrix4.CreateTranslation(new Vector3(-0.005f, 0f, 0f));
            }
            if (choice == 2)
            {
                transform = transform * Matrix4.CreateTranslation(new Vector3(0f, 0f, 0.005f));
            }
            if (choice == 3)
            {
                transform = transform * Matrix4.CreateTranslation(new Vector3(0f, 0f, -0.005f));
            }
            if (choice == 4)
            {
                transform = transform * Matrix4.CreateTranslation(new Vector3(0f, 0.005f, 0f));
            }
            if (choice == 5)
            {
                transform = transform * Matrix4.CreateTranslation(new Vector3(0f, -0.005f, 0f));
            }

            foreach (var item in Child)
            {
                item.move(choice);
            }
        }

        //ADD CHILD
        public void addChild_Ellipsoid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            Asset3d_Boten newChild_Ellipsoid = new Asset3d_Boten();
            newChild_Ellipsoid.createEllipsoid(radiusX, radiusY, radiusZ, _x, _y, _z, sectorCount, stackCount, color);
            Child.Add(newChild_Ellipsoid);
        }
        public void addChild_Ellips(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            Asset3d_Boten newChild_Ellips = new Asset3d_Boten();
            newChild_Ellips.createEllips(radiusX, radiusY, radiusZ, _x, _y, _z, sectorCount, stackCount, color);
            Child.Add(newChild_Ellips);
        }
        public void addChild_EllipticParaboloid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            Asset3d_Boten newChild_EllipticParaboloid = new Asset3d_Boten();
            newChild_EllipticParaboloid.createEllipticParaboloid(radiusX, radiusY, radiusZ, _x, _y, _z, sectorCount, stackCount, color);
            Child.Add(newChild_EllipticParaboloid);
        }
        public void addChild_Long(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            Asset3d_Boten newChild_Long = new Asset3d_Boten();
            newChild_Long.createLong(radiusX, radiusY, radiusZ, _x, _y, _z, sectorCount, stackCount, color);
            Child.Add(newChild_Long);
        }
        public void addChild_EllipticCone(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount, int color)
        {
            Asset3d_Boten newChild_EllipticCone = new Asset3d_Boten();
            newChild_EllipticCone.createEllipticCone(radiusX, radiusY, radiusZ, _x, _y, _z, sectorCount, stackCount, color);
            Child.Add(newChild_EllipticCone);
        }
        public void addChild_BoxVertices(float x, float y, float z, float length, int color)
        {
            Asset3d_Boten newChild_BoxVertices = new Asset3d_Boten();
            newChild_BoxVertices.createBoxVertices(x, y, z, length, color);
            Child.Add(newChild_BoxVertices);
        }
    }
}
