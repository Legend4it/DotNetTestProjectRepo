using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    public sealed class ViewModel
    {
        public ObservableCollection<TabItem> Tabs { get; set; }
        public ViewModel()
        {
            Tabs = new ObservableCollection<TabItem>();
            Tabs.Add(new TabItem { Header = "One", Content = "One's content",Background = Brushes.Wheat});
            Tabs.Add(new TabItem { Header = "Two", Content = "Two's content",Background = Brushes.GhostWhite});
        }
    }

    public sealed class TabItem:UserControl
    {
        public string Header { get; set; }
        public string Content { get; set; }

        public TabItem()
        {
        }
    }
}
