using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ModernUI_ChartTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<SeriesData> Series
        {
            get;
            set;
        }

        public ObservableCollection<TestClass> Errors
        {
            get;
            set;
        }

        public ObservableCollection<TestClass> Warnings
        {
            get;
            set;
        }

        public string test { get=>"Binding Test"; }

        public MainWindow()
        {
            InitializeComponent();

            Series = new List<SeriesData>();

            Errors = new ObservableCollection<TestClass>();
            Warnings = new ObservableCollection<TestClass>();

            Errors.Add(new TestClass() { Category = "Globalization", Number = 66 });
            Errors.Add(new TestClass() { Category = "Features", Number = 23 });
            Errors.Add(new TestClass() { Category = "Content Types", Number = 12 });
            Errors.Add(new TestClass() { Category = "Correctness", Number = 94 });
            Errors.Add(new TestClass() { Category = "Naming", Number = 45 });
            Errors.Add(new TestClass() { Category = "Best Practices", Number = 29 });

            Warnings.Add(new TestClass() { Category = "Globalization", Number = 34 });
            Warnings.Add(new TestClass() { Category = "Features", Number = 23 });
            Warnings.Add(new TestClass() { Category = "Content Types", Number = 15 });
            Warnings.Add(new TestClass() { Category = "Correctness", Number = 66 });
            Warnings.Add(new TestClass() { Category = "Naming", Number = 56 });
            Warnings.Add(new TestClass() { Category = "Best Practices", Number = 34 });

            Series.Add(new SeriesData() { DisplayName = "Errors", Items = Errors });
            Series.Add(new SeriesData() { DisplayName = "Warnings", Items = Warnings });

            this.DataContext = this;

            //this.StackedBarChart.DataContext=this;
        }
    }

    public class TestClass : INotifyPropertyChanged
    {
        public string Category { get; set; }

        private float _number = 0;
        public float Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Number"));
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class SeriesData
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public ObservableCollection<TestClass> Items { get; set; }
    }


}
