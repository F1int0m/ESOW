using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ESOW
{
    public class Translator
    {
        private string TranslateAPI = "trnsl.1.1.20200502T125915Z.14059b0203d9b7cc.5be8e20d8945eaf64ac231f7afc7d638b4654794";

        private readonly string DictAPI =
            "dict.1.1.20200504T132406Z.98106a680b482674.2ec80fe65d53c0349acf03bb37dadc2727a18fdd";
        /// <summary>
        /// Получает перевод слова и ошибку, если перевод не удался
        /// </summary>
        /// <param name="ToTranslateString"> Строка для перевода</param>
        /// <param name="lang"> Направление перевода, например ru-en или en-ru</param>
        /// <returns></returns>
        public string Translate(string ToTranslateString, string lang)
        {
            if (ToTranslateString.Length <= 0) return "";
            var temp = "";
            var request = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr.json/translate?"
                                            + "key=" + TranslateAPI
                                            + "&text=" + ToTranslateString
                                            + "&lang=" + lang);

            var response = request.GetResponse();

            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string line = stream.ReadLine();

                if (line == null) return temp;

                var translation = JsonConvert.DeserializeObject<Translation>(line);

                temp = translation.text.Aggregate(temp, (current, str) => current + str);

                temp += translation.code == "200" ? "" : translation.code;
            }

            return temp;
        }



        public (string  transcription ,List<string> translation) Lookup(string word, string lang)
        {
            if (word.Length <= 0) return ("", new List<string>());
            
           

            var request = WebRequest.Create("https://dictionary.yandex.net/api/v1/dicservice.json/lookup?"
                                            + "key=" + DictAPI
                                            + "&lang=" + lang
                                            + "&text=" + word);
            var response = request.GetResponse();

            var res = new List<string>();
            var tr = "";
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                var line = stream.ReadLine();
                if (line == null) return ("", new List<string>());
                var lookup = JsonConvert.DeserializeObject<Lookup>(line);
                res = lookup.Def.SelectMany(x => x.Tr.Select(z => z.Text)).ToList();
                tr = lookup.Def[0].Ts!=null? lookup.Def[0].Ts:"No transcritption";
            }




            return (tr, res);
        }
    }
    class Translation
    {
        public string code { get; set; }
        public string lang { get; set; }
        public string[] text { get; set; }
    }

    public partial class Lookup
    {
        [JsonProperty("head")]
        public Head Head { get; set; }

        [JsonProperty("def")]
        public Def[] Def { get; set; }
    }

    public partial class Def
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("pos")]
        public string Pos { get; set; }

        [JsonProperty("tr")]
        public Tr[] Tr { get; set; }

        [JsonProperty("ts")]
        public string Ts { get; set; }
    }

    public partial class Tr
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("pos")]
        public string Pos { get; set; }

        [JsonProperty("syn")]
        public Mean[] Syn { get; set; }

        [JsonProperty("mean")]
        public Mean[] Mean { get; set; }

        [JsonProperty("ex")]
        public Ex[] Ex { get; set; }
    }

    public partial class Ex
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("tr")]
        public Mean[] Tr { get; set; }
    }

    public partial class Mean
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class Head
    {
    }

}
