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
    unsafe class Picture
    {
        protected int Width;
        protected int Height;
        protected byte[] DefaultArr;
        protected int stride;
 
        public unsafe Picture(Bitmap InpImg)
        {
            Width = InpImg.Width;
            Height = InpImg.Height;
            BitmapData bd = null;
            try
            {
                bd = InpImg.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);   
                DefaultArr = new byte[3 * Height * Width];
                stride = bd.Stride;
                byte* CurrentPosition;
                fixed (byte* Ptr2 = DefaultArr)
                {
                    byte*  tmp2 = Ptr2;
                    for (int i = 0; i < Height; i++)
                    {
                        CurrentPosition = ((byte*)bd.Scan0) + i * bd.Stride;
                        for (int j = 0; j < Width * 3; j++)
                        {
                            *tmp2 = *(CurrentPosition);
                            tmp2++;
                            CurrentPosition++;
                        }
                    }
                }
            }
            finally
            {
                InpImg.UnlockBits(bd);
            }
        }


    

        virtual public Bitmap TakePicture()
        {

            Bitmap im = new Bitmap(Width, Height, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(DefaultArr, 0));
            return im;
        }

    }
}
