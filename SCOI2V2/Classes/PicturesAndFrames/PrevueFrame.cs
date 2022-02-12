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
    unsafe class PrevueFrame:Picture
    {
        byte[] Arr;
        public int[] Gistagramm;
        public PrevueFrame(Bitmap Source):base(Source)
        {
            Arr = new byte[512 * 3 * 512];
            Gistagramm = new int[256];
        }

        public void SetChanges(InterpolationInterface LineInt)
        {
            Parallel.For(0, 256, i => { Gistagramm[i] = 0; });
            fixed (byte* PtrWrk = Arr)
            {
                fixed (byte* DefWrk = DefaultArr)
                {
                    byte* wrk = PtrWrk, def = DefWrk;
                    Parallel.For(0,Width*3*Height,i=>
                    {
                        *(wrk + i) = LineInt.GetNewByte(*(def + i));
                        Gistagramm[*(wrk + i)]++;
                    });
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
