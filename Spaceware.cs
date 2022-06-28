﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            Console.Title = $"Area 51 Installer | Joshua, Maxie, PandaStudios & Swordsith"; Installer.DisplayBanner(2);
            switch (Console.ReadLine())
            {
                case "1":
                    InitSpaceware();
                    break;
                case "2":
                    Process.Start("https://github.com/Spaceware-Technologies/HWID-Spoofer-For-VRC");
                    break;
            }
        }

        private static void InitSpaceware()
        {
            /*Write Authorization If It does Exist */
            if (!File.Exists("Authorization.json"))
            {
                DownloadAPI.InstallLog($"Enter Token: ");
                AuthToken = Console.ReadLine();
                File.AppendAllText("Authorization.json", AuthToken);
            }

            /*Validates Token Before Install*/
            if (!Installer.ValidateToken(File.ReadAllText("Authorization.json")))
            {
                DownloadAPI.InstallLog("Seems your trying to use an invalid token, please join server and make a ticket.\nDiscord: https://discord.gg/Paul", true);
            }
            else
            {
                /*Install*/
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

        #region Public/Private Variables - Log, APILink, VRChatpath ect. these are gloabally called throughout this project.
        public static DownloadAPI Installer = new DownloadAPI();
        private static string AuthToken = string.Empty;
        #endregion
    }
}