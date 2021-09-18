namespace OnoStore.WebApp.MVC.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int ErrorCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
