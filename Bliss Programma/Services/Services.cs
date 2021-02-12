using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bliss_Programma.Services
{
    public class Functies
    {
        public static int Prio(string prio)
        {
            return (7 * Int32.Parse(prio));
        }

        public static int maxbezetting(int oppervlakte)
        {
            return (int)Math.Round(oppervlakte / 1.95);
        }
    }
}
