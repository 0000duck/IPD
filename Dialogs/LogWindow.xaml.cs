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
using System.Windows.Shapes;
using XmlLogLibrary;
using IPD.Model;

namespace IPD.Dialogs
{
    /// <summary>
    /// LogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogWindow : Window
    {
        private LogList logListModel = LogList.GetInstance;
        private XmlLog log = XmlLog.Instance;

        public LogWindow()
        {
            InitializeComponent();
            logGrid.DataContext = logListModel;
            getLogList();
        }

        private void getLogList()
        {
            List<LogBase> logs = log.LogList;
            logListModel.LogListChange(logs);
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            getLogList();
        }
    }
}