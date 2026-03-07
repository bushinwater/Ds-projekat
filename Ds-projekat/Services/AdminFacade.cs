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

                if (string.IsNullOrWhiteSpace(admin.Username))
                    return ServiceResult.Fail("Username je obavezan.");

                if (_adminRepository.GetByUsername(admin.Username) != null)
                    return ServiceResult.Fail("Vec postoji admin sa tim username-om.");

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
    }
}