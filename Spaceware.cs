using System;
using System.IO;

namespace Spaceware
{
    class Spaceware
    {
        /// <summary>
        /// // Project Name: Main
        /// // Description: Area51-Installer {Server AUTH: api.outerspace.store}
        /// // Project Developer's: Installer: Josh(UrFingPoor) Serverside:  Maxie, PandaStudios 
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Console.Title = $"Area 51 Installer | Joshua, Maxie, PandaStudios, Pyro & Swordsith ";

            if (!File.Exists("Authorization.json"))
            {
                DownloadAPI.InstallLog("Enter Token: ");
                string token = Console.ReadLine();
                File.AppendAllText("Authorization.json", token);
            }

            switch (Installer.InitDownload())
            {
                case true:
                    DownloadAPI.InstallLog($"Download Complete!, Press enter to install.", true);
                    if (Installer.InitExtaction())
                    {
                        DownloadAPI.InstallLog($"Install Complete!, Press enter to exit.", true);
                        File.Delete(Installer.zipPath);
                        break;
                    }
                    break;
                case false:
                    DownloadAPI.InstallLog("You seem to be on the lastest version, no need to update!", true);
                    break;
            }
        }
        //Initializes static reference into the DownloadWorker class to allow use to a none static methods from a static method.
        private static readonly DownloadAPI Installer = new DownloadAPI();
    }
}
