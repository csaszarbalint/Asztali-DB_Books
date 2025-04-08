using Books.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Books.Access
{
    public class DbAuthor : IAccessor<Author>
    {
        DbConnection _connection;
        public DbAuthor(DbConnection connection)
        {
            _connection = connection;
        }

        public bool Create(Author author)
        {
            var isSuccessful = false;
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO author VALUES ({author.Id},'{author.Name}');";
                var rowCount = cmd.ExecuteNonQuery();

                isSuccessful = rowCount == 1;
            }

            return isSuccessful;
        }

        public List<Author> Read()
        {
            var result = new List<Author>();
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM author ORDER BY id;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var author = new Author
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        result.Add(author);
                    }
                }
            }
            return result;
        }
    }
}
