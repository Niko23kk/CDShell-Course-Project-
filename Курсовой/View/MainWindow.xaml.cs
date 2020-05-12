using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Resources;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Collections;

namespace Курсовой.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //System.Windows.Resources.StreamResourceInfo sri = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resourses/pen (2).cur", UriKind.RelativeOrAbsolute));
            //this.Cursor = new Cursor(sri.Stream);
        }
    }
    
}
