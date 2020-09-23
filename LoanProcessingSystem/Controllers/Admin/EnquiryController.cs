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
    public class EnquiryController : Controller
    {
        private MortgageDbEntities db = new MortgageDbEntities();

        // GET: Enquiry
        public ActionResult Index()
        {
            return View(db.EnquiryTables.ToList());
        }

        // GET: Enquiry/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnquiryTable enquiryTable = db.EnquiryTables.Find(id);
            if (enquiryTable == null)
            {
                return HttpNotFound();
            }
            return View(enquiryTable);
        }

        // GET: Enquiry/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult enq_success()
        {
            return View();
        }


        // POST: Enquiry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnquiryId,EmailId,PhoneNo,PropertyType,Date,Message")] EnquiryTable enquiryTable)
        {
            if (ModelState.IsValid)
            {
                db.EnquiryTables.Add(enquiryTable);
                db.SaveChanges();
                SendEmail(enquiryTable.EmailId);
                return RedirectToAction("enq_success","Enquiry");
            }

            return View(enquiryTable);
        }

        // GET: Enquiry/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnquiryTable enquiryTable = db.EnquiryTables.Find(id);
            if (enquiryTable == null)
            {
                return HttpNotFound();
            }
            return View(enquiryTable);
        }

        // POST: Enquiry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnquiryId,EmailId,PhoneNo,PropertyType,Date,Message")] EnquiryTable enquiryTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enquiryTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enquiryTable);
        }

        // GET: Enquiry/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnquiryTable enquiryTable = db.EnquiryTables.Find(id);
            if (enquiryTable == null)
            {
                return HttpNotFound();
            }
            return View(enquiryTable);
        }

        // POST: Enquiry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnquiryTable enquiryTable = db.EnquiryTables.Find(id);
            db.EnquiryTables.Remove(enquiryTable);
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
        public void SendEmail(string EmailId)
        {
            var from = new MailAddress("agrawalsanskriti00@gmail.com", "Santander UK ");
            var to = new MailAddress(EmailId);
            var frompw = "Sanskriti7691807047";
            string sub = "your Query is successfully Submitted";
            string body = "<br/><br/>We are excited to tell you that your Query is Successfully Submitted on <strong>Santander UK</strong>" +
                       "<br/><br/> Congratulations!!"+"We will Contact back as soon as possible";
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
