using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using Newtonsoft.Json;

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



        public List<string> Lookup(string word, string lang)
        {
            return null;
            if (word.Length <= 0) return new List<string>();
            var res = new List<string>();

            var request = WebRequest.Create("https://dictionary.yandex.net/api/v1/dicservice.json/lookup?"
                                            + "key=" + DictAPI
                                            + "&lang=" + lang
                                            + "&text=" + word);
            var response = request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                var line = stream.ReadLine();
                if (line == null) return res;
                var lookup = JsonConvert.DeserializeObject<Lookup>(line);

            }




            return new List<string>();
        }
    }
    class Translation
    {
        public string code { get; set; }
        public string lang { get; set; }
        public string[] text { get; set; }
    }

    class Lookup
    {
        public string def { get; set; }
        public string[] tr { get; set; }
        public string[] syn { get; set; }
        public string[] mean { get; set; }
        public string[] ex { get; set; }
    }



}
