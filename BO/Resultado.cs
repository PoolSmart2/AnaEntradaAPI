using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntradaAPI.BO
{
    public class Resultado
    {
        public bool Estado { get; set; }
        public string CsvString { get; set; }
        public string Error { get; set; }
    }
}
