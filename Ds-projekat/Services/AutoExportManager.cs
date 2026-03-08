using System;
using System.Collections.Generic;
using System.IO;

namespace Ds_projekat.Services
{
    internal class AutoExportManager
    {
        private readonly string _settingsFilePath;
        private readonly ReportFacade _reportFacade;

        public AutoExportManager()
        {
            _settingsFilePath = "auto_export_settings.json";
            _reportFacade = new ReportFacade();
        }

        public AutoExportSettings LoadSettings()
        {
            AutoExportSettings settings = new AutoExportSettings();

            if (!File.Exists(_settingsFilePath))
                return settings;

            string[] lines = File.ReadAllLines(_settingsFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length != 2)
                    continue;

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                switch (key)
                {
                    case "enabled":
                        settings.Enabled = bool.Parse(value);
                        break;

                    case "intervalDays":
                        settings.IntervalDays = int.Parse(value);
                        break;

                    case "folder":
                        settings.ExportFolderPath = value;
                        break;

                    case "lastExport":
                        settings.LastExportDate = DateTime.Parse(value);
                        break;
                }
            }

            return settings;
        }

        public void SaveSettings(AutoExportSettings settings)
        {
            var lines = new List<string>
            {
                "enabled=" + settings.Enabled,
                "intervalDays=" + settings.IntervalDays,
                "folder=" + settings.ExportFolderPath,
                "lastExport=" + settings.LastExportDate.ToString("o")
            };

            File.WriteAllLines(_settingsFilePath, lines);
        }

        public DateTime GetNextExportDate(AutoExportSettings settings)
        {
            if (settings.LastExportDate == DateTime.MinValue)
                return DateTime.Today;

            return settings.LastExportDate.Date.AddDays(settings.IntervalDays);
        }

        public bool ShouldExport(AutoExportSettings settings)
        {
            if (!settings.Enabled)
                return false;

            if (settings.IntervalDays <= 0)
                return false;

            DateTime nextExport = GetNextExportDate(settings);
            return DateTime.Now.Date >= nextExport.Date;
        }

        public ServiceResult RunAutoExportIfNeeded()
        {
            try
            {
                AutoExportSettings settings = LoadSettings();

                if (!ShouldExport(settings))
                    return ServiceResult.Fail("Nije vreme za automatski export.");

                if (string.IsNullOrWhiteSpace(settings.ExportFolderPath))
                    return ServiceResult.Fail("Folder za export nije podešen.");

                if (!Directory.Exists(settings.ExportFolderPath))
                    Directory.CreateDirectory(settings.ExportFolderPath);

                string dateSuffix = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                ServiceResult usersResult = _reportFacade.ExportUsersToCsv(
                    Path.Combine(settings.ExportFolderPath, "users_" + dateSuffix + ".csv"));

                ServiceResult resourcesResult = _reportFacade.ExportResourcesToCsv(
                    Path.Combine(settings.ExportFolderPath, "resources_" + dateSuffix + ".csv"));

                ServiceResult locationsResult = _reportFacade.ExportLocationsToCsv(
                    Path.Combine(settings.ExportFolderPath, "locations_" + dateSuffix + ".csv"));

                ServiceResult membershipResult = _reportFacade.ExportMembershipTypesToCsv(
                    Path.Combine(settings.ExportFolderPath, "membership_types_" + dateSuffix + ".csv"));

                settings.LastExportDate = DateTime.Now;
                SaveSettings(settings);

                return ServiceResult.Ok("Automatski export je uspešno izvršen.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greška pri automatskom exportu: " + ex.Message);
            }
        }
    }
}