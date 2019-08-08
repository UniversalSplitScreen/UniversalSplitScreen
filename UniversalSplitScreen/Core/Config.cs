using System;
using System.IO;
using Newtonsoft.Json;

namespace UniversalSplitScreen.Core
{
	class Config
	{
		#region Properties
		public bool AutomaticallyCheckForUpdatesOnStartup { get; set; } = true;
		#endregion

		#region Methods
		private static string ConfigPath => Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json");
		
		public static Config LoadConfig()
		{
			try
			{
				string configPath = ConfigPath;
				if (!File.Exists(configPath)) return new Config();

				using (StreamReader file = File.OpenText(configPath))
				{
					var serializer = new JsonSerializer();
					return (Config)serializer.Deserialize(file, typeof(Config));
				}
			}
			catch (Exception e)
			{
				Logger.WriteLine(e);
				return null;
			}
		}

		public void SaveConfig()
		{
			try
			{
				using (StreamWriter file = File.CreateText(ConfigPath))
				{
					var serializer = new JsonSerializer
					{
						Formatting = Formatting.Indented
					};
					serializer.Serialize(file, this);
				}
			}
			catch (Exception e)
			{
				Logger.WriteLine(e);
			}
		}
		#endregion
	}
}
