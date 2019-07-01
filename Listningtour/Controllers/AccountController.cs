using Listningtour.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Listningtour.Controllers
{
    public class AccountController : Controller
    {
        DataClasses1DataContext dba = new DataClasses1DataContext();
  
// GET: Account
public ActionResult Index()
        {
            DataClasses1DataContext dba = new DataClasses1DataContext();
           
            return View();
        }

        [HttpGet]
        public ActionResult Login(UserModel Model)

        {
            return View(Model);
        }

        [HttpPost]
        public ActionResult Logins(UserModel Model)
        {
            var loggedInUserr = Session["LoggedInUser"] as User;

            if (loggedInUserr != null)
            {
                return RedirectToAction("Login", "Account");
            }


            var user = Searchuser.GetAuthorizedUser(Model);
            if (user == null)
            {
                @ViewBag.data = "Please Enter your Correct Email and Password";
                return View("Login");
            }
            if (user.IsEmailVerified != null && user.IsEmailVerified == true)
            {
                Session.Add("LoggedInUser", user);
                var isProfileExsits = Searchuser.IsProfileExists(user.Id);
                if (isProfileExsits)
                    return RedirectToAction("Index", "Dashboard");

                //ViewBag.Brokeragelists = dba.Brokeragelists.ToList();
                return View("CreateProfile");
            }
            else
            {
                @ViewBag.data = "Please verify your account Verify link sent on your email address";
                return View("Login");
            }
        }

        public ActionResult Signup(UserModel Model)
        {
   
            return View(Model);
        }

        public ActionResult Activate(string v)
        {
            
            if (string.IsNullOrWhiteSpace(v))
                return RedirectToAction("Signups");

            var user = dba.Users.Where(c => c.ActivationKey == v).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Signups");
            }
            else
            {
                user.IsEmailVerified = true;
                dba.SubmitChanges();
                return RedirectToAction("Login");
            }

        }

        public ActionResult Signups(UserModel Model)
        {

           
            if (ModelState.IsValid)
            {
                Enterintotable en = new Enterintotable();

                Model.ActivationKey = Guid.NewGuid().ToString();

                var isexist = dba.Users.Where(c => c.Email.Trim().ToLower() == Model.Email.Trim().ToLower()).FirstOrDefault();
                if (isexist == null)
                {
                    en.InsertUser(Model);
                }
                else
                {
                    @ViewBag.data = "Email already exists.";
                    return View("Signup");
                }
                            
                CommonCls.SendMailToUser(Model.Email, "", "Registration", Model.ActivationKey);
                ModelState.Clear();
                @ViewBag.Message = "Your account has been created Successfully. An Account Confirmation has been sent to your Email.";
                return View("Signup");
            }
            else
            {
                return View("Signup");
            }
        }

        public ActionResult CreateProfile()
        {
            UserProfile usr = new UserProfile();
          
            return View(usr);
        }

        [HttpPost]
        public ActionResult CreateProfile(UserProfile user, HttpPostedFileBase file, HttpPostedFileBase file1)
        {
            var loggedInUserr = Session["LoggedInUser"] as User;

            if (loggedInUserr == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // For Profile Picture  "~/Images"  //
            string URL = "";
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
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

                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

            }



            //////  Company Logo   URL1 ///////
            string URL1 = "";
            if (file1 != null && file1.ContentLength > 0)
            {
                try
                {
                    if (file1 != null && file1.ContentLength > 0)
                    {
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

                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

            }

            //Save user 
            Userinfo usr = new Userinfo();
            usr.UserId = loggedInUserr.Id;
            usr.Name = user.Name;
            usr.Middlename = user.Middlename;
            usr.Lastname = user.Lastname;
            usr.City = user.City;
            usr.Email = user.Email;
            usr.Zipcode = user.Zipcode;
            usr.Designation = user.Designation;
            usr.CellNo = user.CellNo;
            usr.Brokerage = user.Brokerage;
            usr.OfficeNo = user.OfficeNo;
            usr.Website = user.Website;
            usr.PhotoPath = URL;
            usr.CompanyLogo = URL1;
            usr.OfficeAddress = user.OfficeAddress;
            dba.Userinfos.InsertOnSubmit(usr);
            dba.SubmitChanges();
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult loadlogin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["LoggedInUser"] = null;
            if (Session["LoggedInUser"] == null)
            {
                Session.Remove("Email");           
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Account");
        }

        public ActionResult Signsucessfull()
        {    
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {        
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    ModelState.AddModelError("Email", "Please enter email.");
                }
                if (ModelState.IsValid)
                {
                    var user = dba.Users.Where(c => c.Email.ToLower() == Email.ToLower()).FirstOrDefault();
                    if (user != null)
                    {

                        //Send Email to User                   
                        CommonCls.SendMailToUser(Email, user.Password, "Forgot Password");
                        @ViewBag.data = "Your password has been sent to this email address.";
                        ModelState.Clear();
                        return View("ForgotPassword");
                    }
                    else
                    {
                        @ViewBag.data = "This email address does not exist in our records.";
                        return View("ForgotPassword");
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                //ErrorLogging.LogError(ex);
            }
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);

            return View();
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Support()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Support(SupportModel Model)
        {          
            //Save Support Form
            Support spt = new Support();
            var isSaveSuccess = true;
            if (isSaveSuccess)
            spt.Name = Model.Name;
            spt.Email = Model.Email;
            spt.PhoneNo = Model.PhoneNo;
            spt.Subject = Model.Subject;
            spt.DescribeYourIssue = Model.DescribeYourIssue;
            dba.Supports.InsertOnSubmit(spt);
            dba.SubmitChanges();
            if (ModelState.IsValid)
            {
                try
                {                                 
                    MailMessage mail = new MailMessage();
                    SmtpClient client = new SmtpClient();
                    mail.To.Add("testinguse8@gmail.com");
                    mail.From = new MailAddress("testinguse8@gmail.com");
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("testinguse8@gmail.com","testing@123");
                    mail.Subject = Model.Subject;
                    string msgbody = "";
                    msgbody = "Hi ";
                    string Template = "";
                    Template = "Templates/Support.html";
                    using (StreamReader reader = new StreamReader(Path.Combine(HttpRuntime.AppDomainAppPath, Template)))
                    {
                        msgbody = reader.ReadToEnd();
                        //Replace UserName and Other variables available in body Stream
                        msgbody = msgbody.Replace("{Name}", Model.Name);
                        msgbody = msgbody.Replace("{Email}", Model.Email);
                        msgbody = msgbody.Replace("{PhoneNo}", Model.PhoneNo);
                        msgbody = msgbody.Replace("{Subject}", Model.Subject);
                        msgbody = msgbody.Replace("{DescribeYourIssue}", Model.DescribeYourIssue);
                    }
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(msgbody, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                    System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(msgbody, null, "text/html");
                    mail.AlternateViews.Add(plainView);
                    mail.AlternateViews.Add(htmlView);       
                    mail.IsBodyHtml = true;          
                    client.Send(mail);                
                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }
            return View();
        }

        public ActionResult How_its_work()
        {           
            return View();
        }

        public ActionResult Terms_Conditions()
        {
            return View();
        }

        public ActionResult Privacy_Policy()
        {
            return View();
        }

        public ActionResult Disclaimer()
        {         
            return View();
        }

    }
}