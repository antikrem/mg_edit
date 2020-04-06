using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit
{
    static class MathHelper
    {
        // Applies a generic clamp
        static public T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            else if (value.CompareTo(max) > 0)
                return max;
            else
                return value;
        }

        // Calculates the cartesian fr
        static public (double, double) ToCartesian(double magnitude, double angle)
        {
            return (magnitude * Math.Cos((Math.PI / 180) * angle), magnitude * Math.Sin((Math.PI / 180) * angle));
        }
    }
}
