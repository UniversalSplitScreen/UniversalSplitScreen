using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.Core
{
	public static class UpdateChecker
	{
		public const string currentVersion = "v1.0.0";
		const string request = @"https://api.github.com/repos/UniversalSplitScreen/UniversalSplitScreen/releases/latest";

		/// <summary>
		/// Returns empty string if no update availible, and the new version tag name otherwise
		/// </summary>
		/// <returns></returns>
		public static async Task<string> IsThereAnUpdate()
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					client.DefaultRequestHeaders.Add("User-Agent", "UniversalSplitScreen");//GitHub needs User-Agent to be set or it gives forbidden
					HttpResponseMessage response = await client.GetAsync(request);
					response.EnsureSuccessStatusCode();
					string responseBody = await response.Content.ReadAsStringAsync();

					Logger.WriteLine(responseBody);

					//https://www.newtonsoft.com/json/help/html/ReadingWritingJSON.htm
					var reader = new JsonTextReader(new StringReader(responseBody));

					string versionName = string.Empty;

					while (reader.Read())
					{
						if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "tag_name")
						{
							if (reader.Read() && reader.TokenType == JsonToken.String)
							{
								versionName = (reader.Value as string) ?? string.Empty;
								break;
							}
						}
					}

					Logger.WriteLine($"Github version name = {versionName}");
					return versionName == currentVersion ? string.Empty : versionName;//Return nothing if same version
				}
				catch (Exception e)
				{
					Logger.WriteLine(e);
					return string.Empty;
				}
			}
		}
	}
}
