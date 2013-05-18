using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchemeEditor.Schema
{
    static class Utils
    {
        public static double GetPointsDistance(Point a, Point b)
        {
            var diff = Point.Subtract(a, b);
            return Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
        }
    }
}
