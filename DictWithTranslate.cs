using System.Collections.Generic;
using System.Linq;

namespace ESOW
{
    public class DictWithTranslate
    {
        private Dictionary<string, List<string>> Dict;
        public bool ContainsWord(string word) => Dict.ContainsKey(word);

        public List<string> this[string index] => Dict.ContainsKey(index) ? Dict[index] :new List<string>();


        /// <summary>
        /// Добавляет слово в текущий словарь
        /// </summary>
        /// <param name="word">Слово для перевода</param>
        /// <param name="lang">ru-en, en-ru</param>
        /// <param name="translator">Переводчик</param>
        public void Add(string word, string lang, Translator translator)
        {
            Dict[word] = translator.Lookup(word, lang);
            Update();
        }


        private void Update()
        {
            Dict = Dict.OrderBy(x => x.Key).ToDictionary(x => x.Key, z => z.Value);
        }
    }
    
}
