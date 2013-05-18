using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeEditor.Schema
{
    public class Scheme : IJsonObject
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

        public void FillWithJsonObject(JContainer dict, Scheme scheme)
        {
            Rooms = dict["rooms"].Select(r =>
            {
                Room room = new Room();
                room.FillWithJsonObject((JContainer)r, scheme);
                return room;
            }).ToArray();

            Items = dict["items"].Select(i =>
            {
                Item item;
                string locType = (string)i["location_type"];
                if (locType.Equals("room"))
                    item = new RoomItem();
                else
                    item = new CoordinateItem();
                item.FillWithJsonObject((JContainer)i, scheme);
                return item;
            }).ToArray();
        }

        public List<Wall> GetWalls()
        {
            var res = new List<Wall>();
            foreach (var r in Rooms)
                res.AddRange(r.GetWalls());

            return res.Distinct().ToList();
        }
    }
}
