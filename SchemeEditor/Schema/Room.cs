using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SchemeEditor.Schema
{
    public class Room
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public Color Color { get; set; }
        public Point[] Points { get; set; }
    }
}
