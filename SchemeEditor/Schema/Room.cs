using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;

namespace SchemeEditor.Schema
{
    public class Room : IJsonObject
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public Color Color { get; set; }
        public Point[] Points { get; set; }

        public string GetHexColor()
        {
            return "#" + Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2");
        }

        public Dictionary<string, object> ToJsonObject()
        {
            return new Dictionary<string, object>
            {
                { "title", Title },
                { "id", Id },
                { "color", GetHexColor() },
                { "points", Points.Select(p => new[] { p.X, p.Y }) }
            };
        }
    }
}
