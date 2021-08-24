using JeSch.Presentation.Contracts.Observable;
using JeSch.Presentation.Wpf.Components.Commands;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CashBook.ViewModels
{
    public class CashBookViewModel : ObservableObject
    {
        #region properties
        private readonly string connectionString = "Data Source=C:\\Users\\jenss\\OneDrive\\Kassenbuch\\CashBook.sqlite";
        private readonly Regex _regexNumeric = new Regex(@"^[0-9]+(\,[0-9]{1,2}|\,)?$");
        private List<string[]> entries = new List<string[]>();
        private bool updateOrInsert;
        private List<string[]> updateEntries = new List<string[]>();
        private List<string[]> insertEntries = new List<string[]>();

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                SetProperty(ref _isEnabled, value);
            }
        }


        private int _entryNumber;

        public int EntryNumber
        {
            get => _entryNumber;
            set
            {
                SetProperty(ref _entryNumber, value);
                (ClickButtonAcceptCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }


        private string _date;

        public string Date
        {
            get => _date;
            set
            {
                SetProperty(ref _date, value);
                (ClickButtonAcceptCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }


        public List<string> _categories;

        public List<string> Categories
        {
            get => _categories;
            set
            {
                SetProperty(ref _categories, value);
            }
        }


        private string _category;

        public string Category
        {
            get => _category;
            set
            {
                SetProperty(ref _category, value);
                (ClickButtonAcceptCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }


        private string _excludedCategory;

        public string ExcludedCategory
        {
            get => _excludedCategory;
            set
            {
                SetProperty(ref _excludedCategory, value);
            }
        }


        private string _excludedCategoryWidth;

        public string ExcludedCategoryWidth
        {
            get => _excludedCategoryWidth;
            set
            {
                SetProperty(ref _excludedCategoryWidth, value);
            }
        }


        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value);
                (ClickButtonAcceptCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }


        private int _source;

        public int Source
        {
            get => _source;
            set
            {
                SetProperty(ref _source, value);
                (ClickButtonAcceptCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }


        public List<string> _sources;

        public List<string> Sources
        {
            get => _sources;
            set
            {
                SetProperty(ref _sources, value);
            }
        }


        private int _sign;

        public int Sign
        {
            get => _sign;
            set
            {
                SetProperty(ref _sign, value);
                (ClickButtonAcceptCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }


        public List<string> _signs;

        public List<string> Signs
        {
            get => _signs;
            set
            {
                SetProperty(ref _signs, value);
            }
        }


        private string _amount;

        public string Amount
        {
            get => _amount;
            set
            {
                SetProperty(ref _amount, value);
                (ClickButtonAcceptCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }


        private double _totalFromSource;

        public double TotalFromSource
        {
            get => _totalFromSource;
            set
            {
                SetProperty(ref _totalFromSource, value);
            }
        }


        private int _totalSource;

        public int TotalSource
        {
            get => _totalSource;
            set
            {
                SetProperty(ref _totalSource, value);
                CalculateTotal();
                (ClickButtonAcceptCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }


        private double _total;

        public double Total
        {
            get => _total;
            set
            {
                SetProperty(ref _total, value);
            }
        }


        private string _information;

        public string Information
        {
            get => _information;
            set
            {
                SetProperty(ref _information, value);
            }
        }

        #endregion


        public ICommand ClickButtonAcceptCommand { get; }
        public ICommand ClickButtonCancelCommand { get; }
        public ICommand ClickButtonCloseCommand { get; }
        public ICommand ClickButtonCreateNewEntryCommand { get; }
        public ICommand ClickButtonEditEntryCommand { get; }
        public ICommand ClickButtonEntryNumberDownCommand { get; }
        public ICommand ClickButtonEntryNumberUpCommand { get; }
        public ICommand ClickButtonSaveCommand { get; }

        public CashBookViewModel()
        {
            ClickButtonAcceptCommand = new DelegateCommand(OnExecuteClickButtonAccept, () => ArePropertiesSet());
            ClickButtonCancelCommand = new DelegateCommand(OnExecuteClickButtonCancel);
            ClickButtonCloseCommand = new DelegateCommand(OnExecuteClickButtonClose);
            ClickButtonCreateNewEntryCommand = new DelegateCommand(OnExecuteClickButtonCreateNewEntry);
            ClickButtonEditEntryCommand = new DelegateCommand(OnExecuteClickButtonEditEntry);
            ClickButtonEntryNumberDownCommand = new DelegateCommand(OnExecuteClickButtonEntryNumberDown);
            ClickButtonEntryNumberUpCommand = new DelegateCommand(OnExecuteClickButtonEntryNumberUp);
            ClickButtonSaveCommand = new DelegateCommand(OnExecuteClickButtonSave);

            LoadEntries();
            SetComboBoxItems();
            LoadEntry(LoadLatestEntryNumber());
        }


        #region Button

        private void OnExecuteClickButtonAccept()
        {
            if (Convert.ToInt32(entries.Last()[0]) > EntryNumber - 1)
                entries.RemoveAt(EntryNumber - 1);
            var newEntry = new string[] { EntryNumber.ToString(), Date, Category, Description, LoadSource(Source), Sign == Convert.ToInt32(Enums.Sign.Minus) ? Enums.Sign.Minus.ToString() : Enums.Sign.Plus.ToString(), Amount };
            entries.Insert(EntryNumber - 1, newEntry);
            if (updateOrInsert)
                updateEntries.Add(newEntry);
            else
                insertEntries.Add(newEntry);
            IsEnabled = false;
            LoadEntry(EntryNumber);
        }

        private void OnExecuteClickButtonCancel()
        {
            int currentEntryNumber = EntryNumber;
            LoadEntry(LoadLatestEntryNumber() < currentEntryNumber ? LoadLatestEntryNumber() : EntryNumber);
            IsEnabled = false;
        }

        private void OnExecuteClickButtonClose()
        {
            if (MessageBox.Show("Sind sie sicher, dass sie das Programm schließen wollen?\nAlle nicht gespeicherten Einträge gehen verloren!", "WARNUNG", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void OnExecuteClickButtonCreateNewEntry()
        {
            IsEnabled = true;
            SetNewEntry();
            updateOrInsert = false;
        }

        private void OnExecuteClickButtonEditEntry()
        {
            IsEnabled = true;
            updateOrInsert = true;
        }

        private void OnExecuteClickButtonEntryNumberDown()
        {
            if (EntryNumber > 1)
            {
                EntryNumber--;
                LoadEntry(EntryNumber);
            }
        }

        private void OnExecuteClickButtonEntryNumberUp()
        {
            if (EntryNumber < Convert.ToInt32(entries.Last()[0]))
            {
                EntryNumber++;
                LoadEntry(EntryNumber);
            }
        }

        private void OnExecuteClickButtonSave()
        {
            if (SaveEntries())
                MessageBox.Show("Einträge wurden gespeichert!");
            else
                MessageBox.Show("Speichern fehlgeschlagen!");

            IsEnabled = false;
        }

        #endregion

        #region LoadProperties

        private void LoadEntries()
        {
            SqliteCommand command = new SqliteCommand
            {
                Connection = new SqliteConnection(connectionString),
                CommandText = "SELECT * FROM CashBook"
            };
            command.Connection.Open();
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                entries.Add(new string[] { reader["id"].ToString(), reader["date"].ToString(), reader["category"].ToString(), reader["description"].ToString(), reader["source"].ToString(), reader["sign"].ToString(), reader["amount"].ToString().Replace('.',',') });
            }
            reader.Close();

            command.Dispose();
        }

        private void LoadCategories()
        {
            SqliteCommand command = new SqliteCommand
            {
                Connection = new SqliteConnection(connectionString),
                CommandText = "SELECT * FROM Categories"
            };
            command.Connection.Open();
            SqliteDataReader reader = command.ExecuteReader();

            Categories = new List<string>();
            while (reader.Read())
            {
                Categories.Add(reader["name"].ToString());
            }
            reader.Close();
            command.Dispose();
        }

        private int LoadLatestEntryNumber()
        {
            string[] lastEntry = entries.Last();
            return Convert.ToInt32(lastEntry[0]);
        }

        private void LoadEntry(int entryNumber)
        {
            string[] entry = entries[entryNumber - 1];
            EntryNumber = Convert.ToInt32(entry[0]);
            Date = entry[1];
            if (Categories.Contains(entry[2]))
            {
                Category = entry[2];
                ExcludedCategoryWidth = "0";
                ExcludedCategory = "";
            }
            else
            {
                Category = Categories.First();
                ExcludedCategoryWidth = "200";
                ExcludedCategory = $"Old Category: {entry[2]}";
            }
            Description = entry[3];
            Source = LoadSource(entry[4]);
            Sign = Enums.Sign.Minus.ToString() == entry[5] ? Convert.ToInt32(Enums.Sign.Minus) : Convert.ToInt32(Enums.Sign.Plus);
            Amount = SetCorrectAmount(ref entry[6]);
            IsEnabled = false;
            CalculateTotal();
        }

        private int LoadSource(string source)
        {
            if (Enums.Source.Girokonto.ToString() == source)
                return Convert.ToInt32(Enums.Source.Girokonto);
            if (Enums.Source.Sparbuch.ToString() == source)
                return Convert.ToInt32(Enums.Source.Sparbuch);
            if (Enums.Source.Spardose.ToString() == source)
                return Convert.ToInt32(Enums.Source.Spardose);
            return Convert.ToInt32(Enums.Source.Bargeld);
        }

        private string LoadSource(int source)
        {
            if (Convert.ToInt32(Enums.Source.Girokonto) == source)
                return Enums.Source.Girokonto.ToString();
            if (Convert.ToInt32(Enums.Source.Sparbuch) == source)
                return Enums.Source.Sparbuch.ToString();
            if (Convert.ToInt32(Enums.Source.Spardose) == source)
                return Enums.Source.Spardose.ToString();
            return Enums.Source.Bargeld.ToString();
        }

        #endregion

        #region SetProperties

        private void SetNewEntry()
        {
            var entry = entries.Last();
            EntryNumber = (Convert.ToInt32(entry[0]) + 1);
            var day = DateTime.Now.Day.ToString();
            day = day.Length == 1 ? "0" + day : day;
            var month = DateTime.Now.Month.ToString();
            var year = DateTime.Now.Year.ToString();
            Date = day + "." + month + "." + year;
            Category = Categories.First();
            Description = "";
            Source = Convert.ToInt32(Enums.Source.Bargeld);
            Sign = Convert.ToInt32(Enums.Sign.Minus);
            Amount = "";
        }

        private string SetCorrectAmount(ref string amount)
        {
            if (!amount.Contains(","))
                amount += ",00";
            if (amount.Split(',').Last().Length == 0)
                amount += "00";
            if (amount.Split(',').Last().Length == 1)
                amount += "0";
            if (amount.Split(',').First().Length < 1)
                amount = "0" + amount;
            return amount;
        }

        private void SetComboBoxItems()
        {
            LoadCategories();
            SetSources();
            SetSigns();
        }

        private void SetSources()
        {
            Sources = new List<string>();
            foreach (var source in Enum.GetValues(typeof(Enums.Source)))
            {
                Sources.Add(source.ToString());
            }
        }

        private void SetSigns()
        {
            Signs = new List<string>();
            foreach (var sign in Enum.GetValues(typeof(Enums.Sign)))
            {
                Signs.Add(sign.ToString());
            }
        }

        #endregion

        #region PropertyCheck

        private bool ArePropertiesSet()
        {
            return !(CheckDescription() || CheckAmount());
        }

        private bool CheckDescription()
        {
            return string.IsNullOrWhiteSpace(Description);
        }

        private bool CheckAmount()
        {
            if (string.IsNullOrWhiteSpace(Amount))
                return true;

            Information = !_regexNumeric.IsMatch(Amount) ? "Falsche Format. Richtig: XXX,XX" : "";
            return !_regexNumeric.IsMatch(Amount);
        }

        private void CalculateTotal()
        {
            TotalFromSource = 0;
            Total = 0;

            foreach (var entry in entries)
            {
                var amount = Convert.ToDouble(entry[6].Replace(',', '.'));
                if (entry[5] == Enums.Sign.Plus.ToString())
                {
                    if (entry[4] == LoadSource(TotalSource))
                        TotalFromSource += amount;
                    Total += amount;
                }
                else
                {
                    if (entry[4] == LoadSource(TotalSource))
                        TotalFromSource -= amount;
                    Total -= amount;
                }
            }
        }

        #endregion

        #region SaveProperties

        private bool SaveEntries()
        {
            try
            {
                SqliteCommand command = new SqliteCommand
                {
                    Connection = new SqliteConnection(connectionString)
                };
                command.Connection.Open();
                foreach (var entry in updateEntries)
                {
                    command.CommandText = $"UPDATE CashBook SET date = '{entry[1]}', category = '{entry[2]}', description = '{entry[3]}', source = '{entry[4]}', sign = '{entry[5]}', amount = '{entry[6]}' WHERE id = {entry[0]}";
                    command.ExecuteNonQuery();
                }
                foreach (var entry in insertEntries)
                {
                    command.CommandText = $"INSERT INTO CashBook VALUES({entry[0]}, '{entry[1]}', '{entry[2]}', '{entry[3]}', '{entry[4]}', '{entry[5]}', '{entry[6]}')";
                    command.ExecuteNonQuery();
                }
                command.Dispose();
                updateEntries = new List<string[]>();
                insertEntries = new List<string[]>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
