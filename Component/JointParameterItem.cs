using System.Windows;
using System.Windows.Controls;

namespace IPD.Component
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IPD.Component"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IPD.Component;assembly=IPD.Component"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:JointParameterItem/>
    ///
    /// </summary>
    public class JointParameterItem : Control
    {
        // Using a DependencyProperty as the backing store for AxisDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AxisDirectionProperty =
            DependencyProperty.Register("AxisDirection", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for BackLash.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackLashProperty =
            DependencyProperty.Register("BackLash", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for DeRatedVel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeRatedVelProperty =
            DependencyProperty.Register("DeRatedVel", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for EncoderResolution.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EncoderResolutionProperty =
            DependencyProperty.Register("EncoderResolution", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for MaxAcc.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxAccProperty =
            DependencyProperty.Register("MaxAcc", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for MaxDecel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxDecelProperty =
            DependencyProperty.Register("MaxDecel", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for MaxDeRotSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxDeRotSpeedProperty =
            DependencyProperty.Register("MaxDeRotSpeed", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for MaxRotSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxRotSpeedProperty =
            DependencyProperty.Register("MaxRotSpeed", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for NegSWLimit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NegSWLimitProperty =
            DependencyProperty.Register("NegSWLimit", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for PosSWLimit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PosSWLimitProperty =
            DependencyProperty.Register("PosSWLimit", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for RatedDeRotSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RatedDeRotSpeedProperty =
            DependencyProperty.Register("RatedDeRotSpeed", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for RatedRotSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RatedRotSpeedProperty =
            DependencyProperty.Register("RatedRotSpeed", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for RatedVel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RatedVelProperty =
            DependencyProperty.Register("RatedVel", typeof(string), typeof(JointParameterItem));

        // Using a DependencyProperty as the backing store for ReducRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReducRatioProperty =
            DependencyProperty.Register("ReducRatio", typeof(string), typeof(JointParameterItem));
        static JointParameterItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JointParameterItem), new FrameworkPropertyMetadata(typeof(JointParameterItem)));
        }

        public string AxisDirection
        {
            get { return (string)GetValue(AxisDirectionProperty); }
            set { SetValue(AxisDirectionProperty, value); }
        }

        public string BackLash
        {
            get { return (string)GetValue(BackLashProperty); }
            set { SetValue(BackLashProperty, value); }
        }

        public string DeRatedVel
        {
            get { return (string)GetValue(DeRatedVelProperty); }
            set { SetValue(DeRatedVelProperty, value); }
        }

        public string Direction
        {
            get { return (string)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public string EncoderResolution
        {
            get { return (string)GetValue(EncoderResolutionProperty); }
            set { SetValue(EncoderResolutionProperty, value); }
        }

        public string MaxAcc
        {
            get { return (string)GetValue(MaxAccProperty); }
            set { SetValue(MaxAccProperty, value); }
        }

        public string MaxDecel
        {
            get { return (string)GetValue(MaxDecelProperty); }
            set { SetValue(MaxDecelProperty, value); }
        }

        public string MaxDeRotSpeed
        {
            get { return (string)GetValue(MaxDeRotSpeedProperty); }
            set { SetValue(MaxDeRotSpeedProperty, value); }
        }

        public string MaxRotSpeed
        {
            get { return (string)GetValue(MaxRotSpeedProperty); }
            set { SetValue(MaxRotSpeedProperty, value); }
        }

        public string NegSWLimit
        {
            get { return (string)GetValue(NegSWLimitProperty); }
            set { SetValue(NegSWLimitProperty, value); }
        }

        public string PosSWLimit
        {
            get { return (string)GetValue(PosSWLimitProperty); }
            set { SetValue(PosSWLimitProperty, value); }
        }

        public string RatedDeRotSpeed
        {
            get { return (string)GetValue(RatedDeRotSpeedProperty); }
            set { SetValue(RatedDeRotSpeedProperty, value); }
        }

        public string RatedRotSpeed
        {
            get { return (string)GetValue(RatedRotSpeedProperty); }
            set { SetValue(RatedRotSpeedProperty, value); }
        }

        public string RatedVel
        {
            get { return (string)GetValue(RatedVelProperty); }
            set { SetValue(RatedVelProperty, value); }
        }

        public string ReducRatio
        {
            get { return (string)GetValue(ReducRatioProperty); }
            set { SetValue(ReducRatioProperty, value); }
        }
    }
}