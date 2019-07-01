using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Listningtour.Models.Dto
{
    public class PublishedProperty
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProperyUrl { get; set; }
        public string Address { get; set; }
    }
}
