using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoanProcessingSystem.Models;


namespace LoanProcessingSystem.Controllers
{
    public class InspectorController : Controller
    {
        private MortgageDbEntities db = new MortgageDbEntities();
        // GET: Inspector
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InspectorView()
        {

            using (MortgageDbEntities dc = new MortgageDbEntities())
            {
                int userid = int.Parse(Session["id"].ToString());
                var v = dc.LoanForms.Where(a => a.InspectorId == userid);


                var loanForms = db.LoanForms.Include(l => l.AdminDetail);
                return View(v.ToList());
            }


        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanForm loanForm = db.LoanForms.Find(id);
            if (loanForm == null)
            {
                return HttpNotFound();
            }
            return View(loanForm);
        }


        public ActionResult Logout()
        {
            string Id = (string)Session["Id"];
            Session.Abandon();
            return RedirectToAction("LogIn", "RegisterLogin");
        }
    }
}