using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clibProveedores.Models
{
    public class Cobertura
    {
        public string Periodo { get; set; }
        public decimal? TotalGlobal { get; set; }
        public decimal? AvgCobInventario { get; set; }
        public decimal? AvgCobRotacion { get; set; }
        public decimal? CoberturaInvSKU { get; set; }
        public decimal? CoberturaRotacion { get; set; }
    }
    public class Inventario
    {
        public decimal? StockCosto { get; set; }
        public decimal? CostoNeto { get; set; }
        public decimal? StockUnidades { get; set; }
        public decimal? DiasInv { get; set; }
        public string Unida { get; set; }
        public string Unidb { get; set; }
        public string Unidc { get; set; }
        public string Unidd { get; set; }
        public string Unide { get; set; }

    }
    public class Ventas
    {

        public string Periodo { get; set; }
        public decimal? SumaVenta { get; set; }
        public decimal? Crecimiento { get; set; }
        public decimal? Porcentaje { get; set; }
        public decimal? Presupuesto { get; set; }
        public decimal? Factura { get; set; }
        public string Unida { get; set; }
        public string Unidb { get; set; }
        public string Unidc { get; set; }
    }

    public class SelectModel
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public string Codigo { get; set; }

    }

    public class ModelosSelect
    {
        public ModelosSelect()
        {
            root = new List<Object>(); //antes root wcr
        }

        public string cCodError { get; set; }
        public string cMsgError { get; set; }

        public Boolean lSuccess { get; set; }

        public List<Object> root { get; set; } //antes root wcr
    }

    public class ResumenVenta
    {


        public ResumenVenta()
        {
            root = new List<Object>(); //antes root wcr
        }
        public string cCodError { get; set; }
        public string cMsgError { get; set; }

        public Boolean lSuccess { get; set; }

        public List<Object> root { get; set; } //antes root wcr
    }
}
