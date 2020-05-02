using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ESOW
{
    public class Translator
    {
        private string API = "trnsl.1.1.20200502T125915Z.14059b0203d9b7cc.5be8e20d8945eaf64ac231f7afc7d638b4654794";
        public string Translate(string ToTranslateString, string lang)
        {
            var temp = "";
            if (ToTranslateString.Length > 0)
            {
                var request = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr.json/translate?"
                                                       + "key=" + API
                                                       + "&text=" + ToTranslateString
                                                       + "&lang=" + lang);

                var response = request.GetResponse();

                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;

                    if ((line = stream.ReadLine()) == null) return temp;

                    var translation = JsonConvert.DeserializeObject<Translation>(line);

                    temp = translation.text.Aggregate(temp, (current, str) => current + str);

                    temp += translation.code == "200" ? "" : translation.code;
                }

                return temp;
            }
            return "";
        }
    }
    class Translation
    {
        public string code { get; set; }
        public string lang { get; set; }
        public string[] text { get; set; }
    }



}
