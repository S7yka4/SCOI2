using SCOI2V2.Classes.InterpolationManagers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SCOI2V2.Classes.PicturesAndFrames
{
    unsafe class FunctionFrame:Picture
    {
        byte[] Arr;

        void SetWhite(int i)
        {
            fixed (byte* Ptr = Arr)
            {
                fixed (byte* Ptr2 = DefaultArr)
                {
                    byte* tmp = Ptr, def = Ptr2;
                    *(tmp + i) = 255;
                }
            }
        }
        public FunctionFrame():base(new Bitmap(512,512))
        {
            Arr = new byte[3*512*512];
         
        }

        public void PaintItWhite()
        {
            Parallel.For(0, Width * 3 * Height, SetWhite);
        }

        bool CoordsCheck(int x)
        {
            if ((x >= 0) && (x <= 511))
                return true;
            return false;
        }

        public void DrawPoint(Point Point)
        {

            fixed (byte* Ptr = Arr)
            {
                byte* tmp = Ptr;
                for (int m = Point.X - 5; m <= Point.X + 5; m++)
                    for (int n = Point.Y - 5; n <= Point.Y + 5; n++)
                        if (CoordsCheck(m) && CoordsCheck(n))
                        {
                            *(tmp + n * Width * 3 + m * 3) = 0;
                            *(tmp + n * Width * 3 + m * 3 + 1) = 255;
                            *(tmp + n * Width * 3 + m * 3 + 2) = 0;
                        }
            }
        }

        internal void DrawFunc(List<Point> Points, InterpolationInterface actualFunction)
        {
            LI = actualFunction;
            Parallel.For(0, Points.Count, i => {DrawPoint(Points[i]);});
            Parallel.For(0,511, DrawPartOfLine);
            
        }

        InterpolationInterface LI;
        void DrawPartOfLine(int i)
        {
            fixed (byte* Ptr = Arr)
            {
                byte* tmp = Ptr;
                int y = LI.FindByX(i);
                int y2 = LI.FindByX(i + 1);
                if (y < 0)
                    y = y2;
                if (y2 < 0)
                    y2 = y;
                if ((i >= 0) && (i + 1 <= 511) && (y >= 0) && (y <= 511) && (y2 >= 0) && (y2 <= 511))
                {
                    *(tmp + y * 3 * Width + i * 3) = 0;
                    *(tmp + y * 3 * Width + i * 3 + 1) = 255;
                    *(tmp + y * 3 * Width + i * 3 + 2) = 0;
                
                if (y2 > y)
                    for (int j = y; j < y2; j++)//столбик идет вниз
                    {
                        *(tmp + j * 3 * Width + i * 3) = 0;
                        *(tmp + j * 3 * Width + i * 3 + 1) = 255;
                        *(tmp + j * 3 * Width + i * 3 + 2) = 0;
                    }
                else
                    if (y2 < y)
                    for (int j = y; j > y2; j--)//столбик идет вверх
                    {
                        *(tmp + j * 3 * Width + i * 3) = 0;
                        *(tmp + j * 3 * Width + i * 3 + 1) = 255;
                        *(tmp + j * 3 * Width + i * 3 + 2) = 0;
                    }
                }
            }
        }



        override public Bitmap TakePicture()
        {
            Bitmap im = new Bitmap(Width, Height, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(Arr, 0));
            return im;
        }
    }
}
