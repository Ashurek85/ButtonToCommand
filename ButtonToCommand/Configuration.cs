using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ButtonToCommand
{
    public class Configuration
    {
        private const string DevideGuidKey = "DeviceGuid";
        private const string ReaderDelayMsKey = "ReaderDelayMs";

        public Guid DeviceGuid { get; }
        public int ReaderDelayMs { get; }
        public Dictionary<int, string> ButtonCommands { get; }
        public bool ShowInfo { get; }

        public Configuration(bool showInfo)
        {
            ShowInfo = showInfo;

            // ReaderDelayMs
            string readerDelayMs = ConfigurationManager.AppSettings[ReaderDelayMsKey];
            int readerDelay = 150; // Default value
            if (!int.TryParse(readerDelayMs, out readerDelay))
                Console.WriteLine($"Error reading {ReaderDelayMsKey} key from configuration file; use {ReaderDelayMs}ms");
            ReaderDelayMs = readerDelay;

            // DeviceGuid
            string configGuid = ConfigurationManager.AppSettings[DevideGuidKey];
            Guid objetiveGuid;
            if (!Guid.TryParse(configGuid, out objetiveGuid))
                throw new Exception("DeviceGuid not set or invalid Guid");
            DeviceGuid = objetiveGuid;

            // ButtonComands
            IEnumerable<string> configKeys = ConfigurationManager.AppSettings.AllKeys.Except(new string[] { DevideGuidKey, ReaderDelayMsKey });
            ButtonCommands = new Dictionary<int, string>();
            foreach (string configKey in configKeys)
            {
                int intValue;
                if (int.TryParse(configKey, out intValue))
                {
                    if (ButtonCommands.ContainsKey(intValue))
                        Console.Write($"appSettings key {configKey} duplicated, only works first entry");
                    else
                        ButtonCommands.Add(intValue, ConfigurationManager.AppSettings[configKey].ToString());
                }
                else
                {
                    Console.WriteLine($"appSettings key {configKey} ignored; is not integer button");
                }
            }
        }
    }
}
