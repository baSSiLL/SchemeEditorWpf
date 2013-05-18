using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchemeEditor.Schema
{
    public class Wall : object
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public override bool Equals(object obj)
        {
            Wall w = obj as Wall;
            if (w == null)
                return false;
            return w.Start == Start && w.End == End;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }
    }
}
