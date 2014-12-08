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
    public class ContactsController : Controller
    {
        private WebCRMDBContext db = new WebCRMDBContext();

        // GET: Contacts
        public ActionResult Index()
        {
            // return View(db.Contacts.ToList());
            return View();
        }
        public ActionResult GetContactsList(JQDTblParamModel dTblParams)
        {

            string countSQL = "";
            string selectSQL = "";
            string sqlWhere = " WHERE 1=1 ";
            string sqlOrder = " ORDER BY ";
            string sqlOffSetLimit = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", dTblParams.iDisplayStart, dTblParams.iDisplayLength);

            countSQL = "select count(*) " +
                       "  from Contact c " +
                       " inner join CompanyContact cc on c.ID = cc.contactID" +
                       " inner join Company co on co.ID = cc.companyID ";

            selectSQL = "select c.ID, c.Honorific, c.FirstName, c.MiddleName, c.LastName, " +
                        "       c.Suffix, c.Title, c.Phone, c.Email, co.Name CompanyName " +
                        "  from Contact c " +
                        " inner join CompanyContact cc on c.ID = cc.contactID" +
                        " inner join Company co on co.ID = cc.companyID ";


            var sVal = "";
            if (!string.IsNullOrEmpty(dTblParams.sSearch))
            {
                sVal = string.Format("%{0}%", dTblParams.sSearch);
                // sqlWhere += " AND (c.Honorific like '%@p0%' or c.FirstName like '%@p0%' " +
                //            "  or c.MiddleName like '%@p0%' or c.LastName like '%@p0%' " +
                //            "  or c.Suffix like '%@p0%' or c.Phone like '%@p0%' " +
                //            "  or c.Email like '%@p0%' or co.Name like '%@p0%')";
                sqlWhere += " AND (c.Honorific like @p0 or c.FirstName like @p0 " +
                           "  or c.MiddleName like @p0 or c.LastName like @p0 " +
                           "  or c.Suffix like @p0 or c.Phone like @p0 " +
                           "  or c.Email like @p0 or co.Name like @p0)";
            }

            var sortColIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDir = Request["sSortDir_0"];
            sqlOrder += (sortColIndex == 0 ? string.Format("c.ID {0}", sortDir) :
                        sortColIndex == 1 ? string.Format("c.LastName {0}, c.FirstName {0}", sortDir) :
                        sortColIndex == 2 ? string.Format("c.Title {0}", sortDir) :
                        sortColIndex == 3 ? string.Format("c.Phone {0}", sortDir) :
                        sortColIndex == 4 ? string.Format("c.Email {0}", sortDir) :
                        sortColIndex == 5 ? string.Format("co.Name {0}", sortDir) :
                        string.Format("c.LastName {0}, c.FirstName {0}", sortDir));

            string countSQLFinal = string.Format("{0} {1}", countSQL.ToString(), sqlWhere);
            string selectSQLFinal = string.Format("{0} {1} {2} {3}", selectSQL.ToString(), sqlWhere, sqlOrder, sqlOffSetLimit);

            var totalRecs = db.Database.SqlQuery<Int32>(countSQLFinal, sVal);
            IEnumerable<ContactListViewModel> displayedContacts;
            if (!string.IsNullOrEmpty(sVal))
                displayedContacts = db.Database.SqlQuery<ContactListViewModel>(selectSQLFinal, sVal).ToList();
            else
                displayedContacts = db.Database.SqlQuery<ContactListViewModel>(selectSQLFinal, sVal).ToList();

            var dispRecs = displayedContacts.Count();

            // generate array appropriate for dataTables.net javascript handlers
            /*
            var q = from c in displayedContacts
                    select new[]
                  {
                    c.ID.ToString(),
                    c.FullName.ToString(),
                    c.Title.ToString(),
                    c.Phone.ToString(),
                    c.Email.ToString(),
                    c.CompanyName.ToString(),
                    c.Honorific.ToString(),
                    c.FirstName.ToString(),
                    c.MiddleName.ToString(),
                    c.LastName.ToString()
                  };
            */
            return Json(new
            {
                sEcho = dTblParams.sEcho,
                iTotalRecords = totalRecs.ElementAt(0),
                iTotalDisplayRecords = totalRecs.ElementAt(0),
                data = Json(displayedContacts).Data
            },
            "application/json",
            JsonRequestBehavior.AllowGet);
        }


        // GET: Contacts/Details/5
        public ActionResult Details(int? id, string from)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            if (contact.Companies.Count > 0)
            {
                Company c = contact.Companies.First();
                ViewBag.selectedCompanyName = c.Name;
            }

            ViewBag.from = from;
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create(int? companyId, string from)
        {
            ViewBag.selectedCompanyID = new SelectList(db.Companies, "ID", "Name", companyId);
            ViewBag.from = from;
            ViewBag.fromCompanyId = companyId;
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Honorific,FirstName,MiddleName,LastName,Suffix,Title,Phone,Fax,Email,AlternateEmail")] Contact contact, string selectedCompanyID, string from)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedAt = DateTime.Now;
                contact.UpdatedAt = DateTime.Now;
                if (!string.IsNullOrEmpty(selectedCompanyID))
                {
                    var company = db.Companies.Find(int.Parse(selectedCompanyID));
                    if (company != null)
                    {
                        contact.Companies = new List<Company>();
                        contact.Companies.Add(company);
                    }
                }
                db.Contacts.Add(contact);
                db.SaveChanges();
                if (from == "company_view")
                {
                    return RedirectToAction("Details", "Companies", new { id = selectedCompanyID });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id, string from)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            if (contact.Companies.Count > 0)
            {
                ViewBag.selectedCompanyID = new SelectList(db.Companies, "ID", "Name", contact.Companies.First().ID);
            }
            else
            {
                ViewBag.selectedCompanyID = new SelectList(db.Companies, "ID", "Name");
            }

            if (contact.Companies.Count > 0)
            {
                Company c = contact.Companies.First();
            }

            ViewBag.from = from;

            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Honorific,FirstName,MiddleName,LastName,Suffix,Title,Phone,Fax,Email,AlternateEmail")] Contact contact, string selectedCompanyID, string from)
        {
            if (ModelState.IsValid)
            {
                // TODO: take advantage of the many-to-many model - below ensures contact belongs to 1 company
                // Force loading of Companies collection due to lazy loading
                db.Entry(contact).State = EntityState.Modified;
                db.Entry(contact).Collection(co => co.Companies).Load();
                contact.Companies.Clear();
                if (!string.IsNullOrEmpty(selectedCompanyID))
                {
                    var company = db.Companies.Find(int.Parse(selectedCompanyID));
                    if (company != null)
                    {
                        contact.Companies.Add(company);
                    }
                }

                // TODO: grab createdat from original record - not getting set 
                contact.CreatedAt = DateTime.Now;
                contact.UpdatedAt = DateTime.Now;
                db.SaveChanges();

                if (from == "company_view")
                    return RedirectToAction("Details", "Companies", new { id = selectedCompanyID });
                else
                    return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id, string from)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            ViewBag.from = from;
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string from)
        {

            Contact contact = db.Contacts.Find(id);
            int companyID = contact.Companies.First().ID;
            db.Contacts.Remove(contact);
            db.SaveChanges();

            if (from == "company_view")
                return RedirectToAction("Details", "Companies", new { id = companyID });
            else
                return RedirectToAction("Index");
        }

        // GET: Contacts/ActivityHistory/5
        [HttpGet, ActionName("ActivityHistory")]
        public ActionResult ActivityHistory(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
                return HttpNotFound();

            IQueryable<ActivityHistory> q = from activities in db.ActivityHistory
                                            where activities.ContactID == id
                                            select activities;

            // return View(db.Contacts.ToList());
            ViewBag.ActivityListFrom = "contact_view";
            return PartialView("../ActivityHistories/_ActivityList", q.ToList());

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
