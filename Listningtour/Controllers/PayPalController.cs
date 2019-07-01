using Listningtour.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Listningtour.Controllers
{
    public class PayPalController : Controller
    {

        DataClasses1DataContext dba = new DataClasses1DataContext();

        //[Authorize(Roles="Customers")]
        public ActionResult ValidateCommand(string product, string totalPrice)
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);


            var paypal = new PayPalModel(useSandbox);


            //var user = Session["LoggedInUser"] as User;
            //if (user != null)
            //{
            //    paypal.UserId = user.Id;
            //}


            paypal.item_name = product;
            paypal.amount = totalPrice;

            //dba.Paypaldatas.InsertOnSubmit(paypal);
            //dba.SubmitChanges();

            return View(paypal);

        }


        public ActionResult RedirectFromPaypal()
        {
            PublishedProperty obj = new Models.PublishedProperty();
            var user = Session["LoggedInUser"] as User;
            obj.UserId = user.Id;
            obj.PublishedUrl = "";
            dba.PublishedProperties.InsertOnSubmit(obj);
            dba.SubmitChanges();
            Session["PublishedId"] = obj.Id;
            return RedirectToAction("Steps1", "MultiSteps");
        }

        public ActionResult CancelFromPaypal()
        {
            return View();
        }

        public ActionResult NotifyFromPaypal()
        {
            return View();
        }



        public ActionResult Payment()
        {
            return View();
        }









    }
}