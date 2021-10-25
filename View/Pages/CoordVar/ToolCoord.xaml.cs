using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using IPD.Tcp;

namespace IPD.View.Pages.CoordVar
{
    /// <summary>
    /// ToolCoord.xaml 的交互逻辑
    /// </summary>
    public partial class ToolCoord : UserControl
    {
        private Client client = Client.GetInstance();
        private Model.ToolParameter toolParameter = Model.ToolParameter.GetInstance;
        private Model.RobotCommon robotCommon = Model.RobotCommon.GetInstance;

        public ToolCoord()
        {
            InitializeComponent();
            this.DataContext = toolParameter;
            switchTool.DataContext = robotCommon;
            client.SendMessage(0x380b, "{\"robot\":" + robotCommon.CurrentRobot.ToString() + "}");
            setList();
        }

        private void setList()
        {
            for (int i = 0; i <= 999; i++)
            {
                TextBlock tx = new TextBlock();
                tx.Text = i.ToString();
                toolNum.Items.Add(tx);
            }
        }

        private void toolNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (toolNum.SelectedIndex == 0)
            {
                parameter.IsEnabled = false;
            }
            else
            {
                parameter.IsEnabled = true;
                client.SendMessage(0x3806, "{\"toolNum\":" + toolNum.SelectedIndex.ToString() + "}");
            }
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            string data = JsonConvert.SerializeObject(toolParameter.Tool);
            client.SendMessage(0x3805, data);
        }

        private void switchTool_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage(0x380a, "{\"robot\":" + robotCommon.CurrentRobot.ToString() + ",\"curToolNum\":" + toolNum.SelectedIndex.ToString() + "}");
            robotCommon.CurToolNum.curToolNum = toolNum.SelectedIndex;
        }
    }
}