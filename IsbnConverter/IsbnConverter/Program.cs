using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IsbnConverter
{
    public class Program
    {
        public static string isbnInput ="";
        public static string appPath = AppDomain.CurrentDomain.BaseDirectory;
        public static Dictionary<string, string> CSVData = LoadCSV(appPath + @"\gruppennummern978.csv");

        static void Main(string[] args)
        {
            bool again = false;
            bool correct = true;
            bool check = false;

            do{
                do{
                    UserInput();

                    if (isbnInput.Length >= 9 && isbnInput.Length <= 13)
                    {
                        check = true;
                    } else {
                        check = false;
                        Console.WriteLine("\nDie eigegebene Isbn nummer muss zwischen 9 und 13 zeichen sein!\n");
                    }

                } while (!check);

            Check(isbnInput);
            Convert(isbnInput);

            do
            {
                correct = true;
                Console.WriteLine("\nMöchten Sie eine weitere ISBN Nummer prüfen? J/N");
                string eingabe = Console.ReadLine().ToUpper();
                if (eingabe == "J")
                {
                    again = true;
                    Console.Clear();
                }
                else if (eingabe == "N")
                    again = false;
                else
                    correct = false;
            } while (!correct);
            }while(again);
        }

        private static string DeleteWhiteSpaces(string isbn)
        {
            return isbn.Replace(" ", "").Replace("-", "");
        }

        private static void UserInput()
        {
            bool correct = true;
            do
            {
                correct = true;
                Console.WriteLine("Wollen sie\n[1] eine ISBN Nummer konvertieren.\n[2] eine neue ISBN Nummer anlegen.");
                string eingabe = Console.ReadLine();
                if (eingabe == "1")
                {
                    bool again;
                    do
                    {
                        again = true;
                        Console.WriteLine("\nGeben sie eine ISBN Nummer ein.");
                        isbnInput = DeleteWhiteSpaces(Console.ReadLine());
                        if (isbnInput.Length == 10)
                        {
                            if (!IsDigitsOnly(isbnInput.Remove(9)))
                            {
                                again = false;
                                Console.WriteLine("\nDie ISBN Nummer muss aus Zahlen bestehen! (Außer Prüfziffer)");
                            }
                        }
                        else
                        {
                            if (!IsDigitsOnly(isbnInput))
                            {
                                again = false;
                                Console.WriteLine("\nDie ISBN Nummer muss aus Zahlen bestehen! (Außer Prüfziffer)");
                            }
                            again = true;
                        }
                    } while (!again);
                }
                else if (eingabe == "2")
                {
                    string ean = "";
                    
                    string value;
                    bool input = true;
                    bool digits = true;
                    bool inputCorrect = true;
                    do
                    {
                        do
                        {
                            Console.WriteLine("\nBitte geben Sie die Gruppennummer ein: ");
                            string groupNumber = Console.ReadLine();
                            if (CSVData.TryGetValue(groupNumber, out value))
                            {
                                Console.WriteLine("Die Gruppennummer gehört zum Sprachgebiet: " + value);
                                ean += groupNumber;
                                input = true;
                            }
                            else
                            {
                                input = false;
                                ean = "";
                                Console.WriteLine("Die eigegebene Gruppennummer existiert nicht.");
                            }
                        } while (!input);
                        
                        Console.WriteLine("\nBitte geben Sie die Verlagsnummer ein: ");
                        ean += Console.ReadLine();
                        Console.WriteLine("\nBitte geben Sie die Titelnummer ein: ");
                        ean += Console.ReadLine();
                        isbnInput = "978" + ean;
                        digits = IsDigitsOnly(isbnInput);
                        if (!digits)
                        {
                            inputCorrect = false;
                            ean = "";
                            Console.WriteLine("\nDie ISBN Nummer darf nur aus Zahlen bestehen!");
                        }
                        if (isbnInput.Length != 12)
                        {
                            inputCorrect = false;
                            ean = "";
                            Console.WriteLine("\nDie Gruppen-, Verlags- und Titelnummer müssen aus insgesamt 9 Zahlen bestehen!");
                        }
                        if(digits && isbnInput.Length == 12)
                        {
                            inputCorrect = true;
                            Console.WriteLine("\nEAN: {0}", isbnInput);
                        }
                    } while (!inputCorrect);
                }
                else
                    correct = false;
            } while (!correct);
        }

        private static void ShowVerlagsnummern()
        {
            Console.WriteLine("\nVerlagsnummern: ");
        }

        private static void Check(string isbn) 
        {
            if (isbn.Length == 9)
            {
                isbnInput += IsbnTen(isbn);
                Console.WriteLine("Prüfziffer wurde berechnet.");
            }
            else if (isbn.Length == 10)
            {
                if (isbn[9].ToString().ToUpper() == IsbnTen(isbn))
                {
                    Console.WriteLine("Prüfziffer Korrekt.");
                }
                else
                {
                    Console.WriteLine("Falsche Prüfziffer. Isbn Nummer wird mit richtiger Prüfziffer ausgegeben.");
                    isbnInput = isbn.Remove(9) + IsbnTen(isbn);
                }
            }
            else if (isbn.Length == 12)
            {
                isbnInput += IsbnThirdteen(isbn);
                Console.WriteLine("Prüfziffer wurde berechnet.");
            }
            else if (isbn.Length == 13)
            {
                if (isbn[12].ToString().ToUpper() == IsbnTen(isbn))
                {
                    Console.WriteLine("Prüfziffer Korrekt.");
                }
                else
                {
                    Console.WriteLine("Falsche Prüfziffer. Isbn Nummer wird mit richtiger Prüfziffer ausgegeben.");
                    isbnInput = isbn.Remove(12) + IsbnTen(isbn);
                }
            }
        }

        private static void Convert(string isbnInput)
        {
            string isbn10 ="";
            string isbn13 = "";
            if (isbnInput.Length == 10)
            {
                isbn10 = isbnInput;
                isbn13 = "978" + isbnInput.Remove(9);
                isbn13 = isbn13 + IsbnThirdteen(isbn13);
            }
            else
            {
                isbn13 = isbnInput;
                isbn10 = isbnInput.Remove(0,3).Remove(9);
                isbn10 = isbn10 + IsbnTen(isbn10);
            }
            Console.WriteLine("\nISBN 13: {0}\nISBN 10: {1}", isbn13.ToUpper(), isbn10.ToUpper());
        }

        private static string IsbnTen(string isbn)
        {
            int check = 0;
            string value = "";
            for (int i = 0; i < 9; i++)
            {
                check = check + (System.Convert.ToInt32(isbn[i].ToString()) * (i + 1));
            }
            value = System.Convert.ToString(check % 11);

            if (value == "10")
                value = "X";
            return value;
        }

        private static string IsbnThirdteen(string isbn)
        {
            int check = 0;
            string value = "";
            for (int i = 0; i < 12; i++)
            {
                check = check + (System.Convert.ToInt32(isbn[i].ToString()) * ((i % 2 == 0)? 1 : 3));
            }
            value = System.Convert.ToString(10-(check % 10));
            
            if (value == "10")
                value = "X";
            return value;
        }

        private static Dictionary<string, string> LoadCSV(string CSVFilePath)
        {
            StreamReader sr = new StreamReader(@CSVFilePath);
            Dictionary<string, string> lines = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                string[] Line = sr.ReadLine().Split(';');
                lines[Line[0]] = Line[1];
            }

            sr.Close();

          
            return lines;
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }

}

