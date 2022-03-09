using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoll_aufnahme_Programm
{
    internal class Bestellung
    {
        private string Herkunftsland;
        private string Warenart;
        //Klären ob double oder decimal für den Zollsatz und Warenwert
        private decimal Warenwert;
        private decimal Zollsatz;
        private string Beamter;

        public Bestellung(string Herkunftsland, string Warenart, decimal Warenwert, decimal Zollsatz, string beamter)
        {
            this.Herkunftsland = Herkunftsland;
            this.Warenart = Warenart;
            this.Warenwert = Warenwert;
            this.Zollsatz = Zollsatz;
            this.Beamter = beamter;
        }
        public string getherkunftsland()
        {
            return this.Herkunftsland;
        }
        public string getwarenart()
        {
            return this.Warenart;
        }
        public decimal getWarenwert()
        {
            return this.Warenwert;
        }
        public decimal getZollsatz()
        {
            return this.Zollsatz;
        }
        public string getBeamter()
        {
            return this.Beamter;
        }
    }
}
