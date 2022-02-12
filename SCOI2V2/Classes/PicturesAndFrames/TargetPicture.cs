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
    unsafe class TargetPicture : Picture
    {
        byte[] Arr;
        public TargetPicture(string WayToPic):base(new Bitmap(WayToPic))
        {
            Arr = new byte[Width * 3 * Height];
        }

        InterpolationInterface LI;
        void SetNewMean(int i)
        {

            fixed (byte* PtrWrk = Arr)
            {
                fixed (byte* DefWrk = DefaultArr)
                {
                    byte* wrk = PtrWrk, def = DefWrk;
                    *(wrk + i) = LI.GetNewByte(*(def + i));
               
                }
            }
        }
      
        public void SetChanges(InterpolationInterface LineInt)
        {
            LI = LineInt;
            Parallel.For(0, Width * 3 * Height, SetNewMean);
        }

        override public Bitmap TakePicture()
        {
            Bitmap im = new Bitmap(Width, Height, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(Arr, 0));
            return im;
        }
    }
}
