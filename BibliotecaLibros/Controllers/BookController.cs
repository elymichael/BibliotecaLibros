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

    public class BookController : ApiController
    {

        string booksPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/books/");

        public List<Book> GetAll()
        {
            List<Book> data = new List<Book>();

            string[] files = Directory.GetFiles(booksPath, "*.xml", SearchOption.TopDirectoryOnly);
            foreach(string filename in files)
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(filename);
                data.Add(new Book()
                {
                    Title = doc.DocumentElement.SelectSingleNode(".//title").InnerText,
                    Author = doc.DocumentElement.SelectSingleNode(".//author").InnerText,
                    Pages = int.Parse(doc.DocumentElement.SelectSingleNode(".//pages").InnerText)
                });
            }

            return data;
        }

        public IHttpActionResult Get(int id)
        {
            return Ok();
        }

        public IHttpActionResult Get(int id, string type)
        {
            return Ok();
        }
    }
}
