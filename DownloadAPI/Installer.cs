using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;

namespace Spaceware
{
    class DownloadAPI
    {
        /// <summary>
        /// Method: InitDownload();
        /// Description: Initializes the installer process. 
        /// Usage: DownloadWorker.InitDownload();
        /// Return Values: 0 = Folder Not Found | 1 = Lastest Version | 2 = ERROR | 3 = Valid Response
        /// </summary>
        public int InitDownload()
        {
            try
            {
                using (var client = new WebClient { Headers = { "Accept: text/html, application/xhtml+xml, */*", "User-Agent: Mozilla/9.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; SpacewareAPI/9.0)" } })
                {
                    if (VRChatFolder() == string.Empty | !vrchatPath.Contains("VRChat")) return 0;
                    ClientVersion = FileVersionInfo.GetVersionInfo($"{vrchatPath}\\Area51\\DLL\\Area51.dll").FileVersion;
                    ServerVersion = client.DownloadString($"{APILink}version");
                    zipPath = $"{Path.GetTempPath()}\\{Guid.NewGuid().ToString("N").Substring(0, 8)}.zip";

                    Console.Clear(); Console.ForegroundColor = ConsoleColor.Green;
                    InstallLog("=======================================================================================================================");
                    InstallLog("                                                     INSTALLER                                                         ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("                                    █████╗ ██████╗ ███████╗ █████╗     ███████╗ ██╗                                    ");
                    InstallLog("                                   ██╔══██╗██╔══██╗██╔════╝██╔══██╗    ██╔════╝███║                                    ");
                    InstallLog("                                   ███████║██████╔╝█████╗  ███████║    ███████╗╚██║                                    ");
                    InstallLog("                                   ██╔══██║██╔══██╗██╔══╝  ██╔══██║    ╚════██║ ██║                                    ");
                    InstallLog("                                   ██║  ██║██║  ██║███████╗██║  ██║    ███████║ ██║                                    ");
                    InstallLog("                                   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝    ╚══════╝ ╚═╝                                    ");
                    InstallLog("                               *____________________________________________________*                                  ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("                                        A Spaced out installer for Area51                                              ");
                    InstallLog("                          The Developers's: Joshua, Maxie, PandaStudios, Pyro & Swordsith                              ");
                    InstallLog($"                        Client Website: https://outerspace.store/ | Client Version: {ClientVersion}                   ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("=======================================================================================================================\n");
                    Console.ForegroundColor = ConsoleColor.White; InstallLog("Checking for update....");

                    /*if server version equals version on disk dont download else download the lastest version!*/
                    if (ServerVersion == ClientVersion) return 1;
                    InstallLog($"Downloading New Update: {ServerVersion}");
                    client.DownloadFile(new Uri($"{APILink}update/{File.ReadAllText("Authorization.json")}"), zipPath);
                    return 3;
                }
            }
            catch (Exception ErrorOnInstall)
            {
                if (ErrorOnInstall.InnerException != null)
                {
                    return 2;
                }
            }
            return 2;
        }

        /// <summary>
        /// Method: InitExtaction();
        /// Description: Initializes the extaction process. 
        /// Usage: DownloadWorker.InitExtaction();
        /// </summary>
        public bool InitExtaction()
        {
            try
            {
                /*if path is not empty install else return exception*/
                if (zipPath == string.Empty)  return false;
                InstallLog("Installing....", true);
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, $"{vrchatPath}");
                return true;
            }
            catch (Exception ErrorOnExtaction)
            {
                if(ErrorOnExtaction.InnerException != null)
                {
                    InstallLog("Something really done goofed on Extaction, please join server and make a ticket.\nDiscord: https://discord.gg/Paul", true);
                }
            }
            return false;
        }

        #region Public Functions / Variables - Log, APILink, VRChatpath ect. these are gloabally called throughout this project.
        public static void InstallLog(string text, bool state = false) { Console.WriteLine(text); if (state == true) { Console.Read(); } }
        public string VRChatFolder() { using (FolderBrowserDialog fdb = new FolderBrowserDialog()) { if (fdb.ShowDialog() == DialogResult.OK) vrchatPath = fdb.SelectedPath; return fdb.SelectedPath; } }
        public readonly string APILink = "https://api.outerspace.store/session/download/";
        public string zipPath, vrchatPath, ClientVersion, ServerVersion = string.Empty;
        #endregion

    }
}
