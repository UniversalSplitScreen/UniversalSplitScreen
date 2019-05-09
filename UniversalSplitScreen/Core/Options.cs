using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.Core
{
	class Options
	{
		private static List<OptionsStructure> options = new List<OptionsStructure>();
		public static OptionsStructure CurrentOptions { get; private set; } = new OptionsStructure();

		public static void LoadOptions()
		{
			CurrentOptions = CurrentOptions ?? new OptionsStructure();
			options.Add(CurrentOptions);//Default

			DirectoryInfo dInfo = new DirectoryInfo(GetConfigFolder());

			foreach (var file in dInfo.GetFiles("*.json"))
			{
				if (ReadFromFile(file.FullName, out OptionsStructure o))
				{
					options.Add(o);
					Console.WriteLine($"Loaded {file.Name} : {o.OptionsName}");
				}
			}

			CurrentOptions = options[0];

			var comboBox = Program.Form.OptionsComboBox;
			var array = options.ToArray();
			comboBox.Items.AddRange(array);
			comboBox.SelectedItem = CurrentOptions;
		}
		
		public static void LoadButtonClicked()
		{
			CurrentOptions = (OptionsStructure)Program.Form.OptionsComboBox.SelectedItem;
			Program.Form.PopulateOptionsRefTypes(CurrentOptions);
		}

		public static void SaveButtonClicked()
		{
			WriteToFile(CurrentOptions);
		}

		public static void NewButtonClicked(string name)
		{
			CurrentOptions = CurrentOptions.Clone();
			CurrentOptions.OptionsName = name;
			options.Add(CurrentOptions);

			var cb = Program.Form.OptionsComboBox;
			cb.Items.Add(CurrentOptions);
			cb.SelectedItem = CurrentOptions;
		}

		public static void DeleteButtonClicked()
		{
			//TODO: add ok/cancel
			var cb = Program.Form.OptionsComboBox;
			var toDelete = (OptionsStructure)cb.SelectedItem;
			DeleteFile(toDelete);
			
			if (cb.Items.Count > 1 && cb.Items.Contains(toDelete))
			{
				cb.Items.Remove(toDelete);
				cb.SelectedItem = cb.Items[0];
			}
		}
		
		private static bool WriteToFile(OptionsStructure options)
		{
			try
			{
				string directory = GetConfigFolder();
				Directory.CreateDirectory(directory);

				using (StreamWriter file = File.CreateText(Path.Combine(directory, options.OptionsName + ".json")))
				{
					JsonSerializer serializer = new JsonSerializer();
					serializer.Formatting = Formatting.Indented;
					serializer.Serialize(file, options);
				}

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error writing options to file: {e}");
				return false;
			}
		}
		
		private static  bool ReadFromFile(string path, out OptionsStructure options)
		{
			try
			{
				using (StreamReader file = File.OpenText(path))
				{
					JsonSerializer serializer = new JsonSerializer();
					options = (OptionsStructure)serializer.Deserialize(file, typeof(OptionsStructure));
					return true;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error reading options from a file: {e}");
				options = null;
				return false;
			}
		}

		private static bool DeleteFile(OptionsStructure options)
		{
			try
			{
				string path = Path.Combine(GetConfigFolder(), options.OptionsName + ".json");
				Console.WriteLine($"Deleting {path}");
				File.Delete(path);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error deleting options file: {e}");
				return false;
			}
		}

		private static string GetConfigFolder() => Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config");
	}
}
