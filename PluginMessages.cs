using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace Communications
{
    /// <summary>
    /// This Class provides examples of classes which can be send through the Milestone Communications.
    /// The Newtonsoft Nuget package is used to serialize complex objects.
    /// Each new class MUST be added to Deserialize method of the abstract PluginMessage.
    /// When using Nuget packages in MIP Plugins, you must make sure that all plugins installed on a given server have the same Nuget package versions, or else you can have DLL version issues.
    /// </summary>
    /// 
    [Serializable]
    public class SerializedPluginMessage
    {        
        /// <summary>
        /// This class can be used to send a string between Milestone plugins.
        /// In the case of this plugin, the string contains a JSON serialized by Newtonsoft which means we no longer have to mark the inner 
        /// </summary>
        public SerializedPluginMessage() { }

        public EndPointIdentityData Source;
        public string Entry; //Can be any Serializable data       
    }

    /// <summary>
    /// Inherit from this class to create your own plugin messages.
    /// Each message that derives from must this class must be hardcoded into the Deserialize method.
    /// Classes nested inside your plugin message do not require the [Serializable] attribute.
    /// </summary>
    public abstract class PluginMessage
    {
        public string _TypeName { get; set; }
        public static string PluginMessageId { get { return "CommunicationsPlugin"; } }
        public static string SourceIdEventServer = "Communications-EventServer";
        public static string SourceIdSmartClient = "Communications-SmartClient";
        public static string SourceIdManagementClient = "Communications-ManagementClient";

        public PluginMessage()
        {

        } //for Serialization

        public string EndpointHostname { get; set; }
        public string ErrorMessage { get; set; }


        public static string Serialize(PluginMessage cmd)
        {
            cmd._TypeName = cmd.GetType().Name;
            //Logger.Log(JsonConvert.SerializeObject(cmd));
            return JsonConvert.SerializeObject(cmd);
        }

        /// <summary>
        /// Uses Newtonsoft to deserialize a string back into an object.
        /// Make sure to add all your plugin message classes here.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PluginMessage Deserialize(string data)
        {
            if (data.Contains(typeof(SmartClientInfo).Name))
            {
                return JsonConvert.DeserializeObject<SmartClientInfo>(data);
            }
            else if (data.Contains(typeof(ComplexDataExample).Name)) //make sure to add any new messages/commands here
            {
                return JsonConvert.DeserializeObject<ComplexDataExample>(data);
            }

            return JsonConvert.DeserializeObject<PluginMessage>(data);
        }
    }

    public class SmartClientInfo : PluginMessage
    {
        public SmartClientInfo()
        {

        }
        public SmartClientInfo(string hostname)
        {
            this.HostInfo = hostname;
        }

        public override string ToString()
        {
            return $"{HostInfo}";
        }

        public string HostInfo { get; set; }
        public string Username { get; set; }
        public string Usertype { get; set; }
        public string CurrentWorkspace { get; set; }
        public string ScreenCaptureBase64 { get; set; }
        public List<SmartClientScreen> Monitors { get; set; }

        public static Image Base64ToImage(string base64EncodedImage) 
        {
            return Image.FromStream(new MemoryStream(Convert.FromBase64String(base64EncodedImage)));
        }

        public static string ImageToBase64(Image image) 
        {
            ImageConverter converter = new ImageConverter();
            return Convert.ToBase64String((byte[])converter.ConvertTo(image, typeof(byte[])));
        }
    }

    public class ComplexDataExample : PluginMessage
    {
        public ComplexDataExample()
        {
        }
        
        public ComplexData Info { get; set; }
        public FQID SomeItemFQID { get; set; }
       
    }    
    
    public class SmartClientScreen
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Complex object example with several infamously "non-serializable" fields to prove the functionality.
    /// </summary>
    public class ComplexData
    {
        public ComplexData()
        {

        }

        public ComplexData(string hostname)
        {
            this.Hostname = hostname;
        }

        public override string ToString()
        {
            return $"{Hostname}";
        }
      
        public Dictionary<string, string> Dictionary;
        public string Hostname { get; set; }
        public int Val { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan Timespan { get; set; }
    }
}
