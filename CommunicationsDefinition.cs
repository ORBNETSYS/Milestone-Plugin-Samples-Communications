using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Communications.Background;
using Communications.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace Communications
{
	/// <summary>
	/// The PluginDefinition is the ‘entry’ point to any plugin.  
	/// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
	/// Several PluginDefinitions are allowed to be available within one DLL.
	/// Here the references to all other plugin known objects and classes are defined.
	/// The class is an abstract class where all implemented methods and properties need to be declared with override.
	/// The class is constructed when the environment is loading the DLL.
	/// </summary>
	public class CommunicationsDefinition : PluginDefinition
	{
		private static System.Drawing.Image _treeNodeImage;
		private static System.Drawing.Image _topTreeNodeImage;

		internal static Guid CommunicationsPluginId = new Guid("a8faa068-46cc-4178-b4d9-e1e2ed4d7784");			
		internal static Guid CommunicationsSidePanelId = new Guid("91f8ed5c-bea8-417e-ae82-9afdfccdfdf3");	
		internal static Guid CommunicationsSmartClientBackgroundPluginId = new Guid("cacacee0-1dfd-4cd5-8cb3-47616f14cc2a");
		internal static Guid CommunicationsEventServerBackgroundPluginId = new Guid("6800441c-cfb6-4e27-938d-3a0609538be8");

		#region Private fields

		//
		// Note that all the plugin are constructed during application start, and the constructors
		// should only contain code that references their own dll, e.g. resource load.
		private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();
		private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();

		#endregion

		#region Initialization

		/// <summary>
		/// Load resources 
		/// </summary>
		static CommunicationsDefinition()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string name = assembly.GetName().Name;

			System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.Chat.bmp");
			if (pluginStream != null)
				_treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
			System.IO.Stream configStream = assembly.GetManifestResourceStream(name + ".Resources.Server.png");
			if (configStream != null)
				_topTreeNodeImage = System.Drawing.Image.FromStream(configStream);
		}


		/// <summary>
		/// Get the icon for the plugin
		/// </summary>
		internal static Image TreeNodeImage
		{
			get { return _treeNodeImage; }
		}

		#endregion

		/// <summary>
		/// This method is called when the environment is up and running.
		/// Registration of Messages via RegisterReceiver can be done at this point.
		/// </summary>
		public override void Init()
		{
			_topTreeNodeImage = VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.SDK_VSIx];
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
			{				
				EnvironmentManager.Instance.EnableConfigurationChangedService = true;
				var smartClientBackgroundPlugin = new CommunicationsSmartClientBackgroundPlugin();
				_backgroundPlugins.Add(smartClientBackgroundPlugin);
				_sidePanelPlugins.Add(new CommunicationsSidePanelPlugin(smartClientBackgroundPlugin));

			}
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service)
			{				
				EnvironmentManager.Instance.EnableConfigurationChangedService = true;
				_backgroundPlugins.Add(new CommunicationsEventServerBackgroundPlugin());
			}
		}

		/// <summary>
		/// The main application is about to be in an undetermined state, either logging off or exiting.
		/// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
		/// </summary>
		public override void Close()
		{			
			_sidePanelPlugins.Clear();			
			_backgroundPlugins.Clear();
		}				

		#region Identification Properties

		/// <summary>
		/// Gets the unique id identifying this plugin component
		/// </summary>
		public override Guid Id
		{
			get
			{
				return CommunicationsPluginId;
			}
		}

		/// <summary>
		/// This Guid can be defined on several different IPluginDefinitions with the same value,
		/// and will result in a combination of this top level ProductNode for several plugins.
		/// Set to Guid.Empty if no sharing is enabled.
		/// </summary>
		public override Guid SharedNodeId
		{
			get
			{
				return Guid.Parse("e5af3ecd-fba0-4896-9e1f-bed788a9bbe6");
			}
		}

		/// <summary>
		/// Define name of top level Tree node - e.g. A product name
		/// </summary>
		public override string Name
		{
            get { return "Communications"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return "Node"; }
		}

		/// <summary>
		/// Your company name
		/// </summary>
		public override string Manufacturer
		{
			get
			{
				return "ORBNET Systems";
			}
		}

		/// <summary>
		/// Version of this plugin.
		/// </summary>
		public override string VersionString
		{
			get
			{
				return "1.0.0.0";
			}
		}

		#endregion


		#region Plugin Lists

		/// <summary> 
		/// An extension plugin to add to the side panel of the Smart Client.
		/// </summary>
		public override List<SidePanelPlugin> SidePanelPlugins
		{
			get { return _sidePanelPlugins; }
		}

		/// <summary>
		/// Create and returns the background task.
		/// </summary>
		public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
		{
			get { return _backgroundPlugins; }
		}

		/// <summary>
		/// Icon to be used on top level - e.g. a product or company logo
		/// </summary>
		public override System.Drawing.Image Icon
		{
			get { return _topTreeNodeImage; }
		}

		#endregion

	}
}
