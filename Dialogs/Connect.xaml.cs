using System.Windows;
using System.Windows.Controls;
using IPD.Tcp;
using IPD.Common;
using IPD.Util;
using System.Collections.Generic;
using System.Globalization;

namespace IPD.Dialogs
{
    /// <summary>
    /// Connect.xaml 的交互逻辑
    /// </summary>
    public partial class Connect : Window
    {
        private Client client = Client.GetInstance();
        private Model.Connect connect = Model.Connect.GetInstance;
        private Config config = Config.GetInstance();
        private List<ConnectParameter> connectParameters = new List<ConnectParameter>();

        public Connect()
        {
            InitializeComponent();
            this.DataContext = connect;
            connectParameters = config.ConnectParameterList();
            renderConnectedList();
        }

        private void renderConnectedList()
        {
            connectParameters.ForEach(v =>
            {
                Grid grid = new Grid();
                ColumnDefinition columnDefinition1 = new ColumnDefinition();
                ColumnDefinition columnDefinition2 = new ColumnDefinition();
                columnDefinition1.Width = (GridLength)new GridLengthConverter().ConvertFromString("120");
                columnDefinition2.Width = (GridLength)new GridLengthConverter().ConvertFromString("*");
                grid.ColumnDefinitions.Add(columnDefinition1);
                grid.ColumnDefinitions.Add(columnDefinition2);
                TextBlock textBlock1 = new TextBlock();
                TextBlock textBlock2 = new TextBlock();
                textBlock1.Text = v.ip;
                textBlock2.Text = v.port.ToString();
                Grid.SetColumn(textBlock1, 0);
                Grid.SetColumn(textBlock2, 1);
                grid.Children.Add(textBlock1);
                grid.Children.Add(textBlock2);
                connectedList.Items.Add(grid);
            });
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            client.Connect(IP.Text, int.Parse(Port.Text, CultureInfo.InvariantCulture));
        }

        private void connectedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = connectedList.SelectedIndex;
            IP.Text = connectParameters[selectedIndex].ip;
            Port.Text = connectParameters[selectedIndex].port.ToString();
        }
    }
}