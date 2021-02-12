using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WebSocketStartStopTcpListner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpSocket tc;
        public MainWindow()
        {
            InitializeComponent();
            tc = new TcpSocket();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Start");
            tc.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Stop");
            tc.Stop();
        }
    }
}
