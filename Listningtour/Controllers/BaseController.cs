using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Web.Http;
using System.Web.SessionState;

using Listningtour.Models;


namespace Listningtour.Controllers
{
    public class BaseController : Controller
    {
       
       
        public PartialViewResult DisplayMessage(string ShowMessage, string MessageBody)
        {
            ViewBag.ShowMessage = ShowMessage;
            ViewBag.MessageBody = MessageBody;
            return PartialView("DisplayMessage");
        }       

        public PartialViewResult Paging(string CurrentPageNo = "")
        {

            ViewBag.CurrentPageNo = ((CurrentPageNo != "" && CurrentPageNo != null) ? CurrentPageNo : "1");
            return PartialView("Paging");
        }
        public PartialViewResult ShowProgressBar()
        {
            return PartialView("ShowProgressBar");
        }
       
    }
}