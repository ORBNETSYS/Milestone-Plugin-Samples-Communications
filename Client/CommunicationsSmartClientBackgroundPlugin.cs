using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;
using Message = VideoOS.Platform.Messaging.Message;

namespace Communications.Client
{  
    public class CommunicationsSmartClientBackgroundPlugin : BackgroundPlugin
    {
        public string Username { get; private set; }
        public string Usertype { get; private set; }
        public string HostInfo { get; private set; } //not used   
        public static Item LastSelectedViewItem { get; private set; } = null;
        public static ViewAndLayoutItem LastSelectedView { get; private set; } = null;
        public DateTime PlaybackTime { get; private set; }

        public WorkSpaceState CurrentWorkspaceState = WorkSpaceState.Normal;

        /// <summary>
        /// Checked by the Side Panel for popups.
        /// </summary>
        public ConcurrentQueue<SmartClientInfo> InfoPopupQueue = new ConcurrentQueue<SmartClientInfo>();

        private ConcurrentQueue<SerializedPluginMessage> ServerMessages = new ConcurrentQueue<SerializedPluginMessage>();

        private static System.Timers.Timer HandleServerMessagesTimer;
        private MessageCommunication _messageCommunication;
       
     

        private readonly List<object> _messageRegistrationObjects = new List<object>();
       
       

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return CommunicationsDefinition.CommunicationsEventServerBackgroundPluginId; }
        }

        /// <summary>
        /// The name of this background plugin
        /// </summary>
        public override string Name
        {
            get { return "Communications Smart Client Plugin"; }
        }

        /// <summary>
        /// Called by the Environment when the user has logged in.
        /// </summary>
        public override void Init()
        {
            try
            {      
                MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
                _messageCommunication = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);

                _messageRegistrationObjects.Add(_messageCommunication.RegisterCommunicationFilter(CmdLineHandler, new CommunicationIdFilter(PluginMessage.PluginMessageId)));   
                
                _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(WorkSpaceStateChangedIndication, new MessageIdFilter(MessageId.SmartClient.WorkSpaceStateChangedIndication)));

                if (HandleServerMessagesTimer == null)
                {
                    HandleServerMessagesTimer = new System.Timers.Timer() { AutoReset = true, Enabled = true, Interval = 10, };
                    HandleServerMessagesTimer.Elapsed += HandleServerMessagesTimer_Elapsed;
                }   

                LoginSettings ls = LoginSettingsCache.GetLoginSettings(EnvironmentManager.Instance.MasterSite);
                Username = ls.UserName;
                if (ls.IsBasicUser)
                {
                    Usertype = "Basic";
                }
                else
                {
                    Usertype = "Windows";
                }

                LoadHostInfo();
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }
        }

        public string LoadHostInfo()
        {
            if (HostInfo == null)
            {
                var ip = Dns.GetHostAddresses(Environment.MachineName).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                HostInfo = Environment.MachineName + " [" + ip?.ToString() + "]";
            }
            return HostInfo;
        }

        #region MilestoneMessaging

        private object WorkSpaceStateChangedIndication(Message message, FQID destination, FQID sender)
        {
            
            try
            {
                Debug.WriteLine($"Workspace state changed to: {message.Data}");
                if (message.Data is WorkSpaceState state)
                {
                    CurrentWorkspaceState = state;
                    if (state == WorkSpaceState.Setup)
                    {
                        EnvironmentManager.Instance.Log(false, "CommunicationsSmartClientBackgroundPlugin", "Setup Mode Activated"); 
                    }
                    else if (state == WorkSpaceState.Normal)
                    {
                        EnvironmentManager.Instance.Log(false, "CommunicationsSmartClientBackgroundPlugin", "Normal Mode Activated");
                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }
            return null;
        }

        /// <summary>
        /// Handle Server Messages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleServerMessagesTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                HandleServerMessagesTimer.Stop();
                if (ServerMessages.IsEmpty) return;
                if (ServerMessages.TryDequeue(out SerializedPluginMessage data))
                {
                    PluginMessage pm = PluginMessage.Deserialize(data.Entry);
                    Debug.WriteLine($"{pm}");
                    if (pm == null) return;

                    EnvironmentManager.Instance.Log(false, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"Received {pm} {pm.GetType()} from " + data.Source.IdentityName);
                    if (pm.ErrorMessage != null)
                    {
                        EnvironmentManager.Instance.Log(false, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"Error on {pm}: {pm.ErrorMessage}");
                    }
                    if (pm is SmartClientInfo sci)
                    {
                        HandleSmartClientInfoMessage(sci);
                    }
                    else if (pm is ComplexDataExample cde)
                    {
                        HandleComplexDataMessage(cde);
                    }                    
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }
            finally
            {
                HandleServerMessagesTimer.Start();
            }
        }

        private void HandleSmartClientInfoMessage(SmartClientInfo sci)
        {
            ClientControl.Instance.CallOnUiThread(() =>
            {
                new PopupSmartClientInfo(sci).Show();
            });
        }

        private void HandleComplexDataMessage(ComplexDataExample cde)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(cde));
        }

        private object CmdLineHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            try
            {
                if (message.Data is SerializedPluginMessage data)
                {
                    Debug.WriteLine($"Got {data}");
                    //if (data.Source.IdentityName == PluginMessage.SourceIdSmartClient) return null; //Use the source ID to filter the messages
                    //if (data.Source.IdentityName != PluginMessage.SourceIdEventServer) return null;
                    ServerMessages.Enqueue(data);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }

            return null;
        }

        public void SendCommandToEventServer(PluginMessage message)
        {
            try
            {
                SerializedPluginMessage data = new SerializedPluginMessage()
                {
                    Source = new EndPointIdentityData()
                    {
                        IdentityName = PluginMessage.SourceIdSmartClient,
                        EndPointFQID = MessageCommunicationManager.EndPointFQID
                    },
                    Entry = PluginMessage.Serialize(message)
                };
                _messageCommunication.TransmitMessage(new Message(PluginMessage.PluginMessageId, data), null, null, null); //Broadcast

                EnvironmentManager.Instance.Log(true, $"CommunicationsSmartClientBackgroundPlugin.{MethodBase.GetCurrentMethod().Name}", $"Sending command: {message}");                
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }
        }

        #region Tiles and screens

        private void ShowCamerasInFloatingWindow(List<string> cameraIds)
        {
            List<Item> cams = new List<Item>();
            foreach (var c in cameraIds)
            {
                if (Guid.TryParse(c, out Guid guid))
                {
                    var camItem = Configuration.Instance.GetItem(guid, Kind.Camera);
                    if (camItem != null)
                    {
                        cams.Add(camItem);
                    }
                }
            }
            var data = new ShowCamerasInFloatingWindowData() { Mode = Mode.ClientLive, Cameras = cams };
            ClientControl.Instance.CallOnUiThread(() =>
            {
                EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ShowCamerasInFloatingWindowCommand, data), null, null);
            });
        }
      
        #endregion

        #endregion

        /// <summary>
        /// Called by the Environment when the user log's out.
        /// You should close all remote sessions and flush cache information, as the
        /// user might logon to another server next time.
        /// </summary>
        public override void Close()
        {
            try
            {
                MessageCommunicationManager.Stop(EnvironmentManager.Instance.MasterSite.ServerId);
                foreach (object messageRegistrationObject in _messageRegistrationObjects)
                {
                    EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
                }
                _messageRegistrationObjects?.Clear();
                _messageCommunication?.Dispose();
                Debug.WriteLine("Clean Exit");                
                //Environment.Exit(0); //Helps if anything is stuck                
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }
        }

        /// <summary>
        /// Define in what Environments the current background task should be started.
        /// </summary>
        public override List<EnvironmentType> TargetEnvironments
        {
            get { return new List<EnvironmentType>() { EnvironmentType.SmartClient }; }
        }

        private void RestartSmartClient()
        {
            try
            {
                //MessageBox.Show(Application.StartupPath);
                //run the program again and close this one
                Process.Start(Application.StartupPath + "\\Client.exe");
                //or you can use Application.ExecutablePath

                //close this one
                Process.GetCurrentProcess().Kill();
            }
            catch
            { }
        }
    }
}
