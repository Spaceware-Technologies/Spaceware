using System;
using System.Diagnostics;
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
        [STAThread] //this is for the OpenFolderDialog
        public static void Main()
        {   
            /*Static reference into the DownloadAPI Class.*/
            DownloadAPI Installer = new DownloadAPI();
            Console.Title = $"Area 51 Installer | Joshua, Maxie, PandaStudios & Swordsith ";
            if (!File.Exists("Authorization.json"))
            {

                DownloadAPI.InstallLog($"Enter Token: ");
                {
                    string token =  Console.ReadLine();
                    if (!Installer.ValidateToken(token))
                    {
                        DownloadAPI.InstallLog("Seems your trying to use an invalid token, please join server and make a ticket.\nDiscord: https://discord.gg/Paul", true);
                    }
                    File.AppendAllText("Authorization.json", token);
                }

                

            }
            switch (Installer.InitDownload())
            {
                case 0:
                    Console.Clear();
                    DownloadAPI.InstallLog("Please Make Sure To Select The VRChat Folder Anything, Else Will Give This Message!", true);
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
                        DownloadAPI.InstallLog($"Install Complete!, Press enter to exit & run vrchat.", true);
                        try
                        {
                            File.Delete(Installer.zipPath);
                            File.Move("Authorization.json", $"{Installer.vrchatPath}\\Area51\\Authorization.json"); 
                            Process.Start($"{Installer.vrchatPath}\\VRChat.exe");
                        }
                        catch (Exception InstallError) { DownloadAPI.InstallLog(InstallError.StackTrace); }
                    }
                    break;
            }
        }
    }
}