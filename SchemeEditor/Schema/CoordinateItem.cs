using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchemeEditor.Schema
{
    public class CoordinateItem : Item
    {
        public Point Location { get; set; }

        public override Dictionary<string, object> ToJsonObject()
        {
            var res = base.ToJsonObject();
            res["location"] = new[] { Location.X, Location.Y };
            return res;
        }

        public override void FillWithJsonObject(JContainer dict, Scheme scheme)
        {
            base.FillWithJsonObject(dict, scheme);
            var l = (JContainer)dict["location"];
            Location = new Point((double)l[0], (double)l[1]);
        }
    }
}
