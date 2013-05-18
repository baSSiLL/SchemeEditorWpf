using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchemeEditor.Schema
{
    class CoordinateItem : Item
    {
        public Point Location { get; set; }

        public override Dictionary<string, object> ToJsonObject()
        {
            var res = base.ToJsonObject();
            res["location"] = new[] { Location.X, Location.Y };
            return res;
        }
    }
}
