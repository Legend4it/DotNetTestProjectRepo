using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string Certificate = "C:/Dev/Repos/Source/DotNetTestProjectRepo/CertManagment/shop.apak.se-all.pfx";

            // Load the certificate into an X509Certificate object.
            //X509Certificate cert = new X509Certificate();

            var cert = new X509Certificate2(File.ReadAllBytes(Certificate));

            // Get the value.
            string resultsTrue = cert.ToString(true);

            // Display the value to the console.
            Console.WriteLine(resultsTrue);

            // Get the value.
            string resultsFalse = cert.ToString(false);

            // Display the value to the console.
            Debug.WriteLine(resultsFalse);

            var certFiles = Directory.EnumerateFiles("C:/Dev/Repos/Source/DotNetTestProjectRepo/CertManagment/", "*.pfx", SearchOption.AllDirectories);

            foreach (var item in certFiles)
            {
                cert = new X509Certificate2(File.ReadAllBytes(item));

                // Get the value.
                resultsTrue = cert.ToString(true);

                // Display the value to the console.
                Console.WriteLine(resultsTrue);

                // Get the value.
                resultsFalse = cert.ToString(false);

                // Display the value to the console.
                Debug.WriteLine(resultsFalse);
                Debug.WriteLine($"################## {cert.Subject} #######################");
            }
        }
    }
}
