using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public ActionResult InspectorPanel()
        {
            return View();
        }

        public ActionResult InspectorView()
        {

            using (MortgageDbEntities dc = new MortgageDbEntities())
            {
                int userid = int.Parse(Session["id"].ToString());
                var v = dc.LoanForms.Where(a => a.InspectorId == userid && a.Status == "Inspector Assigned");


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


  

        [HttpPost]
        public ActionResult ApproveForm(int ApplicationId)
        {
            using (MortgageDbEntities db = new MortgageDbEntities())
            {
                //updating status in loanForm
                var loanform = db.LoanForms.FirstOrDefault(x => x.ApplicationId == ApplicationId);
                if (loanform == null) throw new Exception("Invalid id: " + ApplicationId);
                loanform.Status = "Inspector Approved";
                db.LoanForms.Attach(loanform);
                var entry = db.Entry(loanform);
                entry.Property(e => e.Status).IsModified = true;

                //updating status in StatusTrack
                var status = db.StatusTracks.FirstOrDefault(x => x.ApplicationId == ApplicationId);
                status.Status = "Inspector Approved";
                db.StatusTracks.Attach(status);
                var statustrackentry = db.Entry(status);
                statustrackentry.Property(e => e.Status).IsModified = true;

                db.SaveChanges();
                string mailbody = " < br />< br /> We are very Excited to inform you that your Appliction is Approved in inspection  on<strong> Santander UK </ strong > " +
                 "<br><br> We will revert back to you later!!";
                string subjectmail = "your Application is Approved";
                SendEmail(loanform.EmailId, mailbody, subjectmail);
                return RedirectToAction("InspectorView");
            }
        }

        [HttpPost]
        public ActionResult RejectForm(int ApplicationId)
        {
            using (MortgageDbEntities db = new MortgageDbEntities())
            {
                //updating status in loanForm
                var loanform = db.LoanForms.FirstOrDefault(x => x.ApplicationId == ApplicationId);
                if (loanform == null) throw new Exception("Invalid id: " + ApplicationId);
                loanform.Status = "Rejected";
                db.LoanForms.Attach(loanform);
                var entry = db.Entry(loanform);
                entry.Property(e => e.Status).IsModified = true;

                //updating status in StatusTrack
                var status = db.StatusTracks.FirstOrDefault(x => x.ApplicationId == ApplicationId);
                status.Status = "Inspector Rejected";
                db.StatusTracks.Attach(status);
                var statustrackentry = db.Entry(status);
                statustrackentry.Property(e => e.Status).IsModified = true;

                db.SaveChanges();
                string mailbody = " < br />< br /> We are very Sorry to inform you that your Appliction is Rejected on<strong> Santander UK </ strong > " +
                   "<br><br> you can try it later!!";
                string subjectmail = "your Application is Rejected";
                SendEmail(loanform.EmailId, mailbody, subjectmail);

                return RedirectToAction("InspectorView");
            }
        }



        public ActionResult Logout()
        {
            string Id = (string)Session["Id"];
            Session.Abandon();
            return RedirectToAction("LogIn", "RegisterLogin");
        }
        [NonAction]
        public void SendEmail(string EmailId, string mailbody, string subjectmail)
        {
            var from = new MailAddress("agrawalsanskriti00@gmail.com", "Santander UK ");
            var to = new MailAddress(EmailId);
            var frompw = "Sanskriti7691807047";
            string sub = subjectmail;
            string body = mailbody;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, frompw)


            };
            using (var message = new MailMessage(from, to))
            {
                message.Subject = sub;
                message.Body = body;
                message.IsBodyHtml = true;

                smtp.Send(message);

            }
        }
    }
}