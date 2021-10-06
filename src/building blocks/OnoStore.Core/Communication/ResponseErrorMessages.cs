using System.Collections.Generic;

namespace OnoStore.Core.MVC.Models
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