using SimpleIssueTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Net.Mail;

namespace SimpleIssueTracker.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateEnquiry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateEnquiry(Customer model)
        {
            using (TrackingDBEntities db = new TrackingDBEntities())
            {
               var guid = Guid.NewGuid().ToString().Substring(0, 11);
                string result = String.Empty;
                for (int i = 0; i < guid.Length; i++)
                {
                    if (guid[i] != '-') result += guid[i];
                }
                model.Ticket = result;
                model.Status = "Waiting for Staff Response";
                db.Customers.Add(model);
                db.SaveChanges();
                ViewBag.Message = "Customer Enquiry Created Successfully";
                ModelState.Clear();
               

                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("localhost");
                mail.From = new MailAddress("admin@gmail.com");
                mail.To.Add(model.Email);
                mail.Subject = model.EnqSubject;
                mail.Body = model.Ticket;
                smtpServer.Port = 25;
                smtpServer.Send(mail);
                return View();
            }


           
        }
    }
}