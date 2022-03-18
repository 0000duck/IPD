using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Client;

namespace IPD.Tcp
{
    internal class OpcUa
    {
        private static readonly object lockObj = new object();
        private static OpcUa instance = null;

        public static OpcUa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new OpcUa();
                        }
                    }
                }
                return instance;
            }
        }

        private ApplicationConfiguration m_configuration;
        public Session Session;
        private Dictionary<string, Subscription> dicSubscriptions = new Dictionary<string, Subscription>();
        private string ServerUrl = "opc.tcp://192.168.1.13:4840";

        public OpcUa()
        {
            CertificateValidator certificateValidator = new CertificateValidator();
            certificateValidator.CertificateValidation += (sender, eventArgs) =>
            {
                if (ServiceResult.IsGood(eventArgs.Error))
                {
                    eventArgs.Accept = true;
                }
                else if (eventArgs.Error.StatusCode.Code == StatusCodes.BadCertificateUntrusted)
                {
                    eventArgs.Accept = true;
                }
                else
                {
                    throw new Exception("验证错误");
                }
            };
            certificateValidator.Update(new SecurityConfiguration { AutoAcceptUntrustedCertificates = true, RejectSHA1SignedCertificates = false, MinimumCertificateKeySize = 1024 });
            m_configuration = new ApplicationConfiguration
            {
                ApplicationName = "IPD",
                ApplicationType = ApplicationType.Client,
                ApplicationUri = "IPD",
                CertificateValidator = certificateValidator,
                ServerConfiguration = new ServerConfiguration
                {
                    MaxSubscriptionCount = 100000,
                    MaxMessageQueueSize = 100000,
                    MaxNotificationQueueSize = 100000,
                    MaxPublishRequestCount = 10000
                },
                SecurityConfiguration = new SecurityConfiguration
                {
                    AutoAcceptUntrustedCertificates = true,
                    RejectSHA1SignedCertificates = false,
                    MinimumCertificateKeySize = 1024,
                    SuppressNonceValidationErrors = true,
                    ApplicationCertificate = new CertificateIdentifier
                    {
                        StoreType = CertificateStoreType.X509Store,
                        StorePath = "CurrentUser\\IPD",
                    },
                    TrustedIssuerCertificates = new CertificateTrustList
                    {
                        StoreType = CertificateStoreType.X509Store,
                        StorePath = "CurrentUser\\IPD",
                    },
                    TrustedPeerCertificates = new CertificateTrustList
                    {
                        StoreType = CertificateStoreType.X509Store,
                        StorePath = "CurrentUser\\Root",
                    },
                },
                TransportQuotas = new TransportQuotas
                {
                    OperationTimeout = 6000000,
                    MaxStringLength = int.MaxValue,
                    MaxByteStringLength = int.MaxValue,
                    MaxArrayLength = 65535,
                    MaxMessageSize = 419430400,
                    MaxBufferSize = 65535,
                    ChannelLifetime = -1,
                    SecurityTokenLifetime = -1,
                },
                ClientConfiguration = new ClientConfiguration
                {
                    DefaultSessionTimeout = -1,
                    MinSubscriptionLifetime = -1,
                },
                DisableHiResClock = true
            };
            m_configuration.Validate(ApplicationType.Client);
        }

        public async Task<bool> ConnectAsync(string host, string port)
        {
            Model.ConnectParameter ConnectParameter = Model.ConnectParameter.GetInstance;
            //ServerUrl = "opc.tcp://" + ConnectParameter.IP + ":4840";
            ServerUrl = host + ":" + port;
            try
            {
                if (Session != null && Session.Connected == true)
                {
                    System.Diagnostics.Debug.WriteLine("opcua already connected");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("connecting opcua");
                    EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint(ServerUrl, false);
                    EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(m_configuration);
                    ConfiguredEndpoint endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
                    Session session = await Session.Create(m_configuration, endpoint, false, false, m_configuration.ApplicationName, 30 * 60 * 1000, new UserIdentity(new AnonymousIdentityToken()), null);
                    if (session != null && session.Connected)
                    {
                        Session = session;
                    }
                }
                System.Diagnostics.Debug.WriteLine("opcua connected");
                return true;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("opcua连接出问题了");
                return false;
            }
        }

        public void DisConnect()
        {
            try
            {
                if (Session != null)
                {
                    Session.Close();
                    Session.Dispose();
                    Session = null;
                    System.Diagnostics.Debug.WriteLine("opcua断开");
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("opcua断开失败");
            }
        }

        public static ReferenceDescriptionCollection Browse(Session session, BrowseDescriptionCollection nodesToBrowse, bool throwOnError)
        {
            return Browse(session, null, nodesToBrowse, throwOnError);
        }

        public static ReferenceDescriptionCollection Browse(Session session, ViewDescription view, BrowseDescriptionCollection nodesToBrowse, bool throwOnError)
        {
            try
            {
                ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();
                BrowseDescriptionCollection unprocessedOperations = new BrowseDescriptionCollection();

                while (nodesToBrowse.Count > 0)
                {
                    // start the browse operation.
                    BrowseResultCollection results = null;
                    DiagnosticInfoCollection diagnosticInfos = null;

                    session.Browse(
                        null,
                        view,
                        0,
                        nodesToBrowse,
                        out results,
                        out diagnosticInfos);

                    ClientBase.ValidateResponse(results, nodesToBrowse);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);

                    ByteStringCollection continuationPoints = new ByteStringCollection();

                    for (int ii = 0; ii < nodesToBrowse.Count; ii++)
                    {
                        // check for error.
                        if (StatusCode.IsBad(results[ii].StatusCode))
                        {
                            // this error indicates that the server does not have enough
                            // simultaneously active continuation points. This request will need to
                            // be resent after the other operations have been completed and their
                            // continuation points released.
                            if (results[ii].StatusCode == StatusCodes.BadNoContinuationPoints)
                            {
                                unprocessedOperations.Add(nodesToBrowse[ii]);
                            }

                            continue;
                        }

                        // check if all references have been fetched.
                        if (results[ii].References.Count == 0)
                        {
                            continue;
                        }

                        // save results.
                        references.AddRange(results[ii].References);

                        // check for continuation point.
                        if (results[ii].ContinuationPoint != null)
                        {
                            continuationPoints.Add(results[ii].ContinuationPoint);
                        }
                    }

                    // process continuation points.
                    ByteStringCollection revisedContiuationPoints = new ByteStringCollection();

                    while (continuationPoints.Count > 0)
                    {
                        // continue browse operation.
                        session.BrowseNext(
                            null,
                            false,
                            continuationPoints,
                            out results,
                            out diagnosticInfos);

                        ClientBase.ValidateResponse(results, continuationPoints);
                        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints);

                        for (int ii = 0; ii < continuationPoints.Count; ii++)
                        {
                            // check for error.
                            if (StatusCode.IsBad(results[ii].StatusCode))
                            {
                                continue;
                            }

                            // check if all references have been fetched.
                            if (results[ii].References.Count == 0)
                            {
                                continue;
                            }

                            // save results.
                            references.AddRange(results[ii].References);

                            // check for continuation point.
                            if (results[ii].ContinuationPoint != null)
                            {
                                revisedContiuationPoints.Add(results[ii].ContinuationPoint);
                            }
                        }

                        // check if browsing must continue;
                        revisedContiuationPoints = continuationPoints;
                    }

                    // check if unprocessed results exist.
                    nodesToBrowse = unprocessedOperations;
                }

                // return complete list.
                return references;
            }
            catch (Exception exception)
            {
                if (throwOnError)
                {
                    throw new ServiceResultException(exception, StatusCodes.BadUnexpectedError);
                }

                return null;
            }
        }

        public List<ReferenceDescription> BrowseNotes(NodeId rootId)
        {
            NodeId[] referenceTypeIds = { ReferenceTypeIds.Organizes, ReferenceTypeIds.Aggregates };
            try
            {
                BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
                for (int i = 0; i < referenceTypeIds.Length; i++)
                {
                    BrowseDescription nodeToBrowse = new BrowseDescription();
                    nodeToBrowse.NodeId = rootId;
                    nodeToBrowse.BrowseDirection = BrowseDirection.Forward;
                    nodeToBrowse.ReferenceTypeId = referenceTypeIds[i];
                    nodeToBrowse.IncludeSubtypes = true;
                    nodeToBrowse.NodeClassMask = 0;
                    nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;
                    nodesToBrowse.Add(nodeToBrowse);
                }
                ReferenceDescriptionCollection referenceDescriptions = Browse(Session, nodesToBrowse, true);
                return referenceDescriptions;
            }
            catch
            {
                return null;
            }
        }

        public class OpcAttributesAndProperties
        {
            public ReadValueIdCollection readValueIds { get; set; }
            public DataValueCollection dataValues { get; set; }
        }

        public OpcAttributesAndProperties ReadAttributes(NodeId nodeId)
        {
            if (NodeId.IsNull(nodeId)) { return null; }
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            foreach (uint attributeId in Attributes.GetIdentifiers())
            {
                ReadValueId nodeToRead = new ReadValueId();
                nodeToRead.NodeId = nodeId;
                nodeToRead.AttributeId = attributeId;
                nodesToRead.Add(nodeToRead);
            }

            DataValueCollection results;
            DiagnosticInfoCollection diagnosticInfos = null;
            Session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out results, out diagnosticInfos);
            return new OpcAttributesAndProperties { readValueIds = nodesToRead, dataValues = results };
        }

        public OpcAttributesAndProperties ReadProperties(NodeId nodeId)
        {
            BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
            BrowseDescription nodeToBrowse = new BrowseDescription();
            nodeToBrowse.NodeId = nodeId;
            nodeToBrowse.BrowseDirection = BrowseDirection.Forward;
            nodeToBrowse.ReferenceTypeId = Opc.Ua.ReferenceTypeIds.HasProperty;
            nodeToBrowse.IncludeSubtypes = true;
            nodeToBrowse.NodeClassMask = (uint)NodeClass.Variable;
            nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;

            nodesToBrowse.Add(nodeToBrowse);
            ReferenceDescriptionCollection references = Browse(Session, nodesToBrowse, true);
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            for (int i = 0; references != null && i < references.Count; i++)
            {
                ReferenceDescription reference = references[i];
                if (reference.NodeId.IsAbsolute)
                {
                    continue;
                }
                ReadValueId nodeToRead = new ReadValueId();
                nodeToRead.NodeId = (NodeId)reference.NodeId;
                nodeToRead.AttributeId = Attributes.Value;
                nodeToRead.Handle = reference;
                nodesToRead.Add(nodeToRead);
            }
            if (nodesToRead.Count == 0) { return null; }
            DataValueCollection results;
            DiagnosticInfoCollection diagnosticInfos;
            Session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out results, out diagnosticInfos);
            return new OpcAttributesAndProperties { dataValues = results, readValueIds = nodesToRead };
        }

        public string GetAccessLevelDisplayText(byte accessLevel)
        {
            StringBuilder buffer = new StringBuilder();

            if (accessLevel == AccessLevels.None)
            {
                buffer.Append("None");
            }

            if ((accessLevel & AccessLevels.CurrentRead) == AccessLevels.CurrentRead)
            {
                buffer.Append("Read");
            }

            if ((accessLevel & AccessLevels.CurrentWrite) == AccessLevels.CurrentWrite)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("Write");
            }

            if ((accessLevel & AccessLevels.HistoryRead) == AccessLevels.HistoryRead)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("HistoryRead");
            }

            if ((accessLevel & AccessLevels.HistoryWrite) == AccessLevels.HistoryWrite)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("HistoryWrite");
            }

            if ((accessLevel & AccessLevels.SemanticChange) == AccessLevels.SemanticChange)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("SemanticChange");
            }

            return buffer.ToString();
        }

        public string GetEventNotifierDisplayText(byte eventNotifier)
        {
            StringBuilder buffer = new StringBuilder();

            if (eventNotifier == EventNotifiers.None)
            {
                buffer.Append("None");
            }

            if ((eventNotifier & EventNotifiers.SubscribeToEvents) == EventNotifiers.SubscribeToEvents)
            {
                buffer.Append("Subscribe");
            }

            if ((eventNotifier & EventNotifiers.HistoryRead) == EventNotifiers.HistoryRead)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("HistoryRead");
            }

            if ((eventNotifier & EventNotifiers.HistoryWrite) == EventNotifiers.HistoryWrite)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("HistoryWrite");
            }

            return buffer.ToString();
        }

        public string GetValueRankDisplayText(int valueRank)
        {
            switch (valueRank)
            {
                case ValueRanks.Any: return "Any";
                case ValueRanks.Scalar: return "Scalar";
                case ValueRanks.ScalarOrOneDimension: return "ScalarOrOneDimension";
                case ValueRanks.OneOrMoreDimensions: return "OneOrMoreDimensions";
                case ValueRanks.OneDimension: return "OneDimension";
                case ValueRanks.TwoDimensions: return "TwoDimensions";
            }

            return valueRank.ToString();
        }

        public string GetAttributeDisplayText(uint attributeId, Variant value)
        {
            if (value == Variant.Null)
            {
                return "无";
            }
            switch (attributeId)
            {
                case Attributes.AccessLevel:
                case Attributes.UserAccessLevel:
                    {
                        byte? field = value.Value as byte?;

                        if (field != null)
                        {
                            return GetAccessLevelDisplayText(field.Value);
                        }

                        break;
                    }

                case Attributes.EventNotifier:
                    {
                        byte? field = value.Value as byte?;

                        if (field != null)
                        {
                            return GetEventNotifierDisplayText(field.Value);
                        }

                        break;
                    }

                case Attributes.DataType:
                    {
                        return Session.NodeCache.GetDisplayText(value.Value as NodeId);
                    }

                case Attributes.ValueRank:
                    {
                        int? field = value.Value as int?;

                        if (field != null)
                        {
                            return GetValueRankDisplayText(field.Value);
                        }

                        break;
                    }

                case Attributes.NodeClass:
                    {
                        int? field = value.Value as int?;

                        if (field != null)
                        {
                            return ((NodeClass)field.Value).ToString();
                        }

                        break;
                    }

                case Attributes.NodeId:
                    {
                        NodeId field = value.Value as NodeId;

                        if (!NodeId.IsNull(field))
                        {
                            return field.ToString();
                        }

                        return "Null";
                    }

                case Attributes.DataTypeDefinition:
                    {
                        ExtensionObject field = value.Value as ExtensionObject;
                        if (field != null)
                        {
                            return field.ToString();
                        }
                        break;
                    }
            }

            // check for byte strings.
            if (value.Value is byte[])
            {
                return Utils.ToHexString(value.Value as byte[]);
            }

            // use default format.
            return value.ToString();
        }

        public void AddSubscription(string key, NodeId[] nodeIds, Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> callback)
        {
            Subscription subscription = new Subscription(Session.DefaultSubscription);
            subscription.PublishingEnabled = true;
            subscription.PublishingInterval = 0;
            subscription.KeepAliveCount = uint.MaxValue;
            subscription.LifetimeCount = uint.MaxValue;
            subscription.MaxNotificationsPerPublish = uint.MaxValue;
            subscription.Priority = 100;
            subscription.DisplayName = key;

            for (int i = 0; i < nodeIds.Length; i++)
            {
                MonitoredItem item = new MonitoredItem
                {
                    StartNodeId = nodeIds[i],
                    AttributeId = Attributes.Value,
                    DisplayName = nodeIds[i].Format(),
                    SamplingInterval = 100
                };
                item.Notification += (MonitoredItem monitordItem, MonitoredItemNotificationEventArgs monitoredItemNotificationEventArgs) =>
                {
                    callback?.Invoke(key, monitordItem, monitoredItemNotificationEventArgs);
                };
                subscription.AddItem(item);
            }
            Session.AddSubscription(subscription);
            subscription.Create();
            lock (subscription)
            {
                if (dicSubscriptions.ContainsKey(key))
                {
                    // remove
                    dicSubscriptions[key].Delete(true);
                    Session.RemoveSubscription(dicSubscriptions[key]);
                    dicSubscriptions[key].Dispose();
                    dicSubscriptions[key] = subscription;
                }
                else
                {
                    dicSubscriptions.Add(key, subscription);
                }
            }
        }

        public void RemoveSubscription(string key)
        {
            lock (dicSubscriptions)
            {
                dicSubscriptions[key].Delete(true);
                Session.RemoveSubscription(dicSubscriptions[key]);
                dicSubscriptions[key].Dispose();
                dicSubscriptions.Remove(key);
            }
        }

        public void RemoveAllSubscriptions()
        {
            if (dicSubscriptions.Count == 0) { return; }
            lock (dicSubscriptions)
            {
                foreach (var item in dicSubscriptions)
                {
                    item.Value.Delete(true);
                    Session.RemoveSubscription(item.Value);
                    item.Value.Dispose();
                }
                dicSubscriptions.Clear();
            }
        }

        public bool WriteNode<T>(string tag, T value)
        {
            WriteValue valueToWrite = new WriteValue { NodeId = new NodeId(tag), AttributeId = Attributes.Value };
            valueToWrite.Value.Value = value;
            valueToWrite.Value.StatusCode = StatusCodes.Good;
            valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
            valueToWrite.Value.SourceTimestamp = DateTime.MinValue;

            WriteValueCollection valuesToWrite = new WriteValueCollection { valueToWrite };
            Session.Write(null, valuesToWrite, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);
            ClientBase.ValidateResponse(results, valuesToWrite);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);
            if (StatusCode.IsBad(results[0]))
            {
                return false;
            }
            return !StatusCode.IsBad(results[0]);
        }

        public Task<bool> WriteNodeAsync<T>(string tag, T value)
        {
            WriteValue valueToWrite = new WriteValue()
            {
                NodeId = new NodeId(tag),
                AttributeId = Attributes.Value,
            };
            valueToWrite.Value.Value = value;
            valueToWrite.Value.StatusCode = StatusCodes.Good;
            valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
            valueToWrite.Value.SourceTimestamp = DateTime.MinValue;
            WriteValueCollection valuesToWrite = new WriteValueCollection
            {
                valueToWrite
            };

            // Wrap the WriteAsync logic in a TaskCompletionSource, so we can use C# async/await
            // syntax to call it:
            var taskCompletionSource = new TaskCompletionSource<bool>();
            Session.BeginWrite(
                requestHeader: null,
                nodesToWrite: valuesToWrite,
                callback: ar =>
                {
                    var response = Session.EndWrite(
                      result: ar,
                      results: out StatusCodeCollection results,
                      diagnosticInfos: out DiagnosticInfoCollection diag);

                    try
                    {
                        ClientBase.ValidateResponse(results, valuesToWrite);
                        ClientBase.ValidateDiagnosticInfos(diag, valuesToWrite);
                        taskCompletionSource.SetResult(StatusCode.IsGood(results[0]));
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.TrySetException(ex);
                    }
                },
                asyncState: null);
            return taskCompletionSource.Task;
        }
    }
}