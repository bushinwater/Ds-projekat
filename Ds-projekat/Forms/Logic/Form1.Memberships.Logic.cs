using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private void InitializeMembershipsModule()
        {
            if (dgvMembershipTypes == null)
                return;

            dgvMembershipTypes.AutoGenerateColumns = true;
            dgvMembershipTypes.SelectionChanged += dgvMembershipTypes_SelectionChanged;

            btnMembershipAdd.Click += btnMembershipAdd_Click;
            btnMembershipUpdate.Click += btnMembershipUpdate_Click;
            btnMembershipDelete.Click += btnMembershipDelete_Click;
            btnMembershipClear.Click += btnMembershipClear_Click;
            btnMembershipRefresh.Click += btnMembershipRefresh_Click;

            chkMeetingRoomAccess.CheckedChanged += chkMeetingRoomAccess_CheckedChanged;

            LoadMembershipTypes();
            ClearMembershipForm();
        }

        private void LoadMembershipTypes()
        {
            try
            {
                _allMembershipTypesGrid = _membershipFacade.GetAll();

                dgvMembershipTypes.DataSource = null;
                dgvMembershipTypes.DataSource = _allMembershipTypesGrid;

                FormatMembershipsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri učitavanju tipova članstva:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void FormatMembershipsGrid()
        {
            if (dgvMembershipTypes.Columns.Count == 0) return;

            if (dgvMembershipTypes.Columns.Contains("MembershipTypeID"))
                dgvMembershipTypes.Columns["MembershipTypeID"].HeaderText = "ID";

            if (dgvMembershipTypes.Columns.Contains("PackageName"))
                dgvMembershipTypes.Columns["PackageName"].HeaderText = "Package Name";

            if (dgvMembershipTypes.Columns.Contains("Price"))
                dgvMembershipTypes.Columns["Price"].HeaderText = "Price";

            if (dgvMembershipTypes.Columns.Contains("DurationDays"))
                dgvMembershipTypes.Columns["DurationDays"].HeaderText = "Duration Days";

            if (dgvMembershipTypes.Columns.Contains("MaxReservationHoursPerMonth"))
                dgvMembershipTypes.Columns["MaxReservationHoursPerMonth"].HeaderText = "Max Hours / Month";

            if (dgvMembershipTypes.Columns.Contains("MeetingRoomAccess"))
                dgvMembershipTypes.Columns["MeetingRoomAccess"].HeaderText = "Meeting Room Access";

            if (dgvMembershipTypes.Columns.Contains("MeetingRoomHoursPerMonth"))
                dgvMembershipTypes.Columns["MeetingRoomHoursPerMonth"].HeaderText = "Meeting Room Hours";
        }

        private void dgvMembershipTypes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMembershipTypes.CurrentRow == null) return;
            if (dgvMembershipTypes.CurrentRow.DataBoundItem is not MembershipType membership) return;

            FillMembershipForm(membership);
        }

        private void FillMembershipForm(MembershipType membership)
        {
            txtMembershipTypeId.Text = membership.MembershipTypeID.ToString();
            txtPackageName.Text = membership.PackageName;
            txtPackagePrice.Text = membership.Price.ToString();
            txtDurationDays.Text = membership.DurationDays.ToString();
            txtMaxHoursMonth.Text = membership.MaxReservationHoursPerMonth.ToString();
            chkMeetingRoomAccess.Checked = membership.MeetingRoomAccess;
            txtMeetingRoomHours.Text = membership.MeetingRoomHoursPerMonth?.ToString() ?? "";
            UpdateMeetingRoomHoursState();
        }

        private MembershipType ReadMembershipFromForm()
        {
            if (string.IsNullOrWhiteSpace(txtPackageName.Text))
                throw new Exception("Package Name je obavezan.");

            if (!decimal.TryParse(txtPackagePrice.Text.Trim(), out decimal price))
                throw new Exception("Price mora biti broj.");

            if (!int.TryParse(txtDurationDays.Text.Trim(), out int durationDays))
                throw new Exception("Duration Days mora biti ceo broj.");

            if (!int.TryParse(txtMaxHoursMonth.Text.Trim(), out int maxHoursMonth))
                throw new Exception("Max Hours/Month mora biti ceo broj.");

            int? meetingRoomHours = null;

            if (chkMeetingRoomAccess.Checked)
            {
                if (!int.TryParse(txtMeetingRoomHours.Text.Trim(), out int parsedMeetingRoomHours))
                    throw new Exception("Meeting Room Hours mora biti ceo broj.");

                meetingRoomHours = parsedMeetingRoomHours;
            }

            MembershipType membership = new MembershipType
            {
                PackageName = txtPackageName.Text.Trim(),
                Price = price,
                DurationDays = durationDays,
                MaxReservationHoursPerMonth = maxHoursMonth,
                MeetingRoomAccess = chkMeetingRoomAccess.Checked,
                MeetingRoomHoursPerMonth = meetingRoomHours
            };

            if (int.TryParse(txtMembershipTypeId.Text, out int membershipId))
                membership.MembershipTypeID = membershipId;

            return membership;
        }

        private void ClearMembershipForm()
        {
            txtMembershipTypeId.Clear();
            txtPackageName.Clear();
            txtPackagePrice.Clear();
            txtDurationDays.Clear();
            txtMaxHoursMonth.Clear();
            txtMeetingRoomHours.Clear();

            chkMeetingRoomAccess.Checked = false;

            UpdateMeetingRoomHoursState();
            dgvMembershipTypes.ClearSelection();
        }

        private void UpdateMeetingRoomHoursState()
        {
            txtMeetingRoomHours.Enabled = chkMeetingRoomAccess.Checked;

            if (!chkMeetingRoomAccess.Checked)
                txtMeetingRoomHours.Clear();
        }

        private void chkMeetingRoomAccess_CheckedChanged(object sender, EventArgs e)
        {
            UpdateMeetingRoomHoursState();
        }

        private void btnMembershipAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipType membership = ReadMembershipFromForm();
                membership.MembershipTypeID = 0;

                _membershipFacade.AddMembershipType(membership);

                MessageBox.Show(
                    "Tip članstva je uspešno dodat.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadMembershipTypes();
                ClearMembershipForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri dodavanju tipa članstva:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnMembershipUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMembershipTypeId.Text, out _))
                    throw new Exception("Odaberite tip članstva za izmenu.");

                MembershipType membership = ReadMembershipFromForm();
                _membershipFacade.UpdateMembershipType(membership);

                MessageBox.Show(
                    "Tip članstva je uspešno izmenjen.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadMembershipTypes();
                ClearMembershipForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri izmeni tipa članstva:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnMembershipDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMembershipTypeId.Text, out int membershipId))
                    throw new Exception("Odaberite tip članstva za brisanje.");

                DialogResult result = MessageBox.Show(
                    "Da li sigurno želite da obrišete tip članstva?",
                    "Potvrda brisanja",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return;

                _membershipFacade.DeleteMembershipType(membershipId);

                MessageBox.Show(
                    "Tip članstva je uspešno obrisan.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadMembershipTypes();
                ClearMembershipForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri brisanju tipa članstva:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnMembershipClear_Click(object sender, EventArgs e)
        {
            ClearMembershipForm();
        }

        private void btnMembershipRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadMembershipTypes();
                ClearMembershipForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Greška pri osvežavanju tipova članstva:\n" + ex.Message,
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}