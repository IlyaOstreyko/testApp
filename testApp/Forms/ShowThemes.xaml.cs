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
    /// Interaction logic for ShowQuestions.xaml
    /// </summary>
    public partial class ShowThemes: Window
    {
        public ShowThemes()
        {
            InitializeComponent();
            DataContext = new ShowThemesViewModel();
        }
        private void PortBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // xaml.cs code
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void TextBox_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
