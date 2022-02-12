using SCOI2V2.Classes.InterpolationManagers;
using SCOI2V2.Classes.PicturesAndFrames;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI2V2.Classes
{
    internal class PrevueManager
    {
        PrevueFrame Picture;

        public PrevueManager(string Way)
        {
            Bitmap tmp = new Bitmap(Way);
            Picture = new PrevueFrame(new Bitmap(tmp, 512, 512));
            //tmp.Dispose();
        }

        public Bitmap SetChanges(InterpolationInterface LI)
        {
            Picture.SetChanges(LI);
            return Picture.TakePicture();
        }
        public int[] TakeGist()
        {
            return Picture.Gistagramm;
        }
    }
}
