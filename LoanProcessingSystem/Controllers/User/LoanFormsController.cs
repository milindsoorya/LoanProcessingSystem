using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoanProcessingSystem.Models;

namespace LoanProcessingSystem.Controllers.User
{
    public class LoanFormsController : Controller
    {
        private MortgageDbEntities db = new MortgageDbEntities();

        // GET: LoanForms
        public ActionResult Index()
        {
            var loanForms = db.LoanForms.Include(l => l.AdminDetail);
            return View(loanForms.ToList());
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
                return RedirectToAction("Index");
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
            ViewBag.InspectorId = new SelectList(db.AdminDetails, "Id", "Name", loanForm.InspectorId);
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
                db.Entry(loanForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InspectorId = new SelectList(db.AdminDetails, "Id", "Name", loanForm.InspectorId);
            return View(loanForm);
        }

        // GET: LoanForms/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: LoanForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoanForm loanForm = db.LoanForms.Find(id);
            db.LoanForms.Remove(loanForm);
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
            return RedirectToAction("../LoanForms/Index/");
        }
    }
}
