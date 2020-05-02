using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ESOW
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateButtons(CreateDocumentsList());

        }

        private static List<Document> CreateDocumentsList()
        {
            var list = new List<Document>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Document("tittle" + i, Sbld(i, "content"), Sbld(i, "переведно")));
            }

            return list;
        }

        private static string Sbld(int i, string str)
        {
            var c = new StringBuilder();
            for (int j = 0; j < i; j++)
            {
                c.AppendLine(str +" "+ i);
            }
            
            return c.ToString();
        }

        private void CreateButtons(List<Document> d)
        {
            foreach (var doc in d)
            {
                var t = new Button();
                t.Content = doc.Title;
                t.Height = 60;
                t.Click += (s, a) =>
                {
                    WorkTittle.Content = doc.Title;
                    WorkBox.Document.Blocks.Clear();
                    WorkBox.Document.Blocks.Add(new Paragraph(new Run(doc.Content)));
                    TabCont.SelectedIndex = 3;
                };
                Panel.Children.Add(t);
            }
        }

        private void TranslateButton(object sender, RoutedEventArgs e)
        {
            var h =new Translater();
            ResBox.Document.Blocks.Clear();
            ResBox.Document.Blocks.Add(new Paragraph(new Run(h.Translate(WorkBox.Selection.Text))));
        }

        private void ShowTranslateButton(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
