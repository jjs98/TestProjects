using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoll_aufnahme_Programm
{
    internal class Kunde : Person
    {
        private string adresse;
        private string plz;

        private List<Bestellung> BestellungenListe = new List<Bestellung>();

        public Kunde(string Name, string Vorname, DateTime Geburtstag, string adresse, string plz) : base(Name, Vorname, Geburtstag)
        {
            this.adresse = adresse;
            this.plz = plz;
        }

        public string getAdresse()
        {
            return adresse;

        }

        public string getPlz()
        {
            return plz;
        }

        public void setBestellung(string Herkunftsland, string Warenart, decimal Warenwert, decimal Zollsatz, string beamter)
        {
            Bestellung einebestellung = new Bestellung(Herkunftsland, Warenart, Warenwert, Zollsatz, beamter);
            BestellungenListe.Add(einebestellung);
        }

        public List<string> getBestellungenText()
        {
            List<string> bestellungsText = new List<string>();
            foreach (Bestellung bestellung in BestellungenListe)
            {
                bestellungsText.Add("Der Kunde: " + Name + " hat " + bestellung.getwarenart() + " im wert von " + bestellung.getWarenwert() + " Euro aus " + bestellung.getherkunftsland() + " bestellt. Erfassender Beamte: " + bestellung.getBeamter());
            }
            return bestellungsText;
        }

        public List<string> getBestellungToCSV()
        {
            List<string> bestellungcsv = new List<string>();
            foreach (Bestellung bestellung in BestellungenListe)
            {
                bestellungcsv.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};",
                    Name,
                    getVorname(),
                    getGeburtstag().ToShortDateString(),
                    getAdresse(),
                    getPlz(),
                    bestellung.getherkunftsland(),
                    bestellung.getwarenart(),
                    bestellung.getWarenwert(),
                    bestellung.getZollsatz(),
                    bestellung.getBeamter()));
            }
            return bestellungcsv;
        }
    }
}
