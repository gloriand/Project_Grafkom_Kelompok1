using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK.Common;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Grafkom
{
    class Asset3d
    {
        string startupPath = System.IO.Path.GetFullPath(@"..\..\..\");

        List<Vector3> _vertices = new List<Vector3>();
        List<Vector3> _textureVertices = new List<Vector3>();
        List<Vector3> _normals = new List<Vector3>();

        List<uint> _indices = new List<uint>();

        int _vertexBufferObject; //menghandle vertice biar bisa disampaikan ke graphiccard
        int _vertexArayObject;   // mengurus terkait array vertex yg kita kirim
        //@ EBO
        int _elementBufferObject;
        Shader _shader;    //mengurus terkait apa yang mau ditampilkan
        int _verlength  =  0;

        AABB resultCollision = new AABB();

        Vector3 _ObjPos = new Vector3(0, 0, 0);

        int[] _pascal = { };
        Matrix4 transform = Matrix4.Identity;
        //1 0 0 0
        //0 1 0 0
        //0 0 1 0
        //0 0 0 1

        public Vector3 _centerPosition = new Vector3(0, 0, 0);
        public List<Vector3> _euler = new List<Vector3>();
        public List<Vector3> _fixedeuler = new List<Vector3>();
        Matrix4 _view;
        Matrix4 _projection;
        public List<Asset3d> Child = new List<Asset3d>();

        float[] _vertices2 =
       {
             // Position          Normal
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Front face
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Back face
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, // Left face
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Right face
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Bottom face
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Top face
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };

        public Vector3 _materialAmbient = new Vector3(1.0f, 0.5f, 0.31f);
        public Vector3 _materialDiffuse = new Vector3(1.0f, 0.5f, 0.31f);
        public Vector3 _materialSpecular = new Vector3(0.5f, 0.5f, 0.5f);
        public float _materialShininess = 32.0f;

        public Vector3 _lightAmbient = new Vector3(0.5f); //default 0.2f
        public Vector3 _lightDiffuse = new Vector3(0.5f); //default 0.5f
        public Vector3 _lightSpecular = new Vector3(1.0f); //default 1f

        public Asset3d()
        {
            _euler.Add(new Vector3(1, 0, 0)); //sumbu X
            _euler.Add(new Vector3(0, 1, 0)); //sumbu Y
            _euler.Add(new Vector3(0, 0, 1)); //sumbu Z

            _fixedeuler.Add(new Vector3(1, 0, 0)); //sumbu X
            _fixedeuler.Add(new Vector3(0, 1, 0)); //sumbu Y
            _fixedeuler.Add(new Vector3(0, 0, 1)); //sumbu Z

            resultCollision.checkChild = false;
        }

        public struct AABB
        {
            public Vector3 Min;
            public Vector3 Max;
            public bool checkChild;
        };

        // Loop through vertices and get 
        public void CalculateAxisAlignedBox()
        {
            Vector3 max = new Vector3();
            Vector3 min = new Vector3();
            Vector3 tmp = new Vector3();

            for (int i = 0; i < _verlength; ++i)
            {
                if (i == 0)
                {
                    min.X = _vertices[i].X;
                    min.Y = _vertices[i].Y;
                    min.Z = _vertices[i].Z;
                    max.X = _vertices[i].X;
                    max.Y = _vertices[i].Y;
                    max.Z = _vertices[i].Z;
                }
                else
                {
                    tmp.X = _vertices[i].X;
                    tmp.Y = _vertices[i].Y;
                    tmp.Z = _vertices[i].Z;

                    if (tmp.X > max.X)
                    {
                        max.X = tmp.X;
                    }
                    if (tmp.Y > max.Y)
                    {
                        max.Y = tmp.Y;
                    }
                    if (tmp.Z > max.Z)
                    {
                        max.Z = tmp.Z;
                    }
                    //Do this for Y,Z

                    if (tmp.X < min.X)
                    {
                        min.X = tmp.X;
                    }
                    if (tmp.Y < min.Y)
                    {
                        min.Y = tmp.Y;
                    }
                    if (tmp.Z < min.Z)
                    {
                        min.Z = tmp.Z;
                    }
                    //Do this for Y,Z
                }
            }

            if(resultCollision.checkChild == false)
            {
                resultCollision.Min = min;
                resultCollision.Max = max;
                resultCollision.checkChild = true;
            }
            else
            {
                if (min.X < resultCollision.Min.X)
                {
                    resultCollision.Min.X = min.X;
                }
                if (min.Y < resultCollision.Min.Y)
                {
                    resultCollision.Min.Y = min.Y;
                }
                if (min.Z < resultCollision.Min.Z)
                {
                    resultCollision.Min.Z = min.Z;
                }

                if (max.X > resultCollision.Max.X)
                {
                    resultCollision.Max.X = max.X;
                }
                if (max.Y > resultCollision.Max.Y)
                {
                    resultCollision.Max.Y = max.Y;
                }
                if (max.Z > resultCollision.Max.Z)
                {
                    resultCollision.Max.Z = max.Z;
                }
            }

            foreach (var item in Child)
            {
                item.CalculateAxisAlignedBox();
            }
        }
        public Vector3 GetMinAABB()
        {
            return resultCollision.Min;
        }
        public Vector3 GetMaxAABB()
        {
            return resultCollision.Max;
        }

        public void load(int size_x, int size_y)
        {
            _vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);

            _vertexArayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArayObject);

            //par 1 = lokasi index input yang ada di shader
            //par 2 = jum element yg dikirimkan. Dalam contoh ini 3 float tiap vertex
            //par 3 = tipe data yang dikirimkan berjenis apa
            //par 4 = apakah perlu dinormalisasi? true/false
            //par 5 = berapa banyak jum vertex yg kita kirimkan dari variable _vertice. Dalam kasus ini berarti ada 3 vertex * sizeof(float)
            //par 6 = mulai dari index keberapa komputer mulai membaca
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //@5 salah satu
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0 * sizeof(float));
            //menyalakan variable index ke 0 yang ada pada shader
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            if (_indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), size_x / (float)size_y, 0.1f, 100.0f);

            foreach (var item in Child)
            {
                item.load(size_x, size_y);
            }
        }

        //Box object loader, testing
        public void load2(int size_x, int size_y)
        {
            _vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices2.Length * sizeof(float), _vertices2, BufferUsageHint.StaticDraw);

            _vertexArayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArayObject);

            //par 1 = lokasi index input yang ada di shader
            //par 2 = jum element yg dikirimkan. Dalam contoh ini 3 float tiap vertex
            //par 3 = tipe data yang dikirimkan berjenis apa
            //par 4 = apakah perlu dinormalisasi? true/false
            //par 5 = berapa banyak jum vertex yg kita kirimkan dari variable _vertice. Dalam kasus ini berarti ada 3 vertex * sizeof(float)
            //par 6 = mulai dari index keberapa komputer mulai membaca
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            //@5 salah satu
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0 * sizeof(float));
            //menyalakan variable index ke 0 yang ada pada shader
            GL.EnableVertexAttribArray(0);
            //added text
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            if (_indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/lighting.frag");
            _shader.Use();
            //_view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            //_projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), size_x / (float)size_y, 0.1f, 100.0f);

            foreach (var item in Child)
            {
                item.load(size_x, size_y);
            }
        }

        //Rendering - Default
        public void render(int _lines)
        {
            //Step menggambar sebuah object
            //1. enable Shader
            //transform = transform * Matrix4.CreateTranslation(0f, 0.001f, 0f);
            //transform = transform * Matrix4.CreateScale(1.001f);
            //transform = transform * Matrix4.CreateRotationY(0.001f);

            _shader.Use();
            _shader.SetMatrix4("transform", transform);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            //@4
            //mengirim data ke var uniform yg ada di file .vert/.frag
            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, 0.0f, 0.0f, 1.0f, 1.0f);

            //2. Panggil Bind VAO
            GL.BindVertexArray(_vertexArayObject);

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
        }

        //Rendering - With Camera control
        public void render(int _lines, Matrix4 camera_view, Matrix4 camera_projection)
        {
            _shader.Use();
            _shader.SetMatrix4("transform", transform);
            _shader.SetMatrix4("view", camera_view);
            _shader.SetMatrix4("projection", camera_projection);

            GL.BindVertexArray(_vertexArayObject);

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
                item.render(_lines, camera_view, camera_projection);
            }
        }

        //Rendering - With Camera + Lighting...?
        public void render(int _lines, Matrix4 camera_view, Matrix4 camera_projection, Vector3 _lightPos)
        {
            _shader.Use();
            _shader.SetMatrix4("transform", transform);
            _shader.SetMatrix4("view", camera_view);
            _shader.SetMatrix4("projection", camera_projection);

            _shader.SetVector3("lightPos", _lightPos);

            GL.BindVertexArray(_vertexArayObject);

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
                else if (_lines == 4)
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
                }
            }
            foreach (var item in Child)
            {
                item.render(_lines, camera_view, camera_projection, _lightPos);
            }
        }

        //Rendering - With Camera + Lighting...?
        public void render(int _lines, Matrix4 camera_view, Matrix4 camera_projection, Vector3 _lightPos, Vector3 _cameraPos)
        {
            _shader.Use();
            _shader.SetMatrix4("transform", transform);
            _shader.SetMatrix4("view", camera_view);
            _shader.SetMatrix4("projection", camera_projection);

            _shader.SetVector3("viewPos", _cameraPos);

            //Material setting
            _shader.SetVector3("material.ambient", _materialAmbient);
            _shader.SetVector3("material.diffuse", _materialDiffuse);
            _shader.SetVector3("material.specular", _materialSpecular);
            _shader.SetFloat("material.shininess", _materialShininess);

            //Light setting
            _shader.SetVector3("light.ambient", _lightAmbient);
            _shader.SetVector3("light.diffuse", _lightDiffuse);
            _shader.SetVector3("light.specular", _lightSpecular);
            _shader.SetVector3("light.position", _lightPos);

            GL.BindVertexArray(_vertexArayObject);

            if (_indices.Count != 0)
            {
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
                else if (_lines == 4)
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
                }
            }
            foreach (var item in Child)
            {
                item.render(_lines, camera_view, camera_projection, _lightPos, _cameraPos);
            }
        }

        public void animate(string input)
        {
            if (input == "jumpup")
            {
                transform = transform * Matrix4.CreateTranslation(0f, 0.0025f, 0f);
            }
            if (input == "jumpdown")
            {
                transform = transform * Matrix4.CreateTranslation(0f, -0.0025f, 0f);
            }
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

        public void reposition(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            transform = transform * Matrix4.CreateTranslation(new Vector3(x, y, z));

            _ObjPos.X += x;
            _ObjPos.Y += y;
            _ObjPos.Z += z;
        }
        public Vector3 GetPos()
        {
            return _ObjPos;
        }

        public void massrepos(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            reposition(x, y, z);

            foreach (var item in Child)
            {
                item.massrepos(x, y, z);
            }
        }

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

        public void loadObjFile(string path, string colortype, string objectname)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open");
            }

            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(" "));

                    words.RemoveAll(s => s == string.Empty);
                    if (words.Count == 0)
                    {
                        continue;
                    }
                    string type = words[0];
                    words.RemoveAt(0);

                    switch (type)
                    {
                        case "v":
                            _vertices.Add(new Vector3(float.Parse(words[0]) / 2f, float.Parse(words[1]) / 2f, float.Parse(words[2]) / 2f));
                            _verlength++;
                            break;
                        case "vt":
                            _textureVertices.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]), words.Count < 3 ? 0 : float.Parse(words[2])));
                            break;
                        case "vn":
                            _normals.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2])));
                            break;
                        case "f":
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                {
                                    continue;
                                }

                                string[] comps = w.Split("/");

                                _indices.Add(uint.Parse(comps[0]) - 1);
                            }
                            break;
                    }
                }
            }

            _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/lighting.frag");
            _shader.Use();

            colorSelect(colortype, objectname);
        }

        public void colorSelect(string colortype, string objectname)
        {
            if (objectname == "Kirby")
            {
                colorSelect_Kirby(colortype);
            }
            else if (objectname == "Boten")
            {
                colorSelect_Boten(colortype);
            }
            else if (objectname == "Waddle")
            {
                colorSelect_Waddle(colortype);
            }
            else
            {
                colorSelect_Envi(colortype, objectname);
            }
        }

        public void shaderSelect(string type)
        {
            switch (type)
            {
                case "Body":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KBody.frag");
                    break;
                case "Arms":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KArms.frag");
                    break;
                case "Legs":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KLegs.frag");
                    break;
                case "Eyes":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KEyes.frag");
                    break;
                case "Eyes Black":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KEyes_Black.frag");
                    break;
                case "Eyes Highlight":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KEyes_Highlight.frag");
                    break;
                case "Cheek":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KCheek.frag");
                    break;
                case "Mouth":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KMouth.frag");
                    break;
                case "Inner Mouth":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KMouth_Shade.frag");
                    break;
                case "Cap":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KCap_Green.frag");
                    break;
                case "Cap Fluff":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/KCap_Yellow.frag");
                    break;
            }

            _shader.Use();
        }

        public void colorSelect_Kirby(string type)
        {
            switch (type)
            {
                case "Body":
                    _materialAmbient.X = 1.0f;
                    _materialAmbient.Y = 0.7019607843137254f;
                    _materialAmbient.Z = 0.8431372549019608f;

                    _materialDiffuse.X = 1.0f;
                    _materialDiffuse.Y = 0.7019607843137254f;
                    _materialDiffuse.Z = 0.8431372549019608f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Arms":
                    _materialAmbient.X = 1.0f;
                    _materialAmbient.Y = 0.7019607843137254f;
                    _materialAmbient.Z = 0.8431372549019608f;

                    _materialDiffuse.X = 1.0f;
                    _materialDiffuse.Y = 0.7019607843137254f;
                    _materialDiffuse.Z = 0.8431372549019608f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Legs":
                    _materialAmbient.X = 0.8823529411764706f;
                    _materialAmbient.Y = 0.0f;
                    _materialAmbient.Z = 0.30196078431372547f;

                    _materialDiffuse.X = 0.8823529411764706f;
                    _materialDiffuse.Y = 0.0f;
                    _materialDiffuse.Z = 0.30196078431372547f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Eyes":
                    _materialAmbient.X = 0.0f;
                    _materialAmbient.Y = 0.48627450980392156f;
                    _materialAmbient.Z = 0.792156862745098f;

                    _materialDiffuse.X = 0.0f;
                    _materialDiffuse.Y = 0.48627450980392156f;
                    _materialDiffuse.Z = 0.792156862745098f;

                    _materialSpecular.X = 0.5f;
                    _materialSpecular.Y = 0.5f;
                    _materialSpecular.Z = 0.5f;

                    _materialShininess = 32.0f;
                    break;
                case "Eyes Black":
                    _materialAmbient.X = 0.0f;
                    _materialAmbient.Y = 0.0f;
                    _materialAmbient.Z = 0.0f;

                    _materialDiffuse.X = 0.0f;
                    _materialDiffuse.Y = 0.0f;
                    _materialDiffuse.Z = 0.0f;

                    _materialSpecular.X = 0.5f;
                    _materialSpecular.Y = 0.5f;
                    _materialSpecular.Z = 0.5f;

                    _materialShininess = 32.0f;
                    break;
                case "Eyes Highlight":
                    _materialAmbient.X = 1.0f;
                    _materialAmbient.Y = 1.0f;
                    _materialAmbient.Z = 1.0f;

                    _materialDiffuse.X = 1.0f;
                    _materialDiffuse.Y = 1.0f;
                    _materialDiffuse.Z = 1.0f;

                    _materialSpecular.X = 0.5f;
                    _materialSpecular.Y = 0.5f;
                    _materialSpecular.Z = 0.5f;

                    _materialShininess = 32.0f;
                    break;
                case "Cheek":
                    _materialAmbient.X = 0.9803921568627451f;
                    _materialAmbient.Y = 0.48627450980392156f;
                    _materialAmbient.Z = 0.6862745098039216f;

                    _materialDiffuse.X = 0.9803921568627451f;
                    _materialDiffuse.Y = 0.48627450980392156f;
                    _materialDiffuse.Z = 0.6862745098039216f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Mouth":
                    _materialAmbient.X = 0.9803921568627451f;
                    _materialAmbient.Y = 0.48627450980392156f;
                    _materialAmbient.Z = 0.6862745098039216f;

                    _materialDiffuse.X = 0.9803921568627451f;
                    _materialDiffuse.Y = 0.48627450980392156f;
                    _materialDiffuse.Z = 0.6862745098039216f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Inner Mouth":
                    _materialAmbient.X = 0.0f;
                    _materialAmbient.Y = 0.0f;
                    _materialAmbient.Z = 0.0f;

                    _materialDiffuse.X = 0.0f;
                    _materialDiffuse.Y = 0.0f;
                    _materialDiffuse.Z = 0.0f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Cap":
                    _materialAmbient.X = 0.0f;
                    _materialAmbient.Y = 0.7098039215686275f;
                    _materialAmbient.Z = 0.49019607843137253f;

                    _materialDiffuse.X = 0.0f;
                    _materialDiffuse.Y = 0.7098039215686275f;
                    _materialDiffuse.Z = 0.49019607843137253f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Cap Fluff":
                    _materialAmbient.X = 1.0f;
                    _materialAmbient.Y = 0.9254901960784314f;
                    _materialAmbient.Z = 0.37254901960784315f;

                    _materialDiffuse.X = 1.0f;
                    _materialDiffuse.Y = 0.9254901960784314f;
                    _materialDiffuse.Z = 0.37254901960784315f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
            }

            _shader.Use();
        }
        public void colorSelect_Boten(string type)
        {
            switch (type)
            {
                case "Body":
                    _materialAmbient.X = 0.29f;
                    _materialAmbient.Y = 0.66f;
                    _materialAmbient.Z = 0.42f;

                    _materialDiffuse.X = 0.29f;
                    _materialDiffuse.Y = 0.66f;
                    _materialDiffuse.Z = 0.42f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Thorn":
                    _materialAmbient.X = 0.63f;
                    _materialAmbient.Y = 0.64f;
                    _materialAmbient.Z = 0.64f;

                    _materialDiffuse.X = 0.63f;
                    _materialDiffuse.Y = 0.64f;
                    _materialDiffuse.Z = 0.64f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Eyes":
                    _materialAmbient.X = 0.15f;
                    _materialAmbient.Y = 0.15f;
                    _materialAmbient.Z = 0.15f;

                    _materialDiffuse.X = 0.15f;
                    _materialDiffuse.Y = 0.15f;
                    _materialDiffuse.Z = 0.15f;

                    _materialSpecular.X = 0.5f;
                    _materialSpecular.Y = 0.5f;
                    _materialSpecular.Z = 0.5f;

                    _materialShininess = 32.0f;
                    break;
                case "Bristle":
                    _materialAmbient.X = 0.92f;
                    _materialAmbient.Y = 0.66f;
                    _materialAmbient.Z = 0.09f;

                    _materialDiffuse.X = 0.92f;
                    _materialDiffuse.Y = 0.66f;
                    _materialDiffuse.Z = 0.09f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Pita1":
                    _materialAmbient.X = 0.87f;
                    _materialAmbient.Y = 0.37f;
                    _materialAmbient.Z = 0.37f;

                    _materialDiffuse.X = 0.87f;
                    _materialDiffuse.Y = 0.37f;
                    _materialDiffuse.Z = 0.37f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Pita2":
                    _materialAmbient.X = 0.42f;
                    _materialAmbient.Y = 0.29f;
                    _materialAmbient.Z = 0.29f;

                    _materialDiffuse.X = 0.42f;
                    _materialDiffuse.Y = 0.29f;
                    _materialDiffuse.Z = 0.29f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
            }

            _shader.Use();
        }
        public void colorSelect_Waddle(string type)
        {
            switch (type)
            {
                case "Body":
                    _materialAmbient.X = 1.0f;
                    _materialAmbient.Y = 0.4f;
                    _materialAmbient.Z = 0.3f;

                    _materialDiffuse.X = 1.0f;
                    _materialDiffuse.Y = 0.4f;
                    _materialDiffuse.Z = 0.3f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Eyes White":
                    _materialAmbient.X = 1.0f;
                    _materialAmbient.Y = 1.0f;
                    _materialAmbient.Z = 1.0f;

                    _materialDiffuse.X = 1.0f;
                    _materialDiffuse.Y = 1.0f;
                    _materialDiffuse.Z = 1.0f;

                    _materialSpecular.X = 0.5f;
                    _materialSpecular.Y = 0.5f;
                    _materialSpecular.Z = 0.5f;

                    _materialShininess = 32.0f;
                    break;
                case "Eyes Black":
                    _materialAmbient.X = 0.15f;
                    _materialAmbient.Y = 0.15f;
                    _materialAmbient.Z = 0.15f;

                    _materialDiffuse.X = 0.15f;
                    _materialDiffuse.Y = 0.15f;
                    _materialDiffuse.Z = 0.15f;

                    _materialSpecular.X = 0.5f;
                    _materialSpecular.Y = 0.5f;
                    _materialSpecular.Z = 0.5f;

                    _materialShininess = 32.0f;
                    break;
                case "Arms":
                    _materialAmbient.X = 1.0f;
                    _materialAmbient.Y = 0.6f;
                    _materialAmbient.Z = 0.3f;

                    _materialDiffuse.X = 1.0f;
                    _materialDiffuse.Y = 0.6f;
                    _materialDiffuse.Z = 0.3f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Legs":
                    _materialAmbient.X = 1.0f;
                    _materialAmbient.Y = 0.6f;
                    _materialAmbient.Z = 0.3f;

                    _materialDiffuse.X = 1.0f;
                    _materialDiffuse.Y = 0.6f;
                    _materialDiffuse.Z = 0.3f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Hat":
                    _materialAmbient.X = 0.4f;
                    _materialAmbient.Y = 0.8f;
                    _materialAmbient.Z = 1.0f;

                    _materialDiffuse.X = 0.4f;
                    _materialDiffuse.Y = 0.8f;
                    _materialDiffuse.Z = 1.0f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Tie":
                    _materialAmbient.X = 0.7f;
                    _materialAmbient.Y = 0.1f;
                    _materialAmbient.Z = 0.0f;

                    _materialDiffuse.X = 0.7f;
                    _materialDiffuse.Y = 0.1f;
                    _materialDiffuse.Z = 0.0f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
                case "Tie Polka":
                    _materialAmbient.X = 0.2f;
                    _materialAmbient.Y = 0.5f;
                    _materialAmbient.Z = 0.3f;

                    _materialDiffuse.X = 0.2f;
                    _materialDiffuse.Y = 0.5f;
                    _materialDiffuse.Z = 0.3f;

                    _materialSpecular.X = 0.2f;
                    _materialSpecular.Y = 0.2f;
                    _materialSpecular.Z = 0.2f;

                    _materialShininess = 32.0f;
                    break;
            }
        }
        
        public void colorSelect_Envi(string type, string objname)
        {
            if(objname == "Tree")
            {
                switch(type)
                {
                    case "TreeTrunk":
                        _materialAmbient.X = 0.49f;
                        _materialAmbient.Y = 0.22f;
                        _materialAmbient.Z = 0.047f;

                        _materialDiffuse.X = 0.49f;
                        _materialDiffuse.Y = 0.22f;
                        _materialDiffuse.Z = 0.047f;

                        _materialSpecular.X = 0.0f;
                        _materialSpecular.Y = 0.0f;
                        _materialSpecular.Z = 0.0f;

                        _materialShininess = 2.0f;
                        break;
                    case "TreeLeaf":
                        _materialAmbient.X = 0.24f;
                        _materialAmbient.Y = 0.49f;
                        _materialAmbient.Z = 0.09f;

                        _materialDiffuse.X = 0.24f;
                        _materialDiffuse.Y = 0.49f;
                        _materialDiffuse.Z = 0.09f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;

                }
            }

            if (objname == "Bush")
            {
                switch (type)
                {
                    case "Bush":
                        _materialAmbient.X = 0.24f;
                        _materialAmbient.Y = 0.49f;
                        _materialAmbient.Z = 0.09f;

                        _materialDiffuse.X = 0.24f;
                        _materialDiffuse.Y = 0.49f;
                        _materialDiffuse.Z = 0.09f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                }
            }

            if (objname == "House")
            {
                switch (type)
                {
                    case "House":
                        _materialAmbient.X = 1f;
                        _materialAmbient.Y = 0.75f;
                        _materialAmbient.Z = 0.41f;

                        _materialDiffuse.X = 1f;
                        _materialDiffuse.Y = 0.75f;
                        _materialDiffuse.Z = 0.41f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                    case "Roof":
                        _materialAmbient.X = 0.69f;
                        _materialAmbient.Y = 0.18f;
                        _materialAmbient.Z = 0.18f;

                        _materialDiffuse.X = 0.69f;
                        _materialDiffuse.Y = 0.18f;
                        _materialDiffuse.Z = 0.18f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                    case "Door":
                        _materialAmbient.X = 0.36f;
                        _materialAmbient.Y = 0.24f;
                        _materialAmbient.Z = 0.18f;

                        _materialDiffuse.X = 0.36f;
                        _materialDiffuse.Y = 0.24f;
                        _materialDiffuse.Z = 0.18f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                    case "Handle":
                        _materialAmbient.X = 0.62f;
                        _materialAmbient.Y = 0.62f;
                        _materialAmbient.Z = 0.62f;

                        _materialDiffuse.X = 0.62f;
                        _materialDiffuse.Y = 0.62f;
                        _materialDiffuse.Z = 0.62f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                    case "Window":
                        _materialAmbient.X = 0.52f;
                        _materialAmbient.Y = 0.87f;
                        _materialAmbient.Z = 1f;

                        _materialDiffuse.X = 0.52f;
                        _materialDiffuse.Y = 0.87f;
                        _materialDiffuse.Z = 1f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                }
            }

            if (objname == "Terrain")
            {
                switch (type)
                {
                    case "Ground":
                        _materialAmbient.X = 0.32f;
                        _materialAmbient.Y = 0.57f;
                        _materialAmbient.Z = 0.35f;

                        _materialDiffuse.X = 0.32f;
                        _materialDiffuse.Y = 0.57f;
                        _materialDiffuse.Z = 0.35f;

                        _materialSpecular.X = 0.0f;
                        _materialSpecular.Y = 0.0f;
                        _materialSpecular.Z = 0.0f;

                        _materialShininess = 2.0f;
                        break;
                    case "Road":
                        _materialAmbient.X = 0.60f;
                        _materialAmbient.Y = 0.58f;
                        _materialAmbient.Z = 0.51f;

                        _materialDiffuse.X = 0.60f;
                        _materialDiffuse.Y = 0.58f;
                        _materialDiffuse.Z = 0.51f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                }
            }

            if (objname == "Fence")
            {
                switch (type)
                {
                    case "Fence":
                        _materialAmbient.X = 0.78f;
                        _materialAmbient.Y = 0.55f;
                        _materialAmbient.Z = 0.35f;

                        _materialDiffuse.X = 0.78f;
                        _materialDiffuse.Y = 0.55f;
                        _materialDiffuse.Z = 0.35f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                }
            }

            if (objname == "Pot")
            {
                switch (type)
                {
                    case "Pot":
                        _materialAmbient.X = 0.67f;
                        _materialAmbient.Y = 0.64f;
                        _materialAmbient.Z = 0.57f;

                        _materialDiffuse.X = 0.67f;
                        _materialDiffuse.Y = 0.64f;
                        _materialDiffuse.Z = 0.57f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                    case "Plant":
                        _materialAmbient.X = 0.24f;
                        _materialAmbient.Y = 0.49f;
                        _materialAmbient.Z = 0.09f;

                        _materialDiffuse.X = 0.24f;
                        _materialDiffuse.Y = 0.49f;
                        _materialDiffuse.Z = 0.09f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                    case "Board":
                        _materialAmbient.X = 0.36f;
                        _materialAmbient.Y = 0.24f;
                        _materialAmbient.Z = 0.18f;

                        _materialDiffuse.X = 0.36f;
                        _materialDiffuse.Y = 0.24f;
                        _materialDiffuse.Z = 0.18f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                }
            }

            if (objname == "Bench")
            {
                switch (type)
                {
                    case "Bench":
                        _materialAmbient.X = 0.85f;
                        _materialAmbient.Y = 0.7f;
                        _materialAmbient.Z = 0.52f;

                        _materialDiffuse.X = 0.85f;
                        _materialDiffuse.Y = 0.7f;
                        _materialDiffuse.Z = 0.52f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                }
            }

            if (objname == "Trashbin")
            {
                switch (type)
                {
                    case "Trashbin":
                        _materialAmbient.X = 0.56f;
                        _materialAmbient.Y = 0.5f;
                        _materialAmbient.Z = 0.42f;

                        _materialDiffuse.X = 0.56f;
                        _materialDiffuse.Y = 0.5f;
                        _materialDiffuse.Z = 0.42f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                }
            }

            if (objname == "Pool")
            {
                switch (type)
                {
                    case "Pool":
                        _materialAmbient.X = 0.82f;
                        _materialAmbient.Y = 0.91f;
                        _materialAmbient.Z = 0.89f;

                        _materialDiffuse.X = 0.82f;
                        _materialDiffuse.Y = 0.91f;
                        _materialDiffuse.Z = 0.89f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                    case "Water":
                        _materialAmbient.X = 0.52f;
                        _materialAmbient.Y = 0.87f;
                        _materialAmbient.Z = 1f;

                        _materialDiffuse.X = 0.52f;
                        _materialDiffuse.Y = 0.87f;
                        _materialDiffuse.Z = 1f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                }
            }

            if (objname == "Swing")
            {
                switch (type)
                {
                    case "Handle":
                        _materialAmbient.X = 0.62f;
                        _materialAmbient.Y = 0.62f;
                        _materialAmbient.Z = 0.62f;

                        _materialDiffuse.X = 0.62f;
                        _materialDiffuse.Y = 0.62f;
                        _materialDiffuse.Z = 0.62f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                    case "Rope":
                        _materialAmbient.X = 1f;
                        _materialAmbient.Y = 0.99f;
                        _materialAmbient.Z = 0.86f;

                        _materialDiffuse.X = 1f;
                        _materialDiffuse.Y = 0.99f;
                        _materialDiffuse.Z = 0.86f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                    case "Seat":
                        _materialAmbient.X = 1f;
                        _materialAmbient.Y = 0.9f;
                        _materialAmbient.Z = 0.32f;

                        _materialDiffuse.X = 1f;
                        _materialDiffuse.Y = 0.9f;
                        _materialDiffuse.Z = 0.32f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                }
            }

            if (objname == "Fountain")
            {
                switch (type)
                {
                    case "Metal":
                        _materialAmbient.X = 0.67f;
                        _materialAmbient.Y = 0.64f;
                        _materialAmbient.Z = 0.57f;

                        _materialDiffuse.X = 0.67f;
                        _materialDiffuse.Y = 0.64f;
                        _materialDiffuse.Z = 0.57f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                    case "Handle":
                        _materialAmbient.X = 0.62f;
                        _materialAmbient.Y = 0.62f;
                        _materialAmbient.Z = 0.62f;

                        _materialDiffuse.X = 0.62f;
                        _materialDiffuse.Y = 0.62f;
                        _materialDiffuse.Z = 0.62f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                    case "Water":
                        _materialAmbient.X = 0.52f;
                        _materialAmbient.Y = 0.87f;
                        _materialAmbient.Z = 1f;

                        _materialDiffuse.X = 0.52f;
                        _materialDiffuse.Y = 0.87f;
                        _materialDiffuse.Z = 1f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                }
            }

            if (objname == "Slide")
            {
                switch (type)
                {
                    case "Handle":
                        _materialAmbient.X = 0.62f;
                        _materialAmbient.Y = 0.62f;
                        _materialAmbient.Z = 0.62f;

                        _materialDiffuse.X = 0.62f;
                        _materialDiffuse.Y = 0.62f;
                        _materialDiffuse.Z = 0.62f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 32.0f;
                        break;
                    case "Ladder":
                        _materialAmbient.X = 0.64f;
                        _materialAmbient.Y = 0.82f;
                        _materialAmbient.Z = 1f;

                        _materialDiffuse.X = 0.64f;
                        _materialDiffuse.Y = 0.82f;
                        _materialDiffuse.Z = 1f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                    case "Surfpatch":
                        _materialAmbient.X = 0.98f;
                        _materialAmbient.Y = 0.027f;
                        _materialAmbient.Z = 0.086f;

                        _materialDiffuse.X = 0.98f;
                        _materialDiffuse.Y = 0.027f;
                        _materialDiffuse.Z = 0.086f;

                        _materialSpecular.X = 0.2f;
                        _materialSpecular.Y = 0.2f;
                        _materialSpecular.Z = 0.2f;

                        _materialShininess = 2.0f;
                        break;
                }
            }


            if (objname == "")
            {
                switch (type)
                {
                    case "":
                        break;
                }
            }

            _shader.Use();
        }

        public void addChild(float x, float y, float z, float length)
        {

        }

        public void addObjChild(string path, string shadertype, string objectname)
        {
            Asset3d objChild = new Asset3d();
            objChild.loadObjFile(path, shadertype, objectname);
            Child.Add(objChild);
        }

        public void createBoxVertices(float x, float y, float z, float length) //titik pusat dari box dimana
        {
            _centerPosition.X = x;
            _centerPosition.Y = y;
            _centerPosition.Z = z;
            //length panjang dari titik kubus
            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);

            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
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
        }
        public void createHyperboloidSatuSisi(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z)
        {
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            for (float u = -pi; u <= pi; u += pi / 30)

            {
                for (float v = -pi / 2; v <= pi / 2; v += pi / 30)
                {
                    temp_vector.Z = _x + (1.0f / (float)Math.Cos(v)) * (float)Math.Cos(u) * radiusX;
                    temp_vector.Y = _y + (1.0f / (float)Math.Cos(v)) * (float)Math.Sin(u) * radiusY;
                    temp_vector.X = _z + (float)Math.Tan(v) * radiusZ;
                    _vertices.Add(temp_vector);
                }
            }
        }
        public void createHyperboloidDuaSisi(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z)
        {
            float pi = (float)Math.PI;
            Vector3 temp_vector;

            for (float u = -pi / 2; u <= pi / 2; u += pi / 30)//u range di tabel
            {
                for (float v = -pi / 2; v <= pi / 2; v += pi / 30) //v range di tabel, pembagi semakin besar, garis semakin banyak
                {
                    temp_vector.Z = _x + (float)Math.Tan(v) * (float)Math.Cos(u) * radiusX;
                    temp_vector.X = _y + (float)Math.Tan(v) * (float)Math.Sin(u) * radiusY;
                    temp_vector.Y = _z + (1 / (float)Math.Cos(v)) * radiusZ;
                    _vertices.Add(temp_vector);
                }
            }
            for (float u = pi / 2; u <= 3 * (pi / 2); u += pi / 30)//u range di tabel
            {
                for (float v = -pi / 2; v <= pi / 2; v += pi / 30) //v range di tabel, pembagi semakin besar, garis semakin banyak
                {
                    temp_vector.Z = _x + (float)Math.Tan(v) * (float)Math.Cos(u) * radiusX;
                    temp_vector.X = _y + (float)Math.Tan(v) * (float)Math.Sin(u) * radiusY;
                    temp_vector.Y = _z + (1 / (float)Math.Cos(v)) * radiusZ;
                    _vertices.Add(temp_vector);
                }
            }
        }
        public void createEllipticCone(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z)
        {
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            for (float u = -pi; u <= pi; u += pi / 30)
            {
                for (float v = -pi / 2; v <= pi / 2; v += pi / 30)
                {
                    temp_vector.Z = _x + v * (float)Math.Cos(u) * radiusX;
                    temp_vector.X = _y + v * (float)Math.Sin(u) * radiusY;
                    temp_vector.Y = _z + v * radiusZ;
                    _vertices.Add(temp_vector);
                }
            }
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

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * StackAngle;
                y = radiusY * StackAngle;
                z = radiusZ * (float)Math.Pow(StackAngle, 2);

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
                    //temp_vector.X = _x + x * (float)Math.Cos(sectorAngle);
                    //temp_vector.Y = _y + y * (float)Math.Sin(sectorAngle);
                    //temp_vector.Z = _z + z;
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
        }
        public void createHyperboloidParaboloid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z)
        {
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            for (float u = -pi; u <= pi; u += pi / 30)
            {
                for (float v = 0; v <= pi / 2; v += pi / 30)
                {
                    temp_vector.Z = _x + v * (float)Math.Tan(u) * radiusX;
                    temp_vector.Y = _y + v * (1.0f / (float)Math.Cos(u)) * radiusY;
                    temp_vector.X = _z + v * v * radiusZ;
                    _vertices.Add(temp_vector);
                }
            }
        }

        //public void rotate(Vector3 pivot, Vector3 vector, float angle)
        //{
        //    //pivot -> mau rotate di titik mana            
        //    //vector -> mau rotate di sumbu apa? (x,y,z)            
        //    //angle -> rotatenya berapa derajat?
        //    var real_angle = angle;
        //    angle = MathHelper.DegreesToRadians(angle);
        //    //mulai ngerotasi           
        //    for (int i = 0; i < _vertices.Count; i++)
        //    {
        //        _vertices[i] = getRotationResult(pivot, vector, angle, _vertices[i]);
        //    }
        //    //rotate the euler direction           
        //    for (int i = 0; i < 3; i++)
        //    {
        //        _euler[i] = getRotationResult(pivot, vector, angle, _euler[i], true);
        //        //NORMALIZE                
        //        //LANGKAH - LANGKAH                
        //        //length = akar(x^2+y^2+z^2)               
        //        float length = (float)Math.Pow(Math.Pow(_euler[i].X, 2.0f) + Math.Pow(_euler[i].Y, 2.0f) + Math.Pow(_euler[i].Z, 2.0f), 0.5f);
        //        Vector3 temporary = new Vector3(0, 0, 0);
        //        temporary.X = _euler[i].X / length;
        //        temporary.Y = _euler[i].Y / length;
        //        temporary.Z = _euler[i].Z / length;
        //        _euler[i] = temporary;
        //    }
        //    _centerPosition = getRotationResult(pivot, vector, angle, _centerPosition);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        //    GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);
        //    foreach (var item in Child)
        //    {
        //        item.rotate(pivot, vector, real_angle);
        //    }
        //}

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

                _fixedeuler[i] = getRotationResult(pivot, vector, angle, _fixedeuler[i], true);
                //NORMALIZE                
                //LANGKAH - LANGKAH                
                //length = akar(x^2+y^2+z^2)
                float flength = (float)Math.Pow(Math.Pow(_fixedeuler[i].X, 2.0f) + Math.Pow(_fixedeuler[i].Y, 2.0f) + Math.Pow(_fixedeuler[i].Z, 2.0f), 0.5f);
                Vector3 ftemporary = new Vector3(0, 0, 0);
                ftemporary.X = _fixedeuler[i].X / flength;
                ftemporary.Y = _fixedeuler[i].Y / flength;
                ftemporary.Z = _fixedeuler[i].Z / flength;
                _fixedeuler[i] = ftemporary;
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
    }
}

