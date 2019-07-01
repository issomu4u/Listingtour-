using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace Listningtour
{
    public class CommonCls
    {
        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }





        public static string GetURL()
        {
            string URL = "";
            URL = ConfigurationManager.AppSettings["LocalURL"].ToString();
            return URL;
        }

        public static string GetURL1()
        {
            string URL1 = "";
            URL1 = ConfigurationManager.AppSettings["LocalURL1"].ToString();
            return URL1;
        }

        public static bool SendMailToUser(string Email, string Password = "", string Type = "", string token = "")
        {
            string FromEmail = "testinguse8@gmail.com";
            string FromPassword = "testing@123";         
            var link = GetURL() + "/Account/Activate?v=" + token;
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            mail.To.Add(new MailAddress(Email));
            mail.From = new MailAddress(FromEmail);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(FromEmail, FromPassword);
            mail.Subject = Type;
            string msgbody = "";


            msgbody = "Hi " + Email;
            string Template = "";
            if (Type == "Registration")
            {
               Template = "Templates/registration.html";

            }
            else if (Type == "UpdateProfile")
            {
                Template = "Templates/UpdateProfile.html";
            }
            else
            {
                Template = "Templates/forgotPassword.html";
            }
            using (StreamReader reader = new StreamReader(Path.Combine(HttpRuntime.AppDomainAppPath, Template)))
            {

                msgbody = reader.ReadToEnd();

                //Replace UserName and Other variables available in body Stream

                msgbody = msgbody.Replace("{Email}", Email);

                msgbody = msgbody.Replace("{link}", link);

                if (Type != "Registration" && Type != "UpdateProfile" )
                {

                    msgbody = msgbody.Replace("{Password", Password);
                }

            }

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(msgbody, @"<(.|\n)*?>", string.Empty), null, "text/plain");
            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(msgbody, null, "text/html");

            mail.AlternateViews.Add(plainView);
            mail.AlternateViews.Add(htmlView);
            // mail.Body = msgbody;
            mail.IsBodyHtml = true;
            // mail.Body = Body;

            client.Send(mail);
            return true;
        }
    }
    }
