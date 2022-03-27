using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using Message = VideoOS.Platform.Messaging.Message;

namespace Communications.Client
{
    public partial class CommunicationsSidePanelUserControl : SidePanelUserControl
    {
        private CommunicationsSmartClientBackgroundPlugin _smartClientBackgroundPlugin;
        public CommunicationsSidePanelUserControl(CommunicationsSmartClientBackgroundPlugin smartClientBackgroundPlugin)
        {
            InitializeComponent();
            _smartClientBackgroundPlugin = smartClientBackgroundPlugin;
        }

        public override void Init()
        {
            
        }

        private void Button_BroadcastScreencap_Click(object sender, EventArgs e)
        {

        }

        private void Button_BroadcastSmartClientInfo_Click(object sender, EventArgs e)
        {
            try
            {              
                var workspaceResponse = EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.GetCurrentWorkspaceRequest), null, null);
                if (workspaceResponse == null)
                {
                    return;
                }                
                var allScreens = GetAllScreens();
                if (allScreens == null) return;
                List<SmartClientScreen> screens = new List<SmartClientScreen>();
                foreach (var scr in allScreens)
                {
                    screens.Add(new SmartClientScreen() { Name = scr.Name, Index = allScreens.ToList().IndexOf(scr) });
                }
                _smartClientBackgroundPlugin.SendCommandToEventServer(new SmartClientInfo()
                {
                    HostInfo = _smartClientBackgroundPlugin.HostInfo,
                    Username = _smartClientBackgroundPlugin.Username,
                    Usertype = _smartClientBackgroundPlugin.Usertype,
                    Monitors = screens,
                    CurrentWorkspace = workspaceResponse.ToString(),
                    ScreenCaptureBase64 = SmartClientInfo.ImageToBase64(GetSreenshot()),
                });
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", $"{ex}");
            }                   
        }

        internal static Item[] GetAllScreens()
        {
            List<Item> screens = Configuration.Instance.GetItemsByKind(Kind.Screen);
            if (screens != null && screens.Count > 0)
            {
                return screens.ToArray();
            }
            return null;
        }

        private Bitmap GetSreenshot()
        {
            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            return bm;
        }
    }
}
