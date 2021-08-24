using JeSch.Reader;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassenbuchConverter
{
    public class Program
    {
        private static readonly string directoryPath = "C:\\Users\\jenss\\OneDrive\\Kassenbuch\\";
        static void Main(string[] args)
        {
            //CsvReader csvReader = new CsvReader(directoryPath + "Neues-Kassenbuch.csv");
            //var oldEntries = csvReader.LoadCsvAsArrayInList();
            //var entries = new List<string[]>();
            //int entryNumber = 1;
            //string date, category, source, sign, amount;
            //var newEntries = new List<string[]>();

            //#region Zwei Zeilen
            //// Zwei einträge in einer Zeile zu zwei Zeilen

            //foreach (var entry in oldEntries)
            //{
            //    if (!string.IsNullOrWhiteSpace(entry[6]) && !string.IsNullOrWhiteSpace(entry[7]))
            //    {
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], entry[6], "", "", "" });
            //        entryNumber++;
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", entry[7], "", "" });
            //    }
            //    else if (!string.IsNullOrWhiteSpace(entry[6]) && !string.IsNullOrWhiteSpace(entry[8]))
            //    {
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], entry[6], "", "", "" });
            //        entryNumber++;
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", "", entry[8], "" });
            //    }
            //    else if (!string.IsNullOrWhiteSpace(entry[6]) && !string.IsNullOrWhiteSpace(entry[9]))
            //    {
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], entry[6], "", "", "" });
            //        entryNumber++;
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", "", "", entry[9] });
            //    }
            //    else if (!string.IsNullOrWhiteSpace(entry[7]) && !string.IsNullOrWhiteSpace(entry[8]))
            //    {
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", entry[7], "", "" });
            //        entryNumber++;
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", "", entry[8], "" });
            //    }
            //    else if (!string.IsNullOrWhiteSpace(entry[7]) && !string.IsNullOrWhiteSpace(entry[9]))
            //    {
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", entry[7], "", "" });
            //        entryNumber++;
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", "", "", entry[9] });
            //    }
            //    else if (!string.IsNullOrWhiteSpace(entry[8]) && !string.IsNullOrWhiteSpace(entry[9]))
            //    {
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", "", entry[8], "" });
            //        entryNumber++;
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], "", "", "", entry[9] });
            //    }
            //    else
            //    {
            //        entries.Add(new string[] { entryNumber.ToString(), entry[1], entry[2], entry[3], entry[4], entry[5], entry[6], entry[7], entry[8], entry[9] });
            //    }
            //    entryNumber++;
            //}

            ////csvReader.SaveCsvFromList(entries);
            //#endregion

            //#region Sign und Source
            //// für sign und source
            //foreach (var entry in entries)
            //{
            //    date = entry[1].Insert(6, "20").Replace('/', '.');

            //    if (entry[3].Contains("Amazon"))
            //        category = "Amazon";
            //    else if (entry[3].Contains("Einkaufen"))
            //        category = "Einkaufen";
            //    else if (entry[3].Contains("Ebay"))
            //        category = "Ebay";
            //    else if (entry[3].Contains("Essen"))
            //        category = "Essen";
            //    else if (entry[3].Contains("Sparen"))
            //        category = "Sparen";
            //    else if (entry[3].Contains("Steam"))
            //        category = "Steam";
            //    else if (entry[3].Contains("Gehalt"))
            //        category = "Job";
            //    else if (entry[3].Contains("Spotify"))
            //        category = "Spotify";
            //    else if (entry[3].Contains("Friseur") || entry[3].Contains("Frisör"))
            //        category = "Friseur";
            //    else
            //        category = "-";

            //    if (string.IsNullOrWhiteSpace(entry[4]))
            //    {
            //        if (!string.IsNullOrWhiteSpace(entry[6]))
            //        {
            //            source = "Bargeld";
            //            amount = entry[6];
            //        }
            //        else if (!string.IsNullOrWhiteSpace(entry[7]))
            //        {
            //            source = "Girokonto";
            //            amount = entry[7];
            //        }
            //        else if (!string.IsNullOrWhiteSpace(entry[8]))
            //        {
            //            source = "Sparbuch";
            //            amount = entry[8];
            //        }
            //        else
            //        {
            //            source = "Spardose";
            //            amount = entry[9];
            //        }
            //    }
            //    else
            //    {
            //        source = entry[4];
            //        amount = entry[6];
            //    }

            //    if (amount.Contains("-"))
            //        sign = "Minus";
            //    else
            //        sign = "Plus";

            //    amount = amount.Replace("-", "");

            //    newEntries.Add(new string[] { entry[0], date, category, entry[3], source, sign, amount });
            //}
            //#endregion

            //csvReader.SaveCsvFromList(newEntries);

            string connectionString = "Data Source=C:\\Users\\jenss\\OneDrive\\Kassenbuch\\CashBook.sqlite";
            List<string[]> entries = new List<string[]>();

            SqliteCommand command = new SqliteCommand
            {
                Connection = new SqliteConnection(connectionString),
                CommandText = $"SELECT * FROM CashBook"
            };
            command.Connection.Open();
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                entries.Add(new string[] { reader["id"].ToString(), reader["date"].ToString(), reader["category"].ToString(), reader["description"].ToString().Replace("\'",""), reader["source"].ToString(), reader["sign"].ToString(), reader["amount"].ToString().Replace('.', ',') });
            }
            reader.Close();

            foreach (var entry in entries)
            {
                var amount = entry[6];
                if (!amount.Contains(","))
                    amount += ",00";
                if (amount.Split(',').Last().Length == 0)
                    amount += "00";
                if (amount.Split(',').Last().Length == 1)
                    amount += "0";
                if (amount.Split(',').First().Length < 1)
                    amount = "0" + amount;
                entry[6] = amount;
                
                command.CommandText = $"UPDATE CashBook SET date = '{entry[1]}', category = '{entry[2]}', description = '{entry[3]}', source = '{entry[4]}', sign = '{entry[5]}', amount = '{entry[6]}' WHERE id = {entry[0]}";
                command.ExecuteNonQuery();
            }
            command.Dispose();
        }
    }
}
