using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoll_aufnahme_Programm
{
    internal class Beamter : Person 
    {
        string Rang;
        string Dienststelle;
        public Beamter(string Name, string Vorname, DateTime Geburtstag, string Rang, string dienststelle):base (Name, Vorname, Geburtstag)
        {
            this.Rang = Rang;
            this.Dienststelle = dienststelle;
        }
       
    }
}
