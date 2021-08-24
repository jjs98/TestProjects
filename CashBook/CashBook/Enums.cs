using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashBook
{
    public class Enums
    {
        public enum Source
        {
            Bargeld,
            Girokonto,
            Sparbuch,
            Spardose
        }

        public enum Sign
        {
            Minus,
            Plus
        }
    }
}