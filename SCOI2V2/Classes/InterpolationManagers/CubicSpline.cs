using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI2V2.Classes.InterpolationManagers
{
    class CubicSpline : InterpolationInterface
    {
        SplineTuple[] splines; // Сплайн
        List<Point> Points;

        void QuickSort(int low, int high)
        {
            int mid;
            int f = low, l = high;
            mid = Points[(f + l) / 2].X;
            do
            {
                while (Points[f].X < mid) f++;
                while (Points[l].X > mid) l--;
                if (f <= l) //перестановка элементов
                {
                    Points[f] = Points[l].Change(Points[f]);
                    f++;
                    l--;
                }
            } while (f < l);
            if (low < l) QuickSort(low, l);
            if (f < high) QuickSort(f, high);
        }

        public CubicSpline(List<Point> _Points)
        {
            Points = _Points;
            QuickSort(0, Points.Count - 1);
        }


        // Структура, описывающая сплайн на каждом сегменте сетки
        private struct SplineTuple
        {
            public double a, b, c, d, x;
        }

        // Построение сплайна
        // x - узлы сетки, должны быть упорядочены по возрастанию, кратные узлы запрещены
        // y - значения функции в узлах сетки
        // n - количество узлов сетки
        public void Interpolate()
        {
            // Инициализация массива сплайнов
            splines = new SplineTuple[Points.Count];
            for (int i = 0; i < Points.Count; ++i)
            {
                splines[i].x = Points[i].X;
                splines[i].a = Points[i].Y;
            }
            splines[0].c = splines[Points.Count - 1].c = 0.0;

            // Решение СЛАУ относительно коэффициентов сплайнов c[i] методом прогонки для трехдиагональных матриц
            // Вычисление прогоночных коэффициентов - прямой ход метода прогонки
            double[] alpha = new double[Points.Count - 1];
            double[] beta = new double[Points.Count - 1];
            alpha[0] = beta[0] = 0.0;
            for (int i = 1; i < Points.Count - 1; ++i)
            {
                double hi = Points[i].X - Points[i - 1].X;
                double hi1 = Points[i + 1].X - Points[i].X;
                double A = hi;
                double C = 2.0 * (hi + hi1);
                double B = hi1;
                double F = 6.0 * ((Points[i + 1].Y - Points[i].Y) / hi1 - (Points[i].Y - Points[i - 1].Y) / hi);
                double z = (A * alpha[i - 1] + C);
                alpha[i] = -B / z;
                beta[i] = (F - A * beta[i - 1]) / z;
            }

            // Нахождение решения - обратный ход метода прогонки
            for (int i = Points.Count - 2; i > 0; --i)
            {
                splines[i].c = alpha[i] * splines[i + 1].c + beta[i];
            }

            // По известным коэффициентам c[i] находим значения b[i] и d[i]
            for (int i = Points.Count - 1; i > 0; --i)
            {
                double hi = Points[i].X - Points[i - 1].X;
                splines[i].d = (splines[i].c - splines[i - 1].c) / hi;
                splines[i].b = hi * (2.0 * splines[i].c + splines[i - 1].c) / 6.0 + (Points[i].Y - Points[i - 1].Y) / hi;
            }
        }

        // Вычисление значения интерполированной функции в произвольной точке
        public int FindByX(int x, int o = 1)//возвращает Y
        {
            if (splines == null)
            {
                return -1; // Если сплайны ещё не построены - возвращаем NaN
            }

            int n = splines.Length;
            SplineTuple s;

            if (x <= splines[0].x / o) // Если x меньше точки сетки x[0] - пользуемся первым эл-тов массива
            {
                s = splines[0];
            }
            else if (x >= splines[Points.Count - 1].x / o) // Если x больше точки сетки x[n - 1] - пользуемся последним эл-том массива
            {
                s = splines[Points.Count - 1];
            }
            else // Иначе x лежит между граничными точками сетки - производим бинарный поиск нужного эл-та массива
            {
                int i = 0;
                int j = Points.Count - 1;
                while (i + 1 < j)
                {
                    int k = i + (j - i) / 2;
                    if (x <= splines[k].x / o)
                    {
                        j = k;
                    }
                    else
                    {
                        i = k;
                    }
                }
                s = splines[j];
            }

            double dx = x - s.x;
            // Вычисляем значение сплайна в заданной точке по схеме Горнера (в принципе, "умный" компилятор применил бы схему Горнера сам, но ведь не все так умны, как кажутся)
            return (int)(s.a + (s.b + (s.c / 2.0 + s.d * dx / 6.0) * dx) * dx);
        }

        public byte GetNewByte(byte x)
        {
            double result =  255-FindByX(x*2, 1)/2;
            if (result > 255)
                return 255;
            if (result < 0)
                return 0;
            return (byte)result;
        }
    }
}
