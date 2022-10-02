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
    public class studentsController : Controller
    {
        private LibraryEntities acc = new LibraryEntities();
        private string status;

        // GET: students
        public ActionResult Index(int ?id)
        {
            var items = acc.students.ToList();
            var book = acc.books.FirstOrDefault(x => x.bookId == id);
            var studentList = new List<StudentList>();
            int bookid = Convert.ToInt32(Session["temp"]);

            foreach (var item in acc.students.ToList())
            {
                StudentList student = new StudentList();
                student.Student = item;
                List<borrow> borrow = acc.borrows.Where(x => x.studentId == item.studentId && x.bookId == bookid).ToList();
                if (borrow.Count >0)
                {
                    List<borrow> enumerable = borrow.Where(x => x.broughtDate > DateTime.Now).ToList();
                    if (enumerable.Count > 0)
                    {
                        student.status = "*";
                        ViewBag.booked = student.Student.studentId;
                    }
                    else
                    {
                        student.status = "-";

                    }

                }
                else
                {
                    student.status = "-";
                }

                studentList.Add(student);
            }

            return View(studentList);
        }

        // GET: Details
        public ActionResult ReturnBook(int ?id)
        {
            borrow borrow = new borrow();
            int bookid = Convert.ToInt32(Session["temp"]);
            DateTime dateTime = new DateTime();
            dateTime = DateTime.Now;

            acc.borrows.Where(x => x.studentId == id && x.bookId == bookid).ToList().ForEach(c => { c.takenDate = dateTime; c.broughtDate = dateTime; });

            acc.SaveChanges();


            return RedirectToAction("Index","books");

        }
        //Borrow book functionality and parameters involved
        public ActionResult BorrowBook(int? id)
        {
            acc.borrows.FirstOrDefault(x => x.studentId == id).broughtDate = DateTime.Now.AddDays(20);

            borrow borrow = new borrow();
            int bookid = Convert.ToInt32(Session["temp"]);
            borrow.book = acc.books.FirstOrDefault(x=>x.bookId== bookid);
            borrow.student = acc.students.FirstOrDefault(x => x.studentId == id);
            borrow.takenDate = DateTime.Now;
            borrow.broughtDate = DateTime.Now.AddDays(20);

            acc.borrows.Add(borrow);
            acc.SaveChanges();


            return RedirectToAction("Index","books");
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = acc.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: students/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] student student)
        {
            if (ModelState.IsValid)
            {
                acc.students.Add(student);
                acc.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = acc.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] student student)
        {
            if (ModelState.IsValid)
            {
                acc.Entry(student).State = EntityState.Modified;
                acc.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        //GET: Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = acc.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            student student = acc.students.Find(id);
            acc.students.Remove(student);
            acc.SaveChanges();
            return RedirectToAction("Index");
        }

        //dispose db
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
