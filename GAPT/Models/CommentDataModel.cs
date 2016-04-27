using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace GAPT.Models
{
    public class CommentDataModel
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        [Required]
        public string Body { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public int PostId { get; set; }

        // Link to existing Post class 
        public virtual Post Post { get; set; }
    }

    public class Post
    {
        // Current Properties...

        // New relationship property
        public virtual ICollection<CommentDataModel> Comments { get; set; }
    }
}