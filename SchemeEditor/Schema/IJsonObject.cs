using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeEditor.Schema
{
    interface IJsonObject
    {
        Dictionary<string, object> ToJsonObject();
        void FillWithJsonObject(JContainer dict, Scheme scheme);
    }
}
