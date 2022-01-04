using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Launcher
{
    class Program
    {
        class Game
        {
            public String Version { get; set; }
            public String Name { get; set; }
            public string UnzippedName { get; set; }
        }

        static Game game;
        static String EXE_PATH => Directory.GetCurrentDirectory() + "/" + game?.UnzippedName + "/DnD.exe";
        static void Main(string[] args)
        {
            var url = "https://shadowingsproteam.zapto.org/ai4vg/dnd.json";
            Console.WriteLine("Checking game version...");

            
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                game = JsonConvert.DeserializeObject<Game>(json);
            }

            if(game == null)
            {
                throw new Exception("Game null");
            }

            bool needsUpdate = false;
            try
            {
                var path = Directory.GetCurrentDirectory() + "/info.json";
                var file = File.ReadAllText(path);
                var cached = JsonConvert.DeserializeObject<Game>(file);
                needsUpdate = cached.Version != game.Version;
            }
            catch (Exception)
            {
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                Console.WriteLine("Downloading new version...");
                using (WebClient wc = new WebClient())
                {
                    var uri = new Uri("https://shadowingsproteam.zapto.org/ai4vg/DnD.zip");
                    wc.DownloadDataCompleted += Wc_DownloadDataCompleted;
                    wc.DownloadDataAsync(uri);
                }
            } else
            {
                Console.WriteLine("Ok.");
                using (Process proc = Process.Start(EXE_PATH))
                {
                    proc.WaitForExit();
                }
            }
        }

        private static void Wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory() + "/DnD.zip";
            Console.WriteLine("Writing zip to path " + path);
            File.WriteAllBytes(path, e.Result);
            try
            {
                Directory.Delete(Directory.GetCurrentDirectory() + "/" + game.UnzippedName, true);
            }
            catch (Exception) { }

            ZipFile.ExtractToDirectory(path, Directory.GetCurrentDirectory() + "/" + game.UnzippedName);

            Console.WriteLine("Extracted");

            File.Delete(Directory.GetCurrentDirectory() + "/DnD.zip");

            File.WriteAllText(Directory.GetCurrentDirectory() + "/info.json", JsonConvert.SerializeObject(game));

            using (Process proc = Process.Start(EXE_PATH))
            {
                proc.WaitForExit();
            }
        }
    }
}
