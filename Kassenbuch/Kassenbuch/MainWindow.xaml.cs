using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kassenbuch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public static readonly string filePathCsv = "C:\\Users\\jenss\\OneDrive\\Kassenbuch\\Kassenbuch.csv";
        public static readonly string filePathCategories = "C:\\Users\\jenss\\OneDrive\\Kassenbuch\\Kategorien.json";
        public static List<string[]> lines = null;
        public static int entryNumer;
        public static string dateToday = "";
        public static string date = "";
        public static string category = "";
        public static string description = "";
        public static string origin = "";
        public static string sign = "";
        public static string amount = "";
        public static bool isReadyForApply;
        public static string errorMessage = "";

        public MainWindow()
        {
            InitializeComponent();

            lines = LoadCsv(filePathCsv);

            entryNumer = Convert.ToInt32(lines.Last()[0].Replace("\"", "")) + 1;
            TextBoxEntryNumber.Text = entryNumer.ToString();

            var day = DateTime.Now.Day.ToString();
            day = day.Length == 1 ? "0" + day : day;
            var month = DateTime.Now.Month.ToString();
            var year = DateTime.Now.Year.ToString();
            dateToday = day + "." + month + "." + year;
            TextBoxDate.Text = dateToday;
            date = TextBoxDate.Text;

            LoadCategories(ComboBoxCategory, filePathCategories);
        }

        #region Buttons
        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            LoadCategories(ComboBoxCategory, filePathCategories);
            if (ComboBoxCategory.SelectedItem != null)
            {
                date = TextBoxDate.Text;
                category = ComboBoxCategory.Text;
                description = TextBoxDescription.Text;
                origin = ComboBoxOrigin.Text;
                sign = ComboBoxSign.Text;
                amount = TextBoxAmount.Text;
                if (CheckInput())
                {
                    lines.Add(new string[] { entryNumer.ToString(), date, category, description, origin, sign, amount });
                    TextBoxDate.Text = dateToday;
                    TextBoxDescription.Text = "";
                    TextBoxAmount.Text = "0.00";
                    SaveCsv(lines, filePathCsv);
                    entryNumer++;
                    TextBoxEntryNumber.Text = entryNumer.ToString();
                }
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonEditCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow categoryWindow = new CategoryWindow();
            categoryWindow.Show();
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            entryNumer++;
            TextBoxEntryNumber.Text = entryNumer.ToString();
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            entryNumer--;
            TextBoxEntryNumber.Text = entryNumer.ToString();

        }
        #endregion

        #region CsvHandle
        private static List<string[]> LoadCsv(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            List<string[]> lines = new List<string[]>();
            while (!sr.EndOfStream)
            {
                string[] Line = sr.ReadLine().Replace("\"", "").Split(',');
                lines.Add(Line);
            }

            sr.Close();

            return lines;
        }

        private void SaveCsv(List<string[]> lines, string filePath)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filePath);

                foreach (var line in lines)
                {
                    var newLine = "";
                    foreach (var content in line)
                    {
                        newLine += $"\"{content}\"";
                    }
                    sw.WriteLine(newLine.Replace("\"\"", "\",\""));
                }

                sw.Close();
                LabelInformation.Foreground = Brushes.Black;
                LabelInformation.Content = "Neuer Eintrag wurde erfolgreich erstellt.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Erstellen des Eintrags: {ex}");
                LabelInformation.Content = "";
            }
        }
        #endregion

        #region CheckInput
        private void Control_TextChanged(object sender, TextChangedEventArgs e) => CheckInput();
        private bool CheckInput()
        {
            date = TextBoxDate.Text;
            errorMessage = "";
            isReadyForApply = true;
            var dateCheck = date.Replace(" ", "").Split('.');
            if (date.Length > 10 || dateCheck.Length != 3 || dateCheck[0].Length != 2 || dateCheck[1].Length != 2 || dateCheck[2].Length != 4 || !Regex.IsMatch(dateCheck[0], @"^[0-9]+$") || !Regex.IsMatch(dateCheck[1], @"^[0-9]+$") || !Regex.IsMatch(dateCheck[2], @"^[0-9]+$"))
            {
                TextBoxDate.BorderBrush = Brushes.Red;
                isReadyForApply = false;
                errorMessage += "Falsches Datumsformat! Das korrekte Datumsformat lautet DD.MM.YYYY\n";
            }
            else
            {
                TextBoxDate.BorderBrush = Brushes.Black;
            }

            amount = TextBoxAmount.Text;
            if (!Regex.IsMatch(amount, @"^[0-9.]+$"))
            {
                TextBoxAmount.BorderBrush = Brushes.Red;
                isReadyForApply = false;
                errorMessage += "Falsches Format für den Betrag! Das korrekte Format lautet XXX.XX\n";
            }
            else
            {
                TextBoxAmount.BorderBrush = Brushes.Black;
            }

            if (IsActive)
            {
                LabelInformation.Foreground = Brushes.Red;
                LabelInformation.Content = errorMessage;
                if (!isReadyForApply)
                    ButtonApply.IsEnabled = false;
                else
                    ButtonApply.IsEnabled = true;
            }
            return isReadyForApply;
        }

        private void Control_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e) => CorrectInput();
        private void CorrectInput()
        {
            if (!amount.Contains("."))
                amount += ".00";
            if (amount.Split('.').Last().Length == 0)
                amount += "00";
            if (amount.Split('.').Last().Length == 1)
                amount += "0";
            if (amount.Split('.').First().Length < 1)
                amount = "0" + amount;
            if (amount.Split('.').Last().Length > 2)
                amount = amount.Remove(amount.IndexOf('.') + 3, amount.Length - amount.IndexOf('.') - 3);

            foreach (var letter in amount)
            {
                if (amount.Split('.').First().Length > 1)
                {
                    if (amount[0] == '0' && amount[1] != '.')
                        amount = amount.Remove(0, 1);
                }
            }
            TextBoxAmount.Text = amount;
        }
        #endregion

        #region LoadControls
        private void ComboBoxCategory_DropDownOpened(object sender, EventArgs e)
        {
            LoadCategories(ComboBoxCategory, filePathCategories);
        }

        public void LoadCategories(ComboBox comboBoxCategory, string filePath)
        {
            var categories = File.ReadAllLines(filePath);
            var selectedItem = comboBoxCategory.SelectedItem;
            comboBoxCategory.Items.Clear();
            foreach (var category in categories)
            {
                comboBoxCategory.Items.Add(category);
            }
            if (selectedItem == null)
                comboBoxCategory.SelectedIndex = 0;
            else
                comboBoxCategory.SelectedItem = selectedItem;
        }
        #endregion
    }
}
