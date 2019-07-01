using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Listningtour.Models
{
    public class UserPropertyViewModel
    {
        public UserPropertyViewModel()
        {
            Images = new List<ImageViewModel>();
        }

        public int Userinfoid { get; set; }
        public UserProfile User { get; set; }
        public int Stepid { get; set; }

        [Display(Name = "Property Address")]
        public string PropertyAddress { get; set; }
        public string MLS { get; set; }

        [Display(Name = "Zip Code")]
        public string Zipcode { get; set; }

        public string Area { get; set; }
        public string Make { get; set; }

        [Display(Name = "Property Type")]
        public string Propertytype { get; set; }

        [Display(Name = "Types of Home")]
        public string Typesofhome { get; set; }
        public string Taxes { get; set; }
        public string Status { get; set; }
        public string Bedrooms { get; set; }
        public string Price { get; set; }
        public string Washrooms { get; set; }

        [Display(Name = "Property Description")]
        public string PropertyDescription { get; set; }

        [Display(Name = "Youtube Link")]
        public string YoutubeLink { get; set; }

        public int UserId { get; set; }
        public List<ImageViewModel> Images { get; set; }

        public string WalkScore { get; set; }
    }

    public class ImageViewModel
    {
        public int ImgId { get; set; }
        public int UserId { get; set; }
        public string thumbnails { get; set; }
        public string PhotoPath { get; set; }
    }


}