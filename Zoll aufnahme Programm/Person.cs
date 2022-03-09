using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoll_aufnahme_Programm
{
    internal class Person
    {
        private string name;
        private string vorname;
        private DateTime geburtstag;

        public Person(string Name, string Vorname, DateTime Geburtstag)
        {
            this.name = Name;
            this.vorname = Vorname;
            this.geburtstag = Geburtstag;
        }

        public string Name { get { return this.name; } }
            
       
        public string getVorname() 
        { 
            return this.vorname; 
        }

        public DateTime getGeburtstag() 
        { 
            return this.geburtstag;
        }
    }
}
