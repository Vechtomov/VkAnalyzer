namespace VkAnalyzer.Models
{
    public class BaseResponse
    {
        public bool Success { get; set; }
    }

    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }

    public class BaseSuccessResponse : BaseResponse
    {
        public BaseSuccessResponse()
        {
            Success = true;
        }
    }

    public class BaseSuccessResponse<T> : BaseResponse<T>
    {
        public BaseSuccessResponse()
        {
            Success = true;
        }
    }

    public class BaseErrorResponse : BaseResponse
    {
        public string Error { get; set; }

        public BaseErrorResponse()
        {
            Success = false;
        }
    }

    public class BaseErrorResponse<T> : BaseResponse<T>
    {
        public string Error { get; set; }

        public BaseErrorResponse()
        {
            Success = false;
        }
    }
}
