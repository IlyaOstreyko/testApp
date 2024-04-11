using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using testApp.Models;
using Application1 = Microsoft.Office.Interop.Word.Application;

namespace testApp.ViewModels
{
    public class ResaultsViewModel : INotifyPropertyChanged
    {
        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }
        public SaveFileDialog saveFileDialog;
        private UserInfo UserInfo;
        private List<Result> Results;
        public List<TestQuestion> CorrectTestQuestions { get; set; }
        public List<TestQuestion> IncorrectTestQuestions { get; set; }

        public ResaultsViewModel(List<TestQuestion> questions, UserInfo userInfo, List<Result> results)
        {
            Results = results;
            UserInfo = userInfo;
            CorrectTestQuestions = questions.Where(n => n.NameAnswer == null).ToList();
            IncorrectTestQuestions = questions.Where(n => n.NameAnswer != null).ToList();
            SaveCommand = new RelayCommand(Save);
            CloseCommand = new RelayCommand(Close);
        }

        private void Save(object obj)
        {
            Application1 app = new Application1();
            string currentDir = Directory.GetCurrentDirectory();
            Document doc = app.Documents.Open(currentDir + @"\DOC.doc");
            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;

            int tablecount = doc.Tables.Count;
            Table table = doc.Tables[1];
            int sumAll = 0;
            int sum = 0;
            int sumMistake = 0;
            for (int i = 0; i < Results.Count; i++)
            {
                table.Rows.Add();
                table.Cell(i + 2, 1).Range.Text = Results[i].Theme;
                table.Cell(i + 2, 2).Range.Text = Results[i].AllNumberQustions.ToString();
                table.Cell(i + 2, 3).Range.Text = Results[i].NumberQustions.ToString();
                table.Cell(i + 2, 4).Range.Text = Results[i].NumberMistake.ToString();
                sum += Results[i].NumberQustions;
                sumAll += Results[i].AllNumberQustions;
                sumMistake += Results[i].NumberMistake;
            }
            table.Rows.Add();
            table.Cell(Results.Count + 2, 1).Range.Text = "Итого";
            table.Cell(Results.Count + 2, 2).Range.Text = sumAll.ToString();
            table.Cell(Results.Count + 2, 3).Range.Text = sum.ToString();
            table.Cell(Results.Count + 2, 4).Range.Text = sumMistake.ToString();
            table.Rows.Add();
            table.Cell(Results.Count + 3, 1).Range.Text = "Оценка теста";
            if (sumMistake > UserInfo.NumberMistake)
            {
                table.Cell(Results.Count + 3, 2).Range.Text = "неудовлетворительно";
            }
            else
            {
                table.Cell(Results.Count + 3, 2).Range.Text = "удовлетворительно";
            }
            
            table.Rows[Results.Count + 3].Cells[2].Merge(table.Rows[Results.Count + 3].Cells[4]);

            if (doc.Bookmarks.Exists("DataDoc"))
            {
                object oBookMark = "DataDoc";

                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = date.ToString("dd/MM/yyyy");
            }


            if (doc.Bookmarks.Exists("UserDoc"))
            {
                object oBookMark = "UserDoc";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = UserInfo.User;
            }
            if (doc.Bookmarks.Exists("User2Doc"))
            {
                object oBookMark = "User2Doc";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = UserInfo.User;
            }
            if (doc.Bookmarks.Exists("PositionUserDoc"))
            {
                object oBookMark = "PositionUserDoc";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = UserInfo.PositionUser;
            }
            if (doc.Bookmarks.Exists("ChairmanDoc"))
            {
                object oBookMark = "ChairmanDoc";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = UserInfo.Chairman;
            }
            if (doc.Bookmarks.Exists("PositionChairmanDoc"))
            {
                object oBookMark = "PositionChairmanDoc";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = UserInfo.PositionChairman;
            }
            if (doc.Bookmarks.Exists("CommissionMember1Doc"))
            {
                object oBookMark = "CommissionMember1Doc";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = UserInfo.CommissionMember1;
            }
            if (doc.Bookmarks.Exists("PositionCommissionMember1Doc"))
            {
                object oBookMark = "PositionCommissionMember1Doc";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = UserInfo.PositionCommissionMember1;
            }
            if (doc.Bookmarks.Exists("MistakeDoc"))
            {
                object oBookMark = "MistakeDoc";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = UserInfo.NumberMistake.ToString();
            }

            saveFileDialog = new SaveFileDialog()
            {
                Title = "Сохранить результаты теста",
                Filter = "Text file (*.pdf)|*.pdf",
                DefaultExt = ".pdf"
            };

            string filename = "";
            if (saveFileDialog.ShowDialog() == true)
            {
                filename = saveFileDialog.FileName;
                doc.ExportAsFixedFormat(filename, WdExportFormat.wdExportFormatPDF);
            }

            //string nameFilePDF = UserInfo.User + ".pdf";

            doc.Close(WdSaveOptions.wdDoNotSaveChanges);
            app.Quit();
        }

        private void Close(object obj)
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                var type = window.GetType();
                var name = type.Name;
                if (name != "MainWindow")
                {
                    window.Close();
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
