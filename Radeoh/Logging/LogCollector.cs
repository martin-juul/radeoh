using System;
using Xamarin.Forms;

namespace Radeoh.Logging
{
    public class LogCollector
    {
        bool QuickZip(string directoryToZip, string destinationZipFullPath)
        {
            try
            {
                // Delete existing zip file if exists
                if (System.IO.File.Exists(destinationZipFullPath))
                    System.IO.File.Delete(destinationZipFullPath);

                if (!System.IO.Directory.Exists(directoryToZip))
                    return false;
                System.IO.Compression.ZipFile.CreateFromDirectory(directoryToZip, destinationZipFullPath,
                    System.IO.Compression.CompressionLevel.Optimal, true);
                return System.IO.File.Exists(destinationZipFullPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }

        void CreateZipFile()
        {
            if (!NLog.LogManager.IsLoggingEnabled()) return;

            string folder = Device.RuntimePlatform switch
            {
                Device.iOS => System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "Library"),
                Device.Android => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                _ => throw new Exception("Could not show log: Platform undefined.")
            };

            // Delete old zipfiles (housekeeping)
            try
            {
                foreach (var fileName in System.IO.Directory.GetFiles(folder, "*.zip"))
                {
                    System.IO.File.Delete(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting old zip files: {ex.Message}");
            }

            var logFolder = System.IO.Path.Combine(folder, "logs");
            string zipFilename = string.Empty;
            if (System.IO.Directory.Exists(logFolder))
            {
                zipFilename = $"{folder}/{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.zip";
                int filesCount = System.IO.Directory.GetFiles(logFolder, "*.csv").Length;
                if (filesCount > 0)
                {
                    if (!QuickZip(logFolder, zipFilename))
                        zipFilename = string.Empty;
                }
                else
                    zipFilename = string.Empty;
            }
            else
                zipFilename = string.Empty;
        }
    }
}