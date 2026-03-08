using System;
using System.Collections.Generic;

namespace Ds_projekat.Services
{
    internal class LocationFacade
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IReservationRepository _reservationRepository;

        public LocationFacade()
        {
            _locationRepository = new LocationRepository();
            _resourceRepository = new ResourceRepository();
            _reservationRepository = new ReservationRepository();
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

        public ServiceResult DeleteLocationWithCheck(int locationId, bool deleteResourcesToo)
        {
            try
            {
                Location location = _locationRepository.GetById(locationId);
                if (location == null)
                    return ServiceResult.Fail("Lokacija ne postoji.");

                bool hasResources = _resourceRepository.LocationHasResources(locationId);

                if (hasResources && !deleteResourcesToo)
                {
                    return ServiceResult.Fail("Lokacija ima resurse i ne može biti obrisana dok se oni ne obrišu.");
                }

                if (hasResources && deleteResourcesToo)
                {
                    // prvo rezervacije za te resurse
                    _reservationRepository.DeleteByLocationId(locationId);

                    // onda resursi i detalji
                    _resourceRepository.DeleteByLocationId(locationId);
                }

                bool deleted = _locationRepository.Delete(locationId);

                if (!deleted)
                    return ServiceResult.Fail("Lokacija nije obrisana.");

                if (hasResources && deleteResourcesToo)
                    return ServiceResult.Ok("Lokacija i svi njeni resursi su uspešno obrisani.");

                return ServiceResult.Ok("Lokacija je uspešno obrisana.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greška pri brisanju lokacije: " + ex.Message);
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

        public bool LocationHasResources(int locationId)
        {
            return _resourceRepository.LocationHasResources(locationId);
        }
    }
}