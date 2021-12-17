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
    class Window : GameWindow
    {
        string startupPath = System.IO.Path.GetFullPath(@"..\..\..\");

        //List<Vector3> _Vertices = new List<Vector3>();

        Asset3d[] _object3d = new Asset3d[1];

        //Kirby Characters
        Asset3d[] _Kirby = new Asset3d[1];

        Asset3d[] _Boten = new Asset3d[1];

        Asset3d[] _Waddle = new Asset3d[1];

        static int nTree = 3;
        static int nBush = 5;
        static int nFence = 4;
        static int nPlant = 4;

        //Environmental Objects
        Asset3d[] _Tree = new Asset3d[nTree];
        Asset3d[] _Bush = new Asset3d[nBush];
        Asset3d[] _House = new Asset3d[1];
        Asset3d[] _Ground = new Asset3d[1];
        Asset3d[] _Fence = new Asset3d[nFence];
        Asset3d[] _Plant = new Asset3d[nPlant];
        Asset3d[] _Board = new Asset3d[1];
        Asset3d[] _Bench = new Asset3d[1];
        Asset3d[] _Trashbin = new Asset3d[1];
        Asset3d[] _Pool = new Asset3d[1];
        Asset3d[] _Swing = new Asset3d[1];
        Asset3d[] _Fountain = new Asset3d[1];
        Asset3d[] _Slide = new Asset3d[1];

        // Misc variables for objects
        bool _animating = false;
        bool _amove = false;
        bool _awave = false;
        bool _ajump = false;
        int _co = 0;

        bool _wjump = false;
        bool _wwalk = false;
        int _co1 = 0;

        bool _BotenMelirik = false;
        bool _BotenWalk = false;
        bool _BotenJump = false;
        int _co2 = 0;

        // Camera
        Camera _camera;
        bool _firstmove = true;
        Vector2 _lastPos;
        Vector3 _objectPost = new Vector3(0.0f, 0.0f, 0.0f);
        float _rotationSpeed = 0.1f;

        Vector3 _lightPos = new Vector3(0.0f, 10.0f, 0.0f);

        Lamp _lamp;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            KirbyObjectInitialize();
            BotenObjectInitialize();
            WaddleObjectInitialize();

            EnvironmentalObjectInitialize();

            _object3d[0] = new Asset3d();
        }

        public void KirbyObjectInitialize()
        {
            _Kirby[0] = new Asset3d();
        }
        public void BotenObjectInitialize()
        {
            _Boten[0] = new Asset3d();
        }
        public void WaddleObjectInitialize()
        {
            _Waddle[0] = new Asset3d();
        }
        public void EnvironmentalObjectInitialize()
        {
            TreeObjectInitialize();
            BushObjectInitialize();
            HouseObjectInitialize();
            GroundObjectInitialize();
            FenceObjectInitialize();
            PlantObjectInitialize();
            BenchObjectInitialize();
            TrashbinObjectInitialize();
            PoolObjectInitialize();
            SwingObjectInitialize();
            FountainObjectInitialize();
            SlideObjectInitialize();
        }
        public void TreeObjectInitialize()
        {
            for(int i = 0; i < nTree; i++)
            {
                _Tree[i] = new Asset3d();
            }
        }
        public void BushObjectInitialize()
        {
            for (int i = 0; i < nBush; i++)
            {
                _Bush[i] = new Asset3d();
            }
        }
        public void HouseObjectInitialize()
        {
            _House[0] = new Asset3d();
        }
        public void GroundObjectInitialize()
        {
            _Ground[0] = new Asset3d();
        }
        public void FenceObjectInitialize()
        {
            for (int i = 0; i < nFence; i++)
            {
                _Fence[i] = new Asset3d();
            }
        }
        public void PlantObjectInitialize()
        {
            for (int i = 0; i < nPlant; i++)
            {
                _Plant[i] = new Asset3d();
            }
            _Board[0] = new Asset3d();
        }
        public void BenchObjectInitialize()
        {
            _Bench[0] = new Asset3d();
        }
        public void TrashbinObjectInitialize()
        {
            _Trashbin[0] = new Asset3d();
        }
        public void PoolObjectInitialize()
        {
            _Pool[0] = new Asset3d();
        }
        public void SwingObjectInitialize()
        {
            _Swing[0] = new Asset3d();
        }
        public void FountainObjectInitialize()
        {
            _Fountain[0] = new Asset3d();
        }
        public void SlideObjectInitialize()
        {
            _Slide[0] = new Asset3d();
        }

        public Matrix4 generateArbRotationMatrix(Vector3 axis, Vector3 center, float degree)
        {
            var rads = MathHelper.DegreesToRadians(degree);

            //arbitary rotation
            var secretFormula = new float[4, 4]
            {
                { (float)Math.Cos(rads) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(rads)), axis.X* axis.Y * (1 - (float)Math.Cos(rads)) - axis.Z * (float)Math.Sin(rads),    axis.X * axis.Z * (1 - (float)Math.Cos(rads)) + axis.Y * (float)Math.Sin(rads),   0 },
                { axis.Y * axis.X * (1 - (float)Math.Cos(rads)) + axis.Z * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(rads)), axis.Y * axis.Z * (1 - (float)Math.Cos(rads)) - axis.X * (float)Math.Sin(rads),   0 },
                { axis.Z * axis.X * (1 - (float)Math.Cos(rads)) - axis.Y * (float)Math.Sin(rads),   axis.Z * axis.Y * (1 - (float)Math.Cos(rads)) + axis.X * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(rads)), 0 },
                { 0, 0, 0, 1}
            };
            var secretFormulaMatrix = new Matrix4
            (
                new Vector4(secretFormula[0, 0], secretFormula[0, 1], secretFormula[0, 2], secretFormula[0, 3]),
                new Vector4(secretFormula[1, 0], secretFormula[1, 1], secretFormula[1, 2], secretFormula[1, 3]),
                new Vector4(secretFormula[2, 0], secretFormula[2, 1], secretFormula[2, 2], secretFormula[2, 3]),
                new Vector4(secretFormula[3, 0], secretFormula[3, 1], secretFormula[3, 2], secretFormula[3, 3])
            );
            return secretFormulaMatrix;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);

            //Background color
            GL.ClearColor(0.22f, 0.43f, 0.57f, 1.0f);

            //_object3d[0].createBoxVertices(0.0f, 0.0f, 0.0f, 0.25f);
            //_object3d[0].addChild(0.3f, 0.3f, 0.0f, 0.25f);
            
            _object3d[0].load2(Size.X, Size.Y);

            createObjects();

            _camera = new Camera(new Vector3(0, 0, 4), Size.X / Size.Y);
            _lamp = new Lamp(_lightPos);
 
            CursorGrabbed = true;

            _lamp.load();
        }

        public void createObjects()
        {
            createKirby();
            createBoten();
            createWaddle();
            createTree();
            createBush();
            createHouse();
            createGround();
            createFence();
            createPlant();
            createBench();
            createTrashbin();
            createPool();
            createSwing();
            createFountain();
            createSlide();

            repositionItem();
        }

        public void createKirby()
        {
            //X - Body
            _Kirby[0].loadObjFile(startupPath + "/Asset/Kirby/Body.obj", "Body", "Kirby");

            //0 - Cheek
            _Kirby[0].addObjChild(startupPath + "/Asset/Kirby/Cheek.obj", "Cheek", "Kirby");

            //1 - Mouth
            _Kirby[0].addObjChild(startupPath + "/Asset/Kirby/Mouth.obj", "Mouth", "Kirby");
            _Kirby[0].Child[1].addObjChild(startupPath + "/Asset/Kirby/Mouth Inner.obj", "Inner Mouth", "Kirby");

            //2 - Eyes
            _Kirby[0].addObjChild(startupPath + "/Asset/Kirby/Eyes.obj", "Eyes", "Kirby");
            _Kirby[0].Child[2].addObjChild(startupPath + "/Asset/Kirby/Eyes Black.obj", "Eyes Black", "Kirby");
            _Kirby[0].Child[2].addObjChild(startupPath + "/Asset/Kirby/Eyes Highlight.obj", "Eyes Highlight", "Kirby");

            //3 - Cap
            _Kirby[0].addObjChild(startupPath + "/Asset/Kirby/Cap.obj", "Cap", "Kirby");
            _Kirby[0].Child[3].addObjChild(startupPath + "/Asset/Kirby/Cap Ball.obj", "Cap Fluff", "Kirby");
            _Kirby[0].Child[3].addObjChild(startupPath + "/Asset/Kirby/Cap Fluff.obj", "Cap Fluff", "Kirby");

            //4 & 5 - Arms
            _Kirby[0].addObjChild(startupPath + "/Asset/Kirby/Hand Left.obj", "Arms", "Kirby");
            _Kirby[0].addObjChild(startupPath + "/Asset/Kirby/Hand Right.obj", "Arms", "Kirby");

            //6 & 7 - Legs
            _Kirby[0].addObjChild(startupPath + "/Asset/Kirby/Leg Left.obj", "Legs", "Kirby");
            _Kirby[0].addObjChild(startupPath + "/Asset/Kirby/Leg Right.obj", "Legs", "Kirby");


            _Kirby[0].load(Size.X, Size.Y);
        }
        public void createBoten()
        {
            //Body
            _Boten[0].loadObjFile(startupPath + "/Asset/Boten/body_boten.obj", "Body", "Boten");
            //Eyes
            _Boten[0].addObjChild(startupPath + "/Asset/Boten/eyes_boten.obj", "Eyes", "Boten");
            _Boten[0].Child[0].addObjChild(startupPath + "/Asset/Boten/pupil_boten.obj", "Thorn", "Boten");
            //Thorn
            _Boten[0].addObjChild(startupPath + "/Asset/Boten/thorn_boten.obj", "Thorn", "Boten");
            //Bristle
            _Boten[0].addObjChild(startupPath + "/Asset/Boten/bulu_boten.obj", "Bristle", "Boten");
            //Pita
            _Boten[0].addObjChild(startupPath + "/Asset/Boten/pita1_boten.obj", "Pita1", "Boten");
            _Boten[0].Child[3].addObjChild(startupPath + "/Asset/Boten/pita2_boten.obj", "Pita2", "Boten");

            _Boten[0].load(Size.X, Size.Y);
        }
        public void createWaddle()
        {
            //body
            _Waddle[0].loadObjFile(startupPath + "/Asset/Waddle/body.obj", "Body", "Waddle");
            //eyes
            _Waddle[0].addObjChild(startupPath + "/Asset/Waddle/big_white_eyeball.obj", "Eyes White", "Waddle");
            _Waddle[0].Child[0].addObjChild(startupPath + "/Asset/Waddle/small_black_eyeball.obj", "Eyes Black", "Waddle");
            _Waddle[0].Child[0].addObjChild(startupPath + "/Asset/Waddle/small_black_eyeball_2.obj", "Eyes Black", "Waddle");
            _Waddle[0].Child[0].addObjChild(startupPath + "/Asset/Waddle/small_black_eyeball_3.obj", "Eyes White", "Waddle");
            //arms
            _Waddle[0].addObjChild(startupPath + "/Asset/Waddle/left_arm.obj", "Arms", "Waddle");
            _Waddle[0].addObjChild(startupPath + "/Asset/Waddle/right_arm.obj", "Arms", "Waddle");
            //legs
            _Waddle[0].addObjChild(startupPath + "/Asset/Waddle/left_leg.obj", "Legs", "Waddle");
            _Waddle[0].addObjChild(startupPath + "/Asset/Waddle/right_leg.obj", "Legs", "Waddle");
            //topi
            _Waddle[0].addObjChild(startupPath + "/Asset/Waddle/hat.obj", "Hat", "Waddle");
            //tie
            _Waddle[0].addObjChild(startupPath + "/Asset/Waddle/tie_mid.obj", "Tie", "Waddle");
            _Waddle[0].Child[6].addObjChild(startupPath + "/Asset/Waddle/tie.obj", "Tie", "Waddle");

            _Waddle[0].load(Size.X, Size.Y);
        }

        public void createTree()
        {
            for(int i = 0; i < nTree; i++)
            {
                _Tree[i].loadObjFile(startupPath + "/Asset/Environment/Tree/TreeLeaf.obj", "TreeLeaf", "Tree");
                _Tree[i].addObjChild(startupPath + "/Asset/Environment/Tree/TreeTrunk.obj", "TreeTrunk", "Tree");

                _Tree[i].load(Size.X, Size.Y);
            }
        }
        public void createBush()
        {
            for (int i = 0; i < nBush; i++)
            {
                _Bush[i].loadObjFile(startupPath + "/Asset/Environment/Tree/Bush.obj", "Bush", "Bush");

                _Bush[i].load(Size.X, Size.Y);
            }
        }
        public void createHouse()
        {
            _House[0].loadObjFile(startupPath + "/Asset/Environment/House/House.obj", "House", "House");
            _House[0].addObjChild(startupPath + "/Asset/Environment/House/Roof.obj", "Roof", "House");
            _House[0].addObjChild(startupPath + "/Asset/Environment/House/Door.obj", "Door", "House");
            _House[0].addObjChild(startupPath + "/Asset/Environment/House/Handle.obj", "Handle", "House");
            _House[0].addObjChild(startupPath + "/Asset/Environment/House/Jendela.obj", "Door", "House");
            _House[0].addObjChild(startupPath + "/Asset/Environment/House/Kaca.obj", "Window", "House");

            _House[0].load(Size.X, Size.Y);
        }
        public void createGround()
        {
            _Ground[0].loadObjFile(startupPath + "/Asset/Environment/Ground.obj", "Ground", "Terrain");
            _Ground[0].addObjChild(startupPath + "/Asset/Environment/Jalan.obj", "Road", "Terrain");

            _Ground[0].load(Size.X, Size.Y);
        }
        public void createFence()
        {
            for (int i = 0; i < nFence; i++)
            {
                _Fence[i].loadObjFile(startupPath + "/Asset/Environment/Fences.obj", "Fence", "Fence");

                _Fence[i].load(Size.X, Size.Y);
            }
        }
        public void createPlant()
        {
            for (int i = 0; i < nPlant; i++)
            {
                _Plant[i].loadObjFile(startupPath + "/Asset/Environment/Vase/Vase.obj", "Pot", "Pot");
                _Plant[i].addObjChild(startupPath + "/Asset/Environment/Vase/Plant.obj", "Plant", "Pot");

                _Plant[i].load(Size.X, Size.Y);
            }

            _Board[0].loadObjFile(startupPath + "/Asset/Environment/Vase/Papan.obj", "Board", "Pot");

            _Board[0].load(Size.X, Size.Y);
        }
        public void createBench()
        {
            _Bench[0].loadObjFile(startupPath + "/Asset/Environment/Bench.obj", "Bench", "Bench");

            _Bench[0].load(Size.X, Size.Y);
        }
        public void createTrashbin()
        {
            _Trashbin[0].loadObjFile(startupPath + "/Asset/Environment/Trashbin.obj", "Trashbin", "Trashbin");

            _Trashbin[0].load(Size.X, Size.Y);
        }
        public void createPool()
        {
            _Pool[0].loadObjFile(startupPath + "/Asset/Environment/Kolam.obj", "Pool", "Pool");
            _Pool[0].addObjChild(startupPath + "/Asset/Environment/Water.obj", "Water", "Pool");

            _Pool[0].load(Size.X, Size.Y);
        }
        public void createSwing()
        {
            _Swing[0].loadObjFile(startupPath + "/Asset/Environment/Ayunan/Penahan.obj", "Handle", "Swing");
            _Swing[0].addObjChild(startupPath + "/Asset/Environment/Ayunan/Tali.obj", "Rope", "Swing");
            _Swing[0].addObjChild(startupPath + "/Asset/Environment/Ayunan/Kursi.obj", "Seat", "Swing");

            _Swing[0].load(Size.X, Size.Y);
        }
        public void createFountain()
        {
            _Fountain[0].loadObjFile(startupPath + "/Asset/Environment/Pancuran/Bawahnya.obj", "Metal", "Fountain");
            _Fountain[0].addObjChild(startupPath + "/Asset/Environment/Pancuran/Belakang.obj", "Metal", "Fountain");
            _Fountain[0].addObjChild(startupPath + "/Asset/Environment/Pancuran/Kotakair.obj", "Handle", "Fountain");
            _Fountain[0].addObjChild(startupPath + "/Asset/Environment/Pancuran/Round.obj", "Handle", "Fountain");
            _Fountain[0].addObjChild(startupPath + "/Asset/Environment/Pancuran/Air.obj", "Water", "Fountain");

            _Fountain[0].load(Size.X, Size.Y);
        }
        public void createSlide()
        {
            _Slide[0].loadObjFile(startupPath + "/Asset/Environment/Slide/Kaki.obj", "Handle", "Slide");
            _Slide[0].addObjChild(startupPath + "/Asset/Environment/Slide/Tangga.obj", "Ladder", "Slide");
            _Slide[0].addObjChild(startupPath + "/Asset/Environment/Slide/Surfpatch.obj", "Surfpatch", "Slide");

            _Slide[0].load(Size.X, Size.Y);
        }

        public void repositionItem()
        {
            repositionKirby();
            repositionWaddle();
            repositionBoten();
            repositionEnvironment();
            repositionIndividualEnvironment();
            univRepos();
        }

        public void repositionKirby()
        {
            _Kirby[0].massrepos(1.5f, 0.0f, 0.0f);
        }
        public void repositionWaddle()
        {
            _Waddle[0].massrepos(-1.5f, 0.0f, 0.0f);
        }
        public void repositionBoten()
        {
            _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[0], 20);

            _Boten[0].massrepos(0.0f, 0.0f, 0.0f);
        }
        public void repositionIndividualEnvironment()
        {
            // X = blender X
            // Y = blender Z
            // Z = blender -Y

            _Tree[0].massrepos(2.27116f / 2.0f, 0, 17.6437f / 2.0f);
            _Tree[1].massrepos(-9.18851f / 2.0f, 0, 2.03354f / 2.0f);
            _Tree[1].rotate(_Tree[1]._centerPosition, _Tree[1]._euler[1], 74);
            _Tree[2].massrepos(-15.8351f / 2.0f, 0, -21.1244f / 2.0f);
            _Tree[2].rotate(_Tree[2]._centerPosition, _Tree[2]._euler[1], 34);

            _Bush[0].massrepos(-7.77918f / 2.0f, 0.786828f / 2.0f, 0.416356f / 2.0f);
            _Bush[1].massrepos(-7.8758f / 2.0f, 0.786828f / 2.0f, 14.6275f / 2.0f);
            _Bush[2].massrepos(1.61167f / 2.0f, 0.786828f / 2.0f, 15.3686f / 2.0f);
            _Bush[3].massrepos(3.77682f / 2.0f, 0.786828f / 2.0f, 19.2823f / 2.0f);
            _Bush[4].massrepos(-13.9313f / 2.0f, 0.786828f / 2.0f, -22.2895f / 2.0f);

            _House[0].massrepos(-11.6651f / 2.0f, 0, 9.39023f / 2.0f);
            _Ground[0].massrepos(0, 0, 0);
            _Ground[0].Child[0].massrepos(-0.091802f / 2.0f, 0.111237f / 2.0f, 9.46965f / 2.0f);

            _Fence[0].massrepos(-13.0585f / 2.0f, 0, -0.3662f / 2.0f);
            _Fence[1].massrepos(3.0f, 0, -0.3662f / 2.0f);
            _Fence[1].rotate(_Fence[1]._centerPosition, _Fence[1]._euler[1], -90);
            _Fence[2].massrepos(3.0f, 0, 19.274f / 2.0f);
            _Fence[2].rotate(_Fence[2]._centerPosition, _Fence[2]._euler[1], 180);
            _Fence[3].massrepos(-13.0585f / 2.0f, 0, 19.274f / 2.0f);
            _Fence[3].rotate(_Fence[3]._centerPosition, _Fence[3]._euler[1], 90);

            _Plant[0].massrepos(-13.8431f / 2.0f, 0, 14.7552f / 2.0f);
            _Plant[1].massrepos(-13.8431f / 2.0f, 0, 16.7552f / 2.0f);
            _Plant[2].massrepos(-13.8431f / 2.0f, 0, 18.7552f / 2.0f);
            _Plant[3].massrepos(-13.8431f / 2.0f, 0, 20.7552f / 2.0f);
            _Board[0].massrepos(-13.8431f / 2.0f, 0, 17.7552f / 2.0f);

            _Bench[0].massrepos(-12.8712f / 2.0f, 0.281081f / 2.0f, -19.4115f / 2.0f);
            _Trashbin[0].massrepos(-8.89465f / 2.0f, 1.06771f / 2.0f, 20.5709f / 2.0f);
            _Pool[0].massrepos(-0.36175f, 0.13052f, -5.22f);
            _Fountain[0].massrepos(-15.0542f / 2.0f, 0.381986f / 2.0f, -9.68137f / 2.0f);
            _Swing[0].massrepos(0.797033f / 2.0f, 1.24778f / 2.0f, -20.881f / 2.0f);
            _Slide[0].massrepos(9.26131f / 2.0f, 1.26293f / 2.0f, -18.6173f / 2.0f);
        }
        public void repositionEnvironment()
        {
            float kx = 0.0f, ky = -0.6f, kz = 0.0f;

            for(int i = 0; i < nTree; i++)
            {
                _Tree[i].massrepos(kx, ky, kz);
            }
            for (int i = 0; i < nBush; i++)
            {
                _Bush[i].massrepos(kx, ky, kz);
            }

            _House[0].massrepos(kx, ky, kz);
            _Ground[0].massrepos(kx, ky, kz);

            for (int i = 0; i < nFence; i++)
            {
                _Fence[i].massrepos(kx, ky, kz);
            }

            for (int i = 0; i < nPlant; i++)
            {
                _Plant[i].massrepos(kx, ky, kz);
            }
            _Board[0].massrepos(kx, ky, kz);

            _Bench[0].massrepos(kx, ky, kz);
            _Trashbin[0].massrepos(kx, ky, kz);
            _Pool[0].massrepos(kx, ky, kz);
            _Swing[0].massrepos(kx, ky, kz);
            _Fountain[0].massrepos(kx, ky, kz);
            _Slide[0].massrepos(kx, ky, kz);
        }

        public void univRepos()
        {
            float kx = 0.0f, ky = 0.0f, kz = 0.0f;

            _Kirby[0].massrepos(kx, ky, kz);
            _Waddle[0].massrepos(kx, ky, kz);
            _Boten[0].massrepos(kx, ky, kz);
            for (int i = 0; i < nTree; i++)
            {
                _Tree[i].massrepos(kx, ky, kz);
            }
            for (int i = 0; i < nBush; i++)
            {
                _Bush[0].massrepos(kx, ky, kz);
            }
            _House[0].massrepos(kx, ky, kz);
            _Ground[0].massrepos(kx, ky, kz);
            for (int i = 0; i < nTree; i++)
            {
                _Fence[0].massrepos(kx, ky, kz);
            }
            for (int i = 0; i < nPlant; i++)
            {
                _Plant[0].massrepos(kx, ky, kz);
            }
            _Bench[0].massrepos(kx, ky, kz);
            _Trashbin[0].massrepos(kx, ky, kz);
            _Pool[0].massrepos(kx, ky, kz);
            _Swing[0].massrepos(kx, ky, kz);
            _Fountain[0].massrepos(kx, ky, kz);
            _Slide[0].massrepos(kx, ky, kz);
        }

        //ANIMATION MENU
        public void menuDisplay()
        {   //kirby
            Console.WriteLine("A -> Rotate to Left");
            Console.WriteLine("S -> Rotate to Down");
            Console.WriteLine("W -> Rotate to Up");
            Console.WriteLine("D -> Rotate to Right");
            Console.WriteLine("---------------------------");
            //Console.WriteLine("Right -> pindah ke kanan");
            //Console.WriteLine("Up -> pindah ke depan");
            //Console.WriteLine("Down -> pindah ke belakang");
            //Console.WriteLine("Left -> pindah ke kiri");
            //Console.WriteLine("---------------------------");
            Console.WriteLine("Z -> Walking Animation");
            Console.WriteLine("X -> Waving Animation");
            Console.WriteLine("C -> Jumping Animation");

            //waddle
            /*Console.WriteLine("G -> Rotate to Left");
            Console.WriteLine("H -> Rotate to Down");
            Console.WriteLine("Y -> Rotate to Up");
            Console.WriteLine("J -> Rotate to Right");
            Console.WriteLine("---------------------------");
            Console.WriteLine("I -> Jumping Animation");
            Console.WriteLine("O -> Walking Animation");*/
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            //GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //_object3d[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            //_object3d[0].resetEuler();

            renderObject();
            animateObject();

            _lamp.render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());

            //4. Jangan lupa swap 
            SwapBuffers(); //untuk memajukan ke depan background, karena awal" ke gambar di belakang background
        }

        public void renderObject()
        {
            renderKirby();
            renderBoten();
            renderWaddle();
            renderEnvironment();
        }

        public void animateObject()
        {
            animateKirby();
            animateBoten();
            animateWaddle();
        }

        public void renderKirby()
        {
            _Kirby[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
        }
        public void renderBoten()
        {
            _Boten[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
        }
        public void renderWaddle()
        {
            _Waddle[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
        }
        public void renderEnvironment()
        {
            for(int i = 0; i< nTree; i++)
            {
                _Tree[i].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            }
            for (int i = 0; i < nBush; i++)
            {
                _Bush[i].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            }
            _House[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            _Ground[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            for (int i = 0; i < nFence; i++)
            { 
                _Fence[i].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            }
            for (int i = 0; i < nPlant; i++)
            {
                _Plant[i].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            }
            _Board[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            _Bench[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            _Trashbin[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            _Pool[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            _Swing[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            _Fountain[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
            _Slide[0].render(4, _camera.GetViewMatrix(), _camera.GetProjectionMatrix(), _lightPos, _camera.Position);
        }

        public void animateKirby()
        {
            if (_amove)
            {
                animateKirbyMove();
            }
            if (_awave)
            {
                animateKirbyWave();
            }
            if (_ajump)
            {
                animateKirbyJump();
            }
        }
        public void animateWaddle()
        {
            if (_wwalk)
            {
                animateWaddleWalk();
            }
            if (_wjump)
            {
                animateWaddleJump();
            }
        }
        public void animateBoten()
        {
            if (_BotenJump)
            {
                animateBotenJump();
            }
            if (_BotenWalk)
            {
                animateBotenWalk();
            }
            if (_BotenMelirik)
            {
                animateBotenMelirik();
            }
        }

        // Kirby - Move / Walk animation
        public void animateKirbyMove()
        {
            int animatetime = 60;

            if (_co <= animatetime)
            {
                moveLeftStep();
            }
            else if (_co > animatetime && _co <= (animatetime * 3))
            {
                moveRightStep();
            }
            else if(_co > (animatetime * 3) && _co <= (animatetime * 5))
            {
                moveLeftStep();
            }
            else if (_co > (animatetime * 5) && _co <= (animatetime * 7))
            {
                moveRightStep();
            }
            else if (_co > (animatetime * 7) && _co <= (animatetime * 9))
            {
                moveLeftStep();
            }
            else if(_co > (animatetime * 9) && _co <= (animatetime * 10))
            {
                moveRightStep();
            }
            else
            {
                _amove = false;
                _animating = false;
                _co = -1;
            }
            _co++;
        }

        public void moveRightStep()
        {
            //Arms
            _Kirby[0].Child[4].rotate(_Kirby[0].Child[4]._centerPosition, _Kirby[0].Child[4]._fixedeuler[0], -1f);
            _Kirby[0].Child[5].rotate(_Kirby[0].Child[5]._centerPosition, _Kirby[0].Child[5]._fixedeuler[0], 1f);

            //Legs
            _Kirby[0].Child[6].rotate(_Kirby[0].Child[6]._centerPosition, _Kirby[0].Child[6]._fixedeuler[0], 1f);
            _Kirby[0].Child[7].rotate(_Kirby[0].Child[7]._centerPosition, _Kirby[0].Child[7]._fixedeuler[0], -1f);

            EulerReset();
        }
        public void moveLeftStep()
        {
            //Arms
            _Kirby[0].Child[4].rotate(_Kirby[0].Child[4]._centerPosition, _Kirby[0].Child[4]._fixedeuler[0], 1f);
            _Kirby[0].Child[5].rotate(_Kirby[0].Child[5]._centerPosition, _Kirby[0].Child[5]._fixedeuler[0], -1f);

            //Legs
            _Kirby[0].Child[6].rotate(_Kirby[0].Child[6]._centerPosition, _Kirby[0].Child[6]._fixedeuler[0], -1f);
            _Kirby[0].Child[7].rotate(_Kirby[0].Child[7]._centerPosition, _Kirby[0].Child[7]._fixedeuler[0], 1f);

            EulerReset();
        }

        // Kirby - Waving animation
        public void animateKirbyWave()
        {
            int animatetime = 60;

            if (_co <= animatetime)
            {
                _Kirby[0].Child[4].rotate(_Kirby[0].Child[4]._centerPosition, _Kirby[0].Child[4]._fixedeuler[2], -0.2f);
                _Kirby[0].Child[5].rotate(_Kirby[0].Child[5]._centerPosition, _Kirby[0].Child[5]._fixedeuler[2], -0.75f);
                EulerReset();
            }
            else if (_co > animatetime && _co <= (animatetime * 3))
            {
                waveDown();
            }
            else if (_co > (animatetime * 3) && _co <= (animatetime * 5))
            {
                waveUp();
            }
            else if (_co > (animatetime * 5) && _co <= (animatetime * 7))
            {
                waveDown();
            }
            else if (_co > (animatetime * 7) && _co <= (animatetime * 9))
            {
                waveUp();
            }
            else if (_co > (animatetime * 9) && _co <= (animatetime * 10))
            {
                _Kirby[0].Child[4].rotate(_Kirby[0].Child[4]._centerPosition, _Kirby[0].Child[4]._fixedeuler[2], 0.2f);
                _Kirby[0].Child[5].rotate(_Kirby[0].Child[5]._centerPosition, _Kirby[0].Child[5]._fixedeuler[2], 0.75f);
                EulerReset();
            }
            else
            {
                _awave = false;
                _animating = false;
                _co = -1;
            }
            _co++;
        }

        public void waveUp()
        {
            _Kirby[0].Child[5].rotate(_Kirby[0].Child[5]._centerPosition, _Kirby[0].Child[5]._fixedeuler[2], -0.25f);
            EulerReset();
        }
        public void waveDown()
        {
            _Kirby[0].Child[5].rotate(_Kirby[0].Child[5]._centerPosition, _Kirby[0].Child[5]._fixedeuler[2], 0.25f);
            EulerReset();
        }

        // Kirby - Jumping animation
        public void animateKirbyJump()
        {
            int animatetime = 60;

            if (_co <= animatetime)
            {
                _Kirby[0].Child[4].rotate(_Kirby[0].Child[4]._centerPosition, _Kirby[0].Child[4]._fixedeuler[2], -0.2f);
                _Kirby[0].Child[5].rotate(_Kirby[0].Child[5]._centerPosition, _Kirby[0].Child[5]._fixedeuler[2], 0.2f);
                _Kirby[0].Child[6].rotate(_Kirby[0].Child[6]._centerPosition, _Kirby[0].Child[6]._fixedeuler[0], 0.5f);
                _Kirby[0].Child[7].rotate(_Kirby[0].Child[7]._centerPosition, _Kirby[0].Child[7]._fixedeuler[0], 0.5f);
                EulerReset();
            }
            else if (_co > animatetime && _co <= (animatetime * 4))
            {
                _Kirby[0].animate("jumpup");
                _Kirby[0].Child[0].animate("jumpup");
                _Kirby[0].Child[1].animate("jumpup");
                _Kirby[0].Child[1].Child[0].animate("jumpup");
                _Kirby[0].Child[2].animate("jumpup");
                _Kirby[0].Child[2].Child[0].animate("jumpup");
                _Kirby[0].Child[2].Child[1].animate("jumpup");
                _Kirby[0].Child[3].animate("jumpup");
                _Kirby[0].Child[3].Child[0].animate("jumpup");
                _Kirby[0].Child[3].Child[1].animate("jumpup");
                _Kirby[0].Child[4].animate("jumpup");
                _Kirby[0].Child[5].animate("jumpup");
                _Kirby[0].Child[6].animate("jumpup");
                _Kirby[0].Child[7].animate("jumpup");
            }
            else if (_co > (animatetime * 4) && _co <= (animatetime * 7))
            {
                _Kirby[0].animate("jumpdown");
                _Kirby[0].Child[0].animate("jumpdown");
                _Kirby[0].Child[1].animate("jumpdown");
                _Kirby[0].Child[1].Child[0].animate("jumpdown");
                _Kirby[0].Child[2].animate("jumpdown");
                _Kirby[0].Child[2].Child[0].animate("jumpdown");
                _Kirby[0].Child[2].Child[1].animate("jumpdown");
                _Kirby[0].Child[3].animate("jumpdown");
                _Kirby[0].Child[3].Child[0].animate("jumpdown");
                _Kirby[0].Child[3].Child[1].animate("jumpdown");
                _Kirby[0].Child[4].animate("jumpdown");
                _Kirby[0].Child[5].animate("jumpdown");
                _Kirby[0].Child[6].animate("jumpdown");
                _Kirby[0].Child[7].animate("jumpdown");
            }
            else if (_co > (animatetime * 7) && _co <= (animatetime * 8))
            {
                _Kirby[0].Child[4].rotate(_Kirby[0].Child[4]._centerPosition, _Kirby[0].Child[4]._fixedeuler[2], 0.2f);
                _Kirby[0].Child[5].rotate(_Kirby[0].Child[5]._centerPosition, _Kirby[0].Child[5]._fixedeuler[2], -0.2f);
                _Kirby[0].Child[6].rotate(_Kirby[0].Child[6]._centerPosition, _Kirby[0].Child[6]._fixedeuler[0], -0.5f);
                _Kirby[0].Child[7].rotate(_Kirby[0].Child[7]._centerPosition, _Kirby[0].Child[7]._fixedeuler[0], -0.5f);
                EulerReset();
            }
            else
            {
                _ajump = false;
                _animating = false;
                _co = -1;
            }
            _co++;
        }

        // Boten
        public void animateBotenJump()
        {
            if (_co2 < 20)
            {
                _Boten[0].move(4);
            }
            else if (_co2 >= 20 && _co2 < 40)
            {
                _Boten[0].move(5);
            }
            else if (_co2 >= 40 && _co2 < 60)
            {
                _Boten[0].move(4);
            }
            else if (_co2 >= 60 && _co2 < 80)
            {
                _Boten[0].move(5);
            }
            else
            {
                _BotenJump = false;
                _co2 = -1;
            }
            _co2++;
        }
        public void animateBotenWalk()
        {
            if (_co2 < 9)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[1], 10);
            }
            else if (_co2 >= 9 && _co2 < 100)
            {
                _Boten[0].move(0);
            }
            else if (_co2 >= 100 && _co2 < 118)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[1], -10);
            }
            else if (_co2 >= 118 && _co2 < 209)
            {
                _Boten[0].move(1);
            }
            else if (_co2 >= 209 && _co2 < 218)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[1], 10);
            }
            else
            {
                _BotenWalk = false;
                _co2 = -1;
            }
            _co2++;

        }
        public void animateBotenMelirik()
        {
            if (_co2 < 9)
            {
                _Boten[0].Child[0].move(1);
                _Boten[0].Child[0].Child[0].move(1);
            }
            else if (_co2 >= 9 && _co2 < 100) { }
            else if (_co2 >= 100 && _co2 < 118)
            {
                _Boten[0].Child[0].move(0);
                _Boten[0].Child[0].Child[0].move(0);
            }
            else if (_co2 >= 118 && _co2 < 218) { }
            else if (_co2 >= 218 && _co2 < 227)
            {
                _Boten[0].Child[0].move(1);
                _Boten[0].Child[0].Child[0].move(1);
            }
            else
            {
                _BotenMelirik = false;
                _co2 = -1;
            }
            _Boten[0].resetEuler();
            _co2++;

        }

        // Waddle
        public void animateWaddleJump()
        {
            if (_co1 < 20)
            {
                _Waddle[0].move(4);
            }
            else if (_co1 >= 20 && _co1 < 40)
            {
                _Waddle[0].move(5);
            }
            else if (_co1 >= 100 && _co1 <= 100)
            {
                _Waddle[0].Child[5].move(5);
            }
            else
            {
                _wjump = false;
                _co1 = -1;
            }
            _co1++;
        }
        public void animateWaddleWalk()
        {
            if (_co1 < 9)
            {
                _Waddle[0].rotate(_Waddle[0]._centerPosition, _Waddle[0]._euler[1], 10);
            }
            else if (_co1 >= 9 && _co1 < 100)
            {
                _Waddle[0].move(0);
            }
            else if (_co1 >= 100 && _co1 < 118)
            {
                _Waddle[0].rotate(_Waddle[0]._centerPosition, _Waddle[0]._euler[1], -10);
            }
            else if (_co1 >= 118 && _co1 < 209)
            {
                _Waddle[0].move(1);
            }
            else if (_co1 >= 209 && _co1 < 218)
            {
                _Waddle[0].rotate(_Waddle[0]._centerPosition, _Waddle[0]._euler[1], 10);
            }
            else
            {
                _wwalk = false;
                _co1 = -1;
            }
            _co1++;
        }

        //Coll Check
        void CalculateAll()
        {
            _Kirby[0].CalculateAxisAlignedBox();
            _Boten[0].CalculateAxisAlignedBox();
            _Waddle[0].CalculateAxisAlignedBox();
            _Tree[0].CalculateAxisAlignedBox();
            _Bush[0].CalculateAxisAlignedBox();
            _House[0].CalculateAxisAlignedBox();
            _Ground[0].CalculateAxisAlignedBox();
            _Fence[0].CalculateAxisAlignedBox();
            _Plant[0].CalculateAxisAlignedBox();
            _Board[0].CalculateAxisAlignedBox();
            _Bench[0].CalculateAxisAlignedBox();
            _Trashbin[0].CalculateAxisAlignedBox();
            _Pool[0].CalculateAxisAlignedBox();
            _Swing[0].CalculateAxisAlignedBox();
            _Fountain[0].CalculateAxisAlignedBox();
            _Slide[0].CalculateAxisAlignedBox();
        }

        float SquaredDistPointAABB(Vector3 h, Vector3 p, Vector3 min, Vector3 max)
        {
            float sqDist = 0.0f;
            min += h;
            max += h;

            for (int i = 0; i < 3; i++)
            {
                // For each axis, count any excess distance outside box extents
                float v = p[i];
                if (v < min[i])
                {
                    sqDist += (min[i] - v) * (min[i] - v);
                }
                else if (v > max[i])
                {
                    sqDist += (v - max[i]) * (v - max[i]);
                }
            }
            //Console.WriteLine("min");
            //Console.WriteLine(min);
            //Console.WriteLine("max");
            //Console.WriteLine(max);
            return sqDist;
        }
        bool getCamCheck()
        {
            float dKirby = 1.0f;
            float dBoten = 1.0f;
            float dWaddle = 1.0f;
            float dBush = 1.0f;
            float dTree = 1.0f;
            float dHouse = 6.0f;
            float dGround = 0.5f;
            float dFence = 0.5f;
            float dPlant = 1.0f;
            float dBoard = 1.0f;
            float dBench = 1.0f;
            float dTrashbin = 1.0f;
            float dPool = 0.5f;
            float dSwing = 1.0f;
            float dFountain = 1.0f;
            float dSlide = 1.0f;

            bool writeConsole = false;
            //List of Console WriteLines - Only for debugging. Comment later
            if (writeConsole == true)
            {
                if (SquaredDistPointAABB(_Kirby[0].GetPos(), _camera.Position, _Kirby[0].GetMinAABB(), _Kirby[0].GetMaxAABB()) > dKirby)
                {
                    Console.WriteLine(SquaredDistPointAABB(_Kirby[0].GetPos(), _camera.Position, _Kirby[0].GetMinAABB(), _Kirby[0].GetMaxAABB()));
                }
                if (SquaredDistPointAABB(_Boten[0].GetPos(), _camera.Position, _Boten[0].GetMinAABB(), _Boten[0].GetMaxAABB()) > dBoten)
                {
                    Console.WriteLine(SquaredDistPointAABB(_Boten[0].GetPos(), _camera.Position, _Boten[0].GetMinAABB(), _Boten[0].GetMaxAABB()));
                }
                if (SquaredDistPointAABB(_Waddle[0].GetPos(), _camera.Position, _Waddle[0].GetMinAABB(), _Waddle[0].GetMaxAABB()) > dWaddle)
                {
                    Console.WriteLine(SquaredDistPointAABB(_Waddle[0].GetPos(), _camera.Position, _Waddle[0].GetMinAABB(), _Waddle[0].GetMaxAABB()));
                }
                for(int i = 0; i < nTree; i++)
                {
                    if (SquaredDistPointAABB(_Tree[i].GetPos(), _camera.Position, _Tree[i].GetMinAABB(), _Tree[i].GetMaxAABB()) > dTree)
                    {
                        Console.WriteLine(SquaredDistPointAABB(_Tree[i].GetPos(), _camera.Position, _Tree[i].GetMinAABB(), _Tree[i].GetMaxAABB()));
                    }
                }
                for (int i = 0; i < nBush; i++)
                {
                    if (SquaredDistPointAABB(_Bush[i].GetPos(), _camera.Position, _Bush[i].GetMinAABB(), _Bush[i].GetMaxAABB()) > dBush)
                    {
                        Console.WriteLine(SquaredDistPointAABB(_Bush[i].GetPos(), _camera.Position, _Bush[i].GetMinAABB(), _Bush[i].GetMaxAABB()));
                    }
                }
                if (SquaredDistPointAABB(_House[0].GetPos(), _camera.Position, _House[0].GetMinAABB(), _House[0].GetMaxAABB()) > dHouse)
                {
                    Console.WriteLine(SquaredDistPointAABB(_House[0].GetPos(), _camera.Position, _House[0].GetMinAABB(), _House[0].GetMaxAABB()));
                }
                if (SquaredDistPointAABB(_Ground[0].GetPos(), _camera.Position, _Ground[0].GetMinAABB(), _Ground[0].GetMaxAABB()) > dGround)
                {
                    Console.WriteLine(SquaredDistPointAABB(_Ground[0].GetPos(), _camera.Position, _Ground[0].GetMinAABB(), _Ground[0].GetMaxAABB()));
                }
                for (int i = 0; i < nFence; i++)
                {
                    if (SquaredDistPointAABB(_Fence[i].GetPos(), _camera.Position, _Fence[i].GetMinAABB(), _Fence[i].GetMaxAABB()) > dFence)
                    {
                        Console.WriteLine(SquaredDistPointAABB(_Fence[i].GetPos(), _camera.Position, _Fence[i].GetMinAABB(), _Fence[i].GetMaxAABB()));
                    }
                }
            }

            //List of Collision checks
            bool colCheck = true;
            if (true)
            {
                if (SquaredDistPointAABB(_Kirby[0].GetPos(), _camera.Position, _Kirby[0].GetMinAABB(), _Kirby[0].GetMaxAABB()) <= dKirby)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Boten[0].GetPos(), _camera.Position, _Boten[0].GetMinAABB(), _Boten[0].GetMaxAABB()) <= dBoten)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Waddle[0].GetPos(), _camera.Position, _Waddle[0].GetMinAABB(), _Waddle[0].GetMaxAABB()) <= dWaddle)
                {
                    colCheck = false;
                }
                for (int i = 0; i < nTree; i++)
                {
                    if (SquaredDistPointAABB(_Tree[i].GetPos(), _camera.Position, _Tree[i].GetMinAABB(), _Tree[i].GetMaxAABB()) <= dTree)
                    {
                        colCheck = false;
                    }
                }
                for (int i = 0; i < nBush; i++)
                {
                    if (SquaredDistPointAABB(_Bush[i].GetPos(), _camera.Position, _Bush[i].GetMinAABB(), _Bush[i].GetMaxAABB()) <= dBush)
                    {
                        colCheck = false;
                    }
                }
                if (SquaredDistPointAABB(_House[0].GetPos(), _camera.Position, _House[0].GetMinAABB(), _House[0].GetMaxAABB()) <= dHouse)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Ground[0].GetPos(), _camera.Position, _Ground[0].GetMinAABB(), _Ground[0].GetMaxAABB()) <= dGround)
                {
                    colCheck = false;
                }
                for (int i = 0; i < nFence; i++)
                {
                    if (SquaredDistPointAABB(_Fence[i].GetPos(), _camera.Position, _Fence[i].GetMinAABB(), _Fence[i].GetMaxAABB()) <= dFence)
                    {
                        colCheck = false;
                    }
                }
                for (int i = 0; i < nPlant; i++)
                {
                    if (SquaredDistPointAABB(_Plant[i].GetPos(), _camera.Position, _Plant[i].GetMinAABB(), _Plant[i].GetMaxAABB()) <= dPlant)
                    {
                        colCheck = false;
                    }
                }
                if (SquaredDistPointAABB(_Board[0].GetPos(), _camera.Position, _Board[0].GetMinAABB(), _Board[0].GetMaxAABB()) <= dBoard)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Bench[0].GetPos(), _camera.Position, _Bench[0].GetMinAABB(), _Bench[0].GetMaxAABB()) <= dBench)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Trashbin[0].GetPos(), _camera.Position, _Trashbin[0].GetMinAABB(), _Trashbin[0].GetMaxAABB()) <= dTrashbin)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Pool[0].GetPos(), _camera.Position, _Pool[0].GetMinAABB(), _Pool[0].GetMaxAABB()) <= dPool)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Swing[0].GetPos(), _camera.Position, _Swing[0].GetMinAABB(), _Swing[0].GetMaxAABB()) <= dSwing)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Fountain[0].GetPos(), _camera.Position, _Fountain[0].GetMinAABB(), _Fountain[0].GetMaxAABB()) <= dFountain)
                {
                    colCheck = false;
                }
                if (SquaredDistPointAABB(_Slide[0].GetPos(), _camera.Position, _Slide[0].GetMinAABB(), _Slide[0].GetMaxAABB()) <= dSlide)
                {
                    colCheck = false;
                }
            }

            return colCheck;
        }

        //Override functions
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            float cameraSpeed = 2.5f;

            if (input.IsKeyDown(Keys.Up))
            {
                //_camera.Position += _camera.Front * cameraSpeed * (float)args.Time;

                Vector3 tempp = new Vector3();
                tempp = _camera.Front * cameraSpeed * (float)args.Time;
                if (getCamCheck()) { _camera.Position += tempp; }
            }
            if (input.IsKeyDown(Keys.Down))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;

                //Vector3 tempp = new Vector3();
                //tempp = _camera.Front * cameraSpeed * (float)args.Time;
                //if (getCamCheck()) { _camera.Position -= tempp; }
            }
            if (input.IsKeyDown(Keys.Right))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.PageUp))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.PageDown))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
            }

            var mouse = MouseState;
            var sensitivity = 0.2f;

            if (_firstmove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstmove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }

            if (input.IsKeyDown(Keys.Period))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectPost;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objectPost;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPost);
            }

            if (input.IsKeyDown(Keys.Apostrophe))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectPost;
                _camera.Pitch -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objectPost;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPost);
            }

            if (input.IsKeyDown(Keys.Slash))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectPost;
                _camera.Pitch += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objectPost;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPost);
            }
        }

        //Untuk Resize Window
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);

            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov = _camera.Fov - e.OffsetY;
        }

        //Input animation
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
                base.OnKeyDown(e);

            //Camera uses ← ↑ → ↓ keys, so use something else
            
            if(true)
            {
                if (e.Key == Keys.A) //Left
                {
                    _Kirby[0].rotate(_Kirby[0]._centerPosition, _Kirby[0]._euler[1], -10);
                }
                if (e.Key == Keys.D) //Right
                {
                    _Kirby[0].rotate(_Kirby[0]._centerPosition, _Kirby[0]._euler[1], 10);
                }
                if (e.Key == Keys.S) //Up
                {
                    _Kirby[0].rotate(_Kirby[0]._centerPosition, _Kirby[0]._euler[0], 10);
                }
                if (e.Key == Keys.W) //Down
                {
                    _Kirby[0].rotate(_Kirby[0]._centerPosition, _Kirby[0]._euler[0], -10);
                }
                if (e.Key == Keys.Z) //Walk
                {
                    if (_animating == false)
                    {
                        _amove = true;
                        _animating = true;
                    }
                }
                if (e.Key == Keys.X) //Wave
                {
                    if (_animating == false)
                    {
                        _awave = true;
                        _animating = true;
                    }
                }
                if (e.Key == Keys.C) //Jump
                {
                    if (_animating == false)
                    {
                        _ajump = true;
                        _animating = true;
                    }
                }
            } //Kirby - A, S, D, W, Z, X, C

            if(true)
            {
                if(e.Key == Keys.V)
                {
                    _BotenJump = true;
                }
                if(e.Key == Keys.B)
                {
                    _BotenWalk = true;
                }
                if(e.Key == Keys.N)
                {
                    _BotenMelirik = true;
                }
            } //Boten
            
            if(true)
            {
                if (e.Key == Keys.F) //Left
                {
                    _Waddle[0].move(0);
                }
                if (e.Key == Keys.G) //Down
                {
                    _Waddle[0].move(1);
                }
                if (e.Key == Keys.T) //Up
                {
                    _Waddle[0].move(2);
                }
                if (e.Key == Keys.H) //Right
                {
                    _Waddle[0].move(3);
                }
                if (e.Key == Keys.M) //Walk?
                {
                    _wwalk = true;
                }
                if (e.Key == Keys.Comma)
                {
                    _wjump = true;
                }
            } //Waddle - F, T, G, H, V

            EulerReset();
        }

        public void EulerReset()
        {
            //Body
            _Kirby[0].resetEuler();
            _Waddle[0].resetEuler();
            _Boten[0].resetEuler();
        }
    }
}
