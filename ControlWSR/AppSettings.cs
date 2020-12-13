using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ControlWSR
{
    public class AppSettings
    {
		public string StorageConnectionString { get; set; }

		public static AppSettings LoadAppSettings()
		{
			IConfigurationRoot configRoot = new ConfigurationBuilder()
				.AddJsonFile("Settings.json")
				.Build();
			AppSettings appSettings = configRoot.Get<AppSettings>();
			return appSettings;
		}
	}
}
