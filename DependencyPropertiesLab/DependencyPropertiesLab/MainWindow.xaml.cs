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

namespace DependencyPropertiesLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string NormalClr { get; set; }

        public string MyClrProp
        {
            get { return (string)GetValue(MyClrPropProperty); }
            set { SetValue(MyClrPropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClrProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyClrPropProperty =
            DependencyProperty.Register("MyClrProp", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));


        public MainWindow()
        {
            InitializeComponent();

            SetValue(MyClrPropProperty, "Hello ... !");
            NormalClr = "dasdasdasda";


        }

    }
}
