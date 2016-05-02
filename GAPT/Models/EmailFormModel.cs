using System.ComponentModel.DataAnnotations;

namespace MVCEmail.Models
{
    public class EmailFormModel
    {
        string fromEmail;
        
        public string FromName { get; set; }
        public string FromEmail {
            get
            {
                return fromEmail;
            }
            set
            {
                this.fromEmail = value;
            }
        }
        public string Message { get; set; }
    }
}