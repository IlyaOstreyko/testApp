﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using testApp.Forms;
using testApp.Models;
using testApp.Repositories;
using Xceed.Wpf.AvalonDock.Themes;

namespace testApp.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private UserInfo userInfo;
        private int allNumberQuestions;
        private List<TestQuestion> TestQuestions { get; set; }
        private List<Result> Results;

        public UserInfo UserInfo
        {
            get => userInfo;
            set
            {
                userInfo = value;

                RaisePropertyChanged(nameof(UserInfo));
            }
        }
        public int AllNumberQuestions
        {
            get => allNumberQuestions;
            set
            {
                allNumberQuestions = value;

                RaisePropertyChanged(nameof(AllNumberQuestions));
            }
        }

        public UserViewModel(List<TestQuestion> questions, List<Result> results)
        {
            TestQuestions = questions;
            Results = results;
            AllNumberQuestions = questions.Count;
            UserInfo = new UserInfo();
        }

        RelayCommand start;
        public RelayCommand Start
        {

            get
            {
                return start ??
                    (start = new RelayCommand((o) =>
                    {
                        if (UserInfo.User != null && 
                        UserInfo.Chairman != null && 
                        UserInfo.PositionChairman != null && 
                        UserInfo.CommissionMember1 != null &&
                        UserInfo.PositionCommissionMember1 != null &&
                        UserInfo.PositionUser != null)
                        {
                            ShowQuestion showQuestions = new ShowQuestion(TestQuestions, UserInfo, Results);
                            showQuestions.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Заполните все данные.", "Ошибка заполнения формы", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }));
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
