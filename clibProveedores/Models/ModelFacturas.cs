using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace clibProveedores.Models
{

    public class formResponseFacturas
    {
        public formResponseFacturas()
        {
            root = new List<Object>();
        }

        public Boolean success { get; set; }

        public string codError { get; set; }
        public string msgError { get; set; }

        public List<Object> root { get; set; }
    }


    public class facConsSelPedidos
    {
        public int idPedido { get; set; }
        public string numPedido { get; set; }
        public DateTime fechaPedido { get; set; }
        public string codAlmacen { get; set; }
        public string nomAlmacen { get; set; }
        public string codProveedor { get; set; }
        public string nomProveedor { get; set; }
        public string codAlmDestino { get; set; }
        public string zonaOrigen { get; set; }
        public bool esDescargado { get; set; }
        public bool esImpreso { get; set; }
        public string esDescargadoDes { get; set; }
        public string esImpresoDes { get; set; }
        public string estadoDes { get; set; }
        public int cantidadTotalPedido { get; set; }
        public decimal valorTotalPedido { get; set; }
        public decimal totalSumaFacturas { get; set; }
        public string estado { get; set; }
        public decimal valorPendPedido { get; set; }
        public char siValorPend { get; set; }
        public string data { get; set; }
        public string estadoDistri { get; set; }
        public string tipoPedido { get; set; }
        public DateTime fecEntregaActual { get; set; }
    }

    public class facConsRecFacturas
    {
        public int idPedido { get; set; }
        public int idDocumento { get; set; }
        public string numPedido { get; set; }
        public string numFactura { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaEmision { get; set; }
        public string nombreArchivo { get; set; }
        public string estado { get; set; }
        public string estadoDes { get; set; }
        public string codAlmacen { get; set; }
        public string almacen { get; set; }
        public string data { get; set; }
    }

    public class facConsPedidoNumeroCab
    {
        public int idPedido { get; set; }
        public string numPedido { get; set; }
        public string nomProveedor { get; set; }
        public string nomComercial { get; set; }
        public string rucProveedor { get; set; }
        public string dirCallePrinc { get; set; }
        public string dirCalleNum { get; set; }
        public string dirPisoEdificio { get; set; }
        public string nomEmpresa { get; set; }
        public string rucEmpresa { get; set; }
        public string codAlmacen { get; set; }
        public string nomAlmacen { get; set; }
        public string fechaPedido { get; set; }
        public decimal subTotalSumaFacturas { get; set; }
        public decimal valTotalPedido { get; set; }
    }
    public class facConsPedidoNumeroDetItem
    {
        public string item { get; set; }
        public string codArticulo { get; set; }
        public string desArticulo { get; set; }
        public decimal cantPedido { get; set; }
        public decimal cantPendiente { get; set; }
        public decimal precioCosto { get; set; }
        public string tamanoCaja { get; set; }
        public decimal descuento1 { get; set; }
        public decimal descuento2 { get; set; }
        public string indIva1 { get; set; }
        public string indIva1Des { get; set; }
        public decimal subTotalItem { get; set; }
        public decimal totalItem { get; set; }
    }

    public class facConsDocumentoIdCab
    {
        public int idDocumento { get; set; }
        public int idPedido { get; set; }
        public string nombreArchivo { get; set; }
        public string xmlSRI { get; set; }
        public string claveAcceso { get; set; }
        public string estado { get; set; }
        public string nomProveedor { get; set; }
        public string nomComercial { get; set; }
        public string rucProveedor { get; set; }
        public string facEstabl { get; set; }
        public string facPtoEmi { get; set; }
        public string facNumSec { get; set; }
        public string dirMatriz { get; set; }
        public string dirSucursal { get; set; }
        public string fechaEmision { get; set; }
        public string fechaIniVigAut { get; set; }
        public string fechaFinVigAut { get; set; }
        public string numAutorizaTal { get; set; }
        public string nomEmpresa { get; set; }
        public string rucEmpresa { get; set; }
        public string numPedido { get; set; }
        public string fechaPedido { get; set; }
        public string codAlmacen { get; set; }
        public string nomAlmacen { get; set; }
        public decimal subTotalSumaFacturas { get; set; }
        public decimal valTotalPedido { get; set; }
        public string detSubTot_0 { get; set; }
        public string detDescuento_0 { get; set; }
        public string detIrbpnr_0 { get; set; }
        public string detSubTot_12 { get; set; }
        public string detDescuento_12 { get; set; }
        public string detIce { get; set; }
        public string detIrbpnr_12 { get; set; }
        public Boolean chkcompesacion { get; set; }
    }

    public class facGrabaDocumento
    {
        public string tipoACCION { get; set; }
        public int idDocumento { get; set; }
        public string codSAP { get; set; }
        public int idPedido { get; set; }
        public string numPedido { get; set; }
        public string facEstabl { get; set; }
        public string facPtoEmi { get; set; }
        public string facNumSec { get; set; }
        public string fechaEmision { get; set; }
        public string claveAcceso { get; set; }
        public string nombreArchivo { get; set; }
        public string nomProveedor { get; set; }
        public string nomComercial { get; set; }
        public string rucProveedor { get; set; }
        public string dirMatriz { get; set; }
        public string fechaIniVigAut { get; set; }
        public string fechaFinVigAut { get; set; }
        public string numAutorizaTal { get; set; }
        public string nomEmpresa { get; set; }
        public string rucEmpresa { get; set; }
        public string dirSucursal { get; set; }
        public string detSubTotSinImp { get; set; }
        public string detTotDescuento { get; set; }
        public string detPropina { get; set; }
        public string detValTotal { get; set; }
        public string detSubTot_0 { get; set; }
        public string detDescuento_0 { get; set; }
        public string detSubTot_12 { get; set; }
        public string detDescuento_12 { get; set; }
        public string detIce { get; set; }
        public string detIva_12 { get; set; }
        public string detSubTotNoObjIva { get; set; }
        public string detSubTotExenIva { get; set; }
        public string detTotIrbpnr { get; set; }
        public string detIrbpnr_0 { get; set; }
        public string detIrbpnr_12 { get; set; }
        public string codAlmacen { get; set; }
        public string nomAlmacen { get; set; }
        public string txtcompesacion { get; set; }
        public Boolean chkcompesacion { get; set; }
        public string xmlSRI { get; set; }
    }

    public class facGeneraFile
    {
        public string tipo { get; set; }
        public string codSap { get; set; }
        public string ruc { get; set; }
        public string usuario { get; set; }
        public string nombreFile { get; set; }
        public string dataXML { get; set; }
    }

}
