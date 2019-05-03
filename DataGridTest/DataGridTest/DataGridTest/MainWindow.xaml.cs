using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataGridTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Category> Categories { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            Categories = new ObservableCollection<Category>()
            {
                new Category(){ ID = 1, Name = "Cat1", Description = "This is Cat1 Desc"},
                new Category(){ ID = 1, Name = "Cat2", Description = "This is Cat2 Desc"},
                new Category(){ ID = 1, Name = "Cat3", Description = "This is Cat3 Desc"},
                new Category(){ ID = 1, Name = "Cat4", Description = "This is Cat4 Desc"}
            };
            Categories.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Categories_CollectionChanged);
            dataGrid1.ItemsSource = Categories;

        }

        private void dataGrid1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.dataGrid1.SelectedIndex == Categories.Count - 1)
            {
                Categories.Add(new Category());
            }
        }

        void Categories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                MessageBox.Show("New Row Added");
            }
        }

        private void DataGrid1_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("OnMouseRightButtonDown");
        }

        private void DataGrid1_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            MessageBox.Show("OnBeginningEdit");
        }

        private void DataGrid1_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            MessageBox.Show("OnRowEditEnding");
        }

    }
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
