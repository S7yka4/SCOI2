using SCOI2V2.Classes.PicturesAndFrames;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI2V2.Classes
{
    internal class GistagrammManager
    {
        GistagrammFrame Frame;

        public GistagrammManager()
        {
            Frame = new GistagrammFrame();
        }

        public Bitmap DrawGistogramm(int[] Gist)
        {
            Frame.PaintItWhite();
            Frame.DrawGist(Gist);
            return Frame.TakePicture();
        }
    }
}
