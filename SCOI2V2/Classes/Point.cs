using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI2V2.Classes
{
    public class Point
    {
        int y;
        int x;

        public Point()
        { }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                if ((value >= 1) && (value <= 510))
                    x = value;
                else
                    if (value < 1)
                    x = 1;
                else
                    if (value > 510)
                    x = 510;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                if ((value >= 1) && (value <= 510))
                    y = value;
                else
                    if (value < 1)
                    y = 1;
                else
                    if (value > 510)
                    y = 510;
            }
        }
        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public Point Change(Point _Point)
        {

            Point tmp = new Point();
            tmp.x = X;
            tmp.y = Y;

            x = _Point.X;
            y = _Point.Y;

            _Point.x = tmp.X;
            _Point.y = tmp.Y;
            

            return _Point;
        }

        public void Normolize()
        {
            x = 511 - x;
        }

        public bool InPointCoords(Point tmp)
        {
            if ((tmp.X >= x - 5) && (tmp.X <= x + 5) && (tmp.Y <= y + 5) && (tmp.Y >= y - 5))
                return true;
            else
                return false;

        }
    }
}
