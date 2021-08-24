using JeSch.Reader;
using System;

namespace KassenbuchInConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\jenss\\OneDrive\\Kassenbuch\\Kassenbuch.csv";

            CsvReader csvReader = new CsvReader(filePath);

            Console.WriteLine("Kassenbuch");

            var lines = csvReader.LoadCsvAsList();

            foreach (var line in lines)
            {
                foreach (var content in line)
                {
                    Console.Write(content + " ");
                }
                Console.WriteLine();
            }


            Console.Write("Datum: ");
            var date = Console.ReadLine();
            Console.Write("Kategorie: ");
            var category = Console.ReadLine();
            Console.Write("Beschreibung: ");
            var description = Console.ReadLine();
            Console.Write("Quelle: ");
            var origin = Console.ReadLine();
            Console.Write("Betrag: ");
            var amount = Console.ReadLine();

            lines.Add(new string[] { date, category, description, origin, amount });

            if (csvReader.SaveCsvFromList(lines))
                Console.WriteLine("Eintrag wurde erfolgreich hinzugefügt.");
            else
                Console.WriteLine("Fehler beim Erstellen des Eintrags.");


            lines = csvReader.LoadCsvAsList();

            foreach (var line in lines)
            {
                foreach (var content in line)
                {
                    Console.Write(content + " ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
