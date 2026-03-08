using System;
using System.Collections.Generic;

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
                if (admin.UserID <= 0)
                    return ServiceResult.Fail("Moras izabrati korisnika za admin nalog.");

                User user = _userRepository.GetById(admin.UserID);
                if (user == null)
                    return ServiceResult.Fail("Korisnik za admin nalog ne postoji.");

                if (!RoleContainsAdmin(admin.RoleName))
                    return ServiceResult.Fail("Role mora da sadrzi rec 'admin'.");

                if (string.IsNullOrWhiteSpace(admin.Username))
                    return ServiceResult.Fail("Username je obavezan.");

                if (string.IsNullOrWhiteSpace(admin.HashedPass))
                    return ServiceResult.Fail("Lozinka je obavezna.");

                if (_adminRepository.GetByUserId(admin.UserID) != null)
                    return ServiceResult.Fail("Za izabranog korisnika vec postoji admin nalog.");

                if (_adminRepository.GetByUsername(admin.Username) != null)
                    return ServiceResult.Fail("Vec postoji admin sa tim username-om.");

                admin.HashedPass = ProtectPasswordForStorage(admin.HashedPass);

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
                    return ServiceResult.Fail("Admin nalog ne postoji.");

                if (!RoleContainsAdmin(admin.RoleName))
                    return ServiceResult.Fail("Role mora da sadrzi rec 'admin'.");

                if (string.IsNullOrWhiteSpace(admin.Username))
                    return ServiceResult.Fail("Username je obavezan.");

                if (string.IsNullOrWhiteSpace(admin.HashedPass))
                    return ServiceResult.Fail("Lozinka je obavezna.");

                Admin adminWithSameUsername = _adminRepository.GetByUsername(admin.Username);
                if (adminWithSameUsername != null && adminWithSameUsername.UserID != admin.UserID)
                    return ServiceResult.Fail("Vec postoji admin sa tim username-om.");

                admin.HashedPass = ProtectPasswordForStorage(admin.HashedPass);

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

        public Admin GetByUserId(int userId)
        {
            return _adminRepository.GetByUserId(userId);
        }

        public Admin GetByUsername(string username)
        {
            return _adminRepository.GetByUsername(username);
        }

        public List<Admin> GetAllAdmins()
        {
            return _adminRepository.GetAll();
        }

        public ServiceResult AuthenticateAdmin(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    return ServiceResult.Fail("Username je obavezan.");

                if (string.IsNullOrWhiteSpace(password))
                    return ServiceResult.Fail("Lozinka je obavezna.");

                Admin admin = _adminRepository.GetByUsername(username.Trim());
                if (admin == null)
                    return ServiceResult.Fail("Admin nalog ne postoji.");

                if (!RoleContainsAdmin(admin.RoleName))
                    return ServiceResult.Fail("Nalog nema admin prava.");

                if (!PasswordMatches(admin, password))
                    return ServiceResult.Fail("Pogresna lozinka.");

                return ServiceResult.Ok("Uspesna prijava.", admin.UserID);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri prijavi admina: " + ex.Message);
            }
        }

        private static bool RoleContainsAdmin(string roleName)
        {
            return !string.IsNullOrWhiteSpace(roleName) &&
                   roleName.IndexOf("admin", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string ProtectPasswordForStorage(string password)
        {
            // TODO: ovde kasnije ubaciti hash lozinke pre snimanja u bazu.
            return password.Trim();
        }

        private static bool PasswordMatches(Admin admin, string enteredPassword)
        {
            // TODO: ovde kasnije proveravati hash umesto plain-text vrednosti.
            return string.Equals(
                admin.HashedPass,
                ProtectPasswordForStorage(enteredPassword),
                StringComparison.Ordinal);
        }
    }
}
