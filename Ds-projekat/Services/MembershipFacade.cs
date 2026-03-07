using Ds_projekat.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Ds_projekat.Services
{
    internal class MembershipFacade
    {
        private readonly IMembershipTypeRepository _membershipRepository;

        public MembershipFacade()
        {
            _membershipRepository = new MembershipTypeRepository();
        }

        public ServiceResult AddMembershipType(MembershipType mt)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mt.PackageName))
                    return ServiceResult.Fail("Naziv paketa je obavezan.");

                if (mt.Price < 0)
                    return ServiceResult.Fail("Cena ne moze biti negativna.");

                if (mt.DurationDays <= 0)
                    return ServiceResult.Fail("Trajanje mora biti vece od 0.");

                if (mt.MaxReservationHoursPerMonth < 0)
                    return ServiceResult.Fail("Maksimalan broj sati ne moze biti negativan.");

                if (!mt.MeetingRoomAccess && mt.MeetingRoomHoursPerMonth.HasValue)
                    return ServiceResult.Fail("Ako paket nema pristup salama, broj sati sale mora biti prazan.");

                if (mt.MeetingRoomAccess && (!mt.MeetingRoomHoursPerMonth.HasValue || mt.MeetingRoomHoursPerMonth.Value <= 0))
                    return ServiceResult.Fail("Ako paket ima pristup salama, broj sati sale mora biti veci od 0.");

                int id = _membershipRepository.Add(mt);
                return ServiceResult.Ok("Tip clanarine je uspesno dodat.", id);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri dodavanju tipa clanarine: " + ex.Message);
            }
        }

        public ServiceResult UpdateMembershipType(MembershipType mt)
        {
            try
            {
                if (mt.MembershipTypeID <= 0)
                    return ServiceResult.Fail("Neispravan ID tipa clanarine.");

                bool ok = _membershipRepository.Update(mt);
                if (!ok)
                    return ServiceResult.Fail("Tip clanarine nije azuriran.");

                return ServiceResult.Ok("Tip clanarine je uspesno azuriran.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri azuriranju tipa clanarine: " + ex.Message);
            }
        }

        public ServiceResult DeleteMembershipType(int id)
        {
            try
            {
                bool ok = _membershipRepository.Delete(id);
                if (!ok)
                    return ServiceResult.Fail("Tip clanarine nije obrisan.");

                return ServiceResult.Ok("Tip clanarine je uspesno obrisan.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri brisanju tipa clanarine: " + ex.Message);
            }
        }

        public MembershipType GetById(int id)
        {
            return _membershipRepository.GetById(id);
        }

        public List<MembershipType> GetAll()
        {
            return _membershipRepository.GetAll();
        }
    }
}