using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GAPT.Models;

namespace GAPT.Controllers
{
    public class tabletestsController : Controller
    {
        private dummydbEntities db = new dummydbEntities();

        // GET: tabletests
        public ActionResult Index()
        {
            return View(db.tabletests.ToList());
        }
        public ActionResult Search()
        {
            return View();
        }

    // GET: tabletests/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tabletest tabletest = db.tabletests.Find(id);
            if (tabletest == null)
            {
                return HttpNotFound();
            }
            return View(tabletest);
        }

        // GET: tabletests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tabletests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,caption,price")] tabletest tabletest)
        {
            if (ModelState.IsValid)
            {
                db.tabletests.Add(tabletest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tabletest);
        }

        // GET: tabletests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tabletest tabletest = db.tabletests.Find(id);
            if (tabletest == null)
            {
                return HttpNotFound();
            }
            return View(tabletest);
        }

        // POST: tabletests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,caption,price")] tabletest tabletest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabletest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tabletest);
        }

        // GET: tabletests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tabletest tabletest = db.tabletests.Find(id);
            if (tabletest == null)
            {
                return HttpNotFound();
            }
            return View(tabletest);
        }

        // POST: tabletests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tabletest tabletest = db.tabletests.Find(id);
            db.tabletests.Remove(tabletest);
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
