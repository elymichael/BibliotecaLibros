using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub; // External library to open epub files.
using HtmlAgilityPack; // Nutget to read html content and convert to txt.

namespace CreateBookStorefromEpub
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            
            string epubDirectory = string.Format(@"{0}\{1}", Directory.GetParent(directory).Parent.Parent.FullName, "epubs");
            string targetDirectory = string.Format(@"{0}\{1}", Directory.GetParent(directory).Parent.Parent.FullName, "books");

            if (Directory.Exists(epubDirectory))
            {
                Console.WriteLine("Directorio existe");
                string[] bookPaths = Directory.GetFiles(epubDirectory, "*.epub");
                if (bookPaths.Length > 0)
                {
                    List<Book> books = new List<Book>();

                    Console.WriteLine("Existen Libros en el directorio");
                    foreach (string str in bookPaths)
                    {
                        EpubBook book = EpubReader.ReadBook(str);
                        Book readBook = new Book()
                        {
                            Author = book.Author,
                            Title = book.Title,
                            Files = new List<Pages>()
                        };

                        foreach (KeyValuePair<string, EpubTextContentFile> file in book.Content.Html)
                        {
                            addFile(file, readBook);
                        }

                        books.Add(readBook);
                    }

                    for (int i = 0; i < books.Count; i++)
                    {
                        string name = (i + 1).ToString();
                        if (!Directory.Exists(targetDirectory))
                        {
                            Directory.CreateDirectory(targetDirectory);
                        }
                        string filename = string.Format(@"{0}.xml", name);
                        string subfolder = string.Format(@"{0}{1}\", targetDirectory ,name);

                        using (StreamWriter sw = File.CreateText(targetDirectory + filename))
                        {
                            sw.WriteLine("<book>");
                            sw.WriteLine("<title>" + books[i].Title + "</title>");
                            sw.WriteLine("<author>" + books[i].Author + "</author>");
                            sw.WriteLine("<pages>" + (books[i].Files.Count + 1).ToString() + "</pages>");
                            sw.Write("</book>");
                        };

                        Directory.CreateDirectory(subfolder);

                        for (int j = 0; j < books[i].Files.Count; j++)
                        {
                            string pagename = (j + 1).ToString();
                            using (StreamWriter sw = File.CreateText(string.Format("{0}{1}.txt", subfolder, pagename)))
                            {
                                sw.Write(books[i].Files[j].textcontent);
                            };

                            using (StreamWriter sw = File.CreateText(string.Format("{0}{1}.html", subfolder, pagename)))
                            {
                                sw.Write(books[i].Files[j].htmlcontent);
                            };
                        }
                    }
                }
            }
            Console.WriteLine("Proceso Finalizado");
            Console.ReadKey();
        }

        /// <summary>
        /// Add files to temporal book list.
        /// </summary>
        /// <param name="file">Dictionary value with the key and file content.</param>
        /// <param name="book">Book object.</param>
        static void addFile(KeyValuePair<string, EpubTextContentFile> file, Book book)
        {
            if (!file.Key.ToLower().Contains("cover"))
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(file.Value.Content);

                HtmlNodeCollection nodes = 
                    htmlDocument.DocumentNode.SelectSingleNode(".//body").SelectNodes(".//text()");
                
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < nodes.Count; i++)
                {
                    sb.AppendLine(nodes[i].InnerText.Trim());

                    if (i % 21 == 0 && i > 0)
                    {
                        book.Files.Add(GetPages(sb.ToString(), book.Title));
                        sb.Clear();
                    }
                }

                if (sb.Length > 0)
                {
                    book.Files.Add(GetPages(sb.ToString(), book.Title));
                    sb.Clear();
                }
            }
        }

        /// <summary>
        /// Get Page information from epub html content information.
        /// </summary>
        /// <param name="content">Content of the file.</param>
        /// <param name="title">Title of the book.</param>
        /// <returns>return new page information.</returns>
        static Pages GetPages(string content, string title)
        {
            Pages page = new Pages();
            page.htmlcontent = GetHtmlContent(content, title);
            page.textcontent = content;

            return page;
        }

        /// <summary>
        /// Get new HTML file from epub content.
        /// </summary>
        /// <param name="content">Content of the file.</param>
        /// <param name="title">Title of the book.</param>
        /// <returns>return new html string.</returns>
        static string GetHtmlContent(string content, string title)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head><title>" + title + "</title></head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div>");
            sb.AppendLine("<p>");
            sb.AppendLine(content.Replace("\n", "<br/>"));
            sb.AppendLine("</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }

    /// <summary>
    /// This class store temporal book information.
    /// </summary>
    class Book
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public List<Pages> Files { get; set; }
    }

    /// <summary>
    /// This class is used to split the epub html content in two kind of files format (html and txt)
    /// </summary>
    class Pages
    {
        public string textcontent { get; set; }
        public string htmlcontent { get; set; }
    }
}
