namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class ResourceActiveValidationStrategy : IReservationValidationStrategy
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceActiveValidationStrategy()
        {
            _resourceRepository = new ResourceRepository();
        }

        public ReservationValidationResult Validate(ReservationRequest request)
        {
            Resource resource = _resourceRepository.GetResource(request.ResourceID);

            if (resource == null)
                return ReservationValidationResult.Fail("Resurs ne postoji.");

            if (!resource.IsActive)
                return ReservationValidationResult.Fail("Resurs nije aktivan.");

            return ReservationValidationResult.Success();
        }
    }
}