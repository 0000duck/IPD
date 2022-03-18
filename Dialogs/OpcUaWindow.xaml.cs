using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using IPD.Tcp;
using Opc.Ua;
using Opc.Ua.Client;

namespace IPD.Dialogs
{
    /// <summary>
    /// OpcUaWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OpcUaWindow : Window
    {
        private Tcp.OpcUa OpcUa = Tcp.OpcUa.Instance;
        private Model.OpcUa OpcUaModel = Model.OpcUa.GetInstance;

        public OpcUaWindow()
        {
            InitializeComponent();
            attributeData.DataContext = OpcUaModel;
        }

        private int indexOfValue;

        private void addItem(TreeViewItem item, ReferenceDescription referenceDescription)
        {
            if (referenceDescription.NodeClass == NodeClass.Variable) { return; }
            List<ReferenceDescription> referenceDescriptions = OpcUa.BrowseNotes((NodeId)referenceDescription.NodeId);
            for (int i = 0; i < referenceDescriptions.Count; i++)
            {
                TreeViewItem childItem = new TreeViewItem();
                childItem.Header = referenceDescriptions[i];
                childItem.Tag = referenceDescriptions[i];
                addItem(childItem, referenceDescriptions[i]);
                item.Items.Add(childItem);
                item.IsExpanded = true;
            }
        }

        private void getNodeTree()
        {
            TreeViewItem treeViewItem = new TreeViewItem();
            INode node = OpcUa.Session.NodeCache.Find(ObjectIds.ObjectsFolder);
            ReferenceDescription reference = new ReferenceDescription();
            reference.NodeId = node.NodeId;
            reference.NodeClass = node.NodeClass;
            reference.BrowseName = node.BrowseName;
            reference.DisplayName = node.DisplayName;
            reference.TypeDefinition = node.TypeDefinitionId;
            addItem(treeViewItem, reference);
            nodeTree.Items.Add(treeViewItem);
            treeViewItem.IsExpanded = true;
        }

        private void nodeTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OpcUa.RemoveAllSubscriptions();
            ReferenceDescription referenceDescription = (ReferenceDescription)((TreeViewItem)nodeTree.SelectedItem).Tag;
            if (referenceDescription == null)
            {
                return;
            }
            OpcUa.OpcAttributesAndProperties attributes = OpcUa.ReadAttributes((NodeId)referenceDescription.NodeId);
            OpcUa.OpcAttributesAndProperties properties = OpcUa.ReadProperties((NodeId)referenceDescription.NodeId);
            Model.OpcUaAttributeList opcUaAttrributeList = new Model.OpcUaAttributeList();
            opcUaAttrributeList.List = new List<Model.OpcUaAttribute>();
            for (int i = 0; i < attributes.readValueIds.Count; i++)
            {
                if (StatusCode.IsBad(attributes.dataValues[i].StatusCode))
                {
                    if (attributes.dataValues[i].StatusCode == StatusCodes.BadAttributeIdInvalid)
                    {
                        continue;
                    }
                }
                uint attributeId = attributes.readValueIds[i].AttributeId;
                Model.OpcUaAttribute opcUaAttrribute = new Model.OpcUaAttribute();
                opcUaAttrribute.Name = Attributes.GetBrowseName(attributeId);
                opcUaAttrribute.NodeId = attributes.readValueIds[i].NodeId.Format();
                if (Attributes.GetBrowseName(attributeId) == "Value")
                {
                    indexOfValue = i;
                    OpcUa.AddSubscription(attributes.readValueIds[i].NodeId.Format(), new NodeId[] { attributes.readValueIds[i].NodeId }, SubCallback);
                }
                opcUaAttrribute.Type = Attributes.GetBuiltInType(attributeId).ToString();
                if (StatusCode.IsBad(attributes.dataValues[i].StatusCode))
                {
                    opcUaAttrribute.Value = attributes.dataValues[i].StatusCode.ToString();
                }
                else
                {
                    opcUaAttrribute.Value = OpcUa.GetAttributeDisplayText(attributeId, attributes.dataValues[i].WrappedValue);
                }
                opcUaAttrributeList.List.Add(opcUaAttrribute);
            }
            for (int i = 0; properties != null && i < properties.readValueIds.Count; i++)
            {
                ReferenceDescription reference = (ReferenceDescription)properties.readValueIds[i].Handle;
                TypeInfo typeInfo = TypeInfo.Construct(attributes.dataValues[i].Value);
                Model.OpcUaAttribute opcUaAttrribute = new Model.OpcUaAttribute();
                opcUaAttrribute.Name = reference.ToString();
                opcUaAttrribute.Type = typeInfo.BuiltInType.ToString();
                opcUaAttrribute.NodeId = reference.NodeId.Format();
                if (StatusCode.IsBad(attributes.dataValues[i].StatusCode))
                {
                    opcUaAttrribute.Value = attributes.dataValues[i].StatusCode.ToString();
                }
                else
                {
                    opcUaAttrribute.Value = attributes.dataValues[i].WrappedValue.ToString();
                }
            }
            OpcUaModel.ChangeOpcUaAttributeList(opcUaAttrributeList);
        }

        private void SubCallback(string key, MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs args)
        {
            Model.OpcUaAttributeList opcUaAttrributeList = new Model.OpcUaAttributeList();
            opcUaAttrributeList.List = new List<Model.OpcUaAttribute>();
            opcUaAttrributeList.List.AddRange(OpcUaModel.OpcUaAttributeList.List);
            MonitoredItemNotification notification = args.NotificationValue as MonitoredItemNotification;
            opcUaAttrributeList.List.Find(v => { return v.Name.Equals("Value"); }).Value = notification.Value.WrappedValue.Value.ToString();
            OpcUaModel.ChangeOpcUaAttributeList(opcUaAttrributeList);
        }

        private async void nodeTree_Initialized(object sender, EventArgs e)
        {
            bool connection = await OpcUa.ConnectAsync(host.Text, port.Text);
            if (connection)
            {
                getNodeTree();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            OpcUa.RemoveAllSubscriptions();
            OpcUa.DisConnect();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            getNodeTree();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            OpcUa.RemoveAllSubscriptions();
            OpcUa.DisConnect();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            try
            {
                OpcUa.WriteNode((string)tb.Tag, double.Parse(tb.Text));
            }
            catch
            {
                return;
            }
        }

        private async void connect_Click(object sender, RoutedEventArgs e)
        {
            bool connection = await OpcUa.ConnectAsync(host.Text, port.Text);
            if (connection)
            {
                getNodeTree();
            }
        }
    }
}