namespace VkAnalyzer.WebApp.Models
{
    public class BaseResponse
    {
        public bool Success { get; set; }
    }

    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }

        public BaseResponse()
        {
                
        }

        public BaseResponse(T data)
        {
            Data = data;
        }
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

        public BaseSuccessResponse(T data) : base(data)
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

        public BaseErrorResponse(string error) : this()
        {
            Error = error;
        }
    }

    public class BaseErrorResponse<T> : BaseResponse<T>
    {
        public string Error { get; set; }

        public BaseErrorResponse(string error)
        {
            Error = error;
        }

        public BaseErrorResponse(T data) : base(data)
        {
            Success = false;
        }
    }
}
