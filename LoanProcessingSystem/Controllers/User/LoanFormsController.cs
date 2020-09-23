using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LoanProcessingSystem.Models;

namespace LoanProcessingSystem.Controllers.User
{   
    public class LoanFormsController : Controller
    {
        private MortgageDbEntities db = new MortgageDbEntities();

        // GET: LoanForms
        public ActionResult Index()
        {
            var v = db.LoanForms.Where(a => a.Status == "Active");

            var loanForms = db.LoanForms.Include(l => l.AdminDetail);
            return View(v.ToList());
        }

        public ActionResult Success()
        {
            return View();
        }

        // GET: LoanForms/Details/5
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

        // GET: LoanForms/Create
        public ActionResult Create()
        {
            ViewBag.InspectorId = new SelectList(db.AdminDetails, "Id", "Name");
            return View();
        }

        // POST: LoanForms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicationId,Name,EmailId,PhoneNo,Address,Salary,PropertyType,DOB,ImageUrl,Status,InspectorId")] LoanForm loanForm)
        {
            if (ModelState.IsValid)
            {
                loanForm.Status = "Active";                        
                db.LoanForms.Add(loanForm);
                db.SaveChanges();
                return RedirectToAction("Success");
            }

            ViewBag.InspectorId = new SelectList(db.AdminDetails, "Id", "Name", loanForm.InspectorId);
            return View(loanForm);
        }

        // GET: LoanForms/Edit/5
        public ActionResult Edit(int? id)
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
            var query = (from ca in db.AdminDetails
                         where ca.Role == "Inspector"
                         orderby ca.Name
                         select new SelectListItem { Text = ca.Name, Value = ca.Id.ToString() }).Distinct();
            List<SelectListItem> list = query.ToList();

            //IEnumerable<SelectListItem> selectList =
            //from c in db.AdminDetails
            //select new SelectListItem
            //{
            //    Selected = (c.Role == "Inspector"),
            //    Text = c.Name,
            //    Value = c.Name.ToString()
            //};

            ViewBag.InspectorId = new SelectList(list, "Value", "Text", loanForm.InspectorId);
           
            return View(loanForm);
        }

        // POST: LoanForms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationId,Name,EmailId,PhoneNo,Address,Salary,PropertyType,DOB,ImageUrl,Status,InspectorId")] LoanForm loanForm)
        {
            if (ModelState.IsValid)
            {
                loanForm.Status = "Inspector Assigned";
                db.Entry(loanForm).State = EntityState.Modified;
                db.SaveChanges();
                string mailbody = " < br />< br /> We are excited to tell you that your Appliction is in inspection Process on<strong> Santander UK </ strong > " +
                      "<br/><br/> Congratulations!!" + "<br><br> We Will update you later!!";
                string subjectmail = "your Application is in Inspection";
                SendEmail(loanForm.EmailId, mailbody, subjectmail);
                return RedirectToAction("InsertStatus", new { loanForm.ApplicationId, loanForm.InspectorId, loanForm.EmailId });
                //return RedirectToAction("Index");
            }
            ViewBag.InspectorId = new SelectList(db.AdminDetails, "Id", "Name", loanForm.InspectorId);
           
            return View(loanForm);
        }

        public ActionResult InsertStatus(int ApplicationId, int InspectorId, string EmailId)
        {
            using (MortgageDbEntities db = new MortgageDbEntities())
            {
                StatusTrack status = new StatusTrack();
                status.ApplicationId = ApplicationId;
                //var email = Session["Emailid"].ToString();
                var user = db.UserRegisters.Where(x => x.EmailId == EmailId).FirstOrDefault();
                status.UserId = user.UserId;
                status.AuthorityId = InspectorId;
                status.Date = DateTime.Now.Date;
                status.Status = "Application in Inspection";

                db.StatusTracks.Add(status);
                db.SaveChanges();

                //return RedirectToAction("InspectorView");
                return RedirectToAction("Index");
            }
        }




        // GET: LoanForms/Delete/5
        public ActionResult Delete(int? ApplicationId)
        {
            if (ApplicationId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanForm loanForm = db.LoanForms.Find(ApplicationId);
            if (loanForm == null)
            {
                return HttpNotFound();
            }
            return View(loanForm);
        }

        // POST: LoanForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ApplicationId, int? InspectorId, string EmailId)
        {
            LoanForm loanForm = db.LoanForms.Find(ApplicationId);
            loanForm.Status = "Rejected";
            db.LoanForms.Attach(loanForm);
            var entry = db.Entry(loanForm);
            entry.Property(e => e.Status).IsModified = true;

            //db.LoanForms.Remove(loanForm);

            //change in status table
            StatusTrack status = new StatusTrack();
            status.ApplicationId = ApplicationId;
            var user = db.UserRegisters.Where(x => x.EmailId == EmailId).FirstOrDefault();
            status.UserId = user.UserId;
            status.AuthorityId = 1;
            status.Date = DateTime.Now.Date;
            status.Status = "Application Rejected";
            

            db.StatusTracks.Add(status);

            db.SaveChanges();
            string mailbody = " < br />< br /> We are very sorry to inform you that your Appliction is rejected on<strong> Santander UK </ strong > " +
                      "<br><br> You can try again later!!";
            string subjectmail = "your Application is rejected";
            SendEmail(loanForm.EmailId, mailbody, subjectmail);
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
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                MortgageDbEntities db = new MortgageDbEntities();
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/Pics/" + ImageName);
                file.SaveAs(physicalPath);
                LoanForm loanform = new LoanForm();
                loanform.Name = Request.Form["Name"];
                loanform.EmailId = Request.Form["EmailId"];
                loanform.PhoneNo = decimal.Parse(Request.Form["PhoneNo"]);
                loanform.Address = Request.Form["Address"];
                loanform.Salary = decimal.Parse(Request.Form["Salary"]);
                loanform.PropertyType = Request.Form["PropertyType"];
                loanform.DOB = DateTime.Parse(Request.Form["DOB"]);
                loanform.ImageUrl = ImageName;
                loanform.Status = "Active";
                db.LoanForms.Add(loanform);     
                db.SaveChanges();

            }
            return RedirectToAction("../LoanForms/Success/");
        }
        [NonAction]
        public void SendEmail(string EmailId,string mailbody,string subjectmail)
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
