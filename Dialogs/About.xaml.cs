using System.Windows;

namespace IPD.Dialogs
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            DataContext = Model.Versions.GetInstance;
        }
    }
}