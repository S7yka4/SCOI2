using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI2V2.Classes.InterpolationManagers
{
    public interface InterpolationInterface
    {
        byte GetNewByte(byte x);
        void Interpolate();
        int FindByX(int x,int o=1);

    }
}
