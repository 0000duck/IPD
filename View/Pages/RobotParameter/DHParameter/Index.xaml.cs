using System.Windows.Controls;
using System.Windows.Data;
using IPD.Model;
using IPD.Tcp;
using Newtonsoft.Json;

namespace IPD.View.Pages.RobotParameter.DHParameter
{
    /// <summary>
    /// DhParameter.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        private Client client = Client.GetInstance();
        private RobotCommon robotCommon = RobotCommon.GetInstance;
        private Model.RobotParameter robotParameter = Model.RobotParameter.GetInstance;

        public Index()
        {
            InitializeComponent();
            this.DataContext = robotParameter;
            client.SendMessage(0x3a02, "{}");
            viewInit();
        }

        private void saveValue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int num = robotCommon.RobotType.Num;
            if (num == 15)
            {
                string sprayJsonString = JsonConvert.SerializeObject(robotParameter.DhParameterSpray);
                client.SendMessage(0x3a01, sprayJsonString);
            }
            else if (num == 2 || num == 9 || num == 13)
            {
                string scaraJsonString = JsonConvert.SerializeObject(robotParameter.DhParameterScara);
                client.SendMessage(0x3a01, scaraJsonString);
            }
            else
            {
                string jsonString = JsonConvert.SerializeObject(robotParameter.DhParameter);
                client.SendMessage(0x3a01, jsonString);
            }
        }

        private void set6sBinding()
        {
            //Converter
            Converter.StringDoubleConverter converter = new Converter.StringDoubleConverter();
            Converter.StringBoolConverter boolConverter = new Converter.StringBoolConverter();
            // L1 Binding
            Binding l1Binding = new Binding();
            l1Binding.Converter = converter;
            l1Binding.Path = new System.Windows.PropertyPath("DhParameter.Link[0].d");
            l1Binding.Mode = BindingMode.TwoWay;
            L1Value.SetBinding(TextBox.TextProperty, l1Binding);
            //L2 Binding
            Binding l2Binding = new Binding();
            l2Binding.Converter = converter;
            l2Binding.Path = new System.Windows.PropertyPath("DhParameter.Link[1].a");
            l2Binding.Mode = BindingMode.TwoWay;
            L2Value.SetBinding(TextBox.TextProperty, l2Binding);
            //L3 Binding
            Binding l3Binding = new Binding();
            l3Binding.Converter = converter;
            l3Binding.Path = new System.Windows.PropertyPath("DhParameter.Link[2].a");
            l3Binding.Mode = BindingMode.TwoWay;
            L3Value.SetBinding(TextBox.TextProperty, l3Binding);
            //L4 Binding
            Binding l4Binding = new Binding();
            l4Binding.Converter = converter;
            l4Binding.Path = new System.Windows.PropertyPath("DhParameter.Link[3].d");
            l4Binding.Mode = BindingMode.TwoWay;
            L4Value.SetBinding(TextBox.TextProperty, l4Binding);
            //L5 Binding
            Binding l5Binding = new Binding();
            l5Binding.Converter = converter;
            l5Binding.Path = new System.Windows.PropertyPath("DhParameter.Link[5].d");
            l5Binding.Mode = BindingMode.TwoWay;
            L5Value.SetBinding(TextBox.TextProperty, l5Binding);
            //L6 Binding
            Binding l6Binding = new Binding();
            l6Binding.Converter = converter;
            l6Binding.Path = new System.Windows.PropertyPath("DhParameter.Link[0].a");
            l6Binding.Mode = BindingMode.TwoWay;
            L6Value.SetBinding(TextBox.TextProperty, l6Binding);
            //L7 Binding
            Binding l7Binding = new Binding();
            l7Binding.Converter = converter;
            l7Binding.Path = new System.Windows.PropertyPath("DhParameter.Link[2].d");
            l7Binding.Mode = BindingMode.TwoWay;
            L7Value.SetBinding(TextBox.TextProperty, l7Binding);
            //L8 Visible
            L8Item.Visibility = System.Windows.Visibility.Collapsed;
            //theta Binding
            Binding thetaBinding = new Binding();
            thetaBinding.Converter = converter;
            thetaBinding.Path = new System.Windows.PropertyPath("DhParameter.Link[4].theta");
            thetaBinding.Mode = BindingMode.TwoWay;
            thetaValue.SetBinding(TextBox.TextProperty, thetaBinding);
            //upsideDown Binding
            Binding upsideDownBinding = new Binding();
            upsideDownBinding.Converter = boolConverter;
            upsideDownBinding.Path = new System.Windows.PropertyPath("DhParameter.upsideDown");
            upsideDownBinding.Mode = BindingMode.TwoWay;
            upsideDownValue.SetBinding(TextBox.TextProperty, upsideDownBinding);
            pitchItem.Visibility = System.Windows.Visibility.Collapsed;
            dynamicMaxItem.Visibility = System.Windows.Visibility.Collapsed;
            dynamicMinItem.Visibility = System.Windows.Visibility.Collapsed;

            //CoupleCoe1/2 Binding
            Binding coupleCoe12Binding = new Binding();
            coupleCoe12Binding.Converter = converter;
            coupleCoe12Binding.Path = new System.Windows.PropertyPath("DhParameter.CoupleCoe.Couple_Coe_1_2");
            coupleCoe12Binding.Mode = BindingMode.TwoWay;
            coupleCoe12Value.SetBinding(TextBox.TextProperty, coupleCoe12Binding);
            //CoupleCoe2/3 Binding
            Binding coupleCoe23Binding = new Binding();
            coupleCoe23Binding.Converter = converter;
            coupleCoe23Binding.Path = new System.Windows.PropertyPath("DhParameter.CoupleCoe.Couple_Coe_2_3");
            coupleCoe23Binding.Mode = BindingMode.TwoWay;
            coupleCoe23Value.SetBinding(TextBox.TextProperty, coupleCoe23Binding);
            //CoupleCoe3/2 Binding
            Binding coupleCoe32Binding = new Binding();
            coupleCoe32Binding.Converter = converter;
            coupleCoe32Binding.Path = new System.Windows.PropertyPath("DhParameter.CoupleCoe.Couple_Coe_3_2");
            coupleCoe32Binding.Mode = BindingMode.TwoWay;
            coupleCoe32Value.SetBinding(TextBox.TextProperty, coupleCoe32Binding);
            //CoupleCoe3/4 Binding
            Binding coupleCoe34Binding = new Binding();
            coupleCoe34Binding.Converter = converter;
            coupleCoe34Binding.Path = new System.Windows.PropertyPath("DhParameter.CoupleCoe.Couple_Coe_3_4");
            coupleCoe34Binding.Mode = BindingMode.TwoWay;
            coupleCoe34Value.SetBinding(TextBox.TextProperty, coupleCoe34Binding);
            //CoupleCoe4/5 Binding
            Binding coupleCoe45Binding = new Binding();
            coupleCoe45Binding.Converter = converter;
            coupleCoe45Binding.Path = new System.Windows.PropertyPath("DhParameter.CoupleCoe.Couple_Coe_4_5");
            coupleCoe45Binding.Mode = BindingMode.TwoWay;
            coupleCoe45Value.SetBinding(TextBox.TextProperty, coupleCoe45Binding);
            //CoupleCoe4/6 Binding
            Binding coupleCoe46Binding = new Binding();
            coupleCoe46Binding.Converter = converter;
            coupleCoe46Binding.Path = new System.Windows.PropertyPath("DhParameter.CoupleCoe.Couple_Coe_4_6");
            coupleCoe46Binding.Mode = BindingMode.TwoWay;
            coupleCoe46Value.SetBinding(TextBox.TextProperty, coupleCoe46Binding);
            //CoupleCoe5/6 Binding
            Binding coupleCoe56Binding = new Binding();
            coupleCoe56Binding.Converter = converter;
            coupleCoe56Binding.Path = new System.Windows.PropertyPath("DhParameter.CoupleCoe.Couple_Coe_5_6");
            coupleCoe56Binding.Mode = BindingMode.TwoWay;
            coupleCoe56Value.SetBinding(TextBox.TextProperty, coupleCoe56Binding);
        }

        private void setScaraBinding()
        {
            //Converter
            Converter.StringDoubleConverter converter = new Converter.StringDoubleConverter();
            Converter.StringIntConverter intConverter = new Converter.StringIntConverter();
            Converter.StringBoolConverter boolConverter = new Converter.StringBoolConverter();
            // L1 Binding
            Binding l1Binding = new Binding();
            l1Binding.Converter = converter;
            l1Binding.Path = new System.Windows.PropertyPath("DhParameterScara.Link[0].a");
            l1Binding.Mode = BindingMode.TwoWay;
            L1Value.SetBinding(TextBox.TextProperty, l1Binding);
            //L2 Binding
            Binding l2Binding = new Binding();
            l2Binding.Converter = converter;
            l2Binding.Path = new System.Windows.PropertyPath("DhParameterScara.Link[0].d");
            l2Binding.Mode = BindingMode.TwoWay;
            L2Value.SetBinding(TextBox.TextProperty, l2Binding);
            //L3 Binding
            Binding l3Binding = new Binding();
            l3Binding.Converter = converter;
            l3Binding.Path = new System.Windows.PropertyPath("DhParameterScara.Link[1].a");
            l3Binding.Mode = BindingMode.TwoWay;
            L3Value.SetBinding(TextBox.TextProperty, l3Binding);
            //L4 Binding
            Binding l4Binding = new Binding();
            l4Binding.Converter = converter;
            l4Binding.Path = new System.Windows.PropertyPath("DhParameterScara.Link[2].d");
            l4Binding.Mode = BindingMode.TwoWay;
            L4Value.SetBinding(TextBox.TextProperty, l4Binding);
            //Pitch Binding
            Binding pitchBinding = new Binding();
            pitchBinding.Converter = intConverter;
            pitchBinding.Path = new System.Windows.PropertyPath("DhParameterScara.pitch");
            pitchBinding.Mode = BindingMode.TwoWay;
            pitchValue.SetBinding(TextBox.TextProperty, pitchBinding);
            //Visibility
            L5Item.Visibility = System.Windows.Visibility.Collapsed;
            L6Item.Visibility = System.Windows.Visibility.Collapsed;
            L7Item.Visibility = System.Windows.Visibility.Collapsed;
            L8Item.Visibility = System.Windows.Visibility.Collapsed;
            thetaItem.Visibility = System.Windows.Visibility.Collapsed;
            dynamicMaxItem.Visibility = System.Windows.Visibility.Collapsed;
            dynamicMinItem.Visibility = System.Windows.Visibility.Collapsed;
            coupleCoe45Item.Visibility = System.Windows.Visibility.Collapsed;
            coupleCoe46Item.Visibility = System.Windows.Visibility.Collapsed;
            coupleCoe56Item.Visibility = System.Windows.Visibility.Collapsed;
            //upsideDown Binding
            Binding upsideDownBinding = new Binding();
            upsideDownBinding.Converter = boolConverter;
            upsideDownBinding.Path = new System.Windows.PropertyPath("DhParameterScara.upsideDown");
            upsideDownBinding.Mode = BindingMode.TwoWay;
            upsideDownValue.SetBinding(TextBox.TextProperty, upsideDownBinding);
            //CoupleCoe1/2 Binding
            Binding coupleCoe12Binding = new Binding();
            coupleCoe12Binding.Converter = converter;
            coupleCoe12Binding.Path = new System.Windows.PropertyPath("DhParameterScara.CoupleCoe.Couple_Coe_1_2");
            coupleCoe12Binding.Mode = BindingMode.TwoWay;
            coupleCoe12Value.SetBinding(TextBox.TextProperty, coupleCoe12Binding);
            //CoupleCoe2/3 Binding
            Binding coupleCoe23Binding = new Binding();
            coupleCoe23Binding.Converter = converter;
            coupleCoe23Binding.Path = new System.Windows.PropertyPath("DhParameterScara.CoupleCoe.Couple_Coe_2_3");
            coupleCoe23Binding.Mode = BindingMode.TwoWay;
            coupleCoe23Value.SetBinding(TextBox.TextProperty, coupleCoe23Binding);
            //CoupleCoe3/2 Binding
            Binding coupleCoe32Binding = new Binding();
            coupleCoe32Binding.Converter = converter;
            coupleCoe32Binding.Path = new System.Windows.PropertyPath("DhParameterScara.CoupleCoe.Couple_Coe_3_2");
            coupleCoe32Binding.Mode = BindingMode.TwoWay;
            coupleCoe32Value.SetBinding(TextBox.TextProperty, coupleCoe32Binding);
            //CoupleCoe3/4 Binding
            Binding coupleCoe34Binding = new Binding();
            coupleCoe34Binding.Converter = converter;
            coupleCoe34Binding.Path = new System.Windows.PropertyPath("DhParameterScara.CoupleCoe.Couple_Coe_3_4");
            coupleCoe34Binding.Mode = BindingMode.TwoWay;
            coupleCoe34Value.SetBinding(TextBox.TextProperty, coupleCoe34Binding);
        }

        private void setSprayBinding()
        {
            //Converter
            Converter.StringDoubleConverter converter = new Converter.StringDoubleConverter();
            Converter.StringIntConverter intConverter = new Converter.StringIntConverter();
            Converter.StringBoolConverter boolConverter = new Converter.StringBoolConverter();
            // L1 Binding
            Binding l1Binding = new Binding();
            l1Binding.Converter = converter;
            l1Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.Link.L1");
            l1Binding.Mode = BindingMode.TwoWay;
            L1Value.SetBinding(TextBox.TextProperty, l1Binding);
            //L2 Binding
            Binding l2Binding = new Binding();
            l2Binding.Converter = converter;
            l2Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.Link.L2");
            l2Binding.Mode = BindingMode.TwoWay;
            L2Value.SetBinding(TextBox.TextProperty, l2Binding);
            //L3 Binding
            Binding l3Binding = new Binding();
            l3Binding.Converter = converter;
            l3Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.Link.L3");
            l3Binding.Mode = BindingMode.TwoWay;
            L3Value.SetBinding(TextBox.TextProperty, l3Binding);
            //L4 Binding
            Binding l4Binding = new Binding();
            l4Binding.Converter = converter;
            l4Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.Link.L4");
            l4Binding.Mode = BindingMode.TwoWay;
            L4Value.SetBinding(TextBox.TextProperty, l4Binding);
            //L5 Binding
            Binding l5Binding = new Binding();
            l5Binding.Converter = converter;
            l5Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.Link.L5");
            l5Binding.Mode = BindingMode.TwoWay;
            L5Value.SetBinding(TextBox.TextProperty, l5Binding);
            //L6 Binding
            Binding l6Binding = new Binding();
            l6Binding.Converter = converter;
            l6Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.Link.L6");
            l6Binding.Mode = BindingMode.TwoWay;
            L6Value.SetBinding(TextBox.TextProperty, l6Binding);
            //L7 Binding
            Binding l7Binding = new Binding();
            l7Binding.Converter = converter;
            l7Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.Link.L7");
            l7Binding.Mode = BindingMode.TwoWay;
            L7Value.SetBinding(TextBox.TextProperty, l7Binding);
            //L8 Binding
            Binding l8Binding = new Binding();
            l8Binding.Converter = intConverter;
            l8Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.Link.L8");
            l8Binding.Mode = BindingMode.TwoWay;
            L8Value.SetBinding(TextBox.TextProperty, l8Binding);
            //theta Visibility
            thetaItem.Visibility = System.Windows.Visibility.Collapsed;
            pitchItem.Visibility = System.Windows.Visibility.Collapsed;
            dynamicMaxItem.Visibility = System.Windows.Visibility.Collapsed;
            dynamicMinItem.Visibility = System.Windows.Visibility.Collapsed;
            //upsideDown Binding
            Binding upsideDownBinding = new Binding();
            upsideDownBinding.Converter = boolConverter;
            upsideDownBinding.Path = new System.Windows.PropertyPath("DhParameterSpray.upsideDown");
            upsideDownBinding.Mode = BindingMode.TwoWay;
            upsideDownValue.SetBinding(TextBox.TextProperty, upsideDownBinding);
            //CoupleCoe1/2 Binding
            Binding coupleCoe12Binding = new Binding();
            coupleCoe12Binding.Converter = converter;
            coupleCoe12Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.CoupleCoe.Couple_Coe_1_2");
            coupleCoe12Binding.Mode = BindingMode.TwoWay;
            coupleCoe12Value.SetBinding(TextBox.TextProperty, coupleCoe12Binding);
            //CoupleCoe2/3 Binding
            Binding coupleCoe23Binding = new Binding();
            coupleCoe23Binding.Converter = converter;
            coupleCoe23Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.CoupleCoe.Couple_Coe_2_3");
            coupleCoe23Binding.Mode = BindingMode.TwoWay;
            coupleCoe23Value.SetBinding(TextBox.TextProperty, coupleCoe23Binding);
            //CoupleCoe3/2 Binding
            Binding coupleCoe32Binding = new Binding();
            coupleCoe32Binding.Converter = converter;
            coupleCoe32Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.CoupleCoe.Couple_Coe_3_2");
            coupleCoe32Binding.Mode = BindingMode.TwoWay;
            coupleCoe32Value.SetBinding(TextBox.TextProperty, coupleCoe32Binding);
            //CoupleCoe3/4 Binding
            Binding coupleCoe34Binding = new Binding();
            coupleCoe34Binding.Converter = converter;
            coupleCoe34Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.CoupleCoe.Couple_Coe_3_4");
            coupleCoe34Binding.Mode = BindingMode.TwoWay;
            coupleCoe34Value.SetBinding(TextBox.TextProperty, coupleCoe34Binding);
            //CoupleCoe4/5 Binding
            Binding coupleCoe45Binding = new Binding();
            coupleCoe45Binding.Converter = converter;
            coupleCoe45Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.CoupleCoe.Couple_Coe_4_5");
            coupleCoe45Binding.Mode = BindingMode.TwoWay;
            coupleCoe45Value.SetBinding(TextBox.TextProperty, coupleCoe45Binding);
            //CoupleCoe4/6 Binding
            Binding coupleCoe46Binding = new Binding();
            coupleCoe46Binding.Converter = converter;
            coupleCoe46Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.CoupleCoe.Couple_Coe_4_6");
            coupleCoe46Binding.Mode = BindingMode.TwoWay;
            coupleCoe46Value.SetBinding(TextBox.TextProperty, coupleCoe46Binding);
            //CoupleCoe5/6 Binding
            Binding coupleCoe56Binding = new Binding();
            coupleCoe56Binding.Converter = converter;
            coupleCoe56Binding.Path = new System.Windows.PropertyPath("DhParameterSpray.CoupleCoe.Couple_Coe_5_6");
            coupleCoe56Binding.Mode = BindingMode.TwoWay;
            coupleCoe56Value.SetBinding(TextBox.TextProperty, coupleCoe56Binding);
        }

        private void viewInit()
        {
            RobotTypes type = robotCommon.RobotType;
            if (type.AxisSum < 5)
            {
                if (type.Num == 2 || type.Num == 9 || type.Num == 13)
                {
                    setScaraBinding();
                }
            }
            else
            {
                if (type.Num == 15)
                {
                    setSprayBinding();
                }
                else
                {
                    set6sBinding();
                }
            }
        }
    }
}