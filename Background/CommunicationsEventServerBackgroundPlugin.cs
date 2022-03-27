using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace Communications.Background
{
   
    public class CommunicationsEventServerBackgroundPlugin : VideoOS.Platform.Background.BackgroundPlugin
    {
        private readonly List<object> _registrationObjects = new List<object>();

        private MessageCommunication _messageCommunication = null;

        private System.Timers.Timer HandleClientMessagesTimer;

        ConcurrentQueue<SerializedPluginMessage> ClientMessages = new ConcurrentQueue<SerializedPluginMessage>();       

        public CommunicationsEventServerBackgroundPlugin()
        {

        }

        public override void Init()
        {
            // You can create new Threads here, if that is required
            MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
            _messageCommunication = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);            
            _registrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(ApplicationUp, new MessageIdFilter(MessageId.System.ApplicationLoggedOnIndication)));
        }

        public override List<VideoOS.Platform.EnvironmentType> TargetEnvironments
        {
            get
            {
                return new List<EnvironmentType>(new[] { EnvironmentType.Service });
            }
        }

        public override Guid Id
        {
            get { return CommunicationsDefinition.CommunicationsEventServerBackgroundPluginId; }
        }

        public override string Name
        {
            get { return "Communications Background sample"; }
        }

        private object ApplicationUp(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender)
        {
            EnvironmentManager.Instance.Log(false, "CommunicationsBackgroundplugin", "App up");
            if (HandleClientMessagesTimer == null)
            {
                HandleClientMessagesTimer = new System.Timers.Timer()
                {
                    AutoReset = true,
                    Enabled = true,
                    Interval = 10,
                };
                HandleClientMessagesTimer.Elapsed += HandleClientMessagesTimer_Elapsed;
            }
            _registrationObjects.Add(_messageCommunication.RegisterCommunicationFilter(CommandHandler, new CommunicationIdFilter(PluginMessage.PluginMessageId)));

            return null;
        }      

        private void HandleClientMessagesTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            HandleClientMessagesTimer.Stop();
            try
            {
                if (ClientMessages.IsEmpty) return;
                if (ClientMessages.TryDequeue(out SerializedPluginMessage data))
                {
                    if (data.Source.IdentityName == PluginMessage.SourceIdEventServer) return;
                    //Todo Ignore from maps   data.Source
                    PluginMessage cmd = PluginMessage.Deserialize(data.Entry);

                    EnvironmentManager.Instance.Log(false, "CommunicationsBackgroundplugin", $"Received {cmd} {cmd.GetType()} from " + data.Source.IdentityName);

                    if (cmd is SmartClientInfo td)
                    {
                        
                    }
                    else if (cmd is ComplexDataExample cde) 
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }
            finally
            {
                HandleClientMessagesTimer.Start();
            }
        }

        public override void Close()
        {
            foreach (var r in _registrationObjects)
            {
                try
                {
                    _messageCommunication.UnRegisterCommunicationFilter(r);
                }
                catch { }
            }
            foreach (var r in _registrationObjects)
            {
                try
                {
                    EnvironmentManager.Instance.UnRegisterReceiver(r);
                }
                catch { }
            }
            _registrationObjects?.Clear();
        }

        private object CommandHandler(Message message, FQID dest, FQID source)
        {
            try
            {
                if (message.Data is SerializedPluginMessage data)
                {
                    ClientMessages.Enqueue(data);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }
            return null;
        }
    }
}
