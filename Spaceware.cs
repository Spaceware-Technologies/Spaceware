using System;
using System.IO;

namespace Spaceware
{
    class Spaceware
    {
        /// <summary>
        /// Project Name: Main
        /// Description: Area51-Installer {Server AUTH: api.outerspace.store}
        /// Project Developer's: Installer: Josh(UrFingPoor) Serverside:  Maxie, PandaStudios 
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            /*Static reference into the DownloadWorker class.*/
            DownloadAPI Installer = new DownloadAPI();

            Console.Title = $"Area 51 Installer | Joshua, Maxie, PandaStudios, Pyro & Swordsith "; 
            if (!File.Exists("Authorization.json"))
            {
                DownloadAPI.InstallLog($"Enter Token: ");
                File.AppendAllText("Authorization.json", Console.ReadLine());
            }

            switch (Installer.InitDownload())
            {
                case 0:
                    Console.Clear();
                    DownloadAPI.InstallLog("Please Make Sure To Select The VRChat Folder Anything, Else Will Give This Message.", true);
                    break;
                case 1:
                    DownloadAPI.InstallLog("You seem to be on the lastest version, no need to update!", true);
                    break;
                case 2:
                    DownloadAPI.InstallLog("Something really done goofed on install, please join server and make a ticket.\nDiscord: https://discord.gg/Paul", true);
                    break;
                case 3:
                    DownloadAPI.InstallLog($"Download Complete!, Press enter to install.", true);
                    if (Installer.InitExtaction())
                    {
                        DownloadAPI.InstallLog($"Install Complete!, Press enter to exit.", true);
                        File.Delete(Installer.zipPath);
                    }
                    break;
            }
        }
    }
}
 