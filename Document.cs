using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOW
{
    public class Document
    {
        public string Title { get; private set; }
        public string Content { get; private set; }

        public string TranslatedContent { get; private set; }

        public Document(string title, string content, string translate)
        {
            Title = title;
            Content = content;
            TranslatedContent = translate;
        }
        public Document()
        {
            Title = "";
            Content = "";
        }
    }
}
