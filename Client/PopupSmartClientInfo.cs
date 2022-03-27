using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Communications.Client
{
    public partial class PopupSmartClientInfo : Form
    {
        public PopupSmartClientInfo(SmartClientInfo info)
        {
            InitializeComponent();
            PictureBoxScreenCap.Image = SmartClientInfo.Base64ToImage(info.ScreenCaptureBase64);
            info.ScreenCaptureBase64 = String.Empty; //Clear the image data or the whole image string will be printed in the textbox.
            JToken.Parse(JsonConvert.SerializeObject(info)).ToString(Formatting.Indented);
            TextBox_SmartClientInfo.Text = JsonConvert.SerializeObject(info);
        }
    }
}
