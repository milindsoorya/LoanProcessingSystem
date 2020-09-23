using LoanProcessingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanProcessingSystem.Controllers
{
    public class HomeController : Controller
    {
        MortgageDbEntities db = new MortgageDbEntities();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
 
        public new ActionResult Profile()
        {
            string email = Session["EmailId"].ToString();
            ViewBag.Message = "Your contact page.";
            
            string fullname = (from user in db.UserRegisters
                                  where user.EmailId.Equals(email)
                                  select user.FullName).FirstOrDefault();
            Session["FullName"] = fullname;
            //ViewBag.Shows=StatusShow(Session["EmailId"].ToString());

            var loan = db.LoanForms.Where(x => x.EmailId == email).FirstOrDefault();
            if (loan == null)
            {
                ViewBag.LoanCurrentStatus = "You have not applied for loan";
            }
            else
            {
                var applicationid = loan.ApplicationId;
                var loanstatus = db.StatusTracks.Where(x => x.ApplicationId == applicationid).FirstOrDefault();
                var currentStatus = loanstatus.Status;
                ViewBag.LoanCurrentStatus = currentStatus;
            }
            return View();
        }

        public  ActionResult Plans()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [NonAction]
        public string StatusShow(string EmailId)
        {
            var innerquery = from user in db.UserRegisters.ToList()
                    where user.EmailId.Equals(EmailId)
                    select user.UserId;

            var outerquery = (from statustrack in db.StatusTracks.ToList()
                              where statustrack.UserId.Equals(innerquery.ToString())
                              orderby statustrack.Status descending
                              select statustrack.Status).FirstOrDefault();

            return Convert.ToString(outerquery);
        }
    }
}