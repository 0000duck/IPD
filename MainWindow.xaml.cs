using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using IPD.View.Pages;
using IPD.View.RibbonBar;
using IPD.Util;
using IPD.Dialogs;
using XmlLogLibrary;
using System.Collections.Generic;
using System;

namespace IPD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow instance = null;
        private readonly Tcp.Client client = Tcp.Client.GetInstance();
        private Config config = Config.GetInstance();
        private XmlLog log = XmlLog.Instance;

        public MainWindow()
        {
            InitializeComponent();
            mainContent.Content = new InitialPage();
            RibbonBar.ChangeContentEvent += new RibbonBar.ChangeContentHandler(changeContent);
            statusBar.DataContext = Model.RobotCommon.GetInstance;
            logBar.DataContext = Model.ControllerLog.GetInstance;
            config.InitConfig();
            log.InitLog();
            log.AddLog("软件打开", "local", "0");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (client.clientBase.clientChannel != null && client.clientBase.clientChannel.Active)
            {
                client.clientBase.clientChannel.CloseAsync();
            }
            log.AddLog("软件关闭", "local", "0");
            Environment.Exit(0);
        }

        private void changeContent(UserControl userControl)
        {
            this.mainContent.Content = userControl;
        }

        private void logButton_Click(object sender, RoutedEventArgs e)
        {
            LogWindow logWindow = new LogWindow();
            logWindow.Show();
            if (logWindow.IsActive)
            {
                return;
            }
        }
    }
}