using Ds_projekat.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Ds_projekat.Services
{
    internal class UserFacade
    {
        private readonly IUserRepository _userRepository;
        private readonly IMembershipTypeRepository _membershipRepository;

        public UserFacade()
        {
            _userRepository = new UserRepository();
            _membershipRepository = new MembershipTypeRepository();
        }

        public ServiceResult AddUser(User user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.FirstName))
                    return ServiceResult.Fail("Ime je obavezno.");

                if (string.IsNullOrWhiteSpace(user.LastName))
                    return ServiceResult.Fail("Prezime je obavezno.");

                if (string.IsNullOrWhiteSpace(user.Email))
                    return ServiceResult.Fail("Email je obavezan.");

                if (_userRepository.GetByEmail(user.Email) != null)
                    return ServiceResult.Fail("Korisnik sa tim email-om vec postoji.");

                MembershipType mt = _membershipRepository.GetById(user.MembershipTypeID);
                if (mt == null)
                    return ServiceResult.Fail("Izabrani tip clanarine ne postoji.");

                if (user.MembershipStartDate == DateTime.MinValue)
                    user.MembershipStartDate = DateTime.Today;

                user.MembershipEndDate = user.MembershipStartDate.AddDays(mt.DurationDays);

                if (string.IsNullOrWhiteSpace(user.AccountStatus))
                    user.AccountStatus = "Active";

                int id = _userRepository.Add(user);
                return ServiceResult.Ok("Korisnik je uspesno dodat.", id);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri dodavanju korisnika: " + ex.Message);
            }
        }

        public ServiceResult UpdateUser(User user)
        {
            try
            {
                if (user.UserID <= 0)
                    return ServiceResult.Fail("Neispravan ID korisnika.");

                if (string.IsNullOrWhiteSpace(user.FirstName))
                    return ServiceResult.Fail("Ime je obavezno.");

                if (string.IsNullOrWhiteSpace(user.LastName))
                    return ServiceResult.Fail("Prezime je obavezno.");

                if (string.IsNullOrWhiteSpace(user.Email))
                    return ServiceResult.Fail("Email je obavezan.");

                MembershipType mt = _membershipRepository.GetById(user.MembershipTypeID);
                if (mt == null)
                    return ServiceResult.Fail("Izabrani tip clanarine ne postoji.");

                if (user.MembershipEndDate < user.MembershipStartDate)
                    return ServiceResult.Fail("Datum isteka mora biti posle datuma pocetka.");

                bool ok = _userRepository.Update(user);
                if (!ok)
                    return ServiceResult.Fail("Korisnik nije azuriran.");

                return ServiceResult.Ok("Korisnik je uspesno azuriran.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri azuriranju korisnika: " + ex.Message);
            }
        }

        public ServiceResult DeleteUser(int id)
        {
            try
            {
                bool ok = _userRepository.Delete(id);
                if (!ok)
                    return ServiceResult.Fail("Korisnik nije obrisan.");

                return ServiceResult.Ok("Korisnik je uspesno obrisan.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri brisanju korisnika: " + ex.Message);
            }
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }
    }
}
