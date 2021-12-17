using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System.Collections.Generic;
using LearnOpenTK.Common;
using System.Text;
using System.Timers;

namespace KirbyUTS
{
    class Window : GameWindow /*gamewindow=class windownya openTK*/
    {
        string startupPath = System.IO.Path.GetFullPath(@"..\..\..\");

        //BOTEN
        Asset3d_Boten[] _Boten = new Asset3d_Boten[1];
        bool _jump = false;
        bool _walk = false;
        bool _melirik = false;
        int _co = 0;

        //WADDLE DOO
        Asset3d_Waddle[] WaddleDoo = new Asset3d_Waddle[1];
        bool _jump_wad = false;
        bool _walk_wad = false;
        int _co1 = 0;
        Camera _camera;

        //KIRBY
        Asset3d_Kirby[] _Body = new Asset3d_Kirby[1];
        Asset3d_Kirby[] _ArmL = new Asset3d_Kirby[1];
        Asset3d_Kirby[] _ArmR = new Asset3d_Kirby[1];
        Asset3d_Kirby[] _LegL = new Asset3d_Kirby[1];
        Asset3d_Kirby[] _LegR = new Asset3d_Kirby[1];
        Asset3d_Kirby[] _Cheek = new Asset3d_Kirby[1];
        Asset3d_Kirby[] _Mouth = new Asset3d_Kirby[1];
        Asset3d_Kirby[] _Eyes = new Asset3d_Kirby[1];
        Asset3d_Kirby[] _Cap = new Asset3d_Kirby[1];

        bool _animating = false;
        bool _amove = false;
        bool _awave = false;
        bool _ajump = false;
        int _co2 = 0;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            _Boten[0] = new Asset3d_Boten();
            WaddleDoo[0] = new Asset3d_Waddle();
            //KIRBY
            _Body[0] = new Asset3d_Kirby();
            _ArmL[0] = new Asset3d_Kirby();
            _ArmR[0] = new Asset3d_Kirby();
            _LegL[0] = new Asset3d_Kirby();
            _LegR[0] = new Asset3d_Kirby();
            _Cheek[0] = new Asset3d_Kirby();
            _Mouth[0] = new Asset3d_Kirby();
            _Eyes[0] = new Asset3d_Kirby();
            _Cap[0] = new Asset3d_Kirby();
        }

        public void createBoten()
        {
            //========BODY========
            _Boten[0].createEllipsoid(0.5f, 0.5f, 0.5f, 0.03f, 0f, 0f, 72, 24, 1);
            _Boten[0].addChild_Ellipsoid(0.5f, 0.5f, 0.5f, -0.03f, 0f, 0f, 72, 24, 1);  //1

            //========EYES========
            _Boten[0].addChild_Ellips(0.05f, 0.05f, 0.05f, 0.5f, 0.1f, 0f, 72, 24, 3);
            _Boten[0].addChild_Ellips(0.05f, 0.05f, 0.05f, 0.5f, -0.1f, 0f, 72, 24, 3);
            _Boten[0].addChild_Ellipsoid(0.025f, 0.025f, 0.025f, 0.1f, 0.03f, 0.53f, 72, 24, 2);
            _Boten[0].addChild_Ellipsoid(0.025f, 0.025f, 0.025f, -0.1f, 0.03f, 0.53f, 72, 24, 2); //4

            //========THORN========
            //x = kanan kiri, y = naik turun, z = maju mundur
            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, 0f, 0.53f, 0.45f, 72, 24, 2);
            _Boten[0].Child[5].rotate(_Boten[0].Child[5]._centerPosition, _Boten[0].Child[5]._euler[0], 130);

            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, -0.55f, 0.45f, 0.1f, 72, 24, 2);
            _Boten[0].Child[6].rotate(_Boten[0].Child[6]._centerPosition, _Boten[0].Child[6]._euler[0], 100);
            _Boten[0].Child[6].rotate(_Boten[0].Child[6]._centerPosition, _Boten[0].Child[6]._euler[1], 50);

            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, 0.55f, 0.45f, 0.1f, 72, 24, 2);
            _Boten[0].Child[7].rotate(_Boten[0].Child[7]._centerPosition, _Boten[0].Child[7]._euler[0], 100);
            _Boten[0].Child[7].rotate(_Boten[0].Child[7]._centerPosition, _Boten[0].Child[7]._euler[1], -50);

            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, -0.15f, 0.68f, -0.1f, 72, 24, 2);
            _Boten[0].Child[8].rotate(_Boten[0].Child[8]._centerPosition, _Boten[0].Child[8]._euler[0], 80);
            _Boten[0].Child[8].rotate(_Boten[0].Child[8]._centerPosition, _Boten[0].Child[8]._euler[1], 10);

            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, 0.28f, 0.58f, -0.28f, 72, 24, 2);
            _Boten[0].Child[9].rotate(_Boten[0].Child[9]._centerPosition, _Boten[0].Child[9]._euler[0], 65);
            _Boten[0].Child[9].rotate(_Boten[0].Child[9]._centerPosition, _Boten[0].Child[9]._euler[1], -20);

            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, -0.4f, 0.38f, -0.45f, 72, 24, 2);
            _Boten[0].Child[10].rotate(_Boten[0].Child[10]._centerPosition, _Boten[0].Child[10]._euler[0], 42);
            _Boten[0].Child[10].rotate(_Boten[0].Child[10]._centerPosition, _Boten[0].Child[10]._euler[1], 30);

            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, 0.1f, 0.3f, -0.6f, 72, 24, 2);
            _Boten[0].Child[11].rotate(_Boten[0].Child[11]._centerPosition, _Boten[0].Child[11]._euler[0], 25);
            _Boten[0].Child[11].rotate(_Boten[0].Child[11]._centerPosition, _Boten[0].Child[11]._euler[1], -5);

            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, 0.68f, 0f, -0.1f, 72, 24, 2);
            _Boten[0].Child[12].rotate(_Boten[0].Child[12]._centerPosition, _Boten[0].Child[12]._euler[0], 100);
            _Boten[0].Child[12].rotate(_Boten[0].Child[12]._centerPosition, _Boten[0].Child[12]._euler[1], -90);

            _Boten[0].addChild_EllipticParaboloid(0.1f, 0.1f, 0.1f, -0.68f, 0f, -0.1f, 72, 24, 2);
            _Boten[0].Child[13].rotate(_Boten[0].Child[13]._centerPosition, _Boten[0].Child[13]._euler[0], 100);
            _Boten[0].Child[13].rotate(_Boten[0].Child[13]._centerPosition, _Boten[0].Child[13]._euler[1], 90);

            //========BRISTLE FRONT========
            //BRISTLE-> y = kanan kiri, z = naik turun, x = maju mundur, x->y, y->z, z->x
            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.03f, -0.1f, 0.49f, 72, 24, 4);
            _Boten[0].Child[14].rotate(_Boten[0].Child[14]._centerPosition, _Boten[0].Child[14]._euler[0], 100);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.28f, 0f, 0.44f, 72, 24, 4);
            _Boten[0].Child[15].rotate(_Boten[0].Child[15]._centerPosition, _Boten[0].Child[15]._euler[0], 90);
            _Boten[0].Child[15].rotate(_Boten[0].Child[15]._centerPosition, _Boten[0].Child[15]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.4f, 0.15f, 0.31f, 72, 24, 4);
            _Boten[0].Child[16].rotate(_Boten[0].Child[16]._centerPosition, _Boten[0].Child[16]._euler[0], 60);
            _Boten[0].Child[16].rotate(_Boten[0].Child[16]._centerPosition, _Boten[0].Child[16]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.45f, 0.01f, 0.28f, 72, 24, 4);
            _Boten[0].Child[17].rotate(_Boten[0].Child[17]._centerPosition, _Boten[0].Child[17]._euler[0], 80);
            _Boten[0].Child[17].rotate(_Boten[0].Child[17]._centerPosition, _Boten[0].Child[17]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.34f, -0.15f, 0.37f, 72, 24, 4);
            _Boten[0].Child[18].rotate(_Boten[0].Child[18]._centerPosition, _Boten[0].Child[18]._euler[0], 110);
            _Boten[0].Child[18].rotate(_Boten[0].Child[18]._centerPosition, _Boten[0].Child[18]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.2f, -0.2f, 0.43f, 72, 24, 4);
            _Boten[0].Child[19].rotate(_Boten[0].Child[19]._centerPosition, _Boten[0].Child[19]._euler[0], 110);
            _Boten[0].Child[19].rotate(_Boten[0].Child[19]._centerPosition, _Boten[0].Child[19]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.25f, -0.32f, 0.32f, 72, 24, 4);
            _Boten[0].Child[20].rotate(_Boten[0].Child[20]._centerPosition, _Boten[0].Child[20]._euler[0], 110);
            _Boten[0].Child[20].rotate(_Boten[0].Child[20]._centerPosition, _Boten[0].Child[20]._euler[1], -50);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.06f, -0.24f, 0.44f, 72, 24, 4);
            _Boten[0].Child[21].rotate(_Boten[0].Child[21]._centerPosition, _Boten[0].Child[21]._euler[0], 115);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.1f, -0.3f, 0.4f, 72, 24, 4);
            _Boten[0].Child[22].rotate(_Boten[0].Child[22]._centerPosition, _Boten[0].Child[22]._euler[0], 120);
            _Boten[0].Child[22].rotate(_Boten[0].Child[22]._centerPosition, _Boten[0].Child[22]._euler[1], 10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.2f, -0.15f, 0.45f, 72, 24, 4);
            _Boten[0].Child[23].rotate(_Boten[0].Child[23]._centerPosition, _Boten[0].Child[23]._euler[0], 110);
            _Boten[0].Child[23].rotate(_Boten[0].Child[23]._centerPosition, _Boten[0].Child[23]._euler[1], 10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.25f, -0.28f, 0.36f, 72, 24, 4);
            _Boten[0].Child[24].rotate(_Boten[0].Child[24]._centerPosition, _Boten[0].Child[24]._euler[0], 110);
            _Boten[0].Child[24].rotate(_Boten[0].Child[24]._centerPosition, _Boten[0].Child[24]._euler[1], 30);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.32f, -0.12f, 0.4f, 72, 24, 4);
            _Boten[0].Child[25].rotate(_Boten[0].Child[25]._centerPosition, _Boten[0].Child[25]._euler[0], 110);
            _Boten[0].Child[25].rotate(_Boten[0].Child[25]._centerPosition, _Boten[0].Child[25]._euler[1], 10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.4f, 0f, 0.34f, 72, 24, 4);
            _Boten[0].Child[26].rotate(_Boten[0].Child[26]._centerPosition, _Boten[0].Child[26]._euler[0], 90);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.25f, 0.05f, 0.45f, 72, 24, 4);
            _Boten[0].Child[27].rotate(_Boten[0].Child[27]._centerPosition, _Boten[0].Child[27]._euler[0], 90);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.3f, 0.15f, 0.4f, 72, 24, 4);
            _Boten[0].Child[28].rotate(_Boten[0].Child[28]._centerPosition, _Boten[0].Child[28]._euler[0], 90);
            _Boten[0].Child[28].rotate(_Boten[0].Child[28]._centerPosition, _Boten[0].Child[28]._euler[1], -20);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.15f, 0.2f, 0.45f, 72, 24, 4);
            _Boten[0].Child[29].rotate(_Boten[0].Child[29]._centerPosition, _Boten[0].Child[29]._euler[0], 70);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0f, 0.1f, 0.49f, 72, 24, 4);
            _Boten[0].Child[30].rotate(_Boten[0].Child[30]._centerPosition, _Boten[0].Child[30]._euler[0], 80);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.15f, 0.15f, 0.47f, 72, 24, 4);
            _Boten[0].Child[31].rotate(_Boten[0].Child[31]._centerPosition, _Boten[0].Child[31]._euler[0], 75);

            //========BRISTLE BACK========
            //BRISTLE-> y = kanan kiri, z = naik turun, x = maju mundur, x->y, y->z, z->x
            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.03f, -0.1f, -0.49f, 72, 24, 4);
            _Boten[0].Child[32].rotate(_Boten[0].Child[32]._centerPosition, _Boten[0].Child[32]._euler[0], 80);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.28f, 0f, -0.44f, 72, 24, 4);
            _Boten[0].Child[33].rotate(_Boten[0].Child[33]._centerPosition, _Boten[0].Child[33]._euler[0], 90);
            _Boten[0].Child[33].rotate(_Boten[0].Child[33]._centerPosition, _Boten[0].Child[33]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.4f, 0.15f, -0.31f, 72, 24, 4);
            _Boten[0].Child[34].rotate(_Boten[0].Child[34]._centerPosition, _Boten[0].Child[34]._euler[0], 120);
            _Boten[0].Child[34].rotate(_Boten[0].Child[34]._centerPosition, _Boten[0].Child[34]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.45f, 0.01f, -0.28f, 72, 24, 4);
            _Boten[0].Child[35].rotate(_Boten[0].Child[35]._centerPosition, _Boten[0].Child[35]._euler[0], 100);
            _Boten[0].Child[35].rotate(_Boten[0].Child[35]._centerPosition, _Boten[0].Child[35]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.34f, -0.15f, -0.37f, 72, 24, 4);
            _Boten[0].Child[36].rotate(_Boten[0].Child[36]._centerPosition, _Boten[0].Child[36]._euler[0], 70);
            _Boten[0].Child[36].rotate(_Boten[0].Child[36]._centerPosition, _Boten[0].Child[36]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.2f, -0.2f, -0.43f, 72, 24, 4);
            _Boten[0].Child[37].rotate(_Boten[0].Child[37]._centerPosition, _Boten[0].Child[37]._euler[0], 70);
            _Boten[0].Child[37].rotate(_Boten[0].Child[37]._centerPosition, _Boten[0].Child[37]._euler[1], -10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.25f, -0.32f, -0.32f, 72, 24, 4);
            _Boten[0].Child[38].rotate(_Boten[0].Child[38]._centerPosition, _Boten[0].Child[38]._euler[0], 70);
            _Boten[0].Child[38].rotate(_Boten[0].Child[38]._centerPosition, _Boten[0].Child[38]._euler[1], -50);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.06f, -0.24f, -0.44f, 72, 24, 4);
            _Boten[0].Child[39].rotate(_Boten[0].Child[39]._centerPosition, _Boten[0].Child[39]._euler[0], 65);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.1f, -0.3f, -0.4f, 72, 24, 4);
            _Boten[0].Child[40].rotate(_Boten[0].Child[40]._centerPosition, _Boten[0].Child[40]._euler[0], 60);
            _Boten[0].Child[40].rotate(_Boten[0].Child[40]._centerPosition, _Boten[0].Child[40]._euler[1], 10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.2f, -0.15f, -0.45f, 72, 24, 4);
            _Boten[0].Child[41].rotate(_Boten[0].Child[41]._centerPosition, _Boten[0].Child[41]._euler[0], 70);
            _Boten[0].Child[41].rotate(_Boten[0].Child[41]._centerPosition, _Boten[0].Child[41]._euler[1], 10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.25f, -0.28f, -0.36f, 72, 24, 4);
            _Boten[0].Child[42].rotate(_Boten[0].Child[42]._centerPosition, _Boten[0].Child[42]._euler[0], 70);
            _Boten[0].Child[42].rotate(_Boten[0].Child[42]._centerPosition, _Boten[0].Child[42]._euler[1], 30);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.32f, -0.12f, -0.4f, 72, 24, 4);
            _Boten[0].Child[43].rotate(_Boten[0].Child[43]._centerPosition, _Boten[0].Child[43]._euler[0], 70);
            _Boten[0].Child[43].rotate(_Boten[0].Child[43]._centerPosition, _Boten[0].Child[43]._euler[1], 10);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.4f, 0f, -0.34f, 72, 24, 4);
            _Boten[0].Child[44].rotate(_Boten[0].Child[44]._centerPosition, _Boten[0].Child[44]._euler[0], 90);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.25f, 0.05f, -0.45f, 72, 24, 4);
            _Boten[0].Child[45].rotate(_Boten[0].Child[45]._centerPosition, _Boten[0].Child[45]._euler[0], 90);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.15f, 0f, -0.45f, 72, 24, 4);
            _Boten[0].Child[46].rotate(_Boten[0].Child[46]._centerPosition, _Boten[0].Child[46]._euler[0], 110);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.1f, 0.1f, -0.49f, 72, 24, 4);
            _Boten[0].Child[47].rotate(_Boten[0].Child[47]._centerPosition, _Boten[0].Child[47]._euler[0], 100);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.15f, 0f, -0.49f, 72, 24, 4);
            _Boten[0].Child[48].rotate(_Boten[0].Child[48]._centerPosition, _Boten[0].Child[48]._euler[0], 90);

            //========BRISTLE LEFT========
            //BRISTLE-> y = kanan kiri, z = naik turun, x = maju mundur, x->y, y->z, z->x
            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.45f, -0.2f, 0.2f, 72, 24, 4);
            _Boten[0].Child[49].rotate(_Boten[0].Child[49]._centerPosition, _Boten[0].Child[49]._euler[0], 90);
            _Boten[0].Child[49].rotate(_Boten[0].Child[49]._centerPosition, _Boten[0].Child[49]._euler[1], -20);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.52f, 0f, 0.1f, 72, 24, 4);
            _Boten[0].Child[50].rotate(_Boten[0].Child[50]._centerPosition, _Boten[0].Child[50]._euler[0], 90);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.49f, -0.2f, 0f, 72, 24, 4);
            _Boten[0].Child[51].rotate(_Boten[0].Child[51]._centerPosition, _Boten[0].Child[51]._euler[0], 90);
            _Boten[0].Child[51].rotate(_Boten[0].Child[51]._centerPosition, _Boten[0].Child[51]._euler[1], -20);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.38f, -0.35f, 0.1f, 72, 24, 4);
            _Boten[0].Child[52].rotate(_Boten[0].Child[52]._centerPosition, _Boten[0].Child[52]._euler[0], 90);
            _Boten[0].Child[52].rotate(_Boten[0].Child[52]._centerPosition, _Boten[0].Child[52]._euler[1], -40);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.47f, -0.2f, -0.15f, 72, 24, 4);
            _Boten[0].Child[53].rotate(_Boten[0].Child[53]._centerPosition, _Boten[0].Child[53]._euler[0], 90);
            _Boten[0].Child[53].rotate(_Boten[0].Child[53]._centerPosition, _Boten[0].Child[53]._euler[1], -20);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.38f, -0.35f, -0.1f, 72, 24, 4);
            _Boten[0].Child[54].rotate(_Boten[0].Child[54]._centerPosition, _Boten[0].Child[54]._euler[0], 90);
            _Boten[0].Child[54].rotate(_Boten[0].Child[54]._centerPosition, _Boten[0].Child[54]._euler[1], -40);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, 0.35f, -0.3f, -0.25f, 72, 24, 4);
            _Boten[0].Child[55].rotate(_Boten[0].Child[55]._centerPosition, _Boten[0].Child[55]._euler[0], 90);
            _Boten[0].Child[55].rotate(_Boten[0].Child[55]._centerPosition, _Boten[0].Child[55]._euler[1], -40);

            //========BRISTLE RIGHT========
            //BRISTLE-> y = kanan kiri, z = naik turun, x = maju mundur, x->y, y->z, z->x
            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.45f, -0.2f, 0.2f, 72, 24, 4);
            _Boten[0].Child[56].rotate(_Boten[0].Child[56]._centerPosition, _Boten[0].Child[56]._euler[0], 90);
            _Boten[0].Child[56].rotate(_Boten[0].Child[56]._centerPosition, _Boten[0].Child[56]._euler[1], 20);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.52f, 0f, 0.1f, 72, 24, 4);
            _Boten[0].Child[57].rotate(_Boten[0].Child[57]._centerPosition, _Boten[0].Child[57]._euler[0], 90);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.49f, -0.2f, 0f, 72, 24, 4);
            _Boten[0].Child[58].rotate(_Boten[0].Child[58]._centerPosition, _Boten[0].Child[58]._euler[0], 90);
            _Boten[0].Child[58].rotate(_Boten[0].Child[58]._centerPosition, _Boten[0].Child[58]._euler[1], 20);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.38f, -0.35f, 0.1f, 72, 24, 4);
            _Boten[0].Child[59].rotate(_Boten[0].Child[59]._centerPosition, _Boten[0].Child[59]._euler[0], 90);
            _Boten[0].Child[59].rotate(_Boten[0].Child[59]._centerPosition, _Boten[0].Child[59]._euler[1], 40);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.47f, -0.2f, -0.15f, 72, 24, 4);
            _Boten[0].Child[60].rotate(_Boten[0].Child[60]._centerPosition, _Boten[0].Child[60]._euler[0], 90);
            _Boten[0].Child[60].rotate(_Boten[0].Child[60]._centerPosition, _Boten[0].Child[60]._euler[1], 20);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.38f, -0.35f, -0.1f, 72, 24, 4);
            _Boten[0].Child[61].rotate(_Boten[0].Child[61]._centerPosition, _Boten[0].Child[61]._euler[0], 90);
            _Boten[0].Child[61].rotate(_Boten[0].Child[61]._centerPosition, _Boten[0].Child[61]._euler[1], 40);

            _Boten[0].addChild_Long(0.008f, 0.008f, 0.008f, -0.35f, -0.3f, -0.25f, 72, 24, 4);
            _Boten[0].Child[62].rotate(_Boten[0].Child[62]._centerPosition, _Boten[0].Child[62]._euler[0], 90);
            _Boten[0].Child[62].rotate(_Boten[0].Child[62]._centerPosition, _Boten[0].Child[62]._euler[1], 40);

            //========RIBBON========
            _Boten[0].addChild_EllipticCone(0.025f, 0.05f, 0.1f, 0.2f, 0.25f, 0.4f, 72, 24, 5);
            _Boten[0].Child[63].rotate(_Boten[0].Child[63]._centerPosition, _Boten[0].Child[63]._euler[1], 105);
            _Boten[0].Child[63].rotate(_Boten[0].Child[63]._centerPosition, _Boten[0].Child[63]._euler[0], 10);
            _Boten[0].Child[63].rotate(_Boten[0].Child[63]._centerPosition, _Boten[0].Child[63]._euler[2], -20);

            _Boten[0].addChild_Ellipsoid(0.03f, 0.05f, 0.07f, 0.06f, 0.27f, 0.42f, 72, 24, 5);
            _Boten[0].Child[64].rotate(_Boten[0].Child[64]._centerPosition, _Boten[0].Child[64]._euler[0], 75);
            _Boten[0].Child[64].rotate(_Boten[0].Child[64]._centerPosition, _Boten[0].Child[64]._euler[1], -10);

            _Boten[0].addChild_Ellipsoid(0.03f, 0.05f, 0.07f, 0.35f, 0.225f, 0.35f, 72, 24, 5);
            _Boten[0].Child[65].rotate(_Boten[0].Child[65]._centerPosition, _Boten[0].Child[65]._euler[0], 65);
            _Boten[0].Child[65].rotate(_Boten[0].Child[65]._centerPosition, _Boten[0].Child[65]._euler[1], -10);

            _Boten[0].addChild_BoxVertices(0.2f, 0.24f, 0.4f, 0.05f, 6);
            _Boten[0].Child[66].rotate(_Boten[0].Child[66]._centerPosition, _Boten[0].Child[66]._euler[2], -10);
            _Boten[0].Child[66].rotate(_Boten[0].Child[66]._centerPosition, _Boten[0].Child[66]._euler[1], 15);

            _camera = new Camera(new Vector3(0, 0, 0), Size.X / Size.Y);
        }

        public void createWaddleDoo()
        {
            //badan
            WaddleDoo[0].createEllipsoid(0.4f, 0.4f, 0.4f, 0f, 0f, 0f, 72, 24, 1);

            //mata
            WaddleDoo[0].addChild_Ellipsoid(0.3f, 0.3f, 0.3f, 0f, 0.03f, 0.2f, 72, 24, 3); //0.3f, 0.3f, 0.2f, 0.2f, 0f, 0.02f, 72, 24, 3
            WaddleDoo[0].addChild_Ellipsoid(0.25f, 0.25f, 0.24f, 0f, 0.04f, 0.3f, 72, 24, 2);
            WaddleDoo[0].addChild_Ellipsoid(0.1f, 0.1f, 0.08f, 0.0f, 0.03f, 0.423f, 72, 24, 2); //secret receipe
            WaddleDoo[0].addChild_Ellipsoid(0.180f, 0.180f, 0.2f, -0.03f, 0.06f, 0.350f, 72, 24, 3); //bubble kiri
            WaddleDoo[0].addChild_Ellipsoid(0.070f, 0.070f, 0.2f, 0.06f, -0.05f, 0.324f, 72, 24, 3); //bubble kanan

            //tangan
            WaddleDoo[0].addChild_Ellipsoid(0.2f, 0.2f, 0.1f, -0.3f, -0.03f, 0.03f, 72, 24, 1); //tgn kiri
            WaddleDoo[0].addChild_Ellipsoid(0.2f, 0.2f, 0.1f, 0.3f, -0.03f, -0.03f, 72, 24, 1); //tgn kanan

            //kaki
            WaddleDoo[0].addChild_Ellipsoid(0.2f, 0.1f, 0.2f, -0.2f, -0.38f, 0.03f, 72, 24, 4); //kaki kiri
            WaddleDoo[0].addChild_Ellipsoid(0.2f, 0.1f, 0.2f, 0.2f, -0.38f, -0.03f, 72, 24, 4); //kaki kanan

            //topi
            WaddleDoo[0].addChild_EllipticCone1(0.10f, 0.10f, 0.2f, 0f, 0.49f, 0.3f, 5);
            WaddleDoo[0].Child[9].rotate(WaddleDoo[0].Child[9]._centerPosition, WaddleDoo[0].Child[9]._euler[0], 180);
            //WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[0], 180);
            WaddleDoo[0].addChild_Ellipsoid(0.05f, 0.05f, 0.05f, 0f, 0.65f, 0.10f, 72, 24, 1);
            //polkadot depan
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, -0f, 0.39f, 0.250f, 72, 24, 10); //makin besar = makin tenggelam
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, -0.08f, 0.49f, 0.120f, 72, 24, 10);
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 0.08f, 0.49f, 0.120f, 72, 24, 10);
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 0f, 0.54f, 0.170f, 72, 24, 10);
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 0.10f, 0.40f, 0.190f, 72, 24, 10);
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, -0.10f, 0.40f, 0.190f, 72, 24, 10);
            //motif onigiri
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 0f, 0.46f, 0.220f, 72, 24, 9);     //hijau
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, -0.02f, 0.46f, 0.216f, 72, 24, 8); //putih
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 0.02f, 0.46f, 0.216f, 72, 24, 8);  //putih
            WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 0f, 0.48f, 0.216f, 72, 24, 8);     //putih
            //polkadot belakang

            //dasi kupu
            WaddleDoo[0].addChild_EllipticCones(0.03f, 0.03f, 0.06f, 0.351f, -0.26f, 0f, 5);
            WaddleDoo[0].addChild_Vertices(-0f, -0.26f, 0.350f, 0.04f, 6);
            //polkadot kiri
            WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, -0.07f, -0.28f, 0.390f, 72, 24, 7);
            WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, -0.08f, -0.23f, 0.375f, 72, 24, 7);
            WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, -0.03f, -0.25f, 0.375f, 72, 24, 7);
            //polkadot kanan
            WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 0.07f, -0.28f, 0.390f, 72, 24, 7);
            WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 0.08f, -0.23f, 0.375f, 72, 24, 7);
            WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 0.03f, -0.25f, 0.375f, 72, 24, 7);

            ////badan
            //WaddleDoo[0].createEllipsoid(0.4f, 0.4f, 0.4f, 1.8f, 0f, 0f, 72, 24, 1);

            ////mata
            //WaddleDoo[0].addChild_Ellipsoid(0.3f, 0.3f, 0.3f, 1.8f, 0.03f, 0.2f, 72, 24, 3); //0.3f, 0.3f, 0.2f, 0.2f, 0f, 0.02f, 72, 24, 3
            //WaddleDoo[0].addChild_Ellipsoid(0.25f, 0.25f, 0.24f, 1.8f, 0.04f, 0.3f, 72, 24, 2);
            //WaddleDoo[0].addChild_Ellipsoid(0.1f, 0.1f, 0.08f, 1.8f, 0.03f, 0.423f, 72, 24, 2); //secret receipe
            //WaddleDoo[0].addChild_Ellipsoid(0.180f, 0.180f, 0.2f, 1.77f, 0.06f, 0.350f, 72, 24, 3); //bubble kiri
            //WaddleDoo[0].addChild_Ellipsoid(0.070f, 0.070f, 0.2f, 1.86f, -0.05f, 0.324f, 72, 24, 3); //bubble kanan

            ////tangan
            //WaddleDoo[0].addChild_Ellipsoid(0.2f, 0.2f, 0.1f, 1.5f, -0.03f, 0.03f, 72, 24, 1); //tgn kiri
            //WaddleDoo[0].addChild_Ellipsoid(0.2f, 0.2f, 0.1f, 2.1f, -0.03f, -0.03f, 72, 24, 1); //tgn kanan

            ////kaki
            //WaddleDoo[0].addChild_Ellipsoid(0.2f, 0.1f, 0.2f, 1.6f, -0.38f, 0.03f, 72, 24, 4); //kaki kiri
            //WaddleDoo[0].addChild_Ellipsoid(0.2f, 0.1f, 0.2f, 2f, -0.38f, -0.03f, 72, 24, 4); //kaki kanan

            ////topi
            //WaddleDoo[0].addChild_EllipticCone1(0.10f, 0.10f, 0.2f, 1.8f, 0.49f, 0.3f, 5);
            //WaddleDoo[0].Child[9].rotate(WaddleDoo[0].Child[9]._centerPosition, WaddleDoo[0].Child[9]._euler[0], 180);
            ////WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[0], 180);
            //WaddleDoo[0].addChild_Ellipsoid(0.05f, 0.05f, 0.05f, 1.8f, 0.65f, 0.10f, 72, 24, 1);
            ////polkadot depan
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.8f, 0.39f, 0.250f, 72, 24, 10); //makin besar = makin tenggelam
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.72f, 0.49f, 0.120f, 72, 24, 10);
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.88f, 0.49f, 0.120f, 72, 24, 10);
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.8f, 0.54f, 0.170f, 72, 24, 10);
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.9f, 0.40f, 0.190f, 72, 24, 10);
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.7f, 0.40f, 0.190f, 72, 24, 10);
            ////motif onigiri
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.8f, 0.46f, 0.220f, 72, 24, 9);     //hijau
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.78f, 0.46f, 0.216f, 72, 24, 8); //putih
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.82f, 0.46f, 0.216f, 72, 24, 8);  //putih
            //WaddleDoo[0].addChild_Ellipsoid(0.02f, 0.02f, 0.02f, 1.8f, 0.48f, 0.216f, 72, 24, 8);     //putih
            ////polkadot belakang

            ////dasi kupu
            //WaddleDoo[0].addChild_EllipticCones(0.03f, 0.03f, 0.06f, 0.351f, -0.26f, 1.8f, 5);
            //WaddleDoo[0].addChild_Vertices(1.8f, -0.26f, 0.350f, 0.04f, 6);
            ////polkadot kiri
            //WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 1.73f, -0.28f, 0.390f, 72, 24, 7);
            //WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 1.72f, -0.23f, 0.375f, 72, 24, 7);
            //WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 1.77f, -0.25f, 0.375f, 72, 24, 7);
            ////polkadot kanan
            //WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 1.87f, -0.28f, 0.390f, 72, 24, 7);
            //WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 1.88f, -0.23f, 0.375f, 72, 24, 7);
            //WaddleDoo[0].addChild_Ellipsoid(0.01f, 0.01f, 0.01f, 1.83f, -0.25f, 0.375f, 72, 24, 7);

            WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[1], -90);
            for (int i = 0; i < 300; i++) { WaddleDoo[0].move(1); }
            //WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[1], 20);
            WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[1], 110);

            _camera = new Camera(new Vector3(0, 0, 0), Size.X / Size.Y);
        }

        public void createKirby()
        {
            //Body
            _Body[0].loadObjFile(startupPath + "/Asset/Body.obj", "Body");
            //Cheek
            _Cheek[0].loadObjFile(startupPath + "/Asset/Cheek.obj", "Cheek");
            //Mouth
            _Mouth[0].loadObjFile(startupPath + "/Asset/Mouth.obj", "Mouth");
            _Mouth[0].addObjChild(startupPath + "/Asset/Mouth Inner.obj", "Inner Mouth");
            //Eyes
            _Eyes[0].loadObjFile(startupPath + "/Asset/Eyes.obj", "Eyes");
            _Eyes[0].addObjChild(startupPath + "/Asset/Eyes Black.obj", "Eyes Black");
            _Eyes[0].addObjChild(startupPath + "/Asset/Eyes Highlight.obj", "Eyes Highlight");
            //Cap
            _Cap[0].loadObjFile(startupPath + "/Asset/Cap.obj", "Cap");
            _Cap[0].addObjChild(startupPath + "/Asset/Cap Ball.obj", "Cap Fluff");
            _Cap[0].addObjChild(startupPath + "/Asset/Cap Fluff.obj", "Cap Fluff");
            //Arms
            _ArmL[0].loadObjFile(startupPath + "/Asset/Hand Left.obj", "Arms");
            _ArmR[0].loadObjFile(startupPath + "/Asset/Hand Right.obj", "Arms");
            //Legs
            _LegL[0].loadObjFile(startupPath + "/Asset/Leg Left.obj", "Legs");
            _LegR[0].loadObjFile(startupPath + "/Asset/Leg Right.obj", "Legs");


            _Body[0].rotate(_Body[0]._centerPosition, _Body[0]._euler[1], 90);
            _Cheek[0].rotate(_Cheek[0]._centerPosition, _Cheek[0]._euler[1], 90);
            _Mouth[0].rotate(_Mouth[0]._centerPosition, _Mouth[0]._euler[1], 90);
            _Eyes[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[1], 90);
            _Cap[0].rotate(_Cap[0]._centerPosition, _Cap[0]._euler[1], 90);
            _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._euler[1], 90);
            _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._euler[1], 90);
            _LegL[0].rotate(_LegL[0]._centerPosition, _LegL[0]._euler[1], 90);
            _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._euler[1], 90);

            for (int i = 0; i < 300; i++) {
                _Body[0].reposition();
                _Cheek[0].reposition();
                _Mouth[0].reposition();
                _Mouth[0].Child[0].reposition();
                _Eyes[0].reposition();
                _Eyes[0].Child[0].reposition();
                _Eyes[0].Child[1].reposition();
                _Cap[0].reposition();
                _Cap[0].Child[0].reposition();
                _Cap[0].Child[1].reposition();
                _ArmL[0].reposition();
                _ArmR[0].reposition();
                _LegL[0].reposition();
                _LegR[0].reposition();
            }

            _Body[0].rotate(_Body[0]._centerPosition, _Body[0]._euler[1], -110);
            _Cheek[0].rotate(_Cheek[0]._centerPosition, _Cheek[0]._euler[1], -110);
            _Mouth[0].rotate(_Mouth[0]._centerPosition, _Mouth[0]._euler[1], -110);
            _Eyes[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[1], -110);
            _Cap[0].rotate(_Cap[0]._centerPosition, _Cap[0]._euler[1], -110);
            _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._euler[1], -110);
            _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._euler[1], -110);
            _LegL[0].rotate(_LegL[0]._centerPosition, _LegL[0]._euler[1], -110);
            _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._euler[1], -110);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.65f, 0.86f, 0.94f, 1);
            GL.Enable(EnableCap.DepthTest);

            createBoten();
            createWaddleDoo();
            createKirby();

            _Boten[0].load(Size.X, Size.Y);
            WaddleDoo[0].load(Size.X, Size.Y);
            //KIRBY
            _Body[0].load(Size.X, Size.Y);
            _ArmL[0].load(Size.X, Size.Y);
            _ArmR[0].load(Size.X, Size.Y);
            _LegL[0].load(Size.X, Size.Y);
            _LegR[0].load(Size.X, Size.Y);
            _Cheek[0].load(Size.X, Size.Y);
            _Mouth[0].load(Size.X, Size.Y);
            _Eyes[0].load(Size.X, Size.Y);
            _Cap[0].load(Size.X, Size.Y);

            menuDisplay();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //agar frame tidak bertumpuk saat ganti frame

            //BOTEN
            _Boten[0].render(1);
            if (_jump) { jump(); }
            if (_walk) { walk(); }
            if (_melirik) { melirik(); }

            //WADDLE DOO
            WaddleDoo[0].render(1);
            if (_jump_wad) { jump_wad(); }
            if (_walk_wad) { walk_wad(); }

            //KIRBY
            ////Legs
            _LegL[0].render(1);
            _LegR[0].render(1);
            ////Arms
            _ArmL[0].render(1);
            _ArmR[0].render(1);
            //Body
            _Body[0].render(1);
            //Eyes
            _Eyes[0].render(1);
            //Cheek
            _Cheek[0].render(1);
            //Mouth
            _Mouth[0].render(1);
            _Cap[0].render(1);

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

            SwapBuffers();
        }

        //BOTEN ANIMATION
        public void jump()
        {
            if (_co < 20)
            {
                _Boten[0].move(4);
            }
            else if (_co >= 20 && _co < 40)
            {
                _Boten[0].move(5);
            }
            else if (_co >= 40 && _co < 150)
            {
                _Boten[0].Child[5].move(4);
            }
            else if (_co >= 150 && _co < 260)
            {
                _Boten[0].Child[5].move(5);
            }
            else
            {
                _jump = false;
                _co = 0;
            }
            _co++;
        }
        public void walk()
        {
            if (_co < 9)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[1], 10);
            }
            else if (_co >= 9 && _co < 100)
            {
                _Boten[0].move(0);
            }
            else if (_co >= 100 && _co < 118)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[1], -10);
            }
            else if (_co >= 118 && _co < 209)
            {
                _Boten[0].move(1);
            }
            else if (_co >= 209 && _co < 218)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[1], 10);
            }
            else
            {
                _walk = false;
                _co = 0;
            }
            _co++;
            
        }
        public void melirik()
        {
            if (_co < 9)
            {
                for (int i = 1; i <= 4; i++)
                {
                    _Boten[0].Child[i].move(1);
                }
            }
            else if (_co >= 9 && _co < 100) { }
            else if (_co >= 100 && _co < 118)
            {
                for (int i = 1; i <= 4; i++)
                {
                    _Boten[0].Child[i].move(0);
                }
            }
            else if (_co >= 118 && _co < 218) { }
            else if (_co >= 218 && _co < 227)
            {
                for (int i = 1; i <= 4; i++)
                {
                    _Boten[0].Child[i].move(1);
                }
            }
            else
            {
                _melirik = false;
                _co = 0;
            }
            _Boten[0].resetEuler();
            _co++;

        }

        //WADDLE ANIMATION
        public void jump_wad()
        {
            if (_co1 < 20)
            {
                WaddleDoo[0].move(4);
            }
            else if (_co1 >= 20 && _co1 < 40)
            {
                WaddleDoo[0].move(5);
            }
            else if (_co1 >= 100 && _co1 <= 100)
            {
                WaddleDoo[0].Child[5].move(5);
            }
            else
            {
                _jump_wad = false;
                _co1 = 0;
            }
            _co1++;
        }
        public void walk_wad()
        {
            if (_co1 < 9)
            {
                WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[1], 10);
            }
            else if (_co1 >= 9 && _co1 < 100)
            {
                WaddleDoo[0].move(0);
            }
            else if (_co1 >= 100 && _co1 < 118)
            {
                WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[1], -10);
            }
            else if (_co1 >= 118 && _co1 < 209)
            {
                WaddleDoo[0].move(1);
            }
            else if (_co1 >= 209 && _co1 < 218)
            {
                WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[1], 10);
            }
            else
            {
                _walk_wad = false;
                _co1 = 0;
            }
            _co1++;
        }

        //KIRBY ANIMATION
        public void menuDisplay()
        {
            //WADDLE DOO
            Console.WriteLine("==WADDLE DOO==");
            Console.WriteLine("A -> rotate ke kiri");
            Console.WriteLine("S -> rotate ke bawah");
            Console.WriteLine("W -> rotate ke atas");
            Console.WriteLine("D -> rotate ke kanan");
            Console.WriteLine("---------------------------");
            Console.WriteLine("Z -> animasi lompat");
            Console.WriteLine("X -> animasi jalan");
            Console.WriteLine("");

            //BOTEN
            Console.WriteLine("==BOTEN==");
            Console.WriteLine("E -> rotate ke kiri");
            Console.WriteLine("F -> rotate ke bawah");
            Console.WriteLine("R -> rotate ke atas");
            Console.WriteLine("T -> rotate ke kanan");
            Console.WriteLine("---------------------------");
            Console.WriteLine("C -> animasi lompat");
            Console.WriteLine("V-> animasi jalan");
            Console.WriteLine("B -> animasi melirik");
            Console.WriteLine("");

            //KIRBY
            Console.WriteLine("==KIRBY==");
            Console.WriteLine("G -> Rotate to Left");
            Console.WriteLine("H -> Rotate to Down");
            Console.WriteLine("Y -> Rotate to Up");
            Console.WriteLine("J -> Rotate to Right");
            Console.WriteLine("---------------------------");
            Console.WriteLine("I -> Walking Animation");
            Console.WriteLine("O -> Waving Animation");
            Console.WriteLine("P -> Jumping Animation");

            //Console.WriteLine("Right -> pindah ke kanan");
            //Console.WriteLine("Up -> pindah ke depan");
            //Console.WriteLine("Down -> pindah ke belakang");
            //Console.WriteLine("Left -> pindah ke kiri");
            //Console.WriteLine("---------------------------");
        }
        public void SwapAnimKirby(object sender, ElapsedEventArgs e)
        {
            if (_amove == true)
            {
                _amove = false;
                moveLeftStep();
            }
            if (_amove == false)
            {
                _amove = true;
                moveRightStep();
            }
        }
        public void animateKirbyMove()
        {
            int animatetime = 60;

            if (_co2 <= animatetime)
            {
                moveLeftStep();
            }
            else if (_co2 > animatetime && _co2 <= (animatetime * 3))
            {
                moveRightStep();
            }
            else if (_co2 > (animatetime * 3) && _co2 <= (animatetime * 5))
            {
                moveLeftStep();
            }
            else if (_co2 > (animatetime * 5) && _co2 <= (animatetime * 7))
            {
                moveRightStep();
            }
            else if (_co2 > (animatetime * 7) && _co2 <= (animatetime * 9))
            {
                moveLeftStep();
            }
            else if (_co2 > (animatetime * 9) && _co2 <= (animatetime * 10))
            {
                moveRightStep();
            }
            else
            {
                _amove = false;
                _animating = false;
                _co2 = 0;
            }
            _co2++;
        }
        public void moveRightStep()
        {
            //Arms
            _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._fixedeuler[0], -1f);
            _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._fixedeuler[0], 1f);

            //Legs
            _LegL[0].rotate(_LegL[0]._centerPosition, _LegL[0]._fixedeuler[0], 1f);
            _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._fixedeuler[0], -1f);

            EulerReset();
        }
        public void moveLeftStep()
        {
            //Arms
            _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._fixedeuler[0], 1f);
            _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._fixedeuler[0], -1f);

            //Legs
            _LegL[0].rotate(_LegL[0]._centerPosition, _LegL[0]._fixedeuler[0], -1f);
            _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._fixedeuler[0], 1f);

            EulerReset();
        }

        // Waving animation
        public void animateKirbyWave()
        {
            int animatetime = 60;

            if (_co2 <= animatetime)
            {
                _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._fixedeuler[2], -0.2f);
                _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._fixedeuler[2], -0.75f);
                EulerReset();
            }
            else if (_co2 > animatetime && _co2 <= (animatetime * 3))
            {
                waveDown();
            }
            else if (_co2 > (animatetime * 3) && _co2 <= (animatetime * 5))
            {
                waveUp();
            }
            else if (_co2 > (animatetime * 5) && _co2 <= (animatetime * 7))
            {
                waveDown();
            }
            else if (_co2 > (animatetime * 7) && _co2 <= (animatetime * 9))
            {
                waveUp();
            }
            else if (_co2 > (animatetime * 9) && _co2 <= (animatetime * 10))
            {
                _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._fixedeuler[2], 0.2f);
                _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._fixedeuler[2], 0.75f);
                EulerReset();
            }
            else
            {
                _awave = false;
                _animating = false;
                _co2 = 0;
            }
            _co2++;
        }

        public void waveUp()
        {
            _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._fixedeuler[2], -0.25f);
            EulerReset();
        }
        public void waveDown()
        {
            _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._fixedeuler[2], 0.25f);
            EulerReset();
        }

        // Jumping animation
        public void animateKirbyJump()
        {
            int animatetime = 60;

            if (_co2 <= animatetime)
            {
                _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._fixedeuler[2], -0.2f);
                _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._fixedeuler[2], 0.2f);
                _LegL[0].rotate(_LegR[0]._centerPosition, _LegR[0]._fixedeuler[0], 0.5f);
                _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._fixedeuler[0], 0.5f);
                EulerReset();
            }
            else if (_co2 > animatetime && _co2 <= (animatetime * 4))
            {
                _Body[0].animate("jumpup");
                _ArmL[0].animate("jumpup");
                _ArmR[0].animate("jumpup");
                _LegL[0].animate("jumpup");
                _LegR[0].animate("jumpup");
                _Cheek[0].animate("jumpup");
                _Mouth[0].animate("jumpup");
                _Mouth[0].Child[0].animate("jumpup");
                _Eyes[0].animate("jumpup");
                _Eyes[0].Child[0].animate("jumpup");
                _Eyes[0].Child[1].animate("jumpup");
                _Cap[0].animate("jumpup");
                _Cap[0].Child[0].animate("jumpup");
                _Cap[0].Child[1].animate("jumpup");
            }
            else if (_co2 > (animatetime * 4) && _co2 <= (animatetime * 7))
            {
                _Body[0].animate("jumpdown");
                _ArmL[0].animate("jumpdown");
                _ArmR[0].animate("jumpdown");
                _LegL[0].animate("jumpdown");
                _LegR[0].animate("jumpdown");
                _Cheek[0].animate("jumpdown");
                _Mouth[0].animate("jumpdown");
                _Mouth[0].Child[0].animate("jumpdown");
                _Eyes[0].animate("jumpdown");
                _Eyes[0].Child[0].animate("jumpdown");
                _Eyes[0].Child[1].animate("jumpdown");
                _Cap[0].animate("jumpdown");
                _Cap[0].Child[0].animate("jumpdown");
                _Cap[0].Child[1].animate("jumpdown");
            }
            else if (_co2 > (animatetime * 7) && _co2 <= (animatetime * 8))
            {
                _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._fixedeuler[2], 0.2f);
                _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._fixedeuler[2], -0.2f);
                _LegL[0].rotate(_LegR[0]._centerPosition, _LegR[0]._fixedeuler[0], -0.5f);
                _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._fixedeuler[0], -0.5f);
                EulerReset();
            }
            else
            {
                _ajump = false;
                _animating = false;
                _co2 = 0;
            }
            _co2++;
        }
        public void EulerReset()
        {
            //Body
            _Body[0].resetEuler();

            //Arms
            _ArmL[0].resetEuler();
            _ArmR[0].resetEuler();

            //Legs
            _LegL[0].resetEuler();
            _LegR[0].resetEuler();

            //Cheek
            _Cheek[0].resetEuler();

            //Mouth
            _Mouth[0].resetEuler();
            //_MouthInner[0].resetEuler();

            //Eyes
            _Eyes[0].resetEuler();
            //_EyesBl[0].resetEuler();
            //_EyesHl[0].resetEuler();

            _Cap[0].resetEuler();
        }

        //KEYBOARD FUNCTION
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            var input = KeyboardState;

            //exit with escape keyboard
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            };
        }

        //Untuk Resize Window
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        //ROTASI
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            //ROTATE
            //WADDLE DOO
            if (e.Key == Keys.A)
            {
                WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[1], -10);
            }
            if (e.Key == Keys.D)
            {
                WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[1], 10);
            }
            if (e.Key == Keys.S)
            {
                WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[0], 10);
            }
            if (e.Key == Keys.W)
            {
                WaddleDoo[0].rotate(WaddleDoo[0]._centerPosition, WaddleDoo[0]._euler[0], -10);
            }
            //BOTEN
            if (e.Key == Keys.E)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[1], -10);
            }
            if (e.Key == Keys.T)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[1], 10);
            }
            if (e.Key == Keys.F)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[0], 10);
            }
            if (e.Key == Keys.R)
            {
                _Boten[0].rotate(_Boten[0]._centerPosition, _Boten[0]._euler[0], -10);
            }
            //KIRBY
            if (e.Key == Keys.G)
            {
                //Body
                _Body[0].rotate(_Body[0]._centerPosition, _Body[0]._euler[1], -10);

                //Arms
                _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._euler[1], -10);
                _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._euler[1], -10);

                //Legs
                _LegL[0].rotate(_LegL[0]._centerPosition, _LegL[0]._euler[1], -10);
                _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._euler[1], -10);

                //Cheek
                _Cheek[0].rotate(_Cheek[0]._centerPosition, _Cheek[0]._euler[1], -10);

                //Mouth
                _Mouth[0].rotate(_Mouth[0]._centerPosition, _Mouth[0]._euler[1], -10);
                //_MouthInner[0].rotate(_MouthInner[0]._centerPosition, _MouthInner[0]._euler[1], -10);

                //Eyes
                _Eyes[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[1], -10);
                //_EyesBl[0].rotate(_EyesBl[0]._centerPosition, _EyesBl[0]._euler[1], -10);
                //_EyesHl[0].rotate(_EyesHl[0]._centerPosition, _EyesHl[0]._euler[1], -10);

                _Cap[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[1], -10);
            }
            if (e.Key == Keys.J)
            {
                //Body
                _Body[0].rotate(_Body[0]._centerPosition, _Body[0]._euler[1], 10);

                //Arms
                _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._euler[1], 10);
                _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._euler[1], 10);

                //Legs
                _LegL[0].rotate(_LegL[0]._centerPosition, _LegL[0]._euler[1], 10);
                _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._euler[1], 10);

                //Cheek
                _Cheek[0].rotate(_Cheek[0]._centerPosition, _Cheek[0]._euler[1], 10);

                //Mouth
                _Mouth[0].rotate(_Mouth[0]._centerPosition, _Mouth[0]._euler[1], 10);
                //_MouthInner[0].rotate(_MouthInner[0]._centerPosition, _MouthInner[0]._euler[1], 10);

                //Eyes
                _Eyes[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[1], 10);
                //_EyesBl[0].rotate(_EyesBl[0]._centerPosition, _EyesBl[0]._euler[1], 10);
                //_EyesHl[0].rotate(_EyesHl[0]._centerPosition, _EyesHl[0]._euler[1], 10);

                _Cap[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[1], 10);
            }
            if (e.Key == Keys.H)
            {
                //Body
                _Body[0].rotate(_Body[0]._centerPosition, _Body[0]._euler[0], 10);

                //Arms
                _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._euler[0], 10);
                _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._euler[0], 10);

                //Legs
                _LegL[0].rotate(_LegL[0]._centerPosition, _LegL[0]._euler[0], 10);
                _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._euler[0], 10);

                //Cheek
                _Cheek[0].rotate(_Cheek[0]._centerPosition, _Cheek[0]._euler[0], 10);

                //Mouth
                _Mouth[0].rotate(_Mouth[0]._centerPosition, _Mouth[0]._euler[0], 10);
                //_MouthInner[0].rotate(_MouthInner[0]._centerPosition, _MouthInner[0]._euler[0], 10);

                //Eyes
                _Eyes[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[0], 10);
                //_EyesBl[0].rotate(_EyesBl[0]._centerPosition, _EyesBl[0]._euler[0], 10);
                //_EyesHl[0].rotate(_EyesHl[0]._centerPosition, _EyesHl[0]._euler[0], 10);

                _Cap[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[0], 10);
            }
            if (e.Key == Keys.Y)
            {
                //Body
                _Body[0].rotate(_Body[0]._centerPosition, _Body[0]._euler[0], -10);

                //Arms
                _ArmL[0].rotate(_ArmL[0]._centerPosition, _ArmL[0]._euler[0], -10);
                _ArmR[0].rotate(_ArmR[0]._centerPosition, _ArmR[0]._euler[0], -10);

                //Legs
                _LegL[0].rotate(_LegL[0]._centerPosition, _LegL[0]._euler[0], -10);
                _LegR[0].rotate(_LegR[0]._centerPosition, _LegR[0]._euler[0], -10);

                //Cheek
                _Cheek[0].rotate(_Cheek[0]._centerPosition, _Cheek[0]._euler[0], -10);

                //Mouth
                _Mouth[0].rotate(_Mouth[0]._centerPosition, _Mouth[0]._euler[0], -10);
                //_MouthInner[0].rotate(_MouthInner[0]._centerPosition, _MouthInner[0]._euler[0], -10);

                //Eyes
                _Eyes[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[0], -10);
                //_EyesBl[0].rotate(_EyesBl[0]._centerPosition, _EyesBl[0]._euler[0], -10);
                //_EyesHl[0].rotate(_EyesHl[0]._centerPosition, _EyesHl[0]._euler[0], -10);

                _Cap[0].rotate(_Eyes[0]._centerPosition, _Eyes[0]._euler[0], -10);
            }
            //MOVE
            if (e.Key == Keys.Right)
            {
                _Boten[0].move(0);
                WaddleDoo[0].move(0);
            }
            if (e.Key == Keys.Left)
            {
                _Boten[0].move(1);
                WaddleDoo[0].move(1);
            }
            if (e.Key == Keys.Up)
            {
                _Boten[0].move(2);
                WaddleDoo[0].move(2);
            }
            if (e.Key == Keys.Down)
            {
                _Boten[0].move(3);
                WaddleDoo[0].move(3);
            }

            //ANIMATION
            if (e.Key == Keys.C)
            {
                _jump = true;
            }
            if (e.Key == Keys.V)
            {
                _walk = true;
            }
            if (e.Key == Keys.B)
            {
                _melirik = true;
            }
            if (e.Key == Keys.Z)
            {
                _jump_wad = true;
            }
            if (e.Key == Keys.X)
            {
                _walk_wad = true;
            }
            if (e.Key == Keys.I)
            {
                if (_animating == false)
                {
                    _amove = true;
                    _animating = true;
                }
            }
            if (e.Key == Keys.O)
            {
                if (_animating == false)
                {
                    _awave = true;
                    _animating = true;
                }
            }
            if (e.Key == Keys.P)
            {
                if (_animating == false)
                {
                    _ajump = true;
                    _animating = true;
                }
            }
            _Boten[0].resetEuler();
            WaddleDoo[0].resetEuler();
            EulerReset();
        }

    }
}
