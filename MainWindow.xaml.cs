using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private DictWithTranslate Dictionary = new DictWithTranslate();
        public object ListBoxItem { get; set; }
        private string GetLang() => IsTranslate ? "ru-en" : "en-ru";


        public MainWindow()
        {
            InitializeComponent();
            listOfDocuments = CreateDocumentsList();
            CreateButtons(listOfDocuments);
            Dictionary.LoadDict();
            ListBox.ItemsSource = Dictionary.Dict;  //Не обновляет словарь, при  добавлении новго слова. Проблема в XML


        }


        private List<Document> CreateDocumentsList()
        {
            var list = new List<Document>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Document("tittle" + i, Sbld(i, "content"), Sbld(i, "переведно"),  Difficult.Easy));
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
            foreach (var doc in d.OrderBy(x=>x.Difficult))
            {
                CreateButton(doc, true);
            }
        }

        private void CreateButton(Document doc, bool isOurText)
        {
            var t = new Button
            {
                Content = isOurText ? doc.Title : "(*)" + doc.Title,
                Height = 60,
                Background = SelectBackgroundColor(doc.Difficult)
            };
            t.Click += (s, a) =>
            {
                ResBox.Document.Blocks.Clear();
                WorkBox.Document.Blocks.Clear();
                CurrentDocument = doc;
                WorkTittle.Content = CurrentDocument.Title;
                WorkBox.Document.Blocks.Add(new Paragraph(new Run(CurrentDocument.Content)));
                TabCont.SelectedIndex += 3;
            };
            Panel.Children.Add(t);
        }

        private static SolidColorBrush SelectBackgroundColor(Difficult dif)
        {
            return dif == Difficult.Custom ? Brushes.CornflowerBlue :
                dif == Difficult.Easy ? Brushes.LightGreen :
                dif == Difficult.Medium ? Brushes.LightYellow :
                dif == Difficult.Hard ? Brushes.Orange : Brushes.IndianRed;
        }
        //Пожалуйста, научите Рому использовать git
        private void TranslateButton(object sender, RoutedEventArgs e)
        {
            ResBox.Document.Blocks.Clear();
            ResBox.Document.Blocks.Add(new Paragraph(new Run(Translator.Translate(WorkBox.Selection.Text, GetLang()))));
        }

        private void ShowTranslateButton(object sender, RoutedEventArgs e)
        {
            WorkBox.Document.Blocks.Clear();
            if (IsTranslate)
            {
                WorkBox.Document.Blocks.Add(new Paragraph(new Run(CurrentDocument.Content)));
                IsTranslate = false;
            }
            else
            {
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
            var temp = new Document(CustomA.Text+"\n"+ CustomTittle.Text,LoadedFile,"",Difficult.Custom);
            CurrentDocument = temp;
            WorkTittle.Content = CurrentDocument.Title;
            TempBut.Visibility = Visibility.Hidden;
            WorkBox.Document.Blocks.Clear();
            ResBox.Document.Blocks.Clear();
            WorkBox.Document.Blocks.Add(new Paragraph(new Run(CurrentDocument.Content)));
            TabCont.SelectedIndex += 2;
            CreateButton(temp,false);
        }

        private void AddToDict_OnClick(object sender, RoutedEventArgs e)
        {
            Dictionary.Add(WorkBox.Selection.Text, GetLang(), Translator);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dictionary.SaveDict();
        }

        private void CreateToolTip(object sender, MouseEventArgs e)
        {
            var bb = sender as TextBlock;
            var  tt  = new ToolTip();
            tt.Content = Dictionary.GetTranscription(bb.Text);
            bb.ToolTip = tt;

        }

        private void FrameworkElement_OnInitialized(object sender, EventArgs e)
        {
            var btm = sender as Button;

        }

        private void DictLoadingOnPage(object sender, EventArgs e)
        {
            ListBox.ItemsSource = Dictionary.Dict;
        }
    }
}
