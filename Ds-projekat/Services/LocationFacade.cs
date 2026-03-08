using System;
using System.Collections.Generic;
namespace Ds_projekat.Services
{
    internal class LocationFacade
    {
        private readonly ILocationRepository _locationRepository;

        public LocationFacade()
        {
            _locationRepository = new LocationRepository();
        }

        public ServiceResult AddLocation(Location location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location.LocationName))
                    return ServiceResult.Fail("Naziv lokacije je obavezan.");

                if (string.IsNullOrWhiteSpace(location.AddressName))
                    return ServiceResult.Fail("Adresa je obavezna.");

                if (string.IsNullOrWhiteSpace(location.City))
                    return ServiceResult.Fail("Grad je obavezan.");

                if (string.IsNullOrWhiteSpace(location.WorkingHours))
                    return ServiceResult.Fail("Radno vreme je obavezno.");

                if (location.MaxUsers <= 0)
                    return ServiceResult.Fail("Maksimalan broj korisnika mora biti veci od 0.");

                int id = _locationRepository.Add(location);
                return ServiceResult.Ok("Lokacija je uspesno dodata.", id);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri dodavanju lokacije: " + ex.Message);
            }
        }

        public ServiceResult UpdateLocation(Location location)
        {
            try
            {
                if (location.LocationID <= 0)
                    return ServiceResult.Fail("Neispravan ID lokacije.");

                if (string.IsNullOrWhiteSpace(location.LocationName))
                    return ServiceResult.Fail("Naziv lokacije je obavezan.");

                if (string.IsNullOrWhiteSpace(location.AddressName))
                    return ServiceResult.Fail("Adresa je obavezna.");

                if (string.IsNullOrWhiteSpace(location.City))
                    return ServiceResult.Fail("Grad je obavezan.");

                if (string.IsNullOrWhiteSpace(location.WorkingHours))
                    return ServiceResult.Fail("Radno vreme je obavezno.");

                if (location.MaxUsers <= 0)
                    return ServiceResult.Fail("Maksimalan broj korisnika mora biti veci od 0.");

                bool ok = _locationRepository.Update(location);
                if (!ok)
                    return ServiceResult.Fail("Lokacija nije azurirana.");

                return ServiceResult.Ok("Lokacija je uspesno azurirana.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri azuriranju lokacije: " + ex.Message);
            }
        }

        public ServiceResult DeleteLocation(int id)
        {
            try
            {
                bool ok = _locationRepository.Delete(id);
                if (!ok)
                    return ServiceResult.Fail("Lokacija nije obrisana.");

                return ServiceResult.Ok("Lokacija je uspesno obrisana.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri brisanju lokacije: " + ex.Message);
            }
        }

        public Location GetById(int id)
        {
            return _locationRepository.GetById(id);
        }

        public List<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }
    }
}
