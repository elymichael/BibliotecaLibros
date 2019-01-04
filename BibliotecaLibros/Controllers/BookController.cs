namespace BibliotecaLibros.Controllers
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using BibliotecaLibros.Models;

    /// <summary>
    /// Book Controller: this controller is used to interact with the books and the pages of the book.
    /// </summary>
    public class BookController : ApiController
    {
        /// <summary>
        /// Default book store.
        /// </summary>
        string booksPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/books/");

        /// <summary>
        /// Get List of books. [http://localhost:6205/api/book]
        /// </summary>
        /// <returns></returns>
        public List<Book> GetAll()
        {
            List<Book> data = new List<Book>();

            string[] files = Directory.GetFiles(booksPath, "*.xml", SearchOption.TopDirectoryOnly);
            foreach(string filename in files)
            {
                FileInfo fi = new FileInfo(filename);
                
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(filename);

                data.Add(new Book()
                {
                    ID = int.Parse(Path.GetFileNameWithoutExtension(filename)),
                    CreationDate = fi.CreationTime,
                    Title = doc.DocumentElement.SelectSingleNode(".//title").InnerText,
                    Author = doc.DocumentElement.SelectSingleNode(".//author").InnerText,
                    Pages = int.Parse(doc.DocumentElement.SelectSingleNode(".//pages").InnerText)
                });
            }

            return data;
        }

        /// <summary>
        /// Get an specific book using [http://localhost:6205/api/book/1]
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns>return book structure</returns>
        public IHttpActionResult Get(int id)
        {
            string filename = string.Format("{0}\\{1}.{2}", booksPath,id.ToString(),"xml");

            if (!File.Exists(filename))
            {
                return BadRequest(string.Format("Book not found [{0}]", id.ToString()));
            }

            FileInfo fi = new FileInfo(filename);

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(filename);
            
            Book data = new Book()
            {
                ID = int.Parse(Path.GetFileNameWithoutExtension(filename)),
                CreationDate = fi.CreationTime,
                Title = doc.DocumentElement.SelectSingleNode(".//title").InnerText,
                Author = doc.DocumentElement.SelectSingleNode(".//author").InnerText,
                Pages = int.Parse(doc.DocumentElement.SelectSingleNode(".//pages").InnerText)
            };
            return Ok(data);
        }

        /// <summary>
        /// Get an specific page inside the book (html or txt), the two supported format.
        /// [http://localhost:6205/api/book/1/page/11/html]
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <param name="page">page name (it's used by requirements)</param>
        /// <param name="number">Number</param>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public IHttpActionResult Get(int id, string page, int number, string type)
        {
            string bookname = string.Format("{0}\\{1}.{2}", booksPath, id.ToString(), "xml");
            string pagename = string.Format("{0}\\{1}\\{2}.{3}", booksPath, id.ToString(), number.ToString(), type);

            if (!File.Exists(bookname))
            {
                return BadRequest(string.Format("Book not found [{0}]", id.ToString()));
            }

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(bookname);
            int pages = int.Parse(doc.DocumentElement.SelectSingleNode(".//pages").InnerText);

            if (number > pages)
            {
                return BadRequest(string.Format("The page number cannot be greater than total pages [{0} against {1}]", number.ToString(), pages.ToString()));
            }

            if (!File.Exists(pagename))
            {
                return BadRequest(string.Format("Page not found [{0}.{1}]", number.ToString(), type));
            }
            
            return Ok(File.ReadAllText(pagename));
        }
    }
}
