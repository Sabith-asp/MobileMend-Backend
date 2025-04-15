namespace MobileMend.Application.DTOs
{
    public class ResponseDTO<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }
    }
}
