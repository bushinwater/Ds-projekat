using System;
using System.Collections.Generic;

namespace Ds_projekat.Services
{
    internal class ResourceFacade
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ILocationRepository _locationRepository;

        public ResourceFacade()
        {
            _resourceRepository = new ResourceRepository();
            _locationRepository = new LocationRepository();
        }

        public ServiceResult AddDesk(Resource resource, DeskDetails desk)
        {
            try
            {
                if (resource.LocationID <= 0)
                    return ServiceResult.Fail("Izaberi lokaciju.");

                if (_locationRepository.GetById(resource.LocationID) == null)
                    return ServiceResult.Fail("Lokacija ne postoji.");

                if (string.IsNullOrWhiteSpace(resource.ResourceName))
                    return ServiceResult.Fail("Naziv resursa je obavezan.");

                resource.ResourceType = "Desk";

                if (desk == null || string.IsNullOrWhiteSpace(desk.DeskSubType))
                    return ServiceResult.Fail("Tip stola je obavezan.");

                int id = _resourceRepository.AddResource(resource, desk, null);
                return ServiceResult.Ok("Desk resurs je uspesno dodat.", id);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri dodavanju desk resursa: " + ex.Message);
            }
        }

        public ServiceResult AddRoom(Resource resource, RoomDetails room)
        {
            try
            {
                if (resource.LocationID <= 0)
                    return ServiceResult.Fail("Izaberi lokaciju.");

                if (_locationRepository.GetById(resource.LocationID) == null)
                    return ServiceResult.Fail("Lokacija ne postoji.");

                if (string.IsNullOrWhiteSpace(resource.ResourceName))
                    return ServiceResult.Fail("Naziv resursa je obavezan.");

                resource.ResourceType = "Room";

                if (room == null)
                    return ServiceResult.Fail("Detalji prostorije su obavezni.");

                if (room.Capacity <= 0)
                    return ServiceResult.Fail("Kapacitet prostorije mora biti veci od 0.");

                int id = _resourceRepository.AddResource(resource, null, room);
                return ServiceResult.Ok("Room resurs je uspesno dodat.", id);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri dodavanju room resursa: " + ex.Message);
            }
        }

        public ServiceResult AddPrivateOffice(Resource resource)
        {
            try
            {
                if (resource.LocationID <= 0)
                    return ServiceResult.Fail("Izaberi lokaciju.");

                if (_locationRepository.GetById(resource.LocationID) == null)
                    return ServiceResult.Fail("Lokacija ne postoji.");

                if (string.IsNullOrWhiteSpace(resource.ResourceName))
                    return ServiceResult.Fail("Naziv resursa je obavezan.");

                // Privatna kancelarija nema dodatnu detail tabelu u ovoj verziji projekta.
                resource.ResourceType = "Private Office";

                int id = _resourceRepository.AddResource(resource, null, null);
                return ServiceResult.Ok("Private office je uspesno dodat.", id);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri dodavanju private office resursa: " + ex.Message);
            }
        }

        public ServiceResult UpdateDesk(Resource resource, DeskDetails desk)
        {
            try
            {
                if (resource.ResourceID <= 0)
                    return ServiceResult.Fail("Izaberi resurs za azuriranje.");

                if (resource.LocationID <= 0)
                    return ServiceResult.Fail("Izaberi lokaciju.");

                if (desk == null || string.IsNullOrWhiteSpace(desk.DeskSubType))
                    return ServiceResult.Fail("Tip stola je obavezan.");

                resource.ResourceType = "Desk";
                bool ok = _resourceRepository.UpdateResource(resource, desk, null);

                if (!ok)
                    return ServiceResult.Fail("Desk resurs nije azuriran.");

                return ServiceResult.Ok("Desk resurs je uspesno azuriran.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri azuriranju desk resursa: " + ex.Message);
            }
        }

        public ServiceResult UpdateRoom(Resource resource, RoomDetails room)
        {
            try
            {
                if (resource.ResourceID <= 0)
                    return ServiceResult.Fail("Izaberi resurs za azuriranje.");

                if (resource.LocationID <= 0)
                    return ServiceResult.Fail("Izaberi lokaciju.");

                if (room == null || room.Capacity <= 0)
                    return ServiceResult.Fail("Kapacitet prostorije mora biti veci od 0.");

                resource.ResourceType = "Room";
                bool ok = _resourceRepository.UpdateResource(resource, null, room);

                if (!ok)
                    return ServiceResult.Fail("Room resurs nije azuriran.");

                return ServiceResult.Ok("Room resurs je uspesno azuriran.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri azuriranju room resursa: " + ex.Message);
            }
        }

        public ServiceResult UpdatePrivateOffice(Resource resource)
        {
            try
            {
                if (resource.ResourceID <= 0)
                    return ServiceResult.Fail("Izaberi resurs za azuriranje.");

                if (resource.LocationID <= 0)
                    return ServiceResult.Fail("Izaberi lokaciju.");

                if (string.IsNullOrWhiteSpace(resource.ResourceName))
                    return ServiceResult.Fail("Naziv resursa je obavezan.");

                resource.ResourceType = "Private Office";
                bool ok = _resourceRepository.UpdateResource(resource, null, null);

                if (!ok)
                    return ServiceResult.Fail("Private office resurs nije azuriran.");

                return ServiceResult.Ok("Private office resurs je uspesno azuriran.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri azuriranju private office resursa: " + ex.Message);
            }
        }

        public ServiceResult DeleteResource(int resourceId)
        {
            try
            {
                bool ok = _resourceRepository.DeleteResource(resourceId);
                if (!ok)
                    return ServiceResult.Fail("Resurs nije obrisan.");

                return ServiceResult.Ok("Resurs je uspesno obrisan.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Greska pri brisanju resursa: " + ex.Message);
            }
        }

        public Resource GetResource(int resourceId)
        {
            return _resourceRepository.GetResource(resourceId);
        }

        public DeskDetails GetDeskDetails(int resourceId)
        {
            return _resourceRepository.GetDeskDetails(resourceId);
        }

        public RoomDetails GetRoomDetails(int resourceId)
        {
            return _resourceRepository.GetRoomDetails(resourceId);
        }

        public List<Resource> GetAllResources()
        {
            return _resourceRepository.GetAllResources();
        }

        public List<Resource> GetResourcesByLocation(int locationId)
        {
            return _resourceRepository.GetResourcesByLocation(locationId);
        }
    }
}
