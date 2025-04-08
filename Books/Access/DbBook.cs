﻿using Books.Model;
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

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM book ORDER BY id;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Létrehozzuk az új Author példányt és
                        var book = new Book 
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
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
