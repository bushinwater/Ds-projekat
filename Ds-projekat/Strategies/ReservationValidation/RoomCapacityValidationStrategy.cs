namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class RoomCapacityValidationStrategy : IReservationValidationStrategy
    {
        private readonly IResourceRepository _resourceRepository;

        public RoomCapacityValidationStrategy()
        {
            _resourceRepository = new ResourceRepository();
        }

        public ReservationValidationResult Validate(ReservationRequest request)
        {
            Resource resource = _resourceRepository.GetResource(request.ResourceID);

            if (resource == null)
                return ReservationValidationResult.Fail("Resurs ne postoji.");

            if (resource.ResourceType != "Room")
                return ReservationValidationResult.Success();

            RoomDetails room = _resourceRepository.GetRoomDetails(request.ResourceID);

            if (room == null)
                return ReservationValidationResult.Fail("Detalji prostorije ne postoje.");

            if (request.UsersCount.HasValue && request.UsersCount.Value > room.Capacity)
                return ReservationValidationResult.Fail("Broj korisnika je veci od kapaciteta prostorije.");

            return ReservationValidationResult.Success();
        }
    }
}