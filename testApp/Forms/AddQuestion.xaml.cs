using System;
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
using System.Windows.Shapes;
using testApp.Models;
using testApp.ViewModels;

namespace testApp.Forms
{
    /// <summary>
    /// Interaction logic for AddQuestion.xaml
    /// </summary>
    public partial class AddQuestion : Window
    {
        public AddQuestion()
        {
            //InitializeComponent();
            //TestQuestion = testQuestion;
            //DataContext = TestQuestion;

            InitializeComponent();
            DataContext = new AddQuestionViewModel();
        }
        public AddQuestion(TestQuestion testQuestion)
        {
            //InitializeComponent();
            //TestQuestion = testQuestion;
            //DataContext = TestQuestion;

            InitializeComponent();
            DataContext = new AddQuestionViewModel(testQuestion);
        }
    }
}
