using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntradaAPI.BO
{
    public class EvergreenModel
    {
        public decimal Id { get; set; }
        public string tipo { get; set; }
        public string rubro { get; set; }
        public string subregion { get; set; }
        public string anio { get; set; }
        public string municipio { get; set; }
        public string area_total { get; set; }
        public string area_produccion { get; set; }
        public string volumen_produccion { get; set; }
    }
}
