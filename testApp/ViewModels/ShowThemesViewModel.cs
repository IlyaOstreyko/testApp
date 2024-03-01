using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testApp.Forms;
using testApp.Interfaces;
using testApp.Models;
using testApp.Repositories;

namespace testApp.ViewModels
{
    public class ShowThemesViewModel : INotifyPropertyChanged
    {
        public List<TestQuestion> TestQuestions { get; set; }
        public List<TableTheme> TableThemes { get; set; }
        public IEnumerable<string> IsChecked { get; set; }

        public OpenFileDialog openFileDialog;
        public ICommand ShowQuestionsCommand { get; }
        public ICommand AddImageCommand { get; }
        public ICommand CloseWindowsCommand { get; }

        readonly IQuestionRepository<TestQuestion> db;

        public ShowThemesViewModel()
        {
            CloseWindowsCommand = new RelayCommand(CloseWindows);
            ShowQuestionsCommand = new RelayCommand(ShowQuestions);
            db = new QuestionRepository();
            
            var Themes = db.GetThemes().ToList();
            TableThemes = new List<TableTheme>(Themes.Count());
            for (int i = 0; i < Themes.Count(); i++)
            {
                var Theme = new TableTheme
                {
                    Theme = Themes[i],
                    Tag = false,
                    Number = 0,
                    AllNumber = db.GetNumberQuestionInTheme(Themes[i])
                };
                TableThemes.Add(Theme);
            }
        }

        private void ShowQuestions(object obj)
        {
            //var selectedTheme = TableThemes.Where(n => n.Tag == true).ToList();
            var selectedTheme = TableThemes.Where(n => n.Number > 0).ToList();
            if (selectedTheme.Count == 0)
            {
                MessageBox.Show("Выберите колличество вопросов.", "Ошибка заполнения формы", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TestQuestions = new List<TestQuestion>();
                List <Result> Results = new List<Result>();
                foreach (TableTheme a in selectedTheme)
                {
                    Result result = new Result();
                    result.Theme = a.Theme;
                    result.AllNumberQustions = a.AllNumber;
                    result.NumberQustions = a.Number;
                    result.NumberMistake = a.Number;
                    Results.Add(result);
                    var questionsInTheme = db.GetRndQuestionsInTheme(a.Theme, a.Number).ToList();
                    //if (TestQuestions == null)
                    //{
                    //    TestQuestions = new List<TestQuestion>();
                    //}

                    TestQuestions.AddRange(questionsInTheme);

                }

                //ShowQuestion firstQuestion = new ShowQuestion(TestQuestions);
                User enterUser = new User(TestQuestions, Results);
                enterUser.ShowDialog();
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
