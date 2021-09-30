using System.Collections.Generic;

namespace OnoStore.WebApp.MVC.Models
{
    public class ResponseErrorMessages
    {
        public ResponseErrorMessages()
        {
            Messages = new List<string>();
        }
        public List<string> Messages { get; set; }
    }
}