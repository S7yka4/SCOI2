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
    unsafe class GistagrammFrame: Picture
    {
        byte[] Arr;
        public int[] Gistagramm;

        void SetWhite(int i)
        {
            fixed (byte* Ptr = Arr)
            {
                fixed (byte* Ptr2 = DefaultArr)
                {
                    byte* tmp = Ptr,def=Ptr2;
                    *(tmp + i) = 255;
                }
            }
        }
        public void PaintItWhite()
        {
            Parallel.For(0, Width * 3 * Height, SetWhite);
        }
        public GistagrammFrame():base(new Bitmap(1024,170))
        {
            Arr = new byte[1024 * 3 * 170];
            Parallel.For(0, Width * 3 * Height, SetWhite);
        }

        public void DrawTable(int num)
        {
            fixed (byte* Ptr = Arr)
            {
                int height = (int)(170 * 0.9) * Gistagramm[num] / Gistagramm.Max();
                byte* wrk = Ptr;
                for (int i = 169; i >= (169 - height); i--)
                    for (int j = 0; j < 4; j++)
                    {
                        *(wrk + i * Width * 3 + j + num * 3 * 4) = 0;
                        *(wrk + i * Width * 3 + j + num * 3 * 4 + 1) = 0;
                        *(wrk + i * Width * 3 + j + num * 3 * 4 + 2) = 0;
                    }
            }
        }

        public void DrawGist(int[] _Gistagramm)
        {
            Gistagramm = _Gistagramm;
            Parallel.For(0, 256, DrawTable);
        }

        override public Bitmap TakePicture()
        {
            Bitmap im = new Bitmap(Width, Height, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(Arr, 0));
            return im;
        }
    }
}
