using System.Collections.Generic;
using System.Linq;

namespace ESOW
{
    public class DictWithTranslate
    {
        private Dictionary<string, List<string>> MainDict;
        private Dictionary<string, string> TrDict;


        public bool ContainsWord(string word) => MainDict.ContainsKey(word);

        public List<string> GetTranslations(string word) => MainDict.ContainsKey(word) ? MainDict[word] :new List<string>();

        public string GetTranscription(string word) => TrDict.ContainsKey(word) ? TrDict[word] :"";


        /// <summary>
        /// Добавляет слово в текущий словарь
        /// </summary>
        /// <param name="word">Одно слово для перевода</param>
        /// <param name="lang">ru-en, en-ru</param>
        /// <param name="translator">Переводчик</param>
        public void Add(string word, string lang, Translator translator)
        {
            var temp = translator.Lookup(word, lang);
            MainDict[word] = temp.translation;
            TrDict[word] = temp.transcription;
            //Update();
        }


        private void Update()
        {
            MainDict = MainDict.OrderBy(x => x.Key).ToDictionary(x => x.Key, z => z.Value);
        }
    }
    
}
