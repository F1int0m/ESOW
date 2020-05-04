using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public Document CurrentDocument;
        private string LoadedFile;
        private bool IsTranslate = false;
        private Translator Translator = new Translator();
        private List<Document> listOfDocuments;
        public MainWindow()
        {
            InitializeComponent();
            listOfDocuments = CreateDocumentsList();
            CreateButtons(listOfDocuments);
        }

        private List<Document> CreateDocumentsList()
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
                CreateButton(doc, true);
            }
        }

        private void CreateButton(Document doc, bool isOurText)
        {
            var t = new Button();
            t.Content = isOurText?doc.Title:"(*)"+doc.Title;
            t.Height = 60;
            t.Background = isOurText ? Brushes.LightGray :Brushes.CadetBlue;
            t.Click += (s, a) =>
            {
                ResBox.Document.Blocks.Clear();
                WorkBox.Document.Blocks.Clear();
                CurrentDocument = doc;
                WorkTittle.Content = CurrentDocument.Title;
                TempBut.Visibility = isOurText ? Visibility.Visible : Visibility.Hidden;
                WorkBox.Document.Blocks.Add(new Paragraph(new Run(CurrentDocument.Content)));
                TabCont.SelectedIndex += 3;
            };
            Panel.Children.Add(t);
        }

        private void TranslateButton(object sender, RoutedEventArgs e)
        {
            var lang = IsTranslate ? "ru-en" : "en-ru";
            ResBox.Document.Blocks.Clear();
            ResBox.Document.Blocks.Add(new Paragraph(new Run(Translator.Translate(WorkBox.Selection.Text,lang))));
        }

        private void ShowTranslateButton(object sender, RoutedEventArgs e)
        {
            if (IsTranslate)
            {
                WorkBox.Document.Blocks.Clear();
                WorkBox.Document.Blocks.Add(new Paragraph(new Run(CurrentDocument.Content)));
                IsTranslate = false;
            }
            else
            {
                WorkBox.Document.Blocks.Clear();
                WorkBox.Document.Blocks.Add(new Paragraph(new Run(CurrentDocument.TranslatedContent)));
                IsTranslate = true;
            }
        }

        private void DropEvent(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                using (var reader = new StreamReader(paths.First()))
                {
                    LoadedFile = reader.ReadToEnd();
                }
            }

        }


        private void MakeCustomButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CustomTittle.Text=="")
            {
                return;
            }
            var temp = new Document(CustomA.Text+"\n"+ CustomTittle.Text,LoadedFile,"");
            CurrentDocument = temp;
            WorkTittle.Content = CurrentDocument.Title;
            TempBut.Visibility = Visibility.Hidden;
            WorkBox.Document.Blocks.Clear();
            ResBox.Document.Blocks.Clear();
            WorkBox.Document.Blocks.Add(new Paragraph(new Run(CurrentDocument.Content)));
            TabCont.SelectedIndex += 2;
            CreateButton(temp,false);
        }
    }
}
