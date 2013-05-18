﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeEditor.Schema
{
    abstract class Item : IJsonObject
    {
        public string Title { get; set; }
        public Room Room { get; set; }
        public string QRCode { get; set; }
        public string Image { get; set; }
        public bool Visible { get; set; }

        public virtual Dictionary<string, object> ToJsonObject()
        {
            return new Dictionary<string, object>
            {
                { "title", Title },
                { "room", Room.Id },
                { "qr_code", QRCode },
                { "image", Image },
                { "visible", Visible }
            };
        }
    }
}
