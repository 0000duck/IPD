using System.Windows.Controls;
using System.Windows.Data;
using IPD.Component;
using IPD.Tcp;
using Newtonsoft.Json;

namespace IPD.View.Pages.RobotParameter.JointParameter
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        private Client client = Client.GetInstance();
        private Model.RobotParameter robotParameter = Model.RobotParameter.GetInstance;
        private Model.RobotCommon robotCommon = Model.RobotCommon.GetInstance;

        public Index()
        {
            InitializeComponent();
            this.DataContext = robotParameter;
            for (int i = 0; i < robotCommon.RobotType.AxisSum; i++)
            {
                client.SendMessage(0x3b02, "{\"AxisNum\":" + (i + 1).ToString() + "}");
            }
            RenderItems();
        }

        private void RenderItems()
        {
            TabControl tc = new TabControl();
            Converter.StringDoubleConverter stringDoubleConverter = new Converter.StringDoubleConverter();
            Converter.StringIntConverter stringIntConverter = new Converter.StringIntConverter();
            for (int i = 0; i < robotCommon.RobotType.AxisSum; i++)
            {
                TabItem item = new TabItem();
                item.Header = "轴" + (i + 1).ToString();
                item.Width = 60;
                //item.Height = 25;
                JointParameterItem jointParameterItem = new JointParameterItem();
                Binding posSWLimitBinding = new Binding();
                Binding reducRatioBinding = new Binding();
                Binding ratedRotSpeedBinding = new Binding();
                Binding maxRotSpeedBinding = new Binding();
                Binding ratedVelBinding = new Binding();
                Binding maxAccBinding = new Binding();
                Binding directionBinding = new Binding();
                Binding backLashBinding = new Binding();
                Binding negSWLimitBinding = new Binding();
                Binding encoderResolutionBinding = new Binding();
                Binding ratedDeRotSpeedBinding = new Binding();
                Binding maxDeRotSpeedBinding = new Binding();
                Binding deRatedVelBinding = new Binding();
                Binding maxDecelBinding = new Binding();
                Binding axisDirectionBinding = new Binding();
                posSWLimitBinding.Converter = stringDoubleConverter;
                posSWLimitBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.PosSWLimit");
                posSWLimitBinding.Mode = BindingMode.TwoWay;
                reducRatioBinding.Converter = stringDoubleConverter;
                reducRatioBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.ReducRatio");
                reducRatioBinding.Mode = BindingMode.TwoWay;
                ratedRotSpeedBinding.Converter = stringDoubleConverter;
                ratedRotSpeedBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.RatedRotSpeed");
                ratedRotSpeedBinding.Mode = BindingMode.TwoWay;
                maxRotSpeedBinding.Converter = stringDoubleConverter;
                maxRotSpeedBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.MaxRotSpeed");
                maxRotSpeedBinding.Mode = BindingMode.TwoWay;
                ratedVelBinding.Converter = stringDoubleConverter;
                ratedVelBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.RatedVel");
                ratedVelBinding.Mode = BindingMode.TwoWay;
                maxAccBinding.Converter = stringDoubleConverter;
                maxAccBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.MaxAcc");
                maxAccBinding.Mode = BindingMode.TwoWay;
                directionBinding.Converter = stringDoubleConverter;
                directionBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.Direction");
                directionBinding.Mode = BindingMode.TwoWay;
                backLashBinding.Converter = stringDoubleConverter;
                backLashBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.BackLash");
                backLashBinding.Mode = BindingMode.TwoWay;
                negSWLimitBinding.Converter = stringDoubleConverter;
                negSWLimitBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.NegSWLimit");
                negSWLimitBinding.Mode = BindingMode.TwoWay;
                encoderResolutionBinding.Converter = stringDoubleConverter;
                encoderResolutionBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.EncoderResolution");
                encoderResolutionBinding.Mode = BindingMode.TwoWay;
                ratedDeRotSpeedBinding.Converter = stringDoubleConverter;
                ratedDeRotSpeedBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.RatedDeRotSpeed");
                ratedDeRotSpeedBinding.Mode = BindingMode.TwoWay;
                maxDeRotSpeedBinding.Converter = stringDoubleConverter;
                maxDeRotSpeedBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.MaxDeRotSpeed");
                maxDeRotSpeedBinding.Mode = BindingMode.TwoWay;
                deRatedVelBinding.Converter = stringDoubleConverter;
                deRatedVelBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.DeRatedVel");
                deRatedVelBinding.Mode = BindingMode.TwoWay;
                maxDecelBinding.Converter = stringDoubleConverter;
                maxDecelBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.MaxDecel");
                maxDecelBinding.Mode = BindingMode.TwoWay;
                axisDirectionBinding.Converter = stringIntConverter;
                axisDirectionBinding.Path = new System.Windows.PropertyPath("JointList[" + i.ToString() + "].Joint.AxisDirection");
                axisDirectionBinding.Mode = BindingMode.TwoWay;
                jointParameterItem.SetBinding(JointParameterItem.PosSWLimitProperty, posSWLimitBinding);
                jointParameterItem.SetBinding(JointParameterItem.ReducRatioProperty, reducRatioBinding);
                jointParameterItem.SetBinding(JointParameterItem.RatedRotSpeedProperty, ratedRotSpeedBinding);
                jointParameterItem.SetBinding(JointParameterItem.MaxRotSpeedProperty, maxRotSpeedBinding);
                jointParameterItem.SetBinding(JointParameterItem.RatedVelProperty, ratedVelBinding);
                jointParameterItem.SetBinding(JointParameterItem.MaxAccProperty, maxAccBinding);
                jointParameterItem.SetBinding(JointParameterItem.DirectionProperty, directionBinding);
                jointParameterItem.SetBinding(JointParameterItem.BackLashProperty, backLashBinding);
                jointParameterItem.SetBinding(JointParameterItem.NegSWLimitProperty, negSWLimitBinding);
                jointParameterItem.SetBinding(JointParameterItem.EncoderResolutionProperty, encoderResolutionBinding);
                jointParameterItem.SetBinding(JointParameterItem.RatedDeRotSpeedProperty, ratedDeRotSpeedBinding);
                jointParameterItem.SetBinding(JointParameterItem.MaxDeRotSpeedProperty, maxDeRotSpeedBinding);
                jointParameterItem.SetBinding(JointParameterItem.DeRatedVelProperty, deRatedVelBinding);
                jointParameterItem.SetBinding(JointParameterItem.MaxDecelProperty, maxDecelBinding);
                jointParameterItem.SetBinding(JointParameterItem.AxisDirectionProperty, axisDirectionBinding);
                item.Content = jointParameterItem;
                tc.Items.Add(item);
            }
            tabContent.Content = tc;
        }

        private void saveValue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            for (int i = 0; i < robotCommon.RobotType.AxisSum; i++)
            {
                string jsonString = JsonConvert.SerializeObject(robotParameter.JointList[i]);
                client.SendMessage(0x3b01, jsonString);
            }
        }
    }
}