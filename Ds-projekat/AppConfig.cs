using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat
{
    // sealed = ne moze se naslediti
    internal sealed class AppConfig
    {
        // Lazy = pravi se onda kada prvi put zatrebas
        private static readonly Lazy<AppConfig> _instance = new Lazy<AppConfig>(() => new AppConfig());
        // => je isto kao i get { return _instance.Value; }
        public static AppConfig Instance => _instance.Value;

        // znaci da svako moze da ga vidi, samo ova klasa moze da ga setuje, default vrednost je = ""
        public string BrandName
        {
            get;
            private set;
        } = "";
        public string ConnectionString 
        { 
            get; 
            private set; 
        } = "";

        private bool _loaded;
        private AppConfig() { }

        public void Load(string path = "config.txt")
        {
            if (_loaded) return;
            if (!File.Exists(path))
                throw new FileNotFoundException($"Path doesn't exists: {path}! Put it in the same folder as .exe file (bin/Debug)");

            var lines = File.ReadAllLines(path);

            if (lines.Length < 2)
                throw new InvalidOperationException("config.txt file has less then 2 lines");

            BrandName = lines[0].Trim();
            ConnectionString = lines[1].Trim();

            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new InvalidOperationException("Second line (connection string) is empty");

            _loaded = true;
        }
    }
}
