using Books.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Access
{
    public class DbBook
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
                cmd.CommandText = $"INSERT INTO book VALUES ({book.Id}, \"{book.Title}\", {book.AuthorId}, \"{book.Year}\", \"{book.Pages}\");"; 

                // A parancs végrehajtása az ExecuteNonQuery metódussal, melynek
                // a visszatérési értéke a parancs által érintett sorok száma.
                var rowCount = cmd.ExecuteNonQuery();
                // Ha a parancs nem pontosan egy sort érintett, akkor hiba történhetett.
                return rowCount == 1;
            }    
        }

        public List<Book> Read()
        {
            var result = new List<Book>();

            return result;
        }


    }
}
