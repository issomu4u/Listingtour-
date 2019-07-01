using Listningtour.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Listningtour.Models
{
    public class UserProfile
    {
        public int Userinfoid { get; set; }

        [Display(Name = "First Name")]
        public string Name { get; set; }


        [Display(Name = "Middle Name")]
        public string Middlename { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname{ get; set; }

        public string City { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}"
           + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\"
           + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid E-mail Id")]

        public string Email { get; set; }

        public string Zipcode { get; set; }


        public string Designation { get; set; }


        [Display(Name = "Cell No")]
        public string CellNo { get; set; }

        public string Brokerage { get; set; }


        [Display(Name = "Office No")]
        public string OfficeNo { get; set; }

        public string Website { get; set; }

        public string PhotoPath { get; set; }

        [Display(Name = "Company Logo")]
        public string CompanyLogo { get; set; }

        [Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }

  }


 }