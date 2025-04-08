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
            // Adatbázis kapcsolat létrehozása
            // A MySql.Data csomag telepítése szükséges a MySQL Connector/NET
            // használatához. A konstruktorban megadott connection string tartalmazza
            // a szerver címét, felhasználónevet és adatbázis nevét.

            using (var conn = new MySqlConnection("server=localhost:3307,;uid=root;database=books"))
            {
                // A kapcsolatot használat előtt meg kell nyitni!
                conn.Open();

                // A két DbAccess osztályt példányosítjuk, így ezeket többször is használhatjuk.
                // A konstruktor paramétereként átadjuk a kapcsolatot.
                var dbAuthor = new DbAuthor(conn);

                // Létrehozunk egy új könyvet, és a Create metódus segítségével
                // elmentjük az adatbázisba.
                var newAuthor = new Author
                {
                    Id = 27,
                    Name = "James Clavell"
                };
                
                //----------------------------------------

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

                // Ha a Create metódus visszatérési értéke false, akkor kiírunk egy hibaüzenetet.
                if (!dbBook.Create(newBook))
                {
                    Console.WriteLine("Hiba történt az író beszúrásakor!");
                }

                return;

                // Ha a Create metódus visszatérési értéke false, akkor kiírunk egy hibaüzenetet.
                if (!dbAuthor.Create(newAuthor))
                {
                    Console.WriteLine("Hiba történt az író beszúrásakor!");
                }

                // A Read metódus segítségével lekérdezzük az összes írót az adatbázisból.
                // A lekérdezés eredményét egy Author típusú listába mentjük.
                var authorList = dbAuthor.Read();
                // A foreach ciklus segítségével végigiterálunk a listán, és kiírjuk az írók nevét és azonosítóját.
                foreach (var author in authorList)
                {
                    Console.WriteLine($"{author.Id}: {author.Name}");
                }
            }
        }
    }
}
