using Listningtour.Models;
using Listningtour.Models.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Listningtour.Controllers
{
    public class DashboardController : Controller
    {

        DataClasses1DataContext dba = new DataClasses1DataContext();
        public ActionResult payment()
        {

            var loggedInUserr = Session["LoggedInUser"] as User;

            if (loggedInUserr == null)
            {

                return RedirectToAction("Login", "Account");
            }



            Models.PublishedProperty obj = new Models.PublishedProperty();
            var user = Session["LoggedInUser"] as User;
            obj.UserId = user.Id;
            obj.PublishedUrl = "";
            dba.PublishedProperties.InsertOnSubmit(obj);
            dba.SubmitChanges();
            Session["PublishedId"] = obj.Id;
            return View();
        }
// GET: emp  
        public ActionResult Index()
        {
            var loggedInUserr = Session["LoggedInUser"] as User;

            if (loggedInUserr == null)
            {
           
                return RedirectToAction("Login","Account");
            }

            using (var dba = new DataClasses1DataContext())
            {
                var user = dba.Userinfos.FirstOrDefault(x => x.Email == loggedInUserr.Email);
                var publishedProperties = dba.PublishedProperties.Where(x => x.UserId == user.UserId).ToArray();

                var dashboardData = new DashboardData
                {
                    Userinfoid = user.Userinfoid,
                    Name = user.Name,
                    Middlename = user.Middlename,
                    Lastname = user.Lastname,
                    City = user.City,
                    Email = user.Email,
                    Zipcode = user.Zipcode,
                    Designation = user.Designation,
                    CellNo = user.CellNo,
                    Brokerage = user.Brokerage,
                    OfficeAddress = user.OfficeAddress,
                    OfficeNo = user.OfficeNo,
                    Website = user.Website,
                    PhotoPath = user.PhotoPath,
                    CompanyLogo = user.CompanyLogo,
                };
                foreach(var property in publishedProperties)
                {
                    var stepData = dba.Steps.FirstOrDefault(x => x.Stepid == property.StepId);

                    if(stepData!=null)
                    {
                        dashboardData.PublishedProperties.Add(new Models.Dto.PublishedProperty { Id = property.Id, Address = stepData.PropertyAddress, ProperyUrl = property.PublishedUrl, UserId = property.UserId });

                    }
                }
                
                return View(dashboardData);

            }

        }

        [HttpGet]
        public ActionResult Edit(int Userinfoid)
        {

            var loggedInUserr = Session["LoggedInUser"] as User;

            if (loggedInUserr == null)
            {
               
                return RedirectToAction("Login", "Account");
            }


            using (var dba = new DataClasses1DataContext())
            {
                UserProfile Edit = dba.Userinfos.Where(val => val.Userinfoid == Userinfoid).Select(val => new UserProfile()
                {
                    Userinfoid = val.Userinfoid,
                    Name = val.Name,
                    Middlename = val.Middlename,
                    Lastname = val.Lastname,
                    City = val.City,
                    Email = val.Email,
                    Zipcode = val.Zipcode,
                    Designation = val.Designation,
                    CellNo = val.CellNo,
                    Brokerage = val.Brokerage,
                    OfficeNo = val.OfficeNo,
                
                    Website = val.Website,
                    PhotoPath = val.PhotoPath,
                    CompanyLogo = val.CompanyLogo,
                    OfficeAddress = val.OfficeAddress
                }).SingleOrDefault();

                return View(Edit);
            }

        }


        public ActionResult Edit(int Userinfoid, UserProfile Model, HttpPostedFileBase file, HttpPostedFileBase file1)
        {

            var loggedInUserr = Session["LoggedInUser"] as User;

            if (loggedInUserr == null)
            {

                return RedirectToAction("Login", "Account");
            }

            using (var dba = new DataClasses1DataContext())

            {
                Userinfo usr = dba.Userinfos.Where(val => val.Userinfoid == Model.Userinfoid).Single<Userinfo>();

                //  var custTemp = new Customer_temp();
                string URL = usr.PhotoPath;
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        if (usr.PhotoPath != "")
                        {
                            // DeleteImage(Customer.PhotoPath);
                        }

                        string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(file.FileName.Replace(' ', '-')));

                        if (System.IO.File.Exists(path))
                        {
                            var newPath = Path.Combine(Server.MapPath("~/Images"), Path.GetFileNameWithoutExtension(file.FileName.Replace(' ', '-')).ToString() + DateTime.Now.ToString("yyyy-MM-dd HHmmtt") + Path.GetExtension(file.FileName));
                            newPath = newPath.Replace(' ', '-').ToString();
                            URL = CommonCls.GetURL() + "/Images/" + Path.GetFileNameWithoutExtension(file.FileName.Replace(' ', '-')).ToString() + DateTime.Now.ToString("yyyy-MM-dd HHmmtt") + Path.GetExtension(file.FileName);
                            URL = URL.Replace(' ', '-').ToString();
                            file.SaveAs(newPath);
                        }


                        else
                        {
                            file.SaveAs(path);
                            URL = CommonCls.GetURL() + "/Images/" + file.FileName.Replace(' ', '-');
                        }

                        // custTemp.PhotoPath = URL;
                        usr.PhotoPath = URL;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }

                }


                // Company Logo //

                string URL1 = usr.CompanyLogo;
                if (file1 != null && file1.ContentLength > 0)
                {
                    try
                    {
                        if (usr.CompanyLogo != "")
                        {
                            // DeleteImage(Customer.CompanyLogo);
                        }

                        string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(file1.FileName.Replace(' ', '-')));

                        if (System.IO.File.Exists(path))
                        {
                            var newPath = Path.Combine(Server.MapPath("~/Images"), Path.GetFileNameWithoutExtension(file1.FileName.Replace(' ', '-')).ToString() + DateTime.Now.ToString("yyyy-MM-dd HHmmtt") + Path.GetExtension(file1.FileName));
                            newPath = newPath.Replace(' ', '-').ToString();
                            URL1 = CommonCls.GetURL1() + "/Images/" + Path.GetFileNameWithoutExtension(file1.FileName.Replace(' ', '-')).ToString() + DateTime.Now.ToString("yyyy-MM-dd HHmmtt") + Path.GetExtension(file1.FileName);
                            URL1 = URL1.Replace(' ', '-').ToString();
                            file1.SaveAs(newPath);
                        }


                        else
                        {
                            file1.SaveAs(path);
                            URL1 = CommonCls.GetURL1() + "/Images/" + file1.FileName.Replace(' ', '-');
                        }

                        // custTemp.CompanyLogo = URL1;
                        usr.CompanyLogo = URL1;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }

                }
                usr.Userinfoid = Model.Userinfoid;
                usr.Name = Model.Name;
                usr.Middlename = Model.Middlename;
                usr.Lastname = Model.Lastname;
                usr.City = Model.City;
                usr.Email = Model.Email;
                usr.Zipcode = Model.Zipcode;
                usr.Designation = Model.Designation;
                usr.CellNo = Model.CellNo;
                usr.Brokerage = Model.Brokerage;
                usr.OfficeNo = Model.OfficeNo;
                usr.Website = Model.Website;
                //usr.PhotoPath = URL;
                usr.OfficeAddress = Model.OfficeAddress;
                //usr.CompanyLogo = URL1;
                dba.SubmitChanges();
                return RedirectToAction("Index", "Dashboard");
            }

        }



        public void DeleteImage(string filePath)
        {
            try
            {
                var uri = new Uri(filePath);
                var fileName = Path.GetFileName(uri.AbsolutePath);
                var subPath = Server.MapPath("~/Images");
                var path = Path.Combine(subPath, fileName);

                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
              
            }
        }

    }
}