using Books.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Books.Access
{
    public class DbAuthor
    {
        // A DbAuthor már egy létező és megnyitott adatbázis kapcsolatot tároló
        // objektumot kap meg, ezt tárolja az alábbi privát mezőben.
        DbConnection _connection;
        // A connection mező értékét a publikus konstruktoron keresztül adjuk meg.
        public DbAuthor(DbConnection connection)
        {
            _connection = connection;
        }

        // A Create művelet adatbázisbeli megfelelője az INSERT.
        // Bemeneti paramétere egy objektum, amit beszúr az adatbázis táblába.
        public bool Create(Author author)
        {
            // A sikeres beszúrást jelző változó, a metódus visszatérési értéke.
            var isSuccessful = false;
            // A using kulcsszó segítségével a létrehozott DbCommand objektum egy
            // "eldobható" objektum, azaz a kódblokk elhagyásakor automatikusan
            // megsemmisül, így nem kell nekünk törölnünk és takarítanunk utána.
            using (var cmd = _connection.CreateCommand())
            {
                // A parancs végrehajtásához szükséges az SQL parancs, a lenti esetben
                // beszúrás az author táblába.
                // Figyeljünk arra, hogy a Name szöveges érték, ezért idézőjelek kellenek!
                cmd.CommandText = $"INSERT INTO author VALUES ({author.Id},'{author.Name}');";
                // A parancs végrehajtása az ExecuteNonQuery metódussal, melynek
                // a visszatérési értéke a parancs által érintett sorok száma.
                var rowCount = cmd.ExecuteNonQuery();
                // Ha a parancs nem pontosan egy sort érintett, akkor hiba történhetett.
                isSuccessful = rowCount == 1;
            }

            return isSuccessful;
        }

        // A második művelet a beolvasás, melynek első változata feltétel nélkül,
        // a tábla minden sorát beolvassa, így az eredmény egy lista lesz.
        public List<Author> Read()
        {
            // A metódus eredménye az Author osztály példányainak listája.
            var result = new List<Author>();
            // Most is egy "eldobható" parancs objektumot létrehozunk.
            using (var cmd = _connection.CreateCommand())
            {
                // Az írókat azonosító szerint növekvő sorrendben lekérdező SQL.
                cmd.CommandText = "SELECT * FROM author ORDER BY id;";
                // A lekérdezés eredményét egy olvasó objektumon (DbDataReader)
                // keresztül érhetjük el. Ez az ExecuteReader metódus visszatérési
                // értéke. A reader objektum is "eldobható", ezért érdemes itt is
                // a using kulcsszót alkalmazni.
                using (var reader = cmd.ExecuteReader())
                {
                    // A Read metódus mindaddig igaz értéket ad vissza, amíg van
                    // van még beolvasható eredmény, ezért tökéletesen megfelel
                    // a while ciklus feltételeként.
                    while (reader.Read())
                    {
                        // Létrehozzuk az új Author példányt és
                        var author = new Author
                        {
                            // az objektum inicializálás segítségével megadjuk
                            // a jellemzők, tulajdonságok adatait.
                            // A reader Get metódusai sokféle adattípust támogatnak,
                            // mint pl. int, string, float, DataTime.
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        // Az új Author példányt az eredménylistához adjuk.
                        result.Add(author);
                    }
                }
            }
            return result;
        }
    }
}
