using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FootballManagerWPF.Helpers
{
    public static class ConfigurationHelper
    {
        private static readonly IConfigurationRoot _configuration;

        static ConfigurationHelper()
        {
            // Thiết lập đường dẫn và load file appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        public static string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }
    }
}
