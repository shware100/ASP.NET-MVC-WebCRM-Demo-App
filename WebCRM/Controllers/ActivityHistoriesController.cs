using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebCRM.DAL;
using WebCRM.Helpers;
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
            // var activityHistory = db.ActivityHistory.Include(a => a.Company).Include(a => a.Contact);
            // return View(activityHistory.ToList());
            return View();
        }

        public ActionResult GetActivitiesList(JQDTblParamModel dTblParams)
        {

            string countSQL = "";
            string selectSQL = "";
            string sqlWhere = " WHERE 1=1 ";
            string sqlOrder = " ORDER BY ";
            string sqlOffSetLimit = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", dTblParams.iDisplayStart, dTblParams.iDisplayLength);

            countSQL = "select count(*) " +
                       "  from ActivityHistory a " +
                       " inner join Contact c on c.ID = a.ContactID" +
                       " inner join CompanyContact cc on cc.ContactID = c.ID " +
                       " inner join Company co on co.ID = cc.companyID ";

            selectSQL = "select a.ID, a.StartDate, a.EndDate, a.ActivityType, a.ActivityStatus, " +
                        "       co.Name CompanyName, c.LastName + ', ' + c.FirstName ContactName, " +
                        "       a.ActivityStatus, a.Subject, c.LastName, c.FirstName " +
                        "  from ActivityHistory a " +
                        "       inner join Contact c on c.ID = a.ContactID " +
                        "       inner join CompanyContact cc on c.ID = cc.contactID" +
                        "       inner join Company co on co.ID = cc.companyID ";

            var sVal = "";
            if (!string.IsNullOrEmpty(dTblParams.sSearch))
            {
                sVal = string.Format("%{0}%", dTblParams.sSearch);
                sqlWhere += " AND (a.ActivityType like @p0 or a.ActivityStatus like @p0 " +
                            "  or a.Subject like @p0 " +
                            "  or c.LastName like @p0 or c.FirstName like @p0 " +
                            "  or co.Name like @p0) ";
            }

            var sortColIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDir = Request["sSortDir_0"];
            sqlOrder += (sortColIndex == 0 ? string.Format("c.ID {0}", sortDir) :
                         sortColIndex == 1 ? string.Format("a.StartDate {0}", sortDir) :
                         sortColIndex == 2 ? string.Format("a.EndDate {0}", sortDir) :
                         sortColIndex == 3 ? string.Format("co.Name {0}", sortDir) :
                         sortColIndex == 4 ? string.Format("c.LastName {0}, c.FirstName {0}", sortDir) :
                         sortColIndex == 5 ? string.Format("a.ActivityStatus {0}", sortDir) :
                         string.Format("a.StartDate {0}", sortDir));

            string countSQLFinal = string.Format("{0} {1}", countSQL.ToString(), sqlWhere);
            string selectSQLFinal = string.Format("{0} {1} {2} {3}", selectSQL.ToString(), sqlWhere, sqlOrder, sqlOffSetLimit);

            var totalRecs = db.Database.SqlQuery<Int32>(countSQLFinal, sVal);
            IEnumerable<ActivityListViewModel> displayedActivities;
            if (!string.IsNullOrEmpty(sVal))
                displayedActivities = db.Database.SqlQuery<ActivityListViewModel>(selectSQLFinal, sVal).ToList();
            else
                displayedActivities = db.Database.SqlQuery<ActivityListViewModel>(selectSQLFinal).ToList();

            var dispRecs = displayedActivities.Count();

            return Json(new
            {
                sEcho = dTblParams.sEcho,
                iTotalRecords = totalRecs.ElementAt(0),
                iTotalDisplayRecords = totalRecs.ElementAt(0),
                data = Json(displayedActivities).Data
            },
            "application/json",
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCalendarActivities(FullCalParamModel fullCalParams)
        {
            // gets calendar entries to be displayed in the FullCalendar javascript components
            // which retrieves calendar items between a start and end date
            string selectSQL = "";
            string sqlWhere = " WHERE 1=1 ";
            string sqlOrder = " ORDER BY a.StartDate asc";

            // restrict to 250 items
            // TODO: move these into a config file somewhere
            int iDisplayStart = 0;
            int iDisplayLength = 500;
            string sqlOffSetLimit = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", iDisplayStart, iDisplayLength);

            selectSQL = "select a.ID, a.StartDate start, a.EndDate, a.ActivityType, a.ActivityStatus, " +
                        "       co.Name CompanyName, c.LastName + ', ' + c.FirstName ContactName, " +
                        "       a.ActivityStatus, a.Subject, c.LastName, c.FirstName " +
                        "  from ActivityHistory a " + 
                        "       inner join Contact c on c.ID = a.ContactID " +
                        "       inner join CompanyContact cc on c.ID = cc.contactID" +
                        "       inner join Company co on co.ID = cc.companyID ";

            DateTime? startDateParam = System.DateTime.Now;
            DateTime? endDateParam = System.DateTime.Now;

            if  (fullCalParams.StartDate != null && fullCalParams.EndDate != null) {
                sqlWhere += " AND (StartDate >= @p0 " +
                            " AND (EndDate IS NULL OR EndDate <= @p1)) ";
                startDateParam = fullCalParams.StartDate;
                endDateParam = fullCalParams.EndDate;
            } else if (fullCalParams.StartDate != null && fullCalParams.EndDate == null) {
                sqlWhere += " AND (StartDate = @p0 " +
                            " OR EndDate = @p1) ";
                startDateParam = fullCalParams.StartDate;
                endDateParam = fullCalParams.EndDate;
            } else if (fullCalParams.StartDate == null && fullCalParams.EndDate != null) {
                sqlWhere += " AND (StartDate = @p0 " +
                            " OR EndDate = @p1) ";
                startDateParam = fullCalParams.EndDate;
                endDateParam = fullCalParams.StartDate;
            }

            string selectSQLFinal = string.Format("{0} {1} {2} {3}", selectSQL.ToString(), sqlWhere, sqlOrder, sqlOffSetLimit);

            IEnumerable<ActivityCalendarViewModel> displayedActivities;
            displayedActivities = db.Database.SqlQuery<ActivityCalendarViewModel>(selectSQLFinal, startDateParam, endDateParam).ToList();

            return new JsonNetResult() { Data = displayedActivities };

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
        public ActionResult Create(string fromId, string from, string dateIn)
        {
            ActivityHistory activityHistory = new ActivityHistory();
            activityHistory.ActivityStatus = ActivityStatus.Active;
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
            else
                ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", -1);

            if (from == "company_view" || from == "contact_view")
            {
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
            }
            ViewBag.from = from;
            ViewBag.fromId = fromId;

            if (!string.IsNullOrEmpty(dateIn))
            {
                activityHistory.StartDate = DateTime.Parse(dateIn);
                activityHistory.EndDate = activityHistory.StartDate;
            }
            else
            {
                activityHistory.StartDate = DateTime.Now;
                activityHistory.EndDate = DateTime.Now;
            }
            return View(activityHistory);
        }

        // POST: ActivityHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ActivityType,ActivityStatus,CompanyID,ContactID,StartDate,EndDate,Subject,Comments,CreatedAt,CreatedByID,Updatedat,UpdatedByID")] ActivityHistory activityHistory, string fromId, string from)
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
            int coId = -1;
            if (activityHistory.CompanyID != 0)
              coId = activityHistory.CompanyID;

            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", activityHistory.CompanyID);


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

            ViewBag.ContactID = new SelectList(q.ToList<ContactListViewModel>(), "ID", "FullName", activityHistory.ContactID);

            ViewBag.from = from;
            return View(activityHistory);
        }

        // POST: ActivityHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ActivityType,ActivityStatus,CompanyID,ContactID,StartDate,EndDate,Subject,Comments,CreatedAt,CreatedByID,Updatedat,UpdatedByID")] ActivityHistory activityHistory, string from)
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
        // POST: ActivityHistories/UpdateCalendarDates/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCalendarDates(int? activityID, string months, string days, string msecs, string resize)
        {
            string rtn = "+ok";

            ActivityHistory activityHistory = db.ActivityHistory.Find(activityID);
            if (activityHistory == null)
            {
                rtn = "404";
            }
            else
            {
                int monthsOffset = int.Parse(months);
                int daysOffset = int.Parse(days);
                int msecsOffset = int.Parse(msecs);

                if (resize != "true")
                  activityHistory.StartDate = activityHistory.StartDate.AddMonths(monthsOffset).AddDays(daysOffset).AddMilliseconds(msecsOffset);
                activityHistory.EndDate = activityHistory.EndDate.AddMonths(monthsOffset).AddDays(daysOffset).AddMilliseconds(msecsOffset);
                db.Entry(activityHistory).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json (new { status = rtn}, JsonRequestBehavior.AllowGet);

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
