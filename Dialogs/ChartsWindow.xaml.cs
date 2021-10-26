using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using IPD.Model;
using IPD.Tcp;
using MahApps.Metro.Controls;

namespace IPD.Dialogs
{
    /// <summary>
    /// ChartsWindowxaml.xaml 的交互逻辑
    /// </summary>
    public partial class ChartsWindow : MetroWindow
    {
        private Charts charts = Charts.GetInstance;
        private Client7000 client = Client7000.GetInstance();
        private RobotCommon robotCommon = RobotCommon.GetInstance;

        public ChartsWindow()
        {
            InitializeComponent();
            this.DataContext = charts;
            reConnect.DataContext = Model.Connect.GetInstance;
            client.Connect();
            setCheckBox();
            checkGrid.IsEnabled = false;
        }

        private void setCheckBox()
        {
            for (int i = 1; i <= robotCommon.RobotType.AxisSum; i++)
            {
                CheckBox ck = new CheckBox();
                ck.Content = "轴" + i.ToString();
                axis.RegisterName("axis" + i.ToString(), ck);
                axis.Items.Add(ck);
            }
        }

        private void sendRequest_Click(object sender, RoutedEventArgs e)
        {
            charts.ClearValues();
            sendRequest.IsEnabled = false;
            checkGrid.IsEnabled = false;
            string data = "{\"channel\": 1,\"stop\": 0,\"robot\": " + robotCommon.CurrentRobot.ToString() + ",\"mode\": 1,\"interval\": " + cycle.Text + ",\"queryType\": [\"realPosACS\", \"realPosMCS\", \"realPosPCS\", \"realPosUCS\",\"axisVel\", \"axisAcc\",\"torque\", \"electric\"],\"typeCfg\": {}}";
            client.SendMessage(0x9512, data);
            System.Timers.Timer requestTimer = new System.Timers.Timer();
            requestTimer.Interval = int.Parse(cycle.Text) * int.Parse(num.Text);
            requestTimer.AutoReset = false;
            requestTimer.Elapsed += new System.Timers.ElapsedEventHandler(request);
            requestTimer.Enabled = true;
        }

        private void request(object source, System.Timers.ElapsedEventArgs e)
        {
            string data = "{\"channel\": 1,\"stop\": 1,\"robot\": " + robotCommon.CurrentRobot.ToString() + ",\"mode\": 1,\"interval\": 600,\"queryType\": [\"realPosACS\", \"realPosMCS\", \"realPosPCS\", \"realPosUCS\",\"axisVel\", \"axisAcc\",\"torque\", \"electric\"],\"typeCfg\": {}}";
            client.SendMessage(0x9512, data);
            Action<Button, bool> setRequestButtonEnable = new Action<Button, bool>(updateRequestIsEnabled);
            sendRequest.Dispatcher.Invoke(setRequestButtonEnable, sendRequest, true);
            Action<Grid, bool> setCheckGridEnable = new Action<Grid, bool>(updateCheckGridIsEnabled);
            sendRequest.Dispatcher.Invoke(setCheckGridEnable, checkGrid, true);
        }

        private void updateRequestIsEnabled(Button bt, bool isEnabled)
        {
            sendRequest.IsEnabled = isEnabled;
        }

        private void updateCheckGridIsEnabled(Grid gd, bool isEnabled)
        {
            checkGrid.IsEnabled = isEnabled;
        }

        private void cycleAndNumTextChanged(object sender, TextChangedEventArgs e)
        {
            int cycleInt;
            int numInt;
            try
            {
                if (cycle == null || num == null || wholeTime == null) { return; }
                cycleInt = int.Parse(cycle.Text);
                numInt = int.Parse(num.Text);
                maxNum.Text = "10~" + (20000 / cycleInt).ToString();
                wholeTime.Text = (Convert.ToDouble(cycleInt * numInt) / 1000).ToString("0.00");
            }
            catch
            {
                wholeTime.Text = "输入错误！";
            }
        }

        private void cycleAndNumLostFocus(object sender, RoutedEventArgs e)
        {
            int cycleInt;
            int numInt;
            try
            {
                if (cycle == null || num == null || wholeTime == null) { return; }
                cycleInt = int.Parse(cycle.Text);
                numInt = int.Parse(num.Text);
                if (numInt >= (20000 / cycleInt))
                {
                    num.Text = (20000 / cycleInt).ToString();
                }
                else if (numInt < 10)
                {
                    num.Text = 10.ToString();
                }
            }
            catch
            {
                wholeTime.Text = "输入错误！";
            }
        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
            List<int> axisList = new List<int>();
            string typeString = null;
            for (int i = 0; i < type.Items.Count; i++)
            {
                RadioButton rd = (RadioButton)type.Items.GetItemAt(i);
                if (rd.IsChecked == true)
                {
                    typeString = rd.Name;
                    break;
                }
            }
            for (int i = 1; i <= robotCommon.RobotType.AxisSum; i++)
            {
                CheckBox ck = (CheckBox)type.FindName("axis" + i.ToString());
                if (ck.IsChecked == true)
                {
                    axisList.Add(i);
                }
            }
            if (axisList.Count == 0 || typeString == null)
            {
                MessageBox.Show("至少选择一个轴或类型！");
                return;
            }
            charts.ChangeCharts(axisList, typeString);
        }

        private void type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((RadioButton)type.SelectedItem).IsChecked = true;
        }

        private void axis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((CheckBox)axis.SelectedItem).IsChecked = !((CheckBox)axis.SelectedItem).IsChecked;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (client.clientBase.clientChannel != null && client.clientBase.clientChannel.Active)
            {
                client.clientBase.clientChannel.CloseAsync();
            };
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (client.clientBase.clientChannel != null && client.clientBase.clientChannel.Active)
            {
                client.clientBase.clientChannel.CloseAsync();
            };
        }
    }
}