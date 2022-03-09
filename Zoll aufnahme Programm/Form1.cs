using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zoll_aufnahme_Programm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            EingabederdatenListe();
        }

        List<Kunde> KundenListe = new List<Kunde>();
        List<Beamter> BeamtenListe = new List<Beamter>();

        private void EingabederdatenListe()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("Beamte.csv");
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        string zeile = sr.ReadLine();

                        if (zeile != "")
                        {
                            string[] daten = zeile.Split(';');
                            Beamter beamter = new Beamter(daten[0], daten[1], Convert.ToDateTime(daten[2]), daten[3], daten[4]);

                            BeamtenListe.Add(beamter);
                            comboBox3.Items.Add(beamter.Name);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Fehler Beim Einlesen der Liste");
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Textboxen leer
                if (textBox1.Text != "" && textBox2.Text != "" && dtgeb.Text != "" && textBox3.Text != "" && textBox4.Text != "")
                {


                    // Schüler anlegen und in Liste
                    Kunde kunde = new Kunde(textBox1.Text, textBox2.Text, Convert.ToDateTime(dtgeb.Text), textBox3.Text, textBox4.Text);

                    KundenListe.Add(kunde);
                    // Name in Combobox 
                    comboBox1.Items.Add(kunde.Name);
                    comboBox2.Items.Add(kunde.Name);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    dtgeb.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(String.Format("Fehler beim Erstellen des Kundes: {0}", Ex.Message));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (hklnd.Text != "" && wara.Text != "" && warw.Text != "" && zollsat.Text != "" && comboBox1.SelectedItem.ToString() != "" && comboBox3.SelectedItem.ToString() != "")
                {
                    if (!IsDecimal(warw.Text))
                    {
                        MessageBox.Show($"Der Warenwert muss eine Dezimalzahl sein.");
                    }
                    else if (!IsDecimal(zollsat.Text))
                    {
                        MessageBox.Show($"Der Zollsatz muss eine Dezimalzahl sein.");
                    }
                    else
                    {
                        Kunde kunde = KundenListe.FirstOrDefault(x => x.Name == comboBox1.SelectedItem.ToString());

                        if (kunde != null)
                        {

                            decimal zollabgabe = Convert.ToDecimal(warw.Text) * (Convert.ToDecimal(zollsat.Text) / 100);
                            decimal gsw = (Convert.ToDecimal(warw.Text) + zollabgabe) * 19 / 100;
                            decimal endwrt = decimal.Round(gsw, 2);
                            decimal Zollsatz = endwrt;
                            MessageBox.Show($"Der Kunde muss: " + endwrt + "€ Zahlen");

                            kunde.setBestellung(hklnd.Text, wara.Text, Convert.ToDecimal(warw.Text), Zollsatz, comboBox3.SelectedItem.ToString());
                            hklnd.Text = "";
                            wara.Text = "";
                            warw.Text = "";
                            zollsat.Text = "";
                            comboBox1.SelectedItem = null;
                            comboBox3.SelectedItem = null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(String.Format("Fehler beim ausfüllen des Formulars", Ex.Message));
            }

        }

        // Das öffnen der Datein muss noch Implementiert werden so wie das beschreiben der CSV Datein siehe Muster

        private void Datein_Einlese()
        {
            int zeilenNr = 1;
            StreamReader KundenDatei = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            // Datei öffnen
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    KundenListe.Clear();

                    KundenDatei = new StreamReader(openFileDialog1.FileName);

                    // bis Dateiende einlesen
                    while (!KundenDatei.EndOfStream)
                    {
                        string eineZeile = KundenDatei.ReadLine();
                        // leere Zeilen überspringen
                        if (eineZeile == string.Empty)
                            continue;

                        // Semikolon am Ende der Zeile ggf. löschen
                        if (eineZeile[eineZeile.Length - 1] == ';')
                            eineZeile = eineZeile.Remove(eineZeile.Length - 1);
                        // eingelesene Zeile aufsplitten
                        string[] daten = eineZeile.Split(';');

                        Kunde kunde = new Kunde(daten[0], daten[1], Convert.ToDateTime(daten[2]), daten[3], daten[4]);

                        if (!KundenListe.Any(x => x.Name == kunde.Name))
                        {
                            KundenListe.Add(kunde);
                            comboBox1.Items.Add(kunde.Name);
                            comboBox2.Items.Add(kunde.Name);

                            kunde.setBestellung(daten[5], daten[6], Convert.ToDecimal(daten[7]), Convert.ToDecimal(daten[8]), daten[9]);
                        }
                        else
                        {
                            Kunde exestierenderKunde = KundenListe.First(x => x.Name == kunde.Name);
                            exestierenderKunde.setBestellung(daten[5], daten[6], Convert.ToDecimal(daten[7]), Convert.ToDecimal(daten[8]), daten[9]);
                        }


                        zeilenNr++;
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(String.Format("Es ist ein Fehler bei Einlesen der Datei aufgetreten: {0} bei Zeile {1}", Ex.Message, zeilenNr));
                }
                finally
                {
                    if (KundenDatei != null)
                        KundenDatei.Close();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Datein_Einlese();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            StreamWriter SpeicherDatei = null;
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
            // Datei öffnen
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Datei öffnen ohne Anhängen
                    SpeicherDatei = new StreamWriter(saveFileDialog1.FileName, false);

                    foreach (Kunde kunde in KundenListe)
                    {
                        foreach (string bestellungCsv in kunde.getBestellungToCSV())
                        {
                            SpeicherDatei.WriteLine(bestellungCsv);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(String.Format("Es ist ein Fehler bei Schreiben der Datei aufgetreten: {0}", Ex.Message));
                }
                finally
                {
                    if (SpeicherDatei != null)
                        SpeicherDatei.Close();
                }
            }
        }

        private void cbGetFehlzeiten_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (Kunde einePerson in KundenListe)
            {
                if (einePerson.Name == comboBox2.SelectedItem.ToString())
                {
                    List<string> bestellungsText = einePerson.getBestellungenText();
                    foreach (string bestellung in bestellungsText)
                    {
                        listBox1.Items.Add(bestellung);
                    }

                }
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dtgeb_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.zoll.de/SharedDocs/Boxen/DE/Fragen/0082_beispiele_zollsaetze.html?faqCalledDoc=321372&faqCalledDoc=321372");
        }

        private bool IsDecimal(string input)
        {
            return Double.TryParse(input, out var _);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
