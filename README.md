# BibliotecaLibros
Proyecto de prueba solicitado por una empresa. 

**Company X** request a new library online. For that, they need a new **REST API** to allow to the customer get the list of books available, and read the books. 

## Original Requirement: 

> "Company X acaba de contratarlo a ústed para crear su biblioteca en línea. La misma le instruyó construir un REST API el cuál permita sus clientes consumir el listado de libros disponibles, así como también, leer dichos libros página por página en el formato deseado. Para esta primera iteración los libros estarán disponibles (página por página) en **texto plano** y **HTML**. En próximas iteraciones se agregará soporte para más formatos de lectura, y además, se agregará soporte para interconectarse con otros servicios proveedores de libros en línea." 
```
<ArrayOfBook xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/BibliotecaLibros.Models">
<Book>
<Author>Arthur Conan Doyle</Author>
<CreationDate>2019-01-04T14:10:39.6958156-04:00</CreationDate>
<ID>1</ID>
<Pages>268</Pages>
<Title>The Adventures of Sherlock Holmes</Title>
</Book>
<Book>
<Author>Fyodor Mikhailovich Dostoyevsky</Author>
<CreationDate>2019-01-04T14:10:39.731795-04:00</CreationDate>
<ID>2</ID>
<Pages>608</Pages>
<Title>The Brothers Karamazov</Title>
</Book>
</ArrayOfBook>
```
* ### Get book list (example) 
http://localhost:6205/api/book/ 
* ### Get a specific book 
http://localhost:6205/api/book/1 
* ### Navigate throught the pages using the same API 
http://localhost:6205/api/book/1/pages/11/html|txt 

**html version** 
http://localhost:6205/api/book/1/pages/11/html 

**txt version** 
http://localhost:6205/api/book/1/pages/11/txt

Developed using Visual Studio 2015, framework 4.5.2 under windows 10 pro. 
## Project: BibliotecaLibros
.Net Web Api Source: C# This Web API 2 library provides the book information required for company X. The project has configured the publish information pointed to the following directory: **C:\BibliotecaLibros\** and install under valid web server to support .net framework.
 
* **Main Class:** BookControler.cs located in the folder Controller. 
* **Model Class:** Book.cs.
* **File Database:** App_Data/books/ 
## Project: CreateBookStorefromEpub
.Net Console Application C# Description: This library take some **epub files** and create the library genareting the page files in custom **html** and **txt**. 
This application use the following references: 
* **VersOne.Epub** (Open source code to read epub) 
* **Nutget library HtmlAgilityPack** (Xml based component to read html and get txt well formated) 