using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Listningtour.Models
{
    public class SupportModel
    {
        public int SupportId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        [Display(Name = "Phone No")]
        public string PhoneNo{ get; set; }

        public string Subject { get; set; }

        [Display(Name = "Describe Your Issue")]
        public string DescribeYourIssue { get; set; }
    }
}