using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SchemeEditor.Schema
{
    static class JsonHandler
    {
        static void DoActionInBackground(Action action, Action onFinish)
        {
            BackgroundWorker w = new BackgroundWorker();
            w.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                onFinish();
            };

            w.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                action();
            };

            w.RunWorkerAsync();
        }

        static void WriteJson(Scheme scheme, Action onFinish)
        {
            DoActionInBackground(() =>
            {
                Dictionary<string, object> dict = scheme.ToJsonObject();
                string jsonStr = JsonConvert.SerializeObject(dict);

                string jsonName = "floor.json";
                string jsonPath = Path.Combine(Path.GetTempPath(), jsonName);
                Console.WriteLine("json path: {0}", jsonPath);
                using (StreamWriter w = new StreamWriter(jsonPath))
                    w.Write(jsonStr);
            },
            onFinish);
        }
    }
}
