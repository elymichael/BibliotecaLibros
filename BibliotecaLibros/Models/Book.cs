namespace BibliotecaLibros.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public DateTime CreationDate { get; set; }
    }
}