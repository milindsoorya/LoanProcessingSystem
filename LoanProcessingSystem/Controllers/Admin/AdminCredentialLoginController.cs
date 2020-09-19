using LoanProcessingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanProcessingSystem.Controllers.Admin
{
    public class AdminCredentialLoginController : Controller
    {
        // GET: AdminCredentialLogin
        [HttpGet]
        public ActionResult AdminSectionLogIn()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AdminPanel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminSectionLogIn(AdminLogin adminlogin)
        {
            string message = "";
            ViewBag.Message = message;
            using (MortgageDbEntities dc = new MortgageDbEntities())
            {
                var v = dc.AdminDetails.Where(a => a.EmailId == adminlogin.EmailId).FirstOrDefault();
                if (v != null)
                {
                    if ((string.Compare((adminlogin.Password), v.Password) == 0) && (string.Compare((adminlogin.Role), v.Role) == 0))
                    {
                        if (v.Role == "Manager")
                        {
                            Session["id"] = v.Id;
                            return RedirectToAction("AdminPanel", "AdminCredentialLogin");
                        }
                        else if (v.Role == "Inspector")
                        {
                            Session["id"] = v.Id;
                            return RedirectToAction("InspectorView", "Inspector");
                        }
                    }
                    else
                    {
                        message = "Invalid Credential provided";
                    }
                }
                else
                {
                    message = "Invalid Credential provided";
                }

            }
            ViewBag.Message = message;
            return View();

        }
        public ActionResult Logout()
        {
            string Id = (string)Session["Id"];
            Session.Abandon();
            return RedirectToAction("LogIn", "LoginRegister");
        }
    }
}