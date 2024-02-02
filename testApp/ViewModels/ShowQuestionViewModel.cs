﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using testApp.Models;
using Microsoft.Office.Interop.Word;
using static System.Net.Mime.MediaTypeNames;
using Application1 = Microsoft.Office.Interop.Word.Application;
using System.Windows.Controls;
using Window = System.Windows.Window;
using Application = System.Windows.Application;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using Microsoft.Win32;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Binding = System.Windows.Data.Binding;
using testApp.Forms;

namespace testApp.ViewModels
{
    public class ShowQuestionViewModel : INotifyPropertyChanged
    {
        public SaveFileDialog saveFileDialog;
        //public ICommand 
        public ICommand NextQuestionCommand { get; }
        public ICommand CloseWindowsCommand { get; }
        public ImageSource Image { get; private set; }

        private TestQuestion testQuestion;
        private UserInfo UserInfo;
        private Answers answers;
        public bool VisibilityNameSpeciality { get; private set; } = false;
        private bool answer1;
        private bool answer2;
        private bool answer3;
        private bool answer4;
        private int numberQuestion;
        private int mark;
        //private int numberAnswer;
        private int quantityQuestion;
        private string nameButton;

        private List<Result> Results;
        public List<TestQuestion> TestQuestions { get; set; }

        public TestQuestion TestQuestion
        {
            get => testQuestion;
            set
            {
                testQuestion = value;

                RaisePropertyChanged(nameof(TestQuestion));
            }
        }

        public int QuantityQuestion
        {
            get => quantityQuestion;
            set
            {
                quantityQuestion = value;
                RaisePropertyChanged(nameof(QuantityQuestion));
            }
        }

        public int NumberQuestion
        {
            get => numberQuestion;
            set
            {
                numberQuestion = value;
                RaisePropertyChanged(nameof(NumberQuestion));
            }
        }

        public string NameButton
        {
            get => nameButton;
            set
            {
                nameButton = value;
                RaisePropertyChanged(nameof(NameButton));
            }
        }

        //public int NumberAnswer
        //{
        //    get => numberAnswer;
        //    set
        //    {
        //        numberAnswer = value;
        //        RaisePropertyChanged(nameof(NumberAnswer));
        //    }
        //}

        public bool Answer1
        {
            get => answer1;
            set
            {
                answer1 = value;
                RaisePropertyChanged(nameof(Answer1));
            }
        }


        public bool Answer2
        {
            get => answer2;
            set
            {
                answer2 = value;
                RaisePropertyChanged(nameof(Answer2));
            }
        }

        public bool Answer3
        {
            get => answer3;
            set
            {
                answer3 = value;
                RaisePropertyChanged(nameof(Answer3));
            }
        }
        public bool Answer4
        {
            get => answer4;
            set
            {
                answer4 = value;
                RaisePropertyChanged(nameof(Answer4));
            }
        }
        public Answers Answers
        {
            get => answers;
            set
            {
                answers = value;

                RaisePropertyChanged(nameof(Answers));
            }
        }

        public ShowQuestionViewModel(TestQuestion question)
        {
            CloseWindowsCommand = new RelayCommand(CloseWindows);
            NextQuestionCommand = new RelayCommand(NextQuestion);
            //SaveQuestionCommand = new RelayCommand(SaveQuestion);
            //AddImageCommand = new RelayCommand(AddImage);
            //Image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            question.ImageQuestion = "icon_2.jpg";
            //if (question.ImageQuestion == null)
            //{
            //    question.ImageQuestion = "icon_2.jpg";
            //}
            Image = new BitmapImage(new Uri("pack://application:,,,/" + question.ImageQuestion));
            RaisePropertyChanged("Image");
            TestQuestion = question;
        }

        public ShowQuestionViewModel(List<TestQuestion> questions, UserInfo userInfo, List<Result> results)
        {
            Results = results;
            CloseWindowsCommand = new RelayCommand(CloseWindows);
            QuantityQuestion = questions.Count();
            NumberQuestion = 1;
            mark = 0;
            NameButton = "Следующий вопрос";
            NextQuestionCommand = new RelayCommand(NextQuestion);
            TestQuestions = questions;
            UserInfo = userInfo;

            TestQuestion = TestQuestions[NumberQuestion - 1];

            List<string> RndAnswers = new List<string>() { TestQuestion.NameAnswerCorrect1, TestQuestion.NameAnswerIncorrect1, TestQuestion.NameAnswerIncorrect2, TestQuestion.NameAnswerIncorrect3 };
            Shuffle(RndAnswers);

            Answers = new Answers()
            {
                NameAnswer1 = RndAnswers[0],
                NameAnswer2 = RndAnswers[1],
                NameAnswer3 = RndAnswers[2],
                NameAnswer4 = RndAnswers[3]
            };

            //TestQuestion.ImageQuestion = "pack://application:,,,/" + "Images/уца/уац.jpg";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string currentDirectory = Directory.GetCurrentDirectory();

            if (TestQuestion.ImageQuestion == null)
            {
                TestQuestion.ImageQuestion = currentDirectory + @"\images\icon_2.jpg";
            }
            else
            {
                TestQuestion.ImageQuestion = currentDirectory + @"\" + TestQuestion.ImageQuestion;
            }

            Uri link = new Uri(TestQuestion.ImageQuestion);

            Image = new BitmapImage(link);
            if (TestQuestion.NameSpeciality != null)
            {
                VisibilityNameSpeciality = true;
            }
            RaisePropertyChanged("VisibilityNameSpeciality");
            RaisePropertyChanged("Image");
        }

        private void NextQuestion(object obj)
        {
            if (Answer1 == false && Answer2 == false && Answer3 ==  false && Answer4 == false)
            {
                return;
            }
            if (NumberQuestion <= TestQuestions.Count() && NameButton != "Показать результат")
            {
                if (Answer1 == true)
                {
                    if (Answers.NameAnswer1 == TestQuestion.NameAnswerCorrect1)
                    {
                        mark++;
                        Results.First(n => n.Theme == TestQuestion.NameTheme).NumberMistake--;
                    }
                    else
                    {
                        TestQuestion.NameAnswer = Answers.NameAnswer1;
                    }                    
                }
                if (Answer2 == true)
                {
                    if (Answers.NameAnswer2 == TestQuestion.NameAnswerCorrect1)
                    {
                        mark++;
                        Results.First(n => n.Theme == TestQuestion.NameTheme).NumberMistake--;
                    }
                    else
                    {
                        TestQuestion.NameAnswer = Answers.NameAnswer2;
                    }                    
                }
                if (Answer3 == true)
                {
                    if (Answers.NameAnswer3 == TestQuestion.NameAnswerCorrect1)
                    {
                        mark++;
                        Results.First(n => n.Theme == TestQuestion.NameTheme).NumberMistake--;
                    }
                    else
                    {
                        TestQuestion.NameAnswer = Answers.NameAnswer3;
                    }                    
                }
                if (Answer4 == true)
                {
                    if (Answers.NameAnswer4 == TestQuestion.NameAnswerCorrect1)
                    {
                        mark++;
                        Results.First(n => n.Theme == TestQuestion.NameTheme).NumberMistake--;
                    }
                    else
                    {
                        TestQuestion.NameAnswer = Answers.NameAnswer4;
                    }                    
                }
                NumberQuestion++;

                if (NumberQuestion == TestQuestions.Count() + 1)
                {
                    NameButton = "Показать результат";
                    RaisePropertyChanged("NameButton");
                }
                NumberQuestion--;
            }

            if (NumberQuestion < TestQuestions.Count())
            {

                TestQuestion = TestQuestions[NumberQuestion];

                List<string> RndAnswers = new List<string>() { TestQuestion.NameAnswerCorrect1, TestQuestion.NameAnswerIncorrect1, TestQuestion.NameAnswerIncorrect2, TestQuestion.NameAnswerIncorrect3 };
                Shuffle(RndAnswers);

                Answers = new Answers()
                {
                    NameAnswer1 = RndAnswers[0],
                    NameAnswer2 = RndAnswers[1],
                    NameAnswer3 = RndAnswers[2],
                    NameAnswer4 = RndAnswers[3]
                };

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string currentDirectory = Directory.GetCurrentDirectory();

                if (TestQuestion.ImageQuestion == null)
                {
                    TestQuestion.ImageQuestion = currentDirectory + @"\images\icon_2.jpg";
                }
                else
                {
                    TestQuestion.ImageQuestion = currentDirectory + @"\" + TestQuestion.ImageQuestion;
                }

                Uri link = new Uri(TestQuestion.ImageQuestion);

                Image = new BitmapImage(link);

                NumberQuestion++;

                Answer1 = false;
                Answer2 = false;
                Answer3 = false;
                Answer4 = false;

                RaisePropertyChanged("Answer1");
                RaisePropertyChanged("Answer2");
                RaisePropertyChanged("Answer3");
                RaisePropertyChanged("Answer4");
                RaisePropertyChanged("TestQuestion");
                RaisePropertyChanged("Image");
                RaisePropertyChanged("Answers");
            }
            else
            {

                System.Windows.MessageBox.Show("Вы ответили правильно на " + mark.ToString() + " вопросов.", "Результат теста", MessageBoxButton.OK, MessageBoxImage.Information);
                Resaults resultsWindow = new Resaults(TestQuestions, UserInfo, Results);
                resultsWindow.ShowDialog();
            }

        }

        public class RadioButtonCheckedConverter : IValueConverter

        {

            public object Convert(object value, Type targetType, object parameter,

                System.Globalization.CultureInfo culture)

            {

                return value.Equals(parameter);

            }



            public object ConvertBack(object value, Type targetType, object parameter,

                System.Globalization.CultureInfo culture)

            {

                return value.Equals(true) ? parameter : Binding.DoNothing;

            }

        }

        private Random rng = new Random();

        public void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        

        private void CloseWindows(object obj)
        {
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
