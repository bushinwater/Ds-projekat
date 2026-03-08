using System;

namespace Ds_projekat.Services
{
    internal class AdminFacade
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository;

        public AdminFacade()
        {
            _adminRepository = new AdminRepository();
            _userRepository = new UserRepository();
        }

        public ServiceResult AddAdmin(Admin admin)
        {
            try
            {
                User user = _userRepository.GetById(admin.UserID);
                if (user == null)
                    return ServiceResult.Fail("Korisnik za admin nalog ne postoji.");

                if (string.IsNullOrWhiteSpace(admin.RoleName))
                    return ServiceResult.Fail("RoleName je obavezan.");

                if (string.IsNullOrWhiteSpace(admin.Username))
                    return ServiceResult.Fail("Username je obavezan.");

                if (string.IsNullOrWhiteSpace(admin.HashedPass))
                    return ServiceResult.Fail("Lozinka je obavezna.");

                if (_adminRepository.GetByUsername(admin.Username.Trim()) != null)
                    return ServiceResult.Fail("Vec postoji admin sa tim username-om.");

                admin.RoleName = admin.RoleName.Trim();
                admin.Username = admin.Username.Trim();
                admin.HashedPass = PasswordHelper.HashPassword(admin.HashedPass);

                bool ok = _adminRepository.Add(admin);
                if (!ok)
                    return ServiceResult.Fail("Admin nije dodat.");

                return ServiceResult.Ok("Admin je uspesno dodat.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri dodavanju admina: " + ex.Message);
            }
        }

        public ServiceResult UpdateAdmin(Admin admin)
        {
            try
            {
                Admin existing = _adminRepository.GetByUserId(admin.UserID);
                if (existing == null)
                    return ServiceResult.Fail("Admin ne postoji.");

                if (string.IsNullOrWhiteSpace(admin.RoleName))
                    return ServiceResult.Fail("RoleName je obavezan.");

                if (string.IsNullOrWhiteSpace(admin.Username))
                    return ServiceResult.Fail("Username je obavezan.");

                Admin sameUsernameOwner = _adminRepository.GetByUsername(admin.Username.Trim());
                if (sameUsernameOwner != null && sameUsernameOwner.UserID != admin.UserID)
                    return ServiceResult.Fail("Vec postoji admin sa tim username-om.");

                admin.RoleName = admin.RoleName.Trim();
                admin.Username = admin.Username.Trim();

                // Ako nije uneta nova lozinka, ostavi staru hash vrednost
                if (string.IsNullOrWhiteSpace(admin.HashedPass))
                    admin.HashedPass = existing.HashedPass;
                else
                    admin.HashedPass = PasswordHelper.HashPassword(admin.HashedPass);

                bool ok = _adminRepository.Update(admin);
                if (!ok)
                    return ServiceResult.Fail("Admin nije azuriran.");

                return ServiceResult.Ok("Admin je uspesno azuriran.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri azuriranju admina: " + ex.Message);
            }
        }

        public ServiceResult DeleteAdmin(int userId)
        {
            try
            {
                bool ok = _adminRepository.Delete(userId);
                if (!ok)
                    return ServiceResult.Fail("Admin nije obrisan.");

                return ServiceResult.Ok("Admin je uspesno obrisan.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri brisanju admina: " + ex.Message);
            }
        }

        public Admin Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Unesite username.");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Unesite lozinku.");

            var admin = _adminRepository.GetByUsername(username.Trim());
            if (admin == null)
                throw new Exception("Admin nalog ne postoji u bazi.");

            if (!PasswordHelper.VerifyPassword(password, admin.HashedPass))
                throw new Exception("Pogrešna lozinka.");

            return admin;
        }

        public Admin GetByUserId(int userId)
        {
            return _adminRepository.GetByUserId(userId);
        }

        public Admin GetByUsername(string username)
        {
            return _adminRepository.GetByUsername(username);
        }
    }
}