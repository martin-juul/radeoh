using System;
using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using Xamarin.Forms;

namespace Radeoh.Logging
{
    public class Logger : ILogService
    {
        public void Initialize(Assembly assembly, string assemblyName)
        {
            var resourcePrefix = Device.RuntimePlatform switch
            {
                Device.iOS => "Radeoh.iOS",
                Device.Android => "Radeoh.Droid",
                _ => throw new NotImplementedException("Your platform has not been built for.")
            };

            var location = $"{resourcePrefix}.NLog.config";
            var stream = assembly.GetManifestResourceStream(location);
            if (stream == null)
                throw new Exception($"The resource '{location} was not loaded properly.'");

            LogManager.Configuration = new XmlLoggingConfiguration(System.Xml.XmlReader.Create(stream), null);
        }
    }
}