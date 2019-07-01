using Listningtour.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Listningtour.Controllers
{
    [AllowAnonymous]
    public class PublishedPropertyController : Controller
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
          
            var propertyGuid = default(Guid);
            Guid.TryParse(id, out propertyGuid);
            if (propertyGuid != default(Guid))
            {
                using (var dba = new DataClasses1DataContext())
                {
                    var publishedProperty = dba.PublishedProperties.FirstOrDefault(x => x.Guid == propertyGuid);
                    if(publishedProperty != null)
                    {
                        var stepData = dba.Steps.FirstOrDefault(x => x.Stepid == publishedProperty.StepId);
                        var images = dba.Images.Where(x => x.StepId == publishedProperty.StepId);
                        var user = dba.Userinfos.FirstOrDefault(x => x.UserId == publishedProperty.UserId);

                        var propertyData = new UserPropertyViewModel
                        {

                            PropertyAddress = stepData.PropertyAddress,
                            Price = stepData.Price,
                            Washrooms = stepData.Washrooms,
                            Bedrooms = stepData.Bedrooms,
                            Area = stepData.Area,
                            Status = stepData.Status,
                            Propertytype= stepData.Propertytype,
                            Typesofhome = stepData.Typesofhome,
                            MLS = stepData.MLS,
                            YoutubeLink = stepData.YoutubeLink,
                            UserId = publishedProperty.UserId,

                        };
                        propertyData.User = new UserProfile
                        {
                            Userinfoid = user.Userinfoid,
                            Name = user.Name,
                            Middlename = user.Middlename,
                            Lastname = user.Lastname,
                            Email = user.Email,
                            Website = user.Website,
                            Designation = user.Designation,
                            PhotoPath = user.PhotoPath,
                            CompanyLogo = user.CompanyLogo,
                            CellNo = user.CellNo
                        };

                        foreach (var image in images)
                        {
                            propertyData.Images.Add(new ImageViewModel { ImgId = image.ImgId, PhotoPath = image.PhotoPath, thumbnails = image.thumbnails, UserId = image.UserId });
                        }

                        return View(propertyData);
                    }
                    return View("PropertyNotFound");
                }
            }
            return View("PropertyNotFound");
        }
    }
}