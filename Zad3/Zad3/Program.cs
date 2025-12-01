using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using static program;
internal class program
{
    public class User
    {
        public int LP { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public int data { get; set; }
    };

    static void Main(string[] args)
    {
        Random rand = new Random();
        string[] imiona = { "Kasia", "Basia", "Zosia", "Ania" };
        string[] nazwiska = { "Kowalska", "Nowak" };
        var users = new List<User>();

        for (int i = 0; i < 100; i++)
        {
            string imie = imiona[rand.Next(imiona.Length)];
            string nazwisko = nazwiska[rand.Next(nazwiska.Length)];
            int rok = rand.Next(1990, 2001);
            users.Add(new User()
            {
                LP = i + 1,
                imie = imie,
                nazwisko = nazwisko,
                data = rok
            });
        }
        string path = "C:\\test\\Uzytkownicy.csv";
        using (var streamWriter = File.CreateText(path))
        { 
            var writer = new CsvWriter(streamWriter, new CultureInfo("pl-PL"));
            writer.WriteRecords(users);
        }
    }
}



