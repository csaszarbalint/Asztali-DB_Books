using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Books.Access
{
    public class Column
    {
        public string Name { get; set; }
        public PropertyInfo Property { get; set; }
    }
    public abstract class DbAccess<T> 
        where T : class
    {
        protected DbConnection _connection;
        private string TableName { get; set; }
        private Column Key { get; set; }
        private List<Column> Columns { get; set; } = new List<Column>();

        public DbAccess(DbConnection connection)
        {
            _connection = connection;

            if (_connection == null) throw new ArgumentNullException(nameof(connection), "Connection cannot be null");

            this.TableName = MapTableName();
            this.Key = MapTableKey();
            this.Columns = MapTableColumns();
        }

        public abstract bool Create(T obj);
        public virtual IEnumerable<T> Read() 
        {
            var result = new List<T>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText =
                  $"SELECT {Key.Name}, {string.Join(",", Columns.Select(c => c.Name))} FROM {TableName}";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T obj = (T)Activator.CreateInstance(typeof(T));
                        var ix = 0;
                        // Minden korábban kigyűjtött oszlop értékét beállítjuk
                        // a megfelelő tulajdonságban. Figyeljük meg, hogy az
                        // oszlopok sorrendje a lekérdezésben is és itt is ugyanaz,
                        // a ColumnMap Dictionary kulcsainak a sorrendje.
                        foreach (var column in Columns.Select(c => c.Property))
                        {
                            column.Value.SetValue(obj, reader[ix++]);
                        }
                        // Végül szintén a megszokott módon hozzáadjuk az
                        // objektumot a result listához.
                        result.Add(obj);
                    }
                }
            }
            return result;
        }
        public abstract T Read(int id);
        public abstract T Update(int id, T obj);
        public abstract T Delete(int id);

        private string MapTableName() 
        {
            var type = typeof(T);
            var tableAttribute = (TableAttribute)type
              .GetCustomAttributes(false)
                .FirstOrDefault(
                a => a.GetType() == typeof(TableAttribute));
            return tableAttribute != null ?
                tableAttribute.Name :
                type.Name.ToLower(); 
        }
        private Column MapTableKey() 
        {
            var type = typeof(T);
            var propInfos = type.GetProperties();

            foreach (var propInfo in propInfos)
            {
                var keyAttribute = (KeyAttribute)propInfo
                .GetCustomAttributes(false)
                .FirstOrDefault(a => a.GetType() == typeof(KeyAttribute));
                if (keyAttribute != null) return new Column 
                { 
                    Name= propInfo.Name.ToLower(),
                    Property = propInfo
                }; 
            }
            
            //no key attribute found => use first property
            return new Column 
            { 
                    Name= propInfos[0].Name.ToLower(),
                    Property = propInfos[0] 
            }; 
        }
        private List<Column> MapTableColumns() 
        {
            var type = typeof(T);
            var propInfos = type.GetProperties();
            
            return propInfos
                .Select(propInfo =>
                {
                    var columnAttribute = (ColumnAttribute)propInfo
                    .GetCustomAttributes(false)
                    .FirstOrDefault(a => a.GetType() == typeof(ColumnAttribute));
                    var columnName = columnAttribute != null ?
                      columnAttribute.Name :
                      $"{propInfo.Name[0].ToString().ToLower()}{propInfo.Name.Substring(1)}";
                        //can't use ToLower() on the whole string (example AuthroId => autorid; column name authorId)
                    return new Column
                    {
                        Name = columnName,
                        Property = propInfo
                    };
                })
                .ToList();
        }
    }
}
