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
    /// RobotPosition.xaml 的交互逻辑
    /// </summary>
    public partial class RobotPosition : UserControl
    {
        private Client client = Client.GetInstance();
        private Model.RobotCommon robotCommon = Model.RobotCommon.GetInstance;
        private Model.PosVar posVar = Model.PosVar.GetInstance;

        public RobotPosition()
        {
            InitializeComponent();
            client.SendMessage(0x2a02, "{\"robot\":" + robotCommon.CurrentRobot + ",\"coord\":-1}");
            client.SendMessage(0x5605, "{\"robot\":" + robotCommon.CurrentRobot + ",\"posName\":\"GP001\"}");
            initVisible();
            initPosNumList();
            posNum.SelectedItem = 1;
            this.DataContext = posVar;
            currentPos.DataContext = robotCommon;
        }

        private void initPosNumList()
        {
            for (int i = 1; i <= 999; i++)
            {
                TextBlock tx = new TextBlock();
                tx.Text = "GP" + i.ToString("000");
                posNum.Items.Add(tx);
            }
        }

        private void initVisible()
        {
            if (robotCommon.RobotType.AxisSum < 6)
            {
                morphologyItem.Visibility = Visibility.Collapsed;
                if (robotCommon.RobotType.En.IndexOf("SCARA") == -1)
                {
                    handItem.Visibility = Visibility.Collapsed;
                }
                else
                {
                    handItem.Visibility = Visibility.Visible;
                }
            }
            else
            {
                morphologyItem.Visibility = Visibility.Visible;
                handItem.Visibility = Visibility.Collapsed;
            }
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyData data = new ApplyData();
            List<double> pos = new List<double> { coord.SelectedIndex, coord.SelectedIndex == 0 ? 0 : 1, robotCommon.RobotType.En.IndexOf("SCARA") == -1 ? morphology.SelectedIndex : hand.SelectedIndex, double.Parse(tool.Text), double.Parse(user.Text), 0, 0, double.Parse(j1.Text), double.Parse(j2.Text), double.Parse(j3.Text), double.Parse(j4.Text), double.Parse(j5.Text), double.Parse(j6.Text), double.Parse(j7.Text) };
            data.pos = pos;
            data.note = note.Text;
            data.robot = robotCommon.CurrentRobot;
            data.posName = ((TextBlock)posNum.SelectedItem).Text;
            client.SendMessage(0x5604, JsonConvert.SerializeObject(data));
        }

        private void posNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            client.SendMessage(0x5605, "{\"robot\":" + robotCommon.CurrentRobot + ",\"posName\":\"" + ((TextBlock)posNum.SelectedItem).Text + "\"}");
        }
    }

    public class ApplyData
    {
        public List<double> pos { get; set; }
        public int robot { get; set; }
        public string posName { get; set; }
        public string note { get; set; }
    }
}