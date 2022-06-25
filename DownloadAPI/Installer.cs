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
        /// Method: InitDownload();
        /// Description: Initializes the installer process. 
        /// Usage: DownloadWorker.InitDownload();
        /// Return Values: 0 = Folder Not Found | 1 = Lastest Version | 2 = ERROR | 3 = Valid Response
        /// </summary>
        public int InitDownload()
        {
            try
            {
                using (var client = new WebClient { Headers = { "Accept: text/html, application/xhtml+xml, */*", "User-Agent: SpacewareAPI/9.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; SpacewareAPI/9.0)" } })
                {
                    /*if selected folder path is Empty or doesnt = VRChat return folder not found*/
                    if (VRChatFolder() == string.Empty | !vrchatPath.Contains("VRChat")) return 0;

                    /*Version Info / Downlaod Path*/
                    if (File.Exists($"{vrchatPath}\\Area51\\DLL\\Area51.dll"))
                    {
                        ClientVersion = FileVersionInfo.GetVersionInfo($"{vrchatPath}\\Area51\\DLL\\Area51.dll").FileVersion;
                    }
                    else { ClientVersion = "1.0.0.0"; }
                   
                    ServerVersion = client.DownloadString($"{APILink}version");

                    zipPath = $"{Guid.NewGuid().ToString("N").Substring(0, 8)}.zip";
                    Console.ForegroundColor = ConsoleColor.Green;
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

                    /*If server version equals version on disk dont download else download the lastest version!*/
                    if (ServerVersion == ClientVersion) return 1;

                    /*removes older melonloader Folder/proxy.dll*/
                    Console.ForegroundColor = ConsoleColor.Yellow; InstallLog("Initializing clean up....");
                    if (File.Exists($"{vrchatPath}\\version.dll") || File.Exists($"{vrchatPath}\\Mods\\SpaceShip.dll") || File.Exists($"{vrchatPath}\\Mods\\AstralCore.dll"))
                    {
                        File.Delete($"{vrchatPath}\\version.dll");
                        File.Delete($"{vrchatPath}\\Mods\\SpaceShip.dll");
                        File.Delete($"{vrchatPath}\\Mods\\AstralCore.dll");
                        InstallLog("Done!, Removed old dynamic link libraries....");
                    }

                    if (Directory.Exists($"{vrchatPath}\\Area51") || Directory.Exists($"{vrchatPath}\\MelonLoader") || Directory.Exists($"{vrchatPath}\\UserData\\Icons") || Directory.Exists($"{vrchatPath}\\Plugins"))
                    {
                        Directory.Delete($"{vrchatPath}\\Area51", true);
                        Directory.Delete($"{vrchatPath}\\MelonLoader", true);
                        Directory.Delete($"{vrchatPath}\\UserData\\Icons", true);
                        Directory.Delete($"{vrchatPath}\\Plugins", true);
                        InstallLog("Done!, Removed old melon files...."); Console.ForegroundColor = ConsoleColor.White;
                    }
                    InstallLog($"Downloading New Update: {ServerVersion}");
                    client.DownloadFile(new Uri($"{APILink}download/update/{File.ReadAllText("Authorization.json")}"), zipPath);
                    return 3;
                }
            }
            catch (Exception ErrorOnInstall) { Console.WriteLine($"[Install]{ErrorOnInstall.StackTrace}"); if (ErrorOnInstall.StackTrace != null) {  return 2; } }
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
                if (zipPath == string.Empty) return false;
                InstallLog("Installing....", true);
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, $"{vrchatPath}");
                return true;
            }
            catch (Exception ErrorOnExtaction)
            {
                if (ErrorOnExtaction.InnerException != null)
                {
                    Console.WriteLine("[Extaction]{ErrorOnExtaction.StackTrace}"); InstallLog("Something really done goofed on Extaction, please join server and make a ticket.\nDiscord: https://discord.gg/Paul", true);
                }
            }
            return false;
        }

        /// <summary>
        /// Method: ValidateToken(string token);
        /// Description: Checks validity of the token. 
        /// Usage: DownloadWorker.ValidateToken("A513-A512-A514");
        /// </summary>
        public bool ValidateToken(string token)
        {
            try
            {
                using (var client = new WebClient { Headers = { "Accept: text/html, application/xhtml+xml, */*", "User-Agent: SpacewareAPI/9.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; SpacewareAPI/9.0)" } })
                {
                    if (client.DownloadString($"{APILink}check/{token}").Contains("True")) return true;                   
                }
            }
            catch { }
            return false;
        }

        public string VRChatFolder() 
        { 
            using (OpenFileDialog fdb = new OpenFileDialog()) 
            { 
                if (fdb.ShowDialog() == DialogResult.OK)
                {
                    fdb.Title = "Find and Select VRChat.exe";
                    fdb.ValidateNames = false;
                    fdb.CheckFileExists = false;
                    fdb.CheckPathExists = true;
                    fdb.RestoreDirectory = true;
                    vrchatPath = Path.GetDirectoryName(fdb.FileName);
                }
                return vrchatPath;
            } 
        }

        #region Public Functions / Variables - Log, APILink, VRChatpath ect. these are gloabally called throughout this project.
        public static void InstallLog(string text, bool state = false) { Console.WriteLine(text); if (state) { Console.Read(); } }
        public readonly string APILink = "https://api.outerspace.store/session/";
        public string zipPath, vrchatPath, ClientVersion, ServerVersion = string.Empty;
        #endregion

    }
}
