using Books.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Access
{
    public class DbBook : IAccessor<Book>
    {
        DbConnection _connection;
        public DbBook(DbConnection connection)
        {
            _connection = connection;
        }

        public bool Create(Book book)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO book VALUES ({book.Id}, '{book.Title}', {book.AuthorId}, {book.Year}, {book.Pages});"; 

                var rowCount = cmd.ExecuteNonQuery();

                return rowCount == 1;
            }    
        }

        public List<Book> Read()
        {
            var result = new List<Book>();

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM book ORDER BY id;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var book = new Book 
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            AuthorId = reader.GetInt32(2),
                            Year = reader.GetInt32(3),
                            Pages = reader.GetInt32(4)
                        };
                        result.Add(book);
                    }
                }
            }

            return result;
        }


    }
}
