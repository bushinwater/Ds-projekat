namespace Ds_projekat.Strategies.ReservationValidation
{
    internal class ReservationValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

        public ReservationValidationResult()
        {
            IsValid = true;
            Message = "";
        }

        public static ReservationValidationResult Success()
        {
            return new ReservationValidationResult
            {
                IsValid = true,
                Message = ""
            };
        }

        public static ReservationValidationResult Fail(string message)
        {
            return new ReservationValidationResult
            {
                IsValid = false,
                Message = message
            };
        }
    }
}