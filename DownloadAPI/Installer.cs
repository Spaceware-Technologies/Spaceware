using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Spaceware
{
    class DownloadAPI
    {

        /// <summary>
        /// // Method: InitDownload();
        /// // Description: Initializes the installer process. 
        /// // Usage: DownloadWorker.InitDownload();
        /// </summary>
        public bool InitDownload()
        {
            using (var client = new WebClient { Headers = { "Accept: text/html, application/xhtml+xml, */*", "User-Agent: Mozilla/9.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; SpacewareAPI/9.0)" } })
            {
                if (VRChatFolder() == string.Empty || !vrchatPath.Contains("VRChat")) return false;
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

                //if server version equals version on disk dont download else download the lastest version!
                if (ServerVersion == ClientVersion) return false;
                InstallLog($"Downloading New Update: {ServerVersion}");
                client.DownloadFile(new Uri($"{APILink}update/{File.ReadAllText("Authorization.json")}"), zipPath);
                return true;
            }
        }

        /// <summary>
        /// // Method: InitExtaction();
        /// // Description: Initializes the extaction process. 
        /// // Usage: DownloadWorker.InitExtaction();
        /// </summary>
        public bool InitExtaction()
        {
            try
            {
                if (zipPath == string.Empty)  return false;
                InstallLog("Installing....", true);
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, $"{vrchatPath}");
                return true;
            }
            catch (Exception InstallError)
            {
                if(InstallError.InnerException != null)
                {
                    InstallLog("Something really done goofed on install, please join server and make a ticket.", true);
                    return false;
                }
            }
            return false;
        }

        #region Public Functions / Variables - Log, APILink, VRChatpath ect. these are gloabally called throughout this project.
        public static void InstallLog(string text, bool state = false) { Console.WriteLine(text); if (state == true) { Console.Read(); } }
        public string VRChatFolder() { using (FolderBrowserDialog fdb = new FolderBrowserDialog()) { if (fdb.ShowDialog() == DialogResult.OK) vrchatPath = fdb.SelectedPath; return fdb.SelectedPath; } }
        public string APILink = "https://api.outerspace.store/session/";
        public string zipPath, vrchatPath, ClientVersion, ServerVersion = string.Empty;
        #endregion

    }
}
