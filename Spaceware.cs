using System;
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
            Console.Title = $"Area 51 Installer 1.3 | Joshua, Maxie, PandaStudios & Swordsith"; Installer.DisplayBanner(2);
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
            if (!File.Exists("Authorization.json") || !Installer.ValidateToken(AuthToken) || AuthToken == string.Empty)
            {
                File.Delete("Authorization.json");
                DownloadAPI.InstallLog($"[Info] Enter Token: " );
                File.AppendAllText("Authorization.json", Console.ReadLine());
            }
           
            /*Validates Token Before Install*/
            if (!Installer.ValidateToken(AuthToken))
            {
                DownloadAPI.InstallLog("[ERROR] Seems your trying to use an invalid token, please join server and make a ticket.\nDiscord: https://discord.gg/Paul", true);
            }
            else
            {
                /*Install*/
                switch (Installer.InitDownload())
                {
                    case 0:
                        Console.Clear();
                        DownloadAPI.InstallLog("[ERROR] Please Make Sure To Select VRChat.exe, Anything Else Will Give This Message!", true);
                        break;
                    case 1:
                        DownloadAPI.InstallLog("[INFO] You seem to be on the lastest version, no need to update!", true);
                        break;
                    case 2:
                        DownloadAPI.InstallLog("[ERROR] Something really done goofed on install, please join server and make a ticket.\nDiscord: https://discord.gg/Paul", true);
                        break;
                    case 3:
                        DownloadAPI.InstallLog($"[INFO] Download Complete!, Press enter to install.", true);

                        if (Installer.InitExtaction())
                        {
                            DownloadAPI.InstallLog($"[INFO] Install Complete!, Press enter to exit & run vrchat.", true);
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
        private static string AuthToken => File.ReadAllText("Authorization.json");
        #endregion
    }
}