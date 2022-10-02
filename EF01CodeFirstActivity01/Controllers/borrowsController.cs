using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassActivity.Models;

namespace ClassActivity.Controllers
{
    public class borrowsController : Controller
    {
        private LibraryEntities acc = new LibraryEntities();

        // GET: borrows
        public ActionResult Index()
        {
            var borrows = acc.borrows.Include(b => b.book).Include(b => b.student);
            return View(borrows.ToList());
        }

        // GET: Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            borrow borrow = acc.borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // GET: borrows/Create
        public ActionResult Create()
        {
            ViewBag.bookId = new SelectList(acc.books, "bookId", "name");
            ViewBag.studentId = new SelectList(acc.students, "studentId", "name");
            return View();
        }

        // POST: Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "borrowId,studentId,bookId,takenDate,broughtDate")] borrow borrow)
        {
            if (ModelState.IsValid)
            {
                acc.borrows.Add(borrow);
                acc.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.bookId = new SelectList(acc.books, "bookId", "name", borrow.bookId);
            ViewBag.studentId = new SelectList(acc.students, "studentId", "name", borrow.studentId);
            return View(borrow);
        }

        // GET: Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            borrow borrow = acc.borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            ViewBag.bookId = new SelectList(acc.books, "bookId", "name", borrow.bookId);
            ViewBag.studentId = new SelectList(acc.students, "studentId", "name", borrow.studentId);
            return View(borrow);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "borrowId,studentId,bookId,takenDate,broughtDate")] borrow borrow)
        {
            if (ModelState.IsValid)
            {
                acc.Entry(borrow).State = EntityState.Modified;
                acc.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bookId = new SelectList(acc.books, "bookId", "name", borrow.bookId);
            ViewBag.studentId = new SelectList(acc.students, "studentId", "name", borrow.studentId);
            return View(borrow);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            borrow borrow = acc.borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // POST: Delete borrow
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            borrow borrow = acc.borrows.Find(id);
            acc.borrows.Remove(borrow);
            acc.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                acc.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
