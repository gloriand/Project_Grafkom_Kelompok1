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

namespace KirbyUTS
{
    class Asset3d_Kirby
    {
        string startupPath = System.IO.Path.GetFullPath(@"..\..\..\");

        List<Vector3> _vertices = new List<Vector3>();
        List<Vector3> _textureVertices = new List<Vector3>();
        List<Vector3> _normals = new List<Vector3>();

        List<uint> _indices = new List<uint>();
        int _vertexBufferObject; //menghandle vertice biar bisa disampaikan ke graphiccard
        int _vertexArayObject;   // mengurus terkait array vertex yg kita kirim\
        //@ EBO
        int _elementBufferObject;
        Shader _shader;    //mengurus terkait apa yang mau ditampilkan

        int[] _pascal = { };
        Matrix4 transform = Matrix4.Identity;
        public Vector3 _centerPosition = new Vector3(0, 0, 0);
        public List<Vector3> _euler = new List<Vector3>();
        public List<Vector3> _fixedeuler = new List<Vector3>();
        Matrix4 _view;
        Matrix4 _projection;
        public List<Asset3d_Kirby> Child = new List<Asset3d_Kirby>();

        public Asset3d_Kirby()
        {
            _euler.Add(new Vector3(1, 0, 0)); //sumbu X
            _euler.Add(new Vector3(0, 1, 0)); //sumbu Y
            _euler.Add(new Vector3(0, 0, 1)); //sumbu Z

            _fixedeuler.Add(new Vector3(1, 0, 0)); //sumbu X
            _fixedeuler.Add(new Vector3(0, 1, 0)); //sumbu Y
            _fixedeuler.Add(new Vector3(0, 0, 1)); //sumbu Z

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

        public void shaderSelect(string type)
        {
            switch (type)
            {
                case "Body":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Body_Kirby.frag");
                    break;
                case "Arms":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Arms.frag");
                    break;
                case "Legs":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Legs_Kirby.frag");
                    break;
                case "Eyes":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Eyes_Kirby.frag");
                    break;
                case "Eyes Black":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Eyes_Black_Kirby.frag");
                    break;
                case "Eyes Highlight":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Eyes_Highlight.frag");
                    break;
                case "Cheek":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Cheek.frag");
                    break;
                case "Mouth":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Mouth.frag");
                    break;
                case "Inner Mouth":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Mouth_Shade.frag");
                    break;
                case "Cap":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Cap_Green.frag");
                    break;
                case "Cap Fluff":
                    _shader = new Shader(startupPath + "/Shader/Shader.vert",
                                 startupPath + "/Shader/Cap_Yellow.frag");
                    break;
            }

            _shader.Use();
        }

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

        public void reposition()
        {
            transform = transform * Matrix4.CreateTranslation(new Vector3(0.005f, 0f, 0f));
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

        public void loadObjFile(string path, string shadertype)
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

            shaderSelect(shadertype);
        }

        public void addObjChild(string path, string shadertype)
        {
            Asset3d_Kirby objChild = new Asset3d_Kirby();
            objChild.loadObjFile(path, shadertype);
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

