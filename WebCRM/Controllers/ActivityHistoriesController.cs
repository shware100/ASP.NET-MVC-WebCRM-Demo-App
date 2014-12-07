using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebCRM.DAL;
using WebCRM.Models;
using WebCRM.ViewModels;

namespace WebCRM.Controllers
{
    public class ActivityHistoriesController : Controller
    {
        private WebCRMDBContext db = new WebCRMDBContext();

        // GET: ActivityHistories
        public ActionResult Index()
        {
            var activityHistory = db.ActivityHistory.Include(a => a.Company).Include(a => a.Contact);
            return View(activityHistory.ToList());
        }

        // GET: ActivityHistories/Details/5
        public ActionResult Details(int? id, string from)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityHistory activityHistory = db.ActivityHistory.Find(id);
            if (activityHistory == null)
            {
                return HttpNotFound();
            }

            ViewBag.from = from;

            return View(activityHistory);
        }

        // GET: ActivityHistories/Create
        public ActionResult Create(string fromId, string from)
        {
            ViewBag.ContactID = null;
            int coId = -1;
            int contactId = -1;
            if (from == "company_view")
            {
                ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", fromId);
                coId = int.Parse(fromId);
            }
            else if (from == "contact_view")
            {
                contactId = int.Parse(fromId);
                var contact = db.Contacts.Find(contactId);
                coId = contact.Companies.First().ID;
                ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", contact.Companies.First().ID);
            }
            IQueryable<ContactListViewModel> q = from contacts in db.Contacts
                                                 from companies in contacts.Companies
                                                 where companies.ID == coId
                                                 select new ContactListViewModel()
                                                 {
                                                     ID = contacts.ID,
                                                     Honorific = contacts.Honorific,
                                                     FirstName = contacts.FirstName,
                                                     MiddleName = contacts.MiddleName,
                                                     LastName = contacts.LastName,
                                                     Suffix = contacts.Suffix,
                                                     Title = contacts.Title,
                                                     Phone = contacts.Phone,
                                                     Email = contacts.Email,
                                                     CompanyName = companies.Name
                                                 };

            ViewBag.ContactID = new SelectList(q.ToList<ContactListViewModel>(), "ID", "FullName", contactId);

            
            ViewBag.from = from;
            ViewBag.fromId = fromId;
            return View();
        }

        // POST: ActivityHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ActivityType,ActivityStatus,CompanyID,ContactID,ActivityDate,Subject,Comments,CreatedAt,CreatedByID,Updatedat,UpdatedByID")] ActivityHistory activityHistory, string fromId, string from)
        {
            if (ModelState.IsValid)
            {
                activityHistory.CreatedAt = DateTime.Now;
                activityHistory.Updatedat = DateTime.Now;

                db.ActivityHistory.Add(activityHistory);
                db.SaveChanges();

                if (from == "contact_view")
                    return RedirectToAction("Details", "Contacts", new { id = activityHistory.ContactID});
                else if (from == "company_view")
                    return RedirectToAction("Details", "Companies", new { id = activityHistory.CompanyID });
                else
                    return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", activityHistory.CompanyID);
            ViewBag.ContactID = new SelectList(db.Contacts, "ID", "Honorific", activityHistory.ContactID);
            ViewBag.from = from;
            return View(activityHistory);
        }

        // GET: ActivityHistories/Edit/5
        public ActionResult Edit(int? id, string from)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityHistory activityHistory = db.ActivityHistory.Find(id);
            if (activityHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", activityHistory.CompanyID);
            ViewBag.ContactID = new SelectList(db.Contacts, "ID", "Honorific", activityHistory.ContactID);
            ViewBag.from = from;
            return View(activityHistory);
        }

        // POST: ActivityHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ActivityType,ActivityStatus,CompanyID,ContactID,ActivityDate,Subject,Comments,CreatedAt,CreatedByID,Updatedat,UpdatedByID")] ActivityHistory activityHistory, string from)
        {
            if (ModelState.IsValid)
            {
                activityHistory.CreatedAt = DateTime.Now;
                activityHistory.Updatedat = DateTime.Now;

                db.Entry(activityHistory).State = EntityState.Modified;
                db.SaveChanges();

                if (from == "contact_view")
                    return RedirectToAction("Details", "Contacts", new { id = activityHistory.ContactID });
                else if (from == "company_view")
                    return RedirectToAction("Details", "Companies", new { id = activityHistory.CompanyID });
                else
                    return RedirectToAction("Index");
            }
            ViewBag.from = from;
            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", activityHistory.CompanyID);
            ViewBag.ContactID = new SelectList(db.Contacts, "ID", "Honorific", activityHistory.ContactID);

            
            return View(activityHistory);
        }

        // GET: ActivityHistories/Delete/5
        public ActionResult Delete(int? id, string from)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityHistory activityHistory = db.ActivityHistory.Find(id);
            if (activityHistory == null)
            {
                return HttpNotFound();
            }

            ViewBag.from = from;
            return View(activityHistory);
        }

        // POST: ActivityHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string from)
        {
            ActivityHistory activityHistory = db.ActivityHistory.Find(id);
            int companyId = activityHistory.CompanyID;
            int contactId = activityHistory.ContactID;
            db.ActivityHistory.Remove(activityHistory);
            db.SaveChanges();

            if (from == "contact_view")
                return RedirectToAction("Details", "Contacts", new { id = contactId });
            else if (from == "company_view")
                return RedirectToAction("Details", "Companies", new { id = companyId });
            else
                return RedirectToAction("Index");

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
    }
}
