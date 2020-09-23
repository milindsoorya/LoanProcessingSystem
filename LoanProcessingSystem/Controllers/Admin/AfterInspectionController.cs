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

namespace LoanProcessingSystem.Controllers.Admin
{
    public class AfterInspectionController : Controller
    {
        // GET: AfterInspection
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AfterInspection()
        {

            using (MortgageDbEntities dc = new MortgageDbEntities())
            {
                
                var v = dc.LoanForms.Where(a => a.Status == "Inspector Approved");


                var loanForms = dc.LoanForms.Include(l => l.AdminDetail);
                return View(v.ToList());
            }

        }

        [HttpPost]
        public ActionResult ManagerApprove(int ApplicationId)
        {
            using (MortgageDbEntities db = new MortgageDbEntities())
            {
                //updating status in loanForm
                var loanform = db.LoanForms.FirstOrDefault(x => x.ApplicationId == ApplicationId);
                if (loanform == null) throw new Exception("Invalid id: " + ApplicationId);
                loanform.Status = "Approved";
                db.LoanForms.Attach(loanform);
                var entry = db.Entry(loanform);
                entry.Property(e => e.Status).IsModified = true;

                //updating status in StatusTrack
                var status = db.StatusTracks.FirstOrDefault(x => x.ApplicationId == ApplicationId);
                status.Status = "Manager Approved";
                db.StatusTracks.Attach(status);
                var statustrackentry = db.Entry(status);
                statustrackentry.Property(e => e.Status).IsModified = true;

                db.SaveChanges();
                string mailbody = " < br />< br /> We are very Excited to inform you that your Appliction is Approved by Manager on<strong> Santander UK </ strong > " +
                     "<br><br> We will get back to you later!!";
                string subjectmail = "your Application is Approved";
                SendEmail(loanform.EmailId, mailbody, subjectmail);
                return RedirectToAction("AfterInspection");
            }
        }

        [HttpPost]
        public ActionResult ManagerReject(int ApplicationId)
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
                status.Status = "Manager Rejected";
                db.StatusTracks.Attach(status);
                var statustrackentry = db.Entry(status);
                statustrackentry.Property(e => e.Status).IsModified = true;

                db.SaveChanges();
                string mailbody = " < br />< br /> We are very sorry to inform you that your Appliction is rejected on<strong> Santander UK </ strong > " +
                      "<br><br> You can try again later!!";
                string subjectmail = "your Application is rejected";
                SendEmail(loanform.EmailId, mailbody, subjectmail);
                return RedirectToAction("AfterInspection");
            }
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