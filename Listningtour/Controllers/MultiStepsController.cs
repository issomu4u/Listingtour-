using Listningtour.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using static Listningtour.Core.Infrastructure.SecurityFunction;

namespace Listningtour.Controllers
{
    public class MultiStepsController : Controller
    {
        DataClasses1DataContext dba = new DataClasses1DataContext();

        // GET: MultiSteps
        public ActionResult Steps1(string Propertytype, string Typesofhome,  string Type = "")
        {

            if (Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Login", "Account");
            }


            if (Type != "")
            {
                return View("Steps1");
            }

            var userPropertyData = GetSessionData<UserPropertyViewModel>("UserProperty");
            if (userPropertyData == null)
            {
                var data = GetUserPopertyPreview();
                SetSessionData("UserProperty", data);

                userPropertyData = data;
            }
            return View("Steps1", userPropertyData);
        }

        private void SetSessionData<T>(string key, T data)
        {
            Session[key.ToUpper()] = data;
        }

        private T GetSessionData<T>(string key)
        {
            if (Session[key.ToUpper()] == null)

                return default(T);

            else

                return (T)Session[key.ToUpper()];
        }


        private void ClearAllSteps()
        {

            Session.Remove("Steps1");
            Session.Remove("Steps2");
            Session.Remove("Steps3");
        }


        [HttpPost]
        public ActionResult Steps1(string Propertytype, string Typesofhome, UserPropertyViewModel Data)
        {
            var loggedInUserr = Session["LoggedInUser"] as User;
            if (loggedInUserr == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var viewModelData = GetSessionData<UserPropertyViewModel>("UserProperty");

                if (viewModelData != null)
                {
                    Data.Images = viewModelData.Images;
                }
                SetSessionData("UserProperty", Data);
                return RedirectToAction("Steps2", viewModelData);

            }
            return View(Data);
        }




        /// <param name="files"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult Steps2()
        {

            var viewModelData = GetSessionData<UserPropertyViewModel>("UserProperty");
            return View(viewModelData);
        }

        [HttpPost]
        public ActionResult Steps2(IEnumerable<HttpPostedFileBase> file, string Type = "")
        {
            var loggedInUserr = Session["LoggedInUser"] as User;
            if (loggedInUserr == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var firstStepViewModel = GetSessionData<UserPropertyViewModel>("UserProperty");
               // var viewModelData = GetSessionData<UserPropertyViewModel>("UserProperty");
                if(firstStepViewModel.Images==null)
                {
                    firstStepViewModel.Images = new List<ImageViewModel>();
                }
                else
                {
                    firstStepViewModel.Images = firstStepViewModel.Images;// new List<ImageViewModel>();
                }
              
                // firstStepViewModel.Images = new List<ImageViewModel>();
                var imgList = new List<ImageViewModel>();

                var URL = "";
                foreach (var item in file)
                {
                    if (item != null && item.ContentLength > 0)
                    {
                        try
                        {
                            string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(item.FileName));
                            item.SaveAs(path);
                            URL = CommonCls.GetURL() + "/Images/" + item.FileName;
                            //thumbnail


                            // Session["FileName"] = URL;
                            var objImg = new ImageViewModel();
                            objImg.PhotoPath = URL;
                            objImg.thumbnails = "";

                            imgList.Add(objImg);

                        }

                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }

                    }

                }
                if(imgList.Count()>0)
                {
                    firstStepViewModel.Images.AddRange(imgList);
                }
               
               SetSessionData("UserProperty", firstStepViewModel);
                User myUser = Session["LoggedInUser"] as User;// User myUser = (User)Session["LoggedInUser"];
                var user = dba.Userinfos.Where(c => c.UserId == Convert.ToInt32(myUser.Id)).FirstOrDefault();
                if (user != null)
                {
                    UserProfile usr = new UserProfile();
                    usr.Name = user.Name;
                    usr.Middlename = user.Middlename;
                    usr.Lastname = user.Lastname;
                    usr.Email = user.Email;
                    usr.Website = user.Website;
                    usr.Designation = user.Designation;
                    usr.PhotoPath = user.PhotoPath;
                    usr.CompanyLogo = user.CompanyLogo;
                    usr.CellNo = user.CellNo;

                    firstStepViewModel.User = usr;
                }
                return RedirectToAction("Steps3", firstStepViewModel);
            }

            return View();

        }

        [HttpGet]
        public ActionResult Steps3()
        {
            var loggedInUserr = Session["LoggedInUser"] as User;
            if (loggedInUserr == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var viewModelData = GetSessionData<UserPropertyViewModel>("UserProperty");
            //Adress adrs = new Adress();
            //adrs.Address = viewModelData.PropertyAddress;
            //adrs.GeoCode();
            //string lat = adrs.Latitude;
            //string lng = adrs.Longitude;
            //StringBuilder sb = new StringBuilder();
            //DataSet ds = GetWalkingScore1(viewModelData.PropertyAddress, lat, lng);
            //if (ds.Tables.Count > 0)
            //{
            //    //imgg.ImageUrl = ds.Tables[0].Rows[0]["logo_url"].ToString();
            //    viewModelData.WalkScore = ds.Tables[0].Rows[0]["walkscore"].ToString();

            //    // lblwalk.PostBackUrl = "http://www.walkscore.com/score/loc/lat=" + lat + "/lng=" + lng + "/?utm_source=http://roccobuyandsell.com&amp;utm_medium=ws_api&amp;utm_campaign=ws_api";

            //}
            return View(viewModelData);
        }


        [HttpPost]
        public ActionResult Steps3(UserPropertyViewModel data)
        {

            var loggedInUserr = Session["LoggedInUser"] as User;
            if (loggedInUserr == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //  TempData["Fullurl"] = HttpContext.Request.Url.AbsoluteUri;

            var viewModelData = GetSessionData<UserPropertyViewModel>("UserProperty");
            var user = Session["LoggedInUser"] as User;
            if (user != null)
            {
                viewModelData.UserId = user.Id;
            }
            Step stepToSave = null;
            if (viewModelData.Stepid > 0)
            {
                stepToSave = dba.Steps.FirstOrDefault(c => c.UserId == viewModelData.Stepid);
            }

            stepToSave = stepToSave ?? new Step();

            stepToSave.PropertyAddress = viewModelData.PropertyAddress;
            stepToSave.Price = viewModelData.Price;
            stepToSave.Washrooms = viewModelData.Washrooms;
            stepToSave.Bedrooms = viewModelData.Bedrooms;
            stepToSave.Area = viewModelData.Area;
            stepToSave.Status = viewModelData.Status;
            stepToSave.Propertytype = viewModelData.Propertytype;
            stepToSave.Typesofhome = viewModelData.Typesofhome;
            stepToSave.MLS = viewModelData.MLS;
            stepToSave.YoutubeLink = viewModelData.YoutubeLink;
            stepToSave.UserId = viewModelData.UserId;

            if (viewModelData.Stepid == 0)
            dba.Steps.InsertOnSubmit(stepToSave);
            var images = dba.Images.Where(c => c.UserId == viewModelData.UserId).ToList();

            if (images != null && images?.Count != viewModelData.Images?.Count)
            {
                foreach (var img in images)
                    dba.Images.DeleteOnSubmit(img);

                foreach (var img in viewModelData.Images)
                {
                    dba.Images.InsertOnSubmit(new Image
                    {
                        UserId = stepToSave.UserId.GetValueOrDefault(),
                        PhotoPath = img.PhotoPath,
                        thumbnails = img.thumbnails,
                        Step = stepToSave
                    });
                }

            }
            else if (images != null && images?.Count == viewModelData?.Images?.Count && viewModelData?.Images?.Count > 0)
            {
                int idx = 0;
                foreach (var img in images)
                {
                   // img.UserId = stepToSave.UserId.GetValueOrDefault();
                    img.PhotoPath = viewModelData.Images[idx].PhotoPath;
                    img.thumbnails = viewModelData.Images[idx].thumbnails;
                    //img.Step = stepToSave;
                }
            }
            var id = "";
            if (Session["PublishedId"] != null)
            {
                id = Session["PublishedId"].ToString();
            }
            var publishedData = dba.PublishedProperties.Where(c => c.Id == Convert.ToInt32(id)).FirstOrDefault();

            //var publishedProperty = new PublishedProperty
            //{
            publishedData.Guid = Guid.NewGuid();
            publishedData.Step = stepToSave;


            //};
            publishedData.PublishedUrl = ConfigurationManager.AppSettings["LocalURL"] + "/PublishedProperty?id=" + publishedData.Guid;
            dba.SubmitChanges();
            // return View(viewModelData);
            //Adress adrs = new Adress();
            //adrs.Address = stepToSave.PropertyAddress;
            //adrs.GeoCode();
            //string lat = adrs.Latitude;
            //string lng = adrs.Longitude;
            //StringBuilder sb = new StringBuilder();
            //DataSet ds = GetWalkingScore1(stepToSave.PropertyAddress, lat, lng);
            //if (ds.Tables.Count > 0)
            //{
            //    //imgg.ImageUrl = ds.Tables[0].Rows[0]["logo_url"].ToString();
            //    viewModelData.WalkScore = ds.Tables[0].Rows[0]["walkscore"].ToString();

            //   // lblwalk.PostBackUrl = "http://www.walkscore.com/score/loc/lat=" + lat + "/lng=" + lng + "/?utm_source=http://roccobuyandsell.com&amp;utm_medium=ws_api&amp;utm_campaign=ws_api";

            //}
                return RedirectToAction("Index","Dashboard",viewModelData);

        }
        public class Adress
        {
            public Adress()
            {
            }
            //Properties
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Address { get; set; }

            //The Geocoding here i.e getting the latt/long of address
            public void GeoCode()
            {
                //to Read the Stream
                StreamReader sr = null;
                //The Google Maps API Either return JSON or XML. We are using XML Here
                //Saving the url of the Google API 
                string url = String.Format("http://maps.googleapis.com/maps/api/geocode/xml?address=" + Address + "&sensor=false");
                //to Send the request to Web Client 
                WebClient wc = new WebClient();
                try
                {
                    sr = new StreamReader(wc.OpenRead(url));
                }
                catch (Exception ex)
                {
                    throw new Exception("The Error Occured" + ex.Message);
                }
                try
                {
                    XmlTextReader xmlReader = new XmlTextReader(sr);
                    bool latread = false;
                    bool longread = false;
                    while (xmlReader.Read())
                    {
                        xmlReader.MoveToElement();
                        switch (xmlReader.Name)
                        {
                            case "lat":
                                if (!latread)
                                {
                                    xmlReader.Read();
                                    this.Latitude = xmlReader.Value.ToString();
                                    latread = true;
                                }
                                break;
                            case "lng":
                                if (!longread)
                                {
                                    xmlReader.Read();
                                    this.Longitude = xmlReader.Value.ToString();
                                    longread = true;
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An Error Occured" + ex.Message);
                }
            }
        }

        public DataSet GetWalkingScore1(string address, string lat, string lon)
        {
            DataSet dsResult = new DataSet();
            try
            {
                string key = "c02fd15e688b6bffae8913ee64e4ea17";
                string url = @"http://api.walkscore.com/score?format=xml&address=1119%8th%20Avenue%20Seattle%20WA%2098101&lat=" + lat + "&lon=" + lon + "&wsapikey=" + key;

                //string url = @"http://api.walkscore.com/score?format=xml&address=1119%8th%20Avenue%20Seattle%20WA%2098101&lat=47.6085&lon=-122.3295&wsapikey=" + key;
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {

                        dsResult.ReadXml(reader);
                        //Address = dsResult.Tables["result"].Rows[0]["formatted_address"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
            }
            return dsResult;
        }
        public UserPropertyViewModel GetUserPopertyPreview()
        {
            var result = new Models.UserPropertyViewModel();
            return result;
        }

        public JsonResult GetCountries()
        {
            var Countries = new List<string>();
            Countries.Add("Any");
            Countries.Add("Residential");
            Countries.Add("Commercial");
            Countries.Add("Condo");
            return Json(Countries, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStates(string country)
        {
            var States = new List<string>();
            if (!string.IsNullOrWhiteSpace(country))
            {
                if (country.Equals("Any"))
                {

                    States.Add("Any");
                    States.Add("Condo Apt");
                    States.Add("Condo Townhouse");
                    States.Add("Comm Element Condo");
                    States.Add("Parking Space");
                    States.Add("Det Condo");
                    States.Add("Other");
                    States.Add("Locker");
                    States.Add("Semi-Det Condo");
                    States.Add("Co-Op Apt");
                    States.Add("Co-Ownership Apt");
                    States.Add("Phased Condo");
                    States.Add("Leasehold Condo");
                    States.Add("Time Share");
                    States.Add("Lower level");
                    States.Add("Upper level");

                }
                if (country.Equals("Residential"))
                {
                    States.Add("Any");
                    States.Add("Detached");
                    States.Add("Att/Row/Townhouse");
                    States.Add("Semi-Detached");
                    States.Add("Vacant Land");
                    States.Add("Link");
                    States.Add("Duplex");
                    States.Add("Cottage");
                    States.Add("Farm");
                    States.Add("Rural Resid");
                    States.Add("Multiplex");
                    States.Add("Triplex");
                    States.Add("Mobile/Trailer");
                    States.Add("Other");
                    States.Add("Store W/Apt/Offc");
                    States.Add("Upper level");
                    States.Add("Lower Level");
                    States.Add("Fourplex");
                    States.Add("Room");
                    States.Add("Shared Room");

                }
                if (country.Equals("Commercial"))
                {
                    States.Add("Any");
                    States.Add("Commercial/Retail");
                    States.Add("Sale Of Business");
                    States.Add("Office");
                    States.Add("Industrial");
                    States.Add("Land");
                    States.Add("Investment");
                    States.Add("Store W/Apt/office");
                    States.Add("Farm");

                }
                if (country.Equals("Condo"))
                {

                    States.Add("Any");
                    States.Add("Condo Apt");
                    States.Add("Condo Townhouse");
                    States.Add("Comm Element Condo");
                    States.Add("Parking Space");
                    States.Add("Det Condo");
                    States.Add("Other");
                    States.Add("Locker");
                    States.Add("Semi-Det Condo");
                    States.Add("Co-Op Apt");
                    States.Add("Co-Ownership Apt");
                    States.Add("Phased Condo");
                    States.Add("Leasehold Condo");
                    States.Add("Time Share");
                    States.Add("Lower level");
                    States.Add("Upper level");

                }

            }
            return Json(States, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Publish()
        {
            
            return View();
        }

    }
}














