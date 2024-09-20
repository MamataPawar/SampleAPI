using System.Text.Json;

namespace SampleAPI.Entities
{
    public class ApiError
    {
        public ApiError(int errorCode, string errorMessage, string errordetails = "")
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorDetails = errordetails;
        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = "";
        public string ErrorDetails { get; set; } = "";

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
