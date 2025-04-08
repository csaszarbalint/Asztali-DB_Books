using Books.Access;
using Books.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Books
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var conn = new MySqlConnection("server=localhost:3306,;uid=root;database=books"))
            {
                conn.Open();

                //Reset
                { 
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM `book` WHERE id = 58; DELETE FROM author WHERE id = 27;";
                        cmd.ExecuteNonQuery();
                    }
                }

                //---------------[dbAuthor]---------------
                var dbAuthor = new DbAuthor(conn);

                var newAuthor = new Author
                {
                    Id = 27,
                    Name = "James Clavell"
                };

                dbAuthor.Create(newAuthor);

                var authorList = dbAuthor.Read();
                Console.WriteLine(String.Join("\n", authorList.Select(author => $"{author.Id}, {author.Name}").ToList()));
                //---------

                Console.WriteLine();

                //---------------[dbBook]---------------
                var dbBook = new DbBook(conn);

                Book newBook = new Book()
                {
                    Id = 58,
                    Title = "asdf",
                    AuthorId = 27,
                    Author = newAuthor,
                    Year = 2010,
                    Pages = 300
                };

                dbBook.Create(newBook);

                var bookList = dbBook.Read();
                Console.WriteLine(String.Join("\n", bookList.Select(book => $"{book.Id}, {book.Title}").ToList()));
                //---------

                return;

            }
        }
    }
}
