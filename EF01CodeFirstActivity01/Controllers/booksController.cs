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
    public class booksController : Controller
    {
        private LibraryEntities acc = new LibraryEntities();


        // GET: books
        public ActionResult Index()
        {
            BooksViewModel books1 = new BooksViewModel();

            books1.authors = new List<author>();
            books1.authors.AddRange(acc.authors);

            books1.books = new List<book>();
            books1.books.AddRange(acc.books);

            books1.types = new List<type>();
            books1.types.AddRange(acc.types);

            return View(books1);
        }

        // GET: books Details
        public ActionResult Details(int? id)
        {

            Session["temp"] = id;
            book book = new book();
            book = acc.books.FirstOrDefault(x => x.bookId == id);
            BooksDetailsViewModel booksDetailsViewModel = new BooksDetailsViewModel();

            booksDetailsViewModel.Borrows = acc.borrows.Where(x => x.bookId == id).ToList();
            booksDetailsViewModel.numborrows = booksDetailsViewModel.Borrows.Count();
            booksDetailsViewModel.name = acc.books.FirstOrDefault(x => x.bookId == id).name;
            booksDetailsViewModel.id = (int)id;

           if (book.borrows.FirstOrDefault(x => x.broughtDate > DateTime.Now) != null)
           {
                        booksDetailsViewModel.status = "Out";
           }
           else
           {
                        booksDetailsViewModel.status = "Available";

           }
            return View(booksDetailsViewModel);

        }

         public ActionResult Search(int? types, int ? author, string search)
        {

            BooksViewModel books1 = new BooksViewModel();
            //create an object of authors
            books1.authors = new List<author>();
            books1.authors.AddRange(acc.authors);

            //create an object of books
            books1.books = new List<book>();
            books1.books.AddRange(acc.books);

            //create an object of type of books
            books1.types = new List<type>();
            books1.types.AddRange(acc.types);


            if(author!=null)
                books1.books = books1.books.Where(x => x.authorId == author).ToList();
            if(types!=null)
                books1.books = books1.books.Where(x => x.typeId == types).ToList();

            //searched book/author/type
            if(search!="")
            {
                books1.books = books1.books.Where(x => x.name.ToLower().Contains(search.ToLower())).ToList();
            }
            return View("Index", books1);
        }


    }
}
