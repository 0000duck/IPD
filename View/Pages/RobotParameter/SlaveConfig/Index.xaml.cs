using System.Windows.Controls;
using IPD.Tcp;
using IPD.Model;
using IPD.Util;
using IPD.Component;
using System.Collections.Generic;
using System.Windows.Data;
using Newtonsoft.Json;
using System;

namespace IPD.View.Pages.RobotParameter.SlaveConfig
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        private Client client = Client.GetInstance();
        private List<RobotTypes> list = GetRobotType.getRobotTypeList();
        private Model.RobotCommon robotCommon = Model.RobotCommon.GetInstance;
        private Model.RobotParameter robotParameter = Model.RobotParameter.GetInstance;

        private void applyControlCycle(int i)
        {
            int controlCycle;
            switch (i)
            {
                case 0:
                    controlCycle = 1;
                    break;

                case 1:
                    controlCycle = 2;
                    break;

                case 2:
                    controlCycle = 4;
                    break;

                case 3:
                    controlCycle = 8;
                    break;

                default:
                    controlCycle = 1;
                    break;
            }
            client.SendMessage(0x2e07, "{\"controlCycle\":" + controlCycle.ToString() + "}");
        }

        private void applyCycle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            applyControlCycle(eniSelector.SelectedIndex);
        }

        private void changeAxisVisible(int robot)
        {
            StackPanel s = (StackPanel)rootPanel.FindName("robot" + robot.ToString() + "Panel");
            ComboBox robotSelector = (ComboBox)s.FindName("robot" + robot.ToString() + "Selector");
            for (int i = 1; i <= 7; i++)
            {
                ParameterListItem item = (ParameterListItem)s.FindName("robot" + robot.ToString() + "Axis" + i.ToString() + "Item");
                if (i <= list[robotSelector.SelectedIndex].AxisSum)
                {
                    item.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    item.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void changeRobotVisible()
        {
            for (int i = 1; i <= 4; i++)
            {
                StackPanel s = (StackPanel)rootPanel.FindName("robot" + i.ToString() + "Panel");
                if (i <= robotSumSelector.SelectedIndex)
                {
                    s.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    s.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void changeSyncGroupVisible(int robot)
        {
            StackPanel robotPanel = (StackPanel)rootPanel.FindName("robot" + robot.ToString() + "Panel");
            ComboBox robotSyncGroupSumSelector = (ComboBox)robotPanel.FindName("robot" + robot.ToString() + "SyncGroupSumSelector");
            for (int i = 1; i <= 3; i++)
            {
                StackPanel robotSyncGroupPanel = (StackPanel)robotPanel.FindName("robot" + robot.ToString() + "SyncGroup" + i.ToString() + "Panel");
                if (i <= robotSyncGroupSumSelector.SelectedIndex)
                {
                    robotSyncGroupPanel.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    robotSyncGroupPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void changeSyncGroupAxisVisible(int robot, int group)
        {
            StackPanel robotPanel = (StackPanel)rootPanel.FindName("robot" + robot.ToString() + "Panel");
            StackPanel robotSyncGroupPanel = (StackPanel)robotPanel.FindName("robot" + robot.ToString() + "SyncGroup" + group.ToString() + "Panel");
            ComboBox robotSyncGroupTypeSelector = (ComboBox)robotSyncGroupPanel.FindName("robot" + robot.ToString() + "SyncGroup" + group.ToString() + "Selector");
            int sum = robotSyncGroupTypeSelector.SelectedIndex == 2 ? 2 : 1;
            for (int i = 1; i <= 2; i++)
            {
                ParameterListItem robotSyncGroupAxisItem = (ParameterListItem)robotSyncGroupPanel.FindName("robot" + robot.ToString() + "SyncGroup" + group.ToString() + "Axis" + i.ToString() + "Item");
                if (i <= sum)
                {
                    robotSyncGroupAxisItem.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    robotSyncGroupAxisItem.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void initConfig()
        {
            List<string> robotTypeString = new List<string>();
            list.ForEach(v =>
            {
                robotTypeString.Add(v.Name);
            });
            robot1Selector.ItemsSource = robotTypeString;
            robot2Selector.ItemsSource = robotTypeString;
            robot3Selector.ItemsSource = robotTypeString;
            robot4Selector.ItemsSource = robotTypeString;

            for (int i = 1; i <= 4; i++)
            {
                //机器人的面板
                StackPanel robotPanel = (StackPanel)rootPanel.FindName("robot" + i.ToString() + "Panel");
                RobotTypes robot = new RobotTypes();
                //机器人类型
                if (i <= robotCommon.RobotSum)
                {
                    for (int o = 0; o < list.Count; o++)
                    {
                        if (list[o].En == robotParameter.SlaveConfig.robot[i - 1].robotType)
                        {
                            robot = list[o];
                        }
                    }
                    robotPanel.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    robotPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                //机器人类型
                ComboBox rts = (ComboBox)robotPanel.FindName("robot" + i.ToString() + "Selector");
                rts.SelectedIndex = robot.Num;
                Binding axisSelectorBinding = new Binding();
                axisSelectorBinding.Path = new System.Windows.PropertyPath("SlaveSelectorList");
                //单轴
                for (int o = 1; o <= 7; o++)
                {
                    ComboBox axisSelector = (ComboBox)robotPanel.FindName("robot" + i.ToString() + "Axis" + o.ToString() + "Selector");
                    axisSelector.SetBinding(ComboBox.ItemsSourceProperty, axisSelectorBinding);
                    axisSelector.DisplayMemberPath = "Type";
                    axisSelector.SelectedValuePath = "Num";
                    // 轴的显示与否和轴的选项
                    if (i <= robotCommon.RobotSum)
                    {
                        ParameterListItem listItem = (ParameterListItem)robotPanel.FindName("robot" + i.ToString() + "Axis" + o.ToString() + "Item");
                        if (o <= robot.AxisSum)
                        {
                            axisSelector.SelectedIndex = robotParameter.SlaveConfig.robot[i - 1].servoMap[o - 1];
                            listItem.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            listItem.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
                //附加轴
                RobotItem robotItem = new RobotItem();
                ComboBox robotSyncGroupSumSelector = (ComboBox)robotPanel.FindName("robot" + i.ToString() + "SyncGroupSumSelector");
                //附加轴总数
                if (i <= robotCommon.RobotSum)
                {
                    robotItem = robotParameter.SlaveConfig.robot[i - 1];
                    robotSyncGroupSumSelector.SelectedIndex = robotItem.syncGroupSum;
                }
                //附加轴组
                for (int m = 1; m <= 3; m++)
                {
                    StackPanel robotSyncGroupPanel = (StackPanel)robotPanel.FindName("robot" + i.ToString() + "SyncGroup" + m.ToString() + "Panel");
                    //组面板可见性与组类型选项
                    if (i <= robotCommon.RobotSum && m <= robotItem.syncGroupSum)
                    {
                        robotSyncGroupPanel.Visibility = System.Windows.Visibility.Visible;
                        ComboBox robotSyncGroupTypeSelector = (ComboBox)robotSyncGroupPanel.FindName("robot" + i.ToString() + "SyncGroup" + m.ToString() + "Selector");
                        robotSyncGroupTypeSelector.SelectedIndex = robotItem.syncType[m - 1];
                    }
                    else
                    {
                        robotSyncGroupPanel.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    //附加轴组的轴
                    for (int n = 1; n <= 2; n++)
                    {
                        ComboBox robotSyncGroupAxisSelector = (ComboBox)robotSyncGroupPanel.FindName("robot" + i.ToString() + "SyncGroup" + m.ToString() + "Axis" + n.ToString() + "Selector");
                        robotSyncGroupAxisSelector.SetBinding(ComboBox.ItemsSourceProperty, axisSelectorBinding);
                        robotSyncGroupAxisSelector.DisplayMemberPath = "Type";
                        robotSyncGroupAxisSelector.SelectedValuePath = "Num";
                        //轴的可见性与选项
                        if (i <= robotCommon.RobotSum && m <= robotItem.syncGroupSum)
                        {
                            ParameterListItem robotSyncGroupAxisItem = (ParameterListItem)robotSyncGroupPanel.FindName("robot" + i.ToString() + "SyncGroup" + m.ToString() + "Axis" + n.ToString() + "Item");
                            if (n <= robotItem.syncMap[m - 1].Count)
                            {
                                robotSyncGroupAxisItem.Visibility = System.Windows.Visibility.Visible;
                                robotSyncGroupAxisSelector.SelectedIndex = robotItem.syncMap[m - 1][n - 1];
                            }
                            else
                            {
                                robotSyncGroupAxisItem.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }
                    }
                }
            }
        }

        private void inquireParameter()
        {
            client.SendMessage(0x2e08, "");
            client.SendMessage(0x2e1b, "");
            client.SendMessage(0x2e0e, "");
        }

        private void robot1Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeAxisVisible(1);
        }

        private void robot2Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeAxisVisible(2);
        }

        private void robot3Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeAxisVisible(3);
        }

        private void robot4Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeAxisVisible(4);
        }

        private void robotSumSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeRobotVisible();
        }

        private void disPatcherOfShutDown(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("vkljfiouhfiocpjdsiofhsdihvcxlb");
        }

        public Index()
        {
            InitializeComponent();
            this.DataContext = robotParameter;
            robotSumSelector.DataContext = robotCommon;
            inquireParameter();
            initConfig();
        }

        private void robot1SyncGroupSumSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupVisible(1);
        }

        private void robot1SyncGroup1Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(1, 1);
        }

        private void robot1SyncGroup2Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(1, 2);
        }

        private void robot1SyncGroup3Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(1, 3);
        }

        private void robot2SyncGroupSumSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupVisible(2);
        }

        private void robot2SyncGroup1Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(2, 1);
        }

        private void robot2SyncGroup2Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(2, 2);
        }

        private void robot2SyncGroup3Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(2, 3);
        }

        private void robot3SyncGroupSumSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupVisible(3);
        }

        private void robot3SyncGroup1Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(3, 1);
        }

        private void robot3SyncGroup2Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(3, 2);
        }

        private void robot3SyncGroup3Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(3, 3);
        }

        private void robot4SyncGroupSumSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupVisible(4);
        }

        private void robot4SyncGroup1Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(4, 1);
        }

        private void robot4SyncGroup2Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(4, 2);
        }

        private void robot4SyncGroup3Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSyncGroupAxisVisible(4, 3);
        }

        private void applySlaveConfig_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SlaveConigApplyParameter slaveConigApplyParameter = new SlaveConigApplyParameter();
            slaveConigApplyParameter.robot = new List<RobotItem>();
            slaveConigApplyParameter.sum = robotSumSelector.SelectedIndex;
            for (int i = 1; i <= robotSumSelector.SelectedIndex; i++)
            {
                RobotItem robot = new RobotItem();
                string robotNum = "robot" + i.ToString();
                StackPanel robotPanel = (StackPanel)rootPanel.FindName(robotNum + "Panel");
                ComboBox robotSelector = (ComboBox)robotPanel.FindName(robotNum + "Selector");
                robot.robotType = list[robotSelector.SelectedIndex].En;
                robot.servoMap = new List<int>();
                robot.syncMap = new List<List<int>>();
                robot.syncType = new List<int>();
                //机器人轴
                for (int m = 1; m <= list[robotSelector.SelectedIndex].AxisSum; m++)
                {
                    ComboBox robotAxisSelector = (ComboBox)robotPanel.FindName(robotNum + "Axis" + m.ToString() + "Selector");
                    robot.servoMap.Add(robotAxisSelector.SelectedIndex);
                }
                //附加轴
                //附加轴组数
                ComboBox robotSyncGroupSumSelector = (ComboBox)robotPanel.FindName(robotNum + "SyncGroupSumSelector");
                int syncGroupSum = robotSyncGroupSumSelector.SelectedIndex;
                robot.syncGroupSum = syncGroupSum;
                if (syncGroupSum == 0)
                {
                    robot.syncMap = new List<List<int>>();
                    robot.syncSum = 0;
                    robot.syncType = new List<int> { 0, 0, 0 };
                }
                else
                {
                    robot.syncSum = 0;
                    for (int m = 1; m <= syncGroupSum; m++)
                    {
                        StackPanel robotSyncGroupPanel = (StackPanel)robotPanel.FindName(robotNum + "SyncGroup" + m.ToString() + "Panel");
                        ComboBox robotSyncGroupTypeSelector = (ComboBox)robotSyncGroupPanel.FindName(robotNum + "SyncGroup" + m.ToString() + "Selector");
                        robot.syncType.Add(robotSyncGroupTypeSelector.SelectedIndex);
                        int syncSum;
                        if (robotSyncGroupTypeSelector.SelectedIndex == 2) { syncSum = 2; } else { syncSum = 1; }
                        robot.syncSum = syncSum;
                        List<int> robotSyncGroupAxisSelection = new List<int>();
                        for (int n = 1; n <= syncSum; n++)
                        {
                            ComboBox robotSyncGroupAxisSelector = (ComboBox)robotSyncGroupPanel.FindName(robotNum + "SyncGroup" + m.ToString() + "Axis" + n.ToString() + "Selector");
                            robotSyncGroupAxisSelection.Add(robotSyncGroupAxisSelector.SelectedIndex);
                        }
                        robot.syncMap.Add(robotSyncGroupAxisSelection);
                    }
                }
                slaveConigApplyParameter.robot.Add(robot);
            }
            string dataString = JsonConvert.SerializeObject(slaveConigApplyParameter);
            client.SendMessage(0x2e14, dataString);
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            client.SendMessage(0x2e15, "{}");
        }
    }

    public class SlaveConigApplyParameter
    {
        public List<RobotItem> robot { get; set; }
        public int sum { get; set; }
    }
}