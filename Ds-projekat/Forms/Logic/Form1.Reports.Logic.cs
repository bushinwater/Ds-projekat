using System;
using System.Windows.Forms;
using Ds_projekat.Services;

namespace Ds_projekat
{
    public partial class Form1
    {
        private void InitializeReportsModule()
        {
            if (btnExportUsers == null)
                return;

            btnExportUsers.Click += btnExportUsers_Click;
            btnExportResources.Click += btnExportResources_Click;
            btnExportLocations.Click += btnExportLocations_Click;
            btnExportMemberships.Click += btnExportMemberships_Click;
            btnExportReservations.Click += btnExportReservations_Click;

            AppendReportStatus("Reports modul je inicijalizovan.");
        }

        private void AppendReportStatus(string message)
        {
            if (txtReportStatus == null)
                return;

            string line = $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {message}";
            txtReportStatus.AppendText(line + Environment.NewLine);
        }

        private string AskCsvPath(string defaultFileName)
        {
            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Sačuvaj CSV fajl";
            sfd.Filter = "CSV files (*.csv)|*.csv";
            sfd.FileName = defaultFileName;

            if (sfd.ShowDialog() != DialogResult.OK)
                return null;

            return sfd.FileName;
        }

        private void btnExportUsers_Click(object sender, EventArgs e)
        {
            try
            {
                string path = AskCsvPath("users.csv");
                if (string.IsNullOrWhiteSpace(path))
                {
                    AppendReportStatus("Export Users otkazan.");
                    return;
                }

                ServiceResult result = _reportFacade.ExportUsersToCsv(path);

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                AppendReportStatus("Users export uspešan: " + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri exportu Users:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                AppendReportStatus("Greška pri Users exportu: " + ex.Message);
            }
        }

        private void btnExportResources_Click(object sender, EventArgs e)
        {
            try
            {
                string path = AskCsvPath("resources.csv");
                if (string.IsNullOrWhiteSpace(path))
                {
                    AppendReportStatus("Export Resources otkazan.");
                    return;
                }

                ServiceResult result = _reportFacade.ExportResourcesToCsv(path);

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                AppendReportStatus("Resources export uspešan: " + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri exportu Resources:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                AppendReportStatus("Greška pri Resources exportu: " + ex.Message);
            }
        }

        private void btnExportLocations_Click(object sender, EventArgs e)
        {
            try
            {
                string path = AskCsvPath("locations.csv");
                if (string.IsNullOrWhiteSpace(path))
                {
                    AppendReportStatus("Export Locations otkazan.");
                    return;
                }

                ServiceResult result = _reportFacade.ExportLocationsToCsv(path);

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                AppendReportStatus("Locations export uspešan: " + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri exportu Locations:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                AppendReportStatus("Greška pri Locations exportu: " + ex.Message);
            }
        }

        private void btnExportMemberships_Click(object sender, EventArgs e)
        {
            try
            {
                string path = AskCsvPath("membership_types.csv");
                if (string.IsNullOrWhiteSpace(path))
                {
                    AppendReportStatus("Export Membership Types otkazan.");
                    return;
                }

                ServiceResult result = _reportFacade.ExportMembershipTypesToCsv(path);

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                AppendReportStatus("Membership Types export uspešan: " + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri exportu Membership Types:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                AppendReportStatus("Greška pri Membership Types exportu: " + ex.Message);
            }
        }

        private void btnExportReservations_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtReportReservationUserId.Text.Trim(), out int userId))
                    throw new Exception("User ID mora biti ceo broj.");

                string path = AskCsvPath("reservations_user_" + userId + ".csv");
                if (string.IsNullOrWhiteSpace(path))
                {
                    AppendReportStatus("Export Reservations otkazan.");
                    return;
                }

                ServiceResult result = _reportFacade.ExportReservationsByUserToCsv(userId, path);

                if (!result.Success)
                    throw new Exception(result.Message);

                MessageBox.Show(
                    result.Message,
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                AppendReportStatus("Reservations export uspešan za UserID=" + userId + ": " + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri exportu Reservations:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                AppendReportStatus("Greška pri Reservations exportu: " + ex.Message);
            }
        }
    }
}