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
    class DrawingManager
    {
       public  List<Point> Points;
        FunctionFrame Frame;
        public InterpolationInterface ActualFunction;
        public DrawingManager()
        {
            Points = new List<Point>();
            Points.Add(new Point(0, 0));
            Points.Add(new Point(511, 511));
            Frame = new FunctionFrame();
        }

        void NormolizeCoords()
        {
            NormolizedPoints = new List<Point>();
            Parallel.For(0, Points.Count, i =>
              {
                  NormolizedPoints.Add(new Point(511-Points[i].X, Points[i].Y));
              });
                
        }


        List<Point> NormolizedPoints;

        public Bitmap DrawNewFrame(int index)
        {
            NormolizeCoords();
            InterpolationInterface Int=null;
            
            switch (index)
            {
                case 0: {Int = new LinearInterpolationManager(NormolizedPoints); } break;
                case 1: {Int = new CubicSpline(NormolizedPoints); } break;
            }
            Int.Interpolate();
            ActualFunction =Int;
            Frame.PaintItWhite();
            Frame.DrawFunc(NormolizedPoints, Int);
            return Frame.TakePicture();
        }

        public void AddPoint(Point p)
        {
            Points.Add(p);
        }

        public void RemovePoint(int i)
        {
            if((i!=0)&&(i!=1))
            Points.Remove(Points[i]);
        }

    }
}
