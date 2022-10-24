using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace consoleDatabase
{
    class Program
    {
        static void kiirat(string snipet, int col)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "pizza";

            MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = snipet;
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string val = "";
                        for (int i = 0; i < col; i++)
                        {
                            val += dr.GetValue(i) + "\t";
                        }
                        Console.WriteLine(val);
                    }
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("--23: Hány házhoz szállítása volt az egyes futároknak?--");
            kiirat("SELECT fnev, COUNT(fazon) FROM futar JOIN rendeles USING(fazon) GROUP BY fnev;", 2);
            Console.WriteLine("\n--24: A fogyasztás alapján mi a pizzák népszerűségi sorrendje?--");
            kiirat("SELECT pnev FROM tetel JOIN pizza USING(pazon) GROUP BY pnev ORDER BY sum(db) DESC;", 1);
            Console.WriteLine("\n--25: A rendelések összértéke alapján mi a vevők sorrendje?--");
            kiirat("SELECT vnev FROM vevo JOIN rendeles USING(vazon) JOIN tetel USING(razon) JOIN pizza USING(pazon) GROUP BY vnev ORDER BY sum(db) DESC;", 1);
            Console.WriteLine("\n--26: Melyik a legdrágább pizza?--");
            kiirat("SELECT pnev FROM pizza ORDER BY par DESC LIMIT 1;", 1);
            Console.WriteLine("\n--27: Ki szállította házhoz a legtöbb pizzát?--");
            kiirat("SELECT fnev FROM futar JOIN rendeles USING(fazon) GROUP BY fnev ORDER BY COUNT(fazon) DESC LIMIT 1;", 1);
            Console.WriteLine("\n--28: Ki ette a legtöbb pizzát?--");
            kiirat("SELECT vnev FROM vevo JOIN rendeles USING(vazon) JOIN tetel USING(razon) GROUP BY vnev ORDER BY SUM(db) DESC LIMIT 1;", 1);
            Console.ReadKey();
        }
    }
}
