using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hello.DAL;
using Hello.Models;

namespace Hello.Controllers
{
    public class HomeController : Controller
    {
        private PersonContext db = new PersonContext();

        // GET: People
        public ActionResult Index()
        {
            return View(db.Persons.ToList());
        }

        [HttpPost]
        public JsonResult AjaxMethod(string name)
        {
            var query_join = from c in db.Persons
                             join a in db.Files
                             on c.ID equals a.PersonId into gr
                             from c_joined in gr.DefaultIfEmpty()

                             select new
                             {
                                 c.FirstMidName,
                                 c.LastName,
                                 c.Age,
                                 c.Address,
                                 c.Interest,
                                 c_joined.Content,
                                 c_joined.ContentType
                             };

            if (!String.IsNullOrEmpty(name))
            {
               query_join = query_join.Where(s => s.LastName.Contains(name)
                              || s.FirstMidName.Contains(name));
            }

            return Json(query_join);
        }

        
        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Person person = db.Persons.Find(id);

            Person person = db.Persons.Include(s => s.Files).SingleOrDefault(s => s.ID == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
       
        public ActionResult Create()
        {
            return View();
        }
        

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName, FirstMidName, Age, Address, Interest" )] Person person, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var photo = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.Photo,
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        photo.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    person.Files = new List<File> { photo };
                }
    
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
              
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,DateTime")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
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
    }
}
