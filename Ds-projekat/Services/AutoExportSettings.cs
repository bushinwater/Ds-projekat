using System;

namespace Ds_projekat.Services
{
    internal class AutoExportSettings
    {
        public bool Enabled { get; set; }
        public int IntervalDays { get; set; }
        public string ExportFolderPath { get; set; }
        public DateTime LastExportDate { get; set; }

        public AutoExportSettings()
        {
            Enabled = false;
            IntervalDays = 1;
            ExportFolderPath = "";
            LastExportDate = DateTime.MinValue;
        }
    }
}