using Books.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Access
{
    public class DbBook : DbAccess<Book>
    {
        public DbBook(DbConnection connection)
            : base(connection)
        {
        }

        public override bool Create(Book book)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO book VALUES ({book.Id}, '{book.Title}', {book.AuthorId}, {book.Year}, {book.Pages});"; 

                var rowCount = cmd.ExecuteNonQuery();

                return rowCount == 1;
            }    
        }

        public override Book Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Book> Read()
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

        public override Book Read(int id)
        {
            throw new NotImplementedException();
        }

        public override Book Update(int id, Book obj)
        {
            throw new NotImplementedException();
        }
    }
}
