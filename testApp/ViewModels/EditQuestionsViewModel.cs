﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testApp.Forms;
using testApp.Interfaces;
using testApp.Models;
using testApp.Repositories;
using Xceed.Wpf.AvalonDock.Themes;

namespace testApp.ViewModels
{
    public class EditQuestionsViewModel : INotifyPropertyChanged
    {
        public System.Collections.ObjectModel.ObservableCollection<TestQuestion> testQuestions;
        public System.Collections.ObjectModel.ObservableCollection<TestQuestion> TestQuestions
        {
            get { return testQuestions; }
            set
            {
                testQuestions = value;
                // add appropriate event raising pattern
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TestQuestions"));
            }
        }
        public List<string> Themes { get; set; }
        public string SelectionTheme { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public int DeleteQuestionId { get; set; }
        //public int HightRow { get; set; } = 100;
        public bool VisibilityManager { get; set; }
        public ICommand ShowQuestionsCommand { get; }
        public ICommand ShowCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        readonly IQuestionRepository<TestQuestion> db;
        public EditQuestionsViewModel(string title, bool editTrue)
        {
            Title = title;
            VisibilityManager = editTrue;
            ShowQuestionsCommand = new RelayCommand(ShowQuestions);
            ShowCommand = new RelayCommand(Show);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            db = new QuestionRepository();
            Themes = db.GetThemes().ToList();
        }

        public void ShowQuestions(object obj)
        {
            //TestQuestions = db.GetQuestionsInTheme(SelectionTheme);
            var TestQuestionsList = db.GetQuestionsInTheme(SelectionTheme);
            //HightRow = SelectionTheme.Count(x => x == '\n') * 50 + 100;
            TestQuestions = new System.Collections.ObjectModel.ObservableCollection<TestQuestion>();
            foreach (var item in TestQuestionsList)
                TestQuestions.Add(item);
            RaisePropertyChanged("TestQuestions");
        }

        public void Show(object obj)
        {
            //TestQuestions = db.GetQuestionsInTheme(SelectionTheme);
            string theme = obj.ToString();
            var TestQuestionsList = db.GetQuestionsInTheme(theme);
            //HightRow = SelectionTheme.Count(x => x == '\n') * 50 + 100;
            TestQuestions = new System.Collections.ObjectModel.ObservableCollection<TestQuestion>();
            foreach (var item in TestQuestionsList)
                TestQuestions.Add(item);
            RaisePropertyChanged("TestQuestions");
        }

        public void Edit(object obj)
        {
            if (obj is TestQuestion question)
            {
                AddQuestion firstQuestion = new AddQuestion(question);
                firstQuestion.ShowDialog();
                //TestQuestions = db.GetQuestionsInTheme(SelectionTheme);
                //var TestQuestionsList = db.GetQuestionsInTheme(SelectionTheme);
                //TestQuestions = new System.Collections.ObjectModel.ObservableCollection<TestQuestion>();
                //foreach (var item in TestQuestionsList)
                    //estQuestions.Add(item);
                RaisePropertyChanged("TestQuestions");
            }
        }

        public void Delete(object obj)
        {
            if (obj is TestQuestion question)
            {
                var Result = MessageBox.Show("Удалить вопрос?" + "\r\n" + "\r\n" + question.NameQuestion, "Удаление вопроса", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    db.Delete((int)question.QuestionId);
                    //TestQuestions = db.GetQuestionsInTheme(SelectionTheme);
                    var TestQuestionsList = db.GetQuestionsInTheme(SelectionTheme);
                    TestQuestions = new System.Collections.ObjectModel.ObservableCollection<TestQuestion>();
                    foreach (var item in TestQuestionsList)
                        TestQuestions.Add(item);
                    if (TestQuestions.Count == 0)
                    {
                        MessageBox.Show("В данной теме больше нет вопросов", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        TestQuestions = null;
                        SelectionTheme = null;
                        Themes = db.GetThemes().ToList();
                        RaisePropertyChanged("Themes");
                        RaisePropertyChanged("SelectionTheme");
                        RaisePropertyChanged("TestQuestions");
                        return;
                    }
                    RaisePropertyChanged("TestQuestions");
                    MessageBox.Show("Вы удалили вопрос:" + "\r\n" + question.NameQuestion, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (Result == MessageBoxResult.No)
                {
                    return;
                }

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
