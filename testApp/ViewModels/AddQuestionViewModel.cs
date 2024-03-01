using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using testApp.Interfaces;
using testApp.Models;
using testApp.Repositories;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using Xceed.Wpf.AvalonDock.Themes;
using static System.Net.Mime.MediaTypeNames;
using testApp.Forms;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace testApp.ViewModels
{
    public class AddQuestionViewModel : INotifyPropertyChanged
    {
        public OpenFileDialog openFileDialog;
        public ICommand SaveQuestionCommand { get; }
        public ICommand SaveEditQuestionCommand { get; }
        public ICommand AddImageCommand { get; }
        public ICommand AddQuestionFromFileCommand { get; }
        public ICommand FileCommand { get; }
        public ICommand CloseWindowsCommand { get; }
        public ICommand AddNewThemeCommand { get; }
        public ImageSource Image { get; private set; }
        public FileInfo FileInf;
        public string Title { get; set; }
        public string TitleImageButton { get; set; } = " Добавить картинку ";
        public bool VisibilityEdit { get; set; }
        public bool ImageEdit { get; set; } = false;
        
        public bool VisibilityAdd { get; set; }
        public string ImagePath { get; set; }
        private System.Collections.ObjectModel.ObservableCollection<string> themes;
        public System.Collections.ObjectModel.ObservableCollection<string> Themes
        {
            get { return themes; }
            set
            {
                themes = value;
                // add appropriate event raising pattern
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Themes"));
            }
        }
        public string SelectionTheme { get; set; }

        private TestQuestion testQuestion;

        public TestQuestion TestQuestion
        {
            get => testQuestion;
            set
            {
                testQuestion = value;

                RaisePropertyChanged(nameof(TestQuestion));
            }
        }
        IQuestionRepository<TestQuestion> db;

        public AddQuestionViewModel()
        {
            Title = " Добавление вопроса ";
            VisibilityEdit = false;
            VisibilityAdd = true;
            //CloseWindowsCommand = new RelayCommand(CloseWindows);
            AddNewThemeCommand = new RelayCommand(AddNewTheme);
            SaveQuestionCommand = new RelayCommand(SaveQuestion);
            AddImageCommand = new RelayCommand(AddImage);
            AddQuestionFromFileCommand = new RelayCommand(AddQuestionFromFile);
            FileCommand = new RelayCommand(SampleFile);
            TestQuestion = new TestQuestion();
            db = new QuestionRepository();
            var ThemesList = db.GetThemes();
            Themes = new System.Collections.ObjectModel.ObservableCollection<string>();
            foreach (var item in ThemesList)
                Themes.Add(item);
        }

        public AddQuestionViewModel(TestQuestion questionEdit)
        {
            Title = " Редактирование вопроса ";
            VisibilityEdit = true;
            VisibilityAdd = false;
            //CloseWindowsCommand = new RelayCommand(CloseWindows);
            SaveQuestionCommand = new RelayCommand(SaveQuestion);
            AddImageCommand = new RelayCommand(AddImage);
            AddNewThemeCommand = new RelayCommand(AddNewTheme);
            AddQuestionFromFileCommand = new RelayCommand(AddQuestionFromFile);
            TestQuestion = questionEdit;
            db = new QuestionRepository();
            var ThemesList = db.GetThemes();
            Themes = new System.Collections.ObjectModel.ObservableCollection<string>();
            foreach (var item in ThemesList)
                Themes.Add(item);
            SelectionTheme = TestQuestion.NameTheme;
            if (TestQuestion.ImageQuestion  != null)
            {
                ImagePath = TestQuestion.FullImageQuestion;
                TitleImageButton = " Изменить картинку ";
            }
            
            //if (editTrue)
            //{
            //    ImageButton = ;
            //}
            //else
            //{
            //    ImageButton = ;
            //}
        }

        private void AddQuestionFromFile(object obj)
        {
            openFileDialog = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "(*.TXT, *.WOP)|*.txt; *.wop|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                using (var stream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    FileInf = new FileInfo(openFileDialog.FileName);
                }
                List<TestQuestion> TestQuestions = new List<TestQuestion>();
                TestQuestions = ReadQustionsFromFile(FileInf.FullName);

                if(TestQuestions.Count == 0)
                {
                    MessageBox.Show("Произошла ошибка ошибка чтения файла. Вопросы не найдены.", "Ошибка чтения файла", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                db = new QuestionRepository();

                int x = 0;

                foreach (TestQuestion testQuestion in TestQuestions)
                {                    
                    if (!db.CheckQuestions(testQuestion))
                    {
                        db.Create(testQuestion);
                        x++;
                    }
                    else                    
                    {
                        MessageBox.Show("Данный вопрос уже существует в базе:" + "\r\n" + testQuestion.NameQuestion, "Ошибка заполнения базы", MessageBoxButton.OK, MessageBoxImage.Information);
                    }                    
                }

                MessageBox.Show("Вопросы успешно добавлены." + "\r\n" + "Вопросов добавленно в базу: " + x + "\r\n" + "Из найденных в файле: " + TestQuestions.Count, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SampleFile(object obj)
        { 
            Sample sample = new Sample();
            sample.ShowDialog();
        }


            private void AddNewTheme(object obj)
        {
            NewTheme newTheme = new NewTheme();
            if (newTheme.ShowDialog() == true)
            {
                if (newTheme.TextNewTheme != "" && newTheme.TextNewTheme != null)
                {
                    Themes.Add(newTheme.TextNewTheme);
                    RaisePropertyChanged("Themes");
                }
            }
        }


        private void AddImage(object obj)
        {
            openFileDialog = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
                RaisePropertyChanged("ImagePath");
                FileInf = new FileInfo(openFileDialog.FileName);
                if (VisibilityEdit)
                {
                    ImageEdit = true;
                }                    
                TitleImageButton = " Изменить картинку ";
                RaisePropertyChanged("TitleImageButton");
                //using (var stream = new FileStream(openFileDialog.FileName, FileMode.Open))
                //{
                //    ImagePath = openFileDialog.FileName;
                //    RaisePropertyChanged("ImagePath");
                //    FileInf = new FileInfo(openFileDialog.FileName);
                //}
            }
        }

        private void SaveQuestion(object obj)
        {
            if (TestQuestion.NameQuestion == null ||
                SelectionTheme == null ||
                TestQuestion.NameAnswerCorrect1 == null ||
                TestQuestion.NameAnswerIncorrect1 == null ||
                TestQuestion.NameAnswerIncorrect2 == null ||
                TestQuestion.NameAnswerIncorrect3 == null)
            {
                MessageBox.Show("Заполните все поля(поля Сециальность и пояснение необязательные)).", "Ошибка заполнения формы", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                db = new QuestionRepository();
                
                if ((!db.CheckQuestions(TestQuestion) || ImageEdit))
                {
                    //var newQuestion = new TestQuestion();


                    //newQuestion.NameQuestion = testQuestion.NameQuestion;
                    //newQuestion.NameSpeciality = testQuestion.NameSpeciality;
                    //newQuestion.NameTheme = SelectionTheme;
                    //newQuestion.NameArticle = testQuestion.NameArticle;
                    //newQuestion.NameAnswerCorrect1 = testQuestion.NameAnswerCorrect1;
                    //newQuestion.NameAnswerIncorrect1 = testQuestion.NameAnswerIncorrect1;
                    //newQuestion.NameAnswerIncorrect2 = testQuestion.NameAnswerIncorrect2;
                    //newQuestion.NameAnswerIncorrect3 = testQuestion.NameAnswerIncorrect3;



                    TestQuestion.NameTheme = SelectionTheme;
                    if (VisibilityEdit)
                    {
                        //db.Update(newQuestion);
                        if (ImageEdit)
                        {
                            string currentDir = Directory.GetCurrentDirectory();
                            //string dirName = @"Images\" + TestQuestion.NameTheme;
                            //string nameThemeForImage;
                            //if (TestQuestion.NameTheme.Count() > 200)
                            //{
                            //    nameThemeForImage = TestQuestion.NameTheme.Substring(0, 200);
                            //}
                            //else
                            //{
                            //    nameThemeForImage = TestQuestion.NameTheme;
                            //}
                            string dirName = "";
                            if (TestQuestion.ImageQuestion[0] == 'I')
                            {
                                dirName = @"1Images\";
                            }
                            else
                            {
                                dirName = @"Images\";
                            }
                            
                            // если папка не существует
                            if (!Directory.Exists(dirName))
                            {
                                Directory.CreateDirectory(dirName);
                            }
                            string fileType = FileInf.Extension;
                            string newPath = dirName + TestQuestion.QuestionId + fileType;
                            string fullNewPath = currentDir + "\\" + newPath;
                            File.Delete(fullNewPath);
                            TestQuestion.ImageQuestion = newPath;
                            TestQuestion.FullImageQuestion = fullNewPath;
                            if (FileInf.Exists)
                            {
                                //File.Delete(newPath);
                                FileInf.CopyTo(newPath, true);
                            }
                        }
                        db.Update(TestQuestion);

                        MessageBox.Show("Вопрос успешно изменен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        
                        //db.Create(newQuestion);
                        db.Create(TestQuestion);
                        
                        if (FileInf != null)
                        {
                            int idQuestion = (int)db.GetIdQuestions(TestQuestion);
                            //string nameThemeForImage;
                            //if (TestQuestion.NameTheme.Count() > 200)
                            //{
                            //    nameThemeForImage = TestQuestion.NameTheme.Substring(0, 200);
                            //}
                            //else
                            //{
                            //    nameThemeForImage = TestQuestion.NameTheme;
                            //}
                            //string currentDir = Directory.GetCurrentDirectory();
                            //string dirName = @"Images\" + TestQuestion.NameTheme;
                            string dirName = @"Images\";
                            // если папка не существует
                            if (!Directory.Exists(dirName))
                            {
                                Directory.CreateDirectory(dirName);
                            }
                            string fileType = FileInf.Extension;
                            TestQuestion.QuestionId = idQuestion;
                            string newPath = dirName + TestQuestion.QuestionId + fileType;
                            //newQuestion.ImageQuestion = newPath;
                            TestQuestion.ImageQuestion = newPath;
                            if (FileInf.Exists)
                            {
                                FileInf.CopyTo(newPath, true);
                            }
                            db.Update(TestQuestion);
                        }
                        
                        MessageBox.Show("Вопрос успешно добавлен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    

                    
                }

                else
                {
                    MessageBox.Show("Такой вопрос уже существует.", "Ошибка добавления вопроса", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        List<TestQuestion> ReadQustionsFromFile(string path)
        {
            List<TestQuestion> testQuestions = new List<TestQuestion>();
            List<string> Themes = new List<string>();
            string w = "";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string allText = File.ReadAllText(path, Encoding.GetEncoding("windows-1251"));
            allText = allText.Replace("        ", "");
            allText = allText.Replace("       ", "");
            allText = allText.Replace("      ", "");
            allText = allText.Replace("     ", "");
            allText = allText.Replace("    ", "");
            allText = allText.Replace("   ", "");
            allText = allText.Replace("  ", "");
            allText = allText.Replace("\r\n\r\n\r\n\r\n\r\n", "\r\n\r\n");
            allText = allText.Replace("\r\n\r\n\r\n\r\n", "\r\n\r\n");
            allText = allText.Replace("\r\n\r\n\r\n", "\r\n\r\n");

            List<string> paragraphsWithSpace = allText.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> paragraphs = new List<string>();
            foreach (string x in paragraphsWithSpace)
            {
                if (!String.IsNullOrWhiteSpace(x))
                {
                    paragraphs.Add(x);
                }
            }

            bool startQuestion = false;

            for (int i = 0; i < paragraphs.Count; i++)
            {
                string currentParagraph = paragraphs[i].Trim();

                switch (currentParagraph[0])
                {
                    case 'W':
                        w = currentParagraph.Substring(2).Trim();
                        break;
                    case 'Л':
                        string currenеThem = currentParagraph.Trim();
                        Themes.Add(currenеThem);
                        break;
                    case 'N':
                        startQuestion = true;
                        if (Themes.Count == 0)
                        {
                            string input = "";
                            while (input == "")
                            {
                                input = Interaction.InputBox("Введите тему вопросов", "Не найдена тема для вопросов", "введите тему для текущего набора вопросов");
                            }
                            Themes.Add("   " + input);
                        }

                        TestQuestion testQuestion = new TestQuestion();
                        try
                        {
                            testQuestion = StringOnQuestion(currentParagraph, path);
                        }
                        catch
                        {
                            MessageBox.Show("Не удалось преобразовать вопрос:" + "\r\n" + currentParagraph, "Ошибка добавления вопроса", MessageBoxButton.OK, MessageBoxImage.Error);
                            //Console.WriteLine("Вопрос не удалось преобразовать.");
                            break;
                        }

                        if (testQuestion.NameTheme == "")
                        {
                            try
                            {
                                testQuestion.NameTheme = Themes.Where(x => x.Substring(0, 3) == "ЛЛЛ").FirstOrDefault();

                            }
                            catch
                            {
                                MessageBox.Show("Не удалось найти тему." + "\r\n" + "Выбрана тема по-умолчанию:" + "\r\n" + Themes[0], "Ошибка поиска темы", MessageBoxButton.OK, MessageBoxImage.Information);
                                testQuestion.NameTheme = Themes[0].Substring(3);
                            }
                            
                        }
                        else
                        {
                            try
                            {
                                testQuestion.NameTheme = Themes.Where(x => x.Substring(0, testQuestion.NameTheme.Length) == testQuestion.NameTheme).FirstOrDefault().Substring(testQuestion.NameTheme.Length + 1);
                            }
                            catch
                            {
                                MessageBox.Show("Не удалось найти тему:" + "\r\n"+ testQuestion.NameTheme + "\r\n" + "выбрана тема по-умолчанию:" + "\r\n" + Themes[0], "Ошибка поиска темы", MessageBoxButton.OK, MessageBoxImage.Information);
                                testQuestion.NameTheme = Themes[0].Substring(3);
                            }
                        }
                        testQuestions.Add(testQuestion);
                        break;
                    default:
                        {
                            if (!startQuestion)
                            {
                                string currenеThem1 = "ЛЛЛ " + currentParagraph.Trim();
                                Themes.Add(currenеThem1);
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Не удалось считать вопрос:" + "\r\n" + currentParagraph, "Ошибка считывания вопроса", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            break;
                        }
                }
            }
            return testQuestions;
        }

        TestQuestion StringOnQuestion(string currentParagraph, string path)
        {
            TestQuestion testQuestion = new TestQuestion();
            int finishNoAndThemeAndArticle = currentParagraph.IndexOf("\r\n");
            string NoAndThemeAndArticle = currentParagraph.Substring(0, finishNoAndThemeAndArticle).Trim();
            string theme = "";
            string article = "";

            int startThemeAndArticle = NoAndThemeAndArticle.IndexOf('Л');

            if (startThemeAndArticle > -1)
            {
                string themeAndArticle = NoAndThemeAndArticle.Substring(startThemeAndArticle, NoAndThemeAndArticle.Length - 5).Trim(); ;
                int finishTheme = themeAndArticle.IndexOf(' ');
                if (finishTheme > -1)
                {
                    theme = themeAndArticle.Substring(0, finishTheme).Trim(); ;
                    article = themeAndArticle.Substring(finishTheme + 1).Trim(); ;
                }
                else
                {
                    theme = themeAndArticle;
                }
            }

            testQuestion.NameTheme = theme;
            testQuestion.NameArticle = article;

            int startImage = currentParagraph.IndexOf('{') + 1;

            int finishQuestion = currentParagraph.IndexOf("\r\n1.") - 1;

            if (startImage != 0)
            {
                int finishImage;
                finishImage = currentParagraph.IndexOf('}') - 1;
                string image = currentParagraph.Substring(startImage, finishImage - startImage + 1).Trim(); ;

                int startDirImage = 0;
                int lastDirImage = image.LastIndexOf(@"\"); ;
                string dirNameImage = image.Substring(startDirImage, lastDirImage);

                int startDir = 0;
                int lastDir = path.LastIndexOf(@"\") + 1; ;
                string dirName = path.Substring(startDir, lastDir) + image;

                string currentDir = Directory.GetCurrentDirectory();
                string targetPath = currentDir + @"\" + image;
                string newPath = currentDir + @"\" + dirNameImage;
                
                try
                {
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    //if (Directory.Exists(targetPath))
                    //{
                    //    targetPath += "1";
                    //    image += "1";
                    //}
                    File.Copy(dirName, targetPath, true);
                    testQuestion.ImageQuestion = image;
                }
                catch
                {
                    //MessageBox.Show("Картинка не найдена." + "\r\n" + image, "Ошибка копирования картинки", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                testQuestion.NameQuestion = currentParagraph.Substring(finishImage + 2, finishQuestion - finishImage - 1).Trim();
            }
            else
            {
                int startQuestion = currentParagraph.IndexOf("\r\n") + 2;
                testQuestion.NameQuestion = currentParagraph.Substring(startQuestion, finishQuestion - startQuestion).Trim();
            }

            int startCorrect = currentParagraph.IndexOf("\r\n1.") + 4;
            int startInCorrect1 = currentParagraph.IndexOf("\r\n2.") + 4;
            int startInCorrect2 = currentParagraph.IndexOf("\r\n3.") + 4;
            int startInCorrect3 = currentParagraph.IndexOf("\r\n4.") + 4;

            testQuestion.NameAnswerCorrect1 = currentParagraph.Substring(startCorrect, startInCorrect1 - startCorrect - 2).Trim();
            testQuestion.NameAnswerIncorrect1 = currentParagraph.Substring(startInCorrect1, startInCorrect2 - startInCorrect1 - 2).Trim();
            testQuestion.NameAnswerIncorrect2 = currentParagraph.Substring(startInCorrect2, startInCorrect3 - startInCorrect2 - 2).Trim();
            testQuestion.NameAnswerIncorrect3 = currentParagraph.Substring(startInCorrect3).Trim();
            return testQuestion;
        }

        //private void CloseWindows(object obj)
        //{
        //    foreach (Window window in Application.Current.Windows)
        //    {
        //        window.Close();
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
