using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatCircle.MyMathUtils;

public class MyMath
{
    public static bool IsCloseTo(
        double value,
        double target,
        double relativeTolerance = MyMathHelper.relativeTolerance,
        double absoluteTolerance = MyMathHelper.absoluteTolerance)
    {
        /* Determine whether a given value is close to the target.
         * 
         * Based on numpy.isclose, comparing the absolute error to a combined tolerance.
         */
        return Math.Abs(value - target) <= absoluteTolerance + relativeTolerance * Math.Abs(target);
    }

    public static bool AreClose(double value1, double value2, double relativeTolerance = MyMathHelper.relativeTolerance, double absoluteTolerance = MyMathHelper.absoluteTolerance)
    {
        /* Determine whether two values are close to each other.
         * 
         * Based on numpy.isclose but treats value1 and value2 symmetrically.
         */
        double scale = 0.5 * (Math.Abs(value1) + Math.Abs(value2));
        return Math.Abs(value1 - value2) <= absoluteTolerance + relativeTolerance * scale;
    }

}
