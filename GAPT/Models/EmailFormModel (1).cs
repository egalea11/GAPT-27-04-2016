using System.ComponentModel.DataAnnotations;

namespace MVCEmail.Models
{
    public class EmailFormModel
    {
        string fromEmail;

        [Required, Display(Name = "Your name")]
        public string FromName { get; set; }
        [Required, Display(Name = "Your email"), EmailAddress]
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
        [Required, Display(Name = "Your message")]
        public string Message { get; set; }
    }
}