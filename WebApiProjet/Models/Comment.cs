using System;
using System.Collections.Generic;

#nullable disable

namespace WebApiProjet.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public int? PostId { get; set; }
        public string UrlPhoto { get; set; }
    }
}
