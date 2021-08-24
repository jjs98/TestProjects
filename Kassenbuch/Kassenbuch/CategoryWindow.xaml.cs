using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kassenbuch
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {

        public CategoryWindow()
        {
            InitializeComponent();
            RadioButtonAdd.IsChecked = true;
        }

        #region Buttons
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxCategory.Text))
            {
                    var newCategorie = TextBoxCategory.Text.Replace(" ", "");
                    List<string> categories = File.ReadAllLines(MainWindow.filePathCategories).ToList();
                    categories.Add(newCategorie);
                    var temp = categories.ToArray();
                    Array.Sort(temp);
                    File.WriteAllLines(MainWindow.filePathCategories, temp);
                    TextBoxCategory.Text = "";
                    LabelInformation.Content = "Kategorie wurde erfolgreich hinzugefügt.";
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var categories = File.ReadAllLines(MainWindow.filePathCategories);
            List<string> updatedCategories = new List<string>();
            foreach (var categorie in categories)
            {
                if (categorie != ComboBoxCategory.Text)
                    updatedCategories.Add(categorie);
            }
            var temp = updatedCategories.ToArray();
            Array.Sort(temp);
            File.WriteAllLines(MainWindow.filePathCategories, temp);
            ComboBoxCategory.SelectedItem = null;
            LabelInformation.Content = "Kategorie wurde erfolgreich gelöscht.";
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RadioButtonAdd_Checked(object sender, RoutedEventArgs e)
        {
            SwitchEnable(true, false);
        }

        private void RadioButtonDelete_Checked(object sender, RoutedEventArgs e)
        {
            SwitchEnable(false, true);
        }
        #endregion

        #region LoadControls
        private void ComboBoxCategory_DropDownOpened(object sender, EventArgs e)
        {
            LoadCategories(ComboBoxCategory, MainWindow.filePathCategories);
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

        private void SwitchEnable(bool add, bool delete)
        {
            TextBoxCategory.IsEnabled = add;
            ButtonAdd.IsEnabled = add;
            ComboBoxCategory.IsEnabled = delete;
            ButtonDelete.IsEnabled = delete;
            LabelInformation.Content = "";
        }
    }
}
