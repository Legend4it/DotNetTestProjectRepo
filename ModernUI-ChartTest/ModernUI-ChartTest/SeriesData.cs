using System.Collections.ObjectModel;

namespace ModernUI_ChartTest
{
    public class SeriesData
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public ObservableCollection<TestClass> Items { get; set; }
    }


}
