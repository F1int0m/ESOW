using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ESOW
{
    public class DictWithTranslate
    {
        [JsonProperty]
        private Dictionary<string, List<string>> MainDict = new Dictionary<string, List<string>>();
        [JsonProperty]
        private Dictionary<string, string> TrDict = new Dictionary<string, string>();

        public Dictionary<string, List<string>> Dict => MainDict;


        public bool ContainsWord(string word) => MainDict.ContainsKey(word);

        public List<string> GetTranslations(string word) => MainDict.ContainsKey(word) ? MainDict[word] :new List<string>();

        public string GetTranscription(string word) => TrDict.ContainsKey(word) ? TrDict[word] :"";

        public static DictWithTranslate GetDictionaryFromJson()
        {

            return new DictWithTranslate(); 
        }


        public void SaveDict()
        {
            var с = System.Environment.CurrentDirectory;
            File.WriteAllText("../../Dict/Dict.json",JsonConvert.SerializeObject(this));
        }

        public void LoadDict()
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<DictWithTranslate>(File.ReadAllText("../../Dict/Dict.json"));
                if (temp == null) return;
                MainDict = temp.MainDict;
                TrDict = temp.TrDict;
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        /// <summary>
        /// Добавляет слово в текущий словарь
        /// </summary>
        /// <param name="word">Одно слово для перевода</param>
        /// <param name="lang">ru-en, en-ru</param>
        /// <param name="translator">Переводчик</param>
        public void Add(string word, string lang, Translator translator)
        {
            var temp = translator.Lookup(word, lang);
            if(temp.translation.Count<1) return;
            MainDict[word] = temp.translation;
            TrDict[word] = temp.transcription;
            //Update();
        }

        public void Remove(string word)
        {
            if (!MainDict.ContainsKey(word))
            {
                return;
            }
            MainDict.Remove(word);
        }


        private void Update()
        {
            MainDict = MainDict.OrderBy(x => x.Key).ToDictionary(x => x.Key, z => z.Value);
        }
    }
    
}
