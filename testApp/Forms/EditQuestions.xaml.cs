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
using testApp.ViewModels;

namespace testApp.Forms
{
    /// <summary>
    /// Interaction logic for EditQuestions.xaml
    /// </summary>
    public partial class EditQuestions : Window
    {
        public EditQuestions(string title, bool editTrue)
        {
            InitializeComponent();
            DataContext = new EditQuestionsViewModel(title, editTrue);
        }
    }
}
