using System.Windows.Controls;

namespace IPD.View.Pages.RobotParameter
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        public Index()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)TheListBox.SelectedItem;

            switch (item.Name)
            {
                case "slaveConfig":
                    robotParameterContent.Content = new SlaveConfig.Index();
                    break;

                case "jointParameter":
                    robotParameterContent.Content = new JointParameter.Index();
                    break;

                case "dhParameter":
                    robotParameterContent.Content = new DHParameter.Index();
                    break;
            }
        }
    }
}