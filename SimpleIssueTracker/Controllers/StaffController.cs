using SimpleIssueTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SimpleIssueTracker.Controllers
{
    public class StaffController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Staff model)
        {
            using (TrackingDBEntities db = new TrackingDBEntities())
            {
                var obj = db.Staffs.Where(a => a.UserName.Equals(model.UserName) && a.Password.Equals(model.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["StaffID"] = obj.Id.ToString();
                    Session["UserName"] = obj.UserName.ToString();
                    return RedirectToAction("StaffDashBoard");
                }
            }
            return View(model);


        }

        public ActionResult StaffDashBoard()
        {
            if (Session["StaffID"] != null)
            {

                using (TrackingDBEntities db = new TrackingDBEntities())
                {
                    var CustomersList =  db.Customers.ToList();
                    var query = (from Cust in db.Customers
                                 join staff in db.Staffs on Cust.StaffId equals staff.Id
                                 select new
                                 {
                                  staff.UserName
                                 }).ToList();
                    ViewBag.UserName = query;
                    
                    return View(CustomersList);
                }

            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public ActionResult AssignTicket(int id)
        {

            using (TrackingDBEntities db = new TrackingDBEntities())
            {
                ViewBag.StaffList = new SelectList(db.Staffs.ToList().
          Select(x => new { id = x.Id, name = x.UserName.Split('@')[0] }), "Id", "Name "); 


                ViewBag.StatusList= new SelectList(db.Status.ToList().
           Select(x => new { id = x.Id, name = x.Name }), "Id", "Name ");
                var data = db.Customers.Where(x => x.Id == id).FirstOrDefault();
                return View(data);
            }
           
        }

        [HttpPost]
        public ActionResult AssignTicket(Customer Model)
        {
            using (TrackingDBEntities db = new TrackingDBEntities())
            {
                var data = db.Customers.Where(x => x.Id == Model.Id).FirstOrDefault();
                if (data != null)
                {
                    data.StaffId = Model.StaffId;
                    data.Status = Model.Status;
                    db.SaveChanges();
                }

                return RedirectToAction("StaffDashBoard");


            }
               
        }

        [HttpGet]
        public ActionResult Unassigned()
        {
            using (TrackingDBEntities db = new TrackingDBEntities())
            {
                var data = db.Customers.Where(x => x.Status == "1").ToList();
                return View(data);
            }
               
         
        }

        [HttpGet]
        public ActionResult Open()
        {
            using (TrackingDBEntities db = new TrackingDBEntities())
            {
                var data = db.Customers.Where(x => x.Status == "2").ToList();
                return View(data);
            }


        }
        [HttpGet]
        public ActionResult Onhold()
        {
            using (TrackingDBEntities db = new TrackingDBEntities())
            {
                var data = db.Customers.Where(x => x.Status == "3").ToList();
                return View(data);
            }


        }
        [HttpGet]
        public ActionResult Closed()
        {
            using (TrackingDBEntities db = new TrackingDBEntities())
            {
                var data = db.Customers.Where(x => x.Status == "4" || x.Status=="5").ToList();
                return View(data);
            }


        }

        [HttpGet]
        public ActionResult Reply()
        {
                return View();
            }

            [HttpPost]
            public ActionResult Reply(Customer Model)
            {
                using (TrackingDBEntities db = new TrackingDBEntities())
                {

                    return RedirectToAction("StaffDashBoard");


                }

            }

        }
    }
