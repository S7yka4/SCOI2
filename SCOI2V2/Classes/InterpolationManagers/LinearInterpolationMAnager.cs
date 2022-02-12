using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI2V2.Classes.InterpolationManagers
{
    public class PartOfFunc
    {
        public double k;
        public double b;

        public PartOfFunc(Point p2, Point p1)
        {
            k = ((p2.Y) - (p1.Y)) / (double)(p2.X - p1.X);
            if (Double.IsInfinity(k) || (Double.IsNaN(k)))
                k = 0;
            b = (p1.Y) - (p1.X) * k;
        }


        public byte GetByte(byte x)
        {
            double result = ((k * x * 2 + b));
            result = 255 - (result) / 2;
            if (result > 255)
                return 255;
            else
            if (result < 0)
                return 0;
            else
                return (byte)result;
        }


        public double GetY(int x)
        {
            return (k * (x) + b);
        }


    }
    public class LinearInterpolationManager : InterpolationInterface
    {
        List<Point> Points;
        public PartOfFunc[] Function;



        void QuickSort(int low, int high)
        {
            int mid;
            int f = low, l = high;
            mid = Points[(f + l) / 2].X;
            do
            {
                while (Points[f].X < mid) f++;
                while (Points[l].X > mid) l--;
                if (f <= l)
                {
                    Points[f] = Points[l].Change(Points[f]);
                    f++;
                    l--;
                }
            } while (f < l);
            if (low < l) QuickSort(low, l);
            if (f < high) QuickSort(f, high);
        }





        public LinearInterpolationManager(List<Point> _Points)
        {
            Points = _Points;
            Function = new PartOfFunc[Points.Count - 1];
            QuickSort(0, Points.Count - 1);

        }


        public void Interpolate()
        {
            Parallel.For(0, Points.Count - 1, i => { Function[i] = new PartOfFunc(Points[i], Points[i + 1]); });
        }


        int Search_Binary(int key, int left, int right,int flag=1)
        {
            int midd;
            while (true)
            {
                midd = (left + right) / 2;
                if ((left > right) || (midd == 0) || (midd == Points.Count - 1))
                    return 0;
                else
                if ((key > Points[midd].X / flag) && (key < Points[midd + 1].X / flag))
                    return midd;
                else
                if ((key < Points[midd].X / flag) && (key > Points[midd - 1].X / flag))
                    return midd-1;
                if (key < Points[midd].X/flag)
                    right = midd - 1;
                else if (key > Points[midd].X/flag)
                    left = midd + 1;
                else
                    return midd;
            }
        }


        public int FindByX(int x, int o = 1)
        {
            if (Function.Length != 0)
            {
                if ((x == Points[Points.Count - 1].X))
                    return (int)Function[Function.Length - 1].GetY(x);
                return (int)Function[Search_Binary(x, 0, Points.Count - 1)].GetY(x);
            }
            else return 0;
        }



        public byte GetNewByte(byte x)
        {
            if (Function.Length != 0)
            {
                if ((x == Points[Points.Count - 1].X / 2) && (Function.Length != 0))
                    return Function[Function.Length - 1].GetByte(x);
                return Function[Search_Binary(x, 0, Points.Count - 1, 2)].GetByte(x);
            }
            return 0;
        }
    }
}
