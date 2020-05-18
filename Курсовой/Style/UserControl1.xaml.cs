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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Курсовой.Style
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            Root.DataContext = this;
        }
        static UserControl1()
        {
            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(UserControl1));
            PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(UserControl1));
        }

        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty PlaceholderProperty;
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
    }
}
