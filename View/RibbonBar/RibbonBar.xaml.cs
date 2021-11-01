using System.Windows;
using System.Windows.Controls;
using IPD.Dialogs;
using IPD;

namespace IPD.View.RibbonBar
{
    /// <summary>
    /// RibbonBar.xaml 的交互逻辑
    /// </summary>
    public partial class RibbonBar : UserControl
    {
        public delegate void ChangeContentHandler(UserControl userControl);

        public static event ChangeContentHandler ChangeContentEvent;

        private readonly Tcp.Client client = Tcp.Client.GetInstance();

        public RibbonBar()
        {
            this.DataContext = Model.Connect.GetInstance;
            InitializeComponent();
            robotSwitch.DataContext = Model.RobotCommon.GetInstance;
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            Connect connectDialog = new Connect();
            connectDialog.ShowDialog();
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            IPD.Tcp.Client client = IPD.Tcp.Client.GetInstance();
            client.DisConnect();
        }

        private void configurationParameter_Click(object sender, RoutedEventArgs e)
        {
            ChangeContentEvent.Invoke(new Pages.RobotParameter.Index());
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            About aboutDialog = new About();
            aboutDialog.ShowDialog();
        }

        private void switchRobot(int robot)
        {
            client.SendMessage(0x5001, "{\"mode\":0,\"robot\":" + robot.ToString() + "}");
        }

        private void robotSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switchRobot(robotSwitch.SelectedIndex + 1);
        }

        private void oscilloscope_Click(object sender, RoutedEventArgs e)
        {
            ChartsWindow chartsWindow = new ChartsWindow();
            chartsWindow.Show();
        }

        private void toolFrame_Click(object sender, RoutedEventArgs e)
        {
            ChangeContentEvent.Invoke(new Pages.CoordVar.ToolCoord());
        }

        private void GlobalPosition_Click(object sender, RoutedEventArgs e)
        {
            ChangeContentEvent.Invoke(new Pages.CoordVar.RobotPosition());
        }

        private void opcua_Click(object sender, RoutedEventArgs e)
        {
            OpcUaWindow opcUaWindow = new OpcUaWindow();
            opcUaWindow.Show();
        }
    }
}