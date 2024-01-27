namespace LiveHolidayapp.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonResponse<T>
    {
        public int Code { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }

        public string[] Error { get; set; }
    }
}
