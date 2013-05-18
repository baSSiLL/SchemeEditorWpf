using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeEditor.Schema
{
    class Scheme : IJsonObject
    {
        public Room[] Rooms { get; set; }
        public Item[] Items { get; set; }

        public Dictionary<string, object> ToJsonObject()
        {
            return new Dictionary<string, object>
            {
                { "rooms", Rooms.Select(r => r.ToJsonObject()) },
                { "items", Items.Select(i => i.ToJsonObject()) }
            };
        }
    }
}
