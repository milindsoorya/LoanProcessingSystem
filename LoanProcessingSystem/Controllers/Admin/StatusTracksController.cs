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
    public class StatusTracksController : Controller
    {
        private MortgageDbEntities db = new MortgageDbEntities();

        // GET: StatusTracks
        public ActionResult Index()
        {
            var statusTracks = db.StatusTracks.Include(s => s.AdminDetail).Include(s => s.LoanForm).Include(s => s.UserRegister);
            return View(statusTracks.ToList());
        }

        




        // GET: StatusTracks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusTrack statusTrack = db.StatusTracks.Find(id);
            if (statusTrack == null)
            {
                return HttpNotFound();
            }
            return View(statusTrack);
        }

        // GET: StatusTracks/Create
        public ActionResult Create()
        {
            ViewBag.AuthorityId = new SelectList(db.AdminDetails, "Id", "Name");
            ViewBag.ApplicationId = new SelectList(db.LoanForms, "ApplicationId", "Name");
            ViewBag.UserId = new SelectList(db.UserRegisters, "UserId", "FullName");
            return View();
        }

        // POST: StatusTracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusId,ApplicationId,UserId,AuthorityId,Date,Status")] StatusTrack statusTrack)
        {
            if (ModelState.IsValid)
            {
             //   var q = db.UserRegisters.Where(a => a.UserId ==)
                statusTrack.Date = DateTime.Today.Date;
                db.StatusTracks.Add(statusTrack);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.AuthorityId = new SelectList(db.AdminDetails, "Id", "Name", statusTrack.AuthorityId);
            ViewBag.ApplicationId = new SelectList(db.LoanForms, "ApplicationId", "Name", statusTrack.ApplicationId);
            ViewBag.UserId = new SelectList(db.UserRegisters, "UserId", "FullName", statusTrack.UserId);
            return View(statusTrack);
        }

        // GET: StatusTracks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusTrack statusTrack = db.StatusTracks.Find(id);
            if (statusTrack == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorityId = new SelectList(db.AdminDetails, "Id", "Name", statusTrack.AuthorityId);
            ViewBag.ApplicationId = new SelectList(db.LoanForms, "ApplicationId", "Name", statusTrack.ApplicationId);
            ViewBag.UserId = new SelectList(db.UserRegisters, "UserId", "FullName", statusTrack.UserId);
            return View(statusTrack);
        }

        // POST: StatusTracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusId,ApplicationId,UserId,AuthorityId,Date,Status")] StatusTrack statusTrack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(statusTrack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorityId = new SelectList(db.AdminDetails, "Id", "Name", statusTrack.AuthorityId);
            ViewBag.ApplicationId = new SelectList(db.LoanForms, "ApplicationId", "Name", statusTrack.ApplicationId);
            ViewBag.UserId = new SelectList(db.UserRegisters, "UserId", "FullName", statusTrack.UserId);
            return View(statusTrack);
        }

        // GET: StatusTracks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusTrack statusTrack = db.StatusTracks.Find(id);
            if (statusTrack == null)
            {
                return HttpNotFound();
            }
            return View(statusTrack);
        }

        // POST: StatusTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StatusTrack statusTrack = db.StatusTracks.Find(id);
            db.StatusTracks.Remove(statusTrack);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [NonAction]
        public void SendEmail(string EmailId,int ApplicationId,string Status)
        {
            var from = new MailAddress("agrawalsanskriti00@gmail.com", "Santander UK ");
            var to = new MailAddress(EmailId);
            var frompw = "Sanskriti7691807047";
            string sub = "your Application"+ApplicationId+"Status";
            string body = "<br/><br/>We are excited to tell you that your Application Status is Successfully Updated on <strong>Santander UK</strong>" +
                       "<br/><br/> Congratulations!!" + "<br><br>Your Application Status is"+Status;
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
        //[NonAction]
        //public string Emailgenerated(string ApplicationId)
        //{
        //    var q = db.UserRegisters.Where(a=>a.UserId==)

        //    return Convert.ToString(q).ToString();


//        }
    }
}
