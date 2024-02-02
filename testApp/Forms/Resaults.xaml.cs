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
    /// Interaction logic for Resaults.xaml
    /// </summary>
    public partial class Resaults : Window
    {
        public Resaults(List<TestQuestion> testQuestions, UserInfo userInfo, List<Result> results)
        {
            InitializeComponent();
            DataContext = new ResaultsViewModel(testQuestions, userInfo, results);
        }
    }
}
