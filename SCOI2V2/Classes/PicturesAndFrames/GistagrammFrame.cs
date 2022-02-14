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
        int[] Gistagramm;

        void SetWhite(int i)
        {
            fixed (byte* Ptr = DefaultArr)
            {

                byte* tmp = Ptr;
                *(tmp + i) = 255;
            }
        }
        public void PaintItWhite()
        {
            Parallel.For(0, Width * 3 * Height, SetWhite);
        }
        public GistagrammFrame():base(new Bitmap(1024,170))
        {
            PaintItWhite();
        }

        public void DrawTable(int num)
        {
            fixed (byte* Ptr = DefaultArr)
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
    }
}
