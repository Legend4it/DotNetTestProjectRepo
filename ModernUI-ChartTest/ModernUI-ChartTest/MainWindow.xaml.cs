using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<TestClass> Done
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
            Done = new ObservableCollection<TestClass>();


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

            Done.Add(new TestClass() { Category = "Globalization", Number = 66 });
            Done.Add(new TestClass() { Category = "Features", Number = 23 });
            Done.Add(new TestClass() { Category = "Content Types", Number = 12 });
            Done.Add(new TestClass() { Category = "Correctness", Number = 94 });
            Done.Add(new TestClass() { Category = "Naming", Number = 45 });
            Done.Add(new TestClass() { Category = "Best Practices", Number = 29 });

            Series.Add(new SeriesData() { DisplayName = "Errors", Items = Errors });
            Series.Add(new SeriesData() { DisplayName = "Warnings", Items = Warnings });
            Series.Add(new SeriesData() { DisplayName = "Warnings", Items = Done });


            this.DataContext = this;

            //this.StackedBarChart.DataContext=this;
        }
    }


}
