namespace Ds_projekat.Services
{
    internal class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int NewId { get; set; }

        public ServiceResult()
        {
            Success = false;
            Message = "";
            NewId = 0;
        }

        public static ServiceResult Ok(string message, int newId = 0)
        {
            return new ServiceResult
            {
                Success = true,
                Message = message,
                NewId = newId
            };
        }

        public static ServiceResult Fail(string message)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                NewId = 0
            };
        }
    }
}