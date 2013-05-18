using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SchemeEditor.Schema
{
    public static class JsonHandler
    {

        public static void WriteJson(Scheme scheme, string jsonPath)
        {
            Dictionary<string, object> dict = scheme.ToJsonObject();
            string jsonStr = JsonConvert.SerializeObject(dict);

            Console.WriteLine("json path: {0}", jsonPath);
            using (StreamWriter w = new StreamWriter(jsonPath))
                w.Write(jsonStr);
        }

        public static Scheme ReadJson(string jsonPath)
        {
            string jsonStr;
            using (StreamReader s = new StreamReader(jsonPath))
                jsonStr = s.ReadToEnd();

            JContainer json = (JContainer)JsonConvert.DeserializeObject(jsonStr);
            var scheme = new Scheme();
            scheme.FillWithJsonObject(json, scheme);

            return scheme;
        }
    }
}
