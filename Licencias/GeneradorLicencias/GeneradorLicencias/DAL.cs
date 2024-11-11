using GeneradorLicencias;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using System.Configuration;

namespace GeneradorLicencias
{
    public static class DAL
    {
        public static List<ClientesViewModel> ClientesInfo()
        {
            var list = new List<ClientesViewModel>();

            //var planes = PlanesList();

            //var data = ClientesList().Select(c => new ClientesViewModel 
            //{ 
            //    Codigo = c.ruc + c.codProveedor, 
            //    Cliente = c.proveedor, 
            //    Plan = planes[0].plan, 
            //     PlanId = 
            //    Cantidad = planes[0].cantidad 
            //});

            var sql = "select lic.Id, lic.clienteId, lic.planId, cli.proveedor, lic.licencia, p.[plan] ,Codigo = cli.ruc + cli.codProveedor , lic.cantidadMaxProveedores cantidad" +
                " from Licencias lic " +
                " inner join Clientes cli on lic.clienteId = cli.Id" +
                " inner join Planes p on lic.planId = p.Id where lic.activo = 1";

            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultContext"].ConnectionString))
            {
                cnn.Open();

                var cmd = new SqlCommand(sql, cnn);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new ClientesViewModel
                    {
                         Id = int.Parse(dr["Id"].ToString()),
                        Codigo = dr["Codigo"].ToString(),
                        Cliente = dr["proveedor"].ToString(),
                        Plan = dr["plan"].ToString(),
                        Licencia = dr["licencia"].ToString(),
                        PlanId = int.Parse(dr["planId"].ToString()),
                        ClienteId = int.Parse(dr["clienteId"].ToString()),
                        Cantidad = int.Parse(dr["cantidad"].ToString())
                    });
                }
            }

            return list;
        }

        public static void InsertLicencia(Licencias licencias)
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultContext"].ConnectionString))
            {
                cnn.Open();
                var sql = "";

                if (licencias.Id == 0)
                {
                    sql = "INSERT INTO [dbo].[Licencias] ([clienteId],[planId],[licencia],[vencimiento],[cantidadMaxProveedores],[fechaCreacion],[activo])";
                    sql += $"VALUES  ({licencias.ClienteId},{licencias.PlanId},'{licencias.Licencia}','{licencias.Vencimiento.ToString("yyyy-MM-dd")}',{licencias.cantidadMaxProveedores},GETDATE(),{(licencias.Activo ? 1 : 0)})";
                }
                else
                {
                    sql = $"UPDATE [dbo].[Licencias]   " +
                        $"SET " +
                        $"[planId] = {licencias.PlanId}" +
                        $",[licencia] = '{licencias.Licencia}'" +
                        $",[cantidadMaxProveedores] = {licencias.cantidadMaxProveedores}" +
                        $" WHERE Id = {licencias.Id}";
                }

                var cmd = new SqlCommand(sql, cnn);

                var res = cmd.ExecuteNonQuery();

            }
        }

        public static List<Plan> PlanesList()
        {
            var list = new List<Plan>();

            //list.Add(new Plan { codPlan = "1" , plan ="Basico", Activo = true, cantidad = 1000  });
            //list.Add(new Plan { codPlan = "2" , plan ="Intermedio", Activo = true, cantidad = 2000 });
            //list.Add(new Plan { codPlan = "3" , plan ="Avanzado", Activo = true, cantidad = 3000 });
            //list.Add(new Plan { codPlan = "4" , plan ="Premium", Activo = true, cantidad = 5000 });

            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultContext"].ConnectionString))
            {
                cnn.Open();

                var cmd = new SqlCommand("SELECT * FROM PLANES WHERE ACTIVO = 1", cnn);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Plan
                    {
                        Id = int.Parse(dr["Id"].ToString()),
                        codPlan = dr["codPlan"].ToString(),
                        plan = dr["plan"].ToString(),
                        Activo = true,
                        cantidad = int.Parse(dr["cantidad"].ToString())
                    });
                }
            }

            return list;
        }
        public static List<Clientes> ClientesList()
        {
            var list = new List<Clientes>();
            //list.Add(new Clientes { ruc = "990000670001", codProveedor = "TT", proveedor = "ALIANZA ABBOTT.NUTRI - COMPLEMENTOS" });
            //list.Add(new Clientes { ruc = "0990000670001", codProveedor = "9", proveedor = "ALIANZA ABBOTT.NUTRI - FORMULAS"  });
            //list.Add(new Clientes { ruc = "0990000670001", codProveedor = "WF", proveedor = "ALIANZA ABBOTT.NUTRI - REHIDRATANTES" });
            //list.Add(new Clientes { ruc = "0990000670001", codProveedor = "AB", proveedor = "ALIANZA ABBOTT.FARMA" });
            //list.Add(new Clientes { ruc = "0990000670001", codProveedor = "ÁÎ", proveedor = "ABBOTT ESPECIALIDAD"  });
            //list.Add(new Clientes { ruc = "0990000670001", codProveedor = "WQ", proveedor = "ABBOTT DIAGNOSTICAL"  });
            //list.Add(new Clientes { ruc = "1791405552001", codProveedor = "AL", proveedor = "ALIANZA ALCON OTC"  });
            //list.Add(new Clientes { ruc = "1790074889001", codProveedor = "QA", proveedor = "ALIANZA QUIMICA ARISTON"  });
            //list.Add(new Clientes { ruc = "0992152834001", codProveedor = "IY", proveedor = "ZELKRO"  });
            //list.Add(new Clientes { ruc = "0891737386001", codProveedor = "@Î", proveedor = "ANGVICBOR S.A."  });
            //list.Add(new Clientes { ruc = "1790280179001", codProveedor = "N0", proveedor = "BOEHRINGER INGELHEIM - ESPECIALIDAD"  });
            //list.Add(new Clientes { ruc = "1790280179001", codProveedor = "BI", proveedor = "BOEHRINGER INGELHEIM - FARMA"  });
            //list.Add(new Clientes { ruc = "0990160422001", codProveedor = "W0", proveedor = "GRUNENTHAL"  });
            //list.Add(new Clientes { ruc = "1790721450001", codProveedor = "CF", proveedor = "CHALVER"  });
            //list.Add(new Clientes { ruc = "1790721450001", codProveedor = "JU", proveedor = "CHALVER - SALUD ANIMAL"  });
            //list.Add(new Clientes { ruc = "0990036349001", codProveedor = "CH", proveedor = "CHEFAR"  });
            //list.Add(new Clientes { ruc = "1790163466001", codProveedor = "PR", proveedor = "ORGANON"  });
            //list.Add(new Clientes { ruc = "1790001024001", codProveedor = "M3", proveedor = "MERCK BIOTEC"  });
            //list.Add(new Clientes { ruc = "1790001024001", codProveedor = "MR", proveedor = "ALIANZA MERCK FARMA"  });
            //list.Add(new Clientes { ruc = "1790001024001", codProveedor = "MU", proveedor = "MERCK GENERICOS"  });
            //list.Add(new Clientes { ruc = "1791831004001", codProveedor = "G5", proveedor = "GAMMATRADE - TRIM"  });
            //list.Add(new Clientes { ruc = "0990014450001", codProveedor = "ID", proveedor = "INDUNIDAS"  });
            //list.Add(new Clientes { ruc = "0990013314001", codProveedor = "IN", proveedor = "FARMAYALA - FARMAYALA"  });
            //list.Add(new Clientes { ruc = "0992539917001", codProveedor = "CP", proveedor = "BIOINDUSTRIA"  });
            //list.Add(new Clientes { ruc = "0990101175001", codProveedor = "MS", proveedor = "MSD SPECIALTY"  });
            //list.Add(new Clientes { ruc = "1790717658001", codProveedor = "GQ", proveedor = "ALIANZA GLAXO SMITHKLINE PHARMA"  });
            //list.Add(new Clientes { ruc = "1790717658001", codProveedor = "SK", proveedor = "GLAXO SMITHKLINE CONSUMO"  });
            //list.Add(new Clientes { ruc = "1790717658001", codProveedor = "GW", proveedor = "GLAXO SMITHKLINE ESPECIALIDAD"  });
            //list.Add(new Clientes { ruc = "0990545030001", codProveedor = "BW", proveedor = "IMP BOHORQUEZ HOSPITALARIO"  });
            //list.Add(new Clientes { ruc = "0990545030001", codProveedor = "BO", proveedor = "IMP BOHORQUEZ INFANTIL"  });
            //list.Add(new Clientes { ruc = "0990014825001", codProveedor = "W9", proveedor = "ALIANZA PFIZER CIA BIOPHARMACEUTICALS"  });
            //list.Add(new Clientes { ruc = "0990014825001", codProveedor = "V9", proveedor = "ALIANZA PFIZER CIA UPJOHN"  });
            //list.Add(new Clientes { ruc = "0990014825001", codProveedor = "UE", proveedor = "PFIZER CIA BIOPHARMACEUTICALS INSTITUCIONAL"  });
            //list.Add(new Clientes { ruc = "0990040559001", codProveedor = "HG", proveedor = "LABORATORIOS H.G."  });
            //list.Add(new Clientes { ruc = "1792322863001", codProveedor = "0", proveedor = "CELFAR"  });
            //list.Add(new Clientes { ruc = "1792890667001", codProveedor = "LD", proveedor = "INTIAROME"  });
            //list.Add(new Clientes { ruc = "1792733685001", codProveedor = "VK", proveedor = "CALPRANDINA"  });
            //list.Add(new Clientes { ruc = "1791359372001", codProveedor = "MK", proveedor = "TECNOQUIMICAS - GENERICOS"  });
            //list.Add(new Clientes { ruc = "1790411605001", codProveedor = "KA", proveedor = "SANOFI - PASTEUR"  });
            //list.Add(new Clientes { ruc = "1790411605001", codProveedor = "DZ", proveedor = "SANOFI - GENZYME"  });
            //list.Add(new Clientes { ruc = "1790411605001", codProveedor = "AV", proveedor = "SANOFI - FARMA"  });
            //list.Add(new Clientes { ruc = "1792680425001", codProveedor = "EM", proveedor = "LABCOCASAM "  });
            //list.Add(new Clientes { ruc = "1790013502001", codProveedor = "LI", proveedor = "ALIANZA LIFE"  });
            //list.Add(new Clientes { ruc = "1790013502001", codProveedor = "31", proveedor = "LIFE.SALUD ANIMAL"  });
            //list.Add(new Clientes { ruc = "0990018707001", codProveedor = "NC", proveedor = "ECUAQUIMICA - NUTRICIA"  });
            //list.Add(new Clientes { ruc = "0990018707001", codProveedor = "HE", proveedor = "ECUAQUIMICA - HERMES"  });
            //list.Add(new Clientes { ruc = "0990018707001", codProveedor = "SQ", proveedor = "ECUAQUIMICA - BSN MEDICAL"  });
            //list.Add(new Clientes { ruc = "1791321596001", codProveedor = "JD", proveedor = "ALIANZA UNILEVER DESODORANTES"  });
            //list.Add(new Clientes { ruc = "1791321596001", codProveedor = "AF", proveedor = "ALIANZA UNILEVER MISCELANEOS"  });
            //list.Add(new Clientes { ruc = "1791321596001", codProveedor = "JS", proveedor = "ALIANZA UNILEVER SHAMPOO"  });
            //list.Add(new Clientes { ruc = "1791321596001", codProveedor = "PO", proveedor = "ALIANZA UNILEVER PONDS"  });
            //list.Add(new Clientes { ruc = "1791321596001", codProveedor = "JB", proveedor = "ALIANZA UNILEVER JABON TOCADOR"  });
            //list.Add(new Clientes { ruc = "1791321596001", codProveedor = "EF", proveedor = "ALIANZA UNILEVER HELADOS PINGUINO"  });
            //list.Add(new Clientes { ruc = "1790017478001", codProveedor = "3M", proveedor = "3M MEDICAL"  });
            //list.Add(new Clientes { ruc = "1790017478001", codProveedor = "3B", proveedor = "3M FUTURO"  });
            //list.Add(new Clientes { ruc = "0990034249001", codProveedor = "EV", proveedor = "EVEREADY ENERGIZER"  });
            //list.Add(new Clientes { ruc = "1790337979001", codProveedor = "CO", proveedor = "COLGATE PALMOLIVE DEL ECUADOR"  });
            //list.Add(new Clientes { ruc = "1711060101001", codProveedor = "WE", proveedor = "PULPY"  });
            //list.Add(new Clientes { ruc = "1792128730001", codProveedor = "W4", proveedor = "PREMIER TRADING"  });
            //list.Add(new Clientes { ruc = "0990604169001", codProveedor = "PA", proveedor = "ALIANZA J.J. OTC"  });
            //list.Add(new Clientes { ruc = "0990604169001", codProveedor = "JÑ", proveedor = "ALIANZA J.J. INFANTIL"  });
            //list.Add(new Clientes { ruc = "0990604169001", codProveedor = "JC", proveedor = "ALIANZA J.J. CONSUMO"  });
            //list.Add(new Clientes { ruc = "0990604169001", codProveedor = "JE", proveedor = "ALIANZA J.J. NEUTROGENA"  });
            //list.Add(new Clientes { ruc = "0990604169001", codProveedor = "LB", proveedor = "JANSSEN"  });
            //list.Add(new Clientes { ruc = "0990032246001", codProveedor = "NE", proveedor = "ALIANZA NESTLE - NIDO"  });
            //list.Add(new Clientes { ruc = "0990032246001", codProveedor = "ND", proveedor = "ALIANZA NESTLE - FORMULAS INFANTILES"  });
            //list.Add(new Clientes { ruc = "0990032246001", codProveedor = "GB", proveedor = "ALIANZA NESTLE - BABY FOOD"  });
            //list.Add(new Clientes { ruc = "0990032246001", codProveedor = "JJ", proveedor = "ALIANZA NESTLE - PETCARE"  });
            //list.Add(new Clientes { ruc = "0990032246001", codProveedor = "WL", proveedor = "ALIANZA NESTLE - CONFITERIA"  });
            //list.Add(new Clientes { ruc = "0990032246001", codProveedor = "WN", proveedor = "ALIANZA NESTLE - BEBIDAS UHT"  });
            //list.Add(new Clientes { ruc = "0990032246001", codProveedor = "WG", proveedor = "ALIANZA NESTLE - CAFE Y BEBIDAS EN POLVO"  });
            //list.Add(new Clientes { ruc = "0990032246001", codProveedor = "NJ", proveedor = "ALIANZA NESTLE - CEREALES"  });
            //list.Add(new Clientes { ruc = "0991449485001", codProveedor = "ÑB", proveedor = "BIDELSA - EQUIPOS Y ACCESORIOS MEDICOS"  });
            //list.Add(new Clientes { ruc = "0991449485001", codProveedor = "PI", proveedor = "BIDELSA - PIGEON"  });
            //list.Add(new Clientes { ruc = "0991449485001", codProveedor = "KB", proveedor = "BIDELSA - ALIMENTOS"  });
            //list.Add(new Clientes { ruc = "1790708799001", codProveedor = "NF", proveedor = "GARCOS - NIFA GENERICOS"  });
            //list.Add(new Clientes { ruc = "0992878029001", codProveedor = "CA", proveedor = "CARDY"  });
            //list.Add(new Clientes { ruc = "1791888650001", codProveedor = "53", proveedor = "ETICAL LABORATORIOS"  });
            //list.Add(new Clientes { ruc = "1791314379001", codProveedor = "FO", proveedor = "ALIANZA FAMILIA - FEMPRO"  });
            //list.Add(new Clientes { ruc = "1791314379001", codProveedor = "AY", proveedor = "ALIANZA FAMILIA - PEQUEÑIN"  });
            //list.Add(new Clientes { ruc = "1791314379001", codProveedor = "TE", proveedor = "ALIANZA FAMILIA - INCO"  });
            //list.Add(new Clientes { ruc = "1791314379001", codProveedor = "TS", proveedor = "ALIANZA FAMILIA - TISSUE"  });
            //list.Add(new Clientes { ruc = "0993300527001", codProveedor = "14", proveedor = "LABORATORIOS EL MANA"  });
            //list.Add(new Clientes { ruc = "0990947325001", codProveedor = "SU", proveedor = "SUTTON"  });
            //list.Add(new Clientes { ruc = "1791287118001", codProveedor = "KC", proveedor = "ALIANZA KIMBERLY CLARK - CUID INFANTIL"  });
            //list.Add(new Clientes { ruc = "1791287118001", codProveedor = "MI", proveedor = "ALIANZA KIMBERLY CLARK - CUID FEMENINO"  });
            //list.Add(new Clientes { ruc = "1791287118001", codProveedor = "KD", proveedor = "ALIANZA KIMBERLY CLARK - ADULT CARE"  });
            //list.Add(new Clientes { ruc = "1791287118001", codProveedor = "LR", proveedor = "ALIANZA KIMBERLY CLARK - CUID FAMILIAR"  });
            //list.Add(new Clientes { ruc = "1791287118001", codProveedor = "KI", proveedor = "KIMBERLY CLARK - PROMO"  });
            //list.Add(new Clientes { ruc = "1791302400001", codProveedor = "A6", proveedor = "ALPINA"  });
            //list.Add(new Clientes { ruc = "0993237450001", codProveedor = "LC", proveedor = "DISPACIF"  });
            //list.Add(new Clientes { ruc = "0993221376001", codProveedor = "TV", proveedor = "PHARMEXSA"  });
            //list.Add(new Clientes { ruc = "1793082521001", codProveedor = "0", proveedor = "SANOFI CHC"  });
            //list.Add(new Clientes { ruc = "1791294262001", codProveedor = "TB", proveedor = "B.D.F. DERMATOLOGICO"  });
            //list.Add(new Clientes { ruc = "1791294262001", codProveedor = "BE", proveedor = "B.D.F. MEDICAL"  });
            //list.Add(new Clientes { ruc = "1791294262001", codProveedor = "BN", proveedor = "B.D.F. NIVEA CREMA"  });
            //list.Add(new Clientes { ruc = "1791294262001", codProveedor = "BP", proveedor = "B.D.F. NIVEA CUID-CORPORAL"  });
            //list.Add(new Clientes { ruc = "1791294262001", codProveedor = "ST", proveedor = "B.D.F. NIVEA MISCELANEOS"  });
            //list.Add(new Clientes { ruc = "0992427264001", codProveedor = "TG", proveedor = "TESIA"  });
            //list.Add(new Clientes { ruc = "0991266461001", codProveedor = "IP", proveedor = "INPROFARM"  });
            //list.Add(new Clientes { ruc = "1793107613001", codProveedor = "KL", proveedor = "BIOGENET"  });
            //list.Add(new Clientes { ruc = "1791877365001", codProveedor = "VB", proveedor = "VITABEAUTY INTERNACIONAL"  });
            //list.Add(new Clientes { ruc = "1500050487001", codProveedor = "OI", proveedor = "DURAN ROLDAN - TREBOL"  });
            //list.Add(new Clientes { ruc = "1792459397001", codProveedor = "21", proveedor = "TRIDERMA"  });
            //list.Add(new Clientes { ruc = "1792459397001", codProveedor = "ZY", proveedor = "LIOMONT"  });
            //list.Add(new Clientes { ruc = "1703213593001", codProveedor = "0", proveedor = "CARRASCO RECALDE - LOS OLIVOS"  });
            //list.Add(new Clientes { ruc = "0990546231001", codProveedor = "JW", proveedor = "JOHNSONWAX"  });
            //list.Add(new Clientes { ruc = "0990546231001", codProveedor = "J0", proveedor = "JOHNSONWAX - INSECTIDAS"  });
            //list.Add(new Clientes { ruc = "1792365597001", codProveedor = "CT", proveedor = "IAMECPRODUCTOS"  });
            //list.Add(new Clientes { ruc = "1791332873001", codProveedor = "RK", proveedor = "VIRUMEC"  });
            //list.Add(new Clientes { ruc = "0990917388001", codProveedor = "BB", proveedor = "BABYS - BABYS"  });
            //list.Add(new Clientes { ruc = "0990917388001", codProveedor = "UG", proveedor = "BABYS - DISNEY"  });
            //list.Add(new Clientes { ruc = "0990917388001", codProveedor = "XT", proveedor = "BABYS - NUBY"  });
            //list.Add(new Clientes { ruc = "0991351264001", codProveedor = "DS", proveedor = "DROCARAS"  });
            //list.Add(new Clientes { ruc = "0990135630001", codProveedor = "LQ", proveedor = "CALBAQ"  });
            //list.Add(new Clientes { ruc = "0990135630001", codProveedor = "VA", proveedor = "CALBAQ - MENTOS"  });
            //list.Add(new Clientes { ruc = "0990135630001", codProveedor = "-2", proveedor = "CALBAQ - CAROZZI"  });
            //list.Add(new Clientes { ruc = "0990135630001", codProveedor = "WO", proveedor = "CALBAQ - WRIGLEY-ORBIT"  });
            //list.Add(new Clientes { ruc = "0990333319001", codProveedor = "GC", proveedor = "ROCNARF GENERICO"  });
            //list.Add(new Clientes { ruc = "0914562764001", codProveedor = "HM", proveedor = "MONTALVAN CAMPOVERDE - MASON NATURAL"  });
            //list.Add(new Clientes { ruc = "1791297385001", codProveedor = "H7", proveedor = "ALIANZA ZAIMELLA - CUID INFANTIL"  });
            //list.Add(new Clientes { ruc = "1791297385001", codProveedor = "TX", proveedor = "ALIANZA ZAIMELLA - CUID ADULTO"  });
            //list.Add(new Clientes { ruc = "1791297385001", codProveedor = "NK", proveedor = "ALIANZA ZAIMELLA - CUID INFANTIL COSMETICA"  });
            //list.Add(new Clientes { ruc = "1791297385001", codProveedor = "H8", proveedor = "ALIANZA ZAIMELLA - CUID FEMENINO"  });
            //list.Add(new Clientes { ruc = "1791297385001", codProveedor = "ÏV", proveedor = "ALIANZA ZAIMELLA - INSUMOS MEDICOS"  });
            //list.Add(new Clientes { ruc = "1791297385001", codProveedor = "DM", proveedor = "ALIANZA ZAIMELLA - MASCOTAS"  });
            //list.Add(new Clientes { ruc = "1791251237001", codProveedor = "PT", proveedor = "CLARO"  });
            //list.Add(new Clientes { ruc = "1790378489001", codProveedor = "BL", proveedor = "ALIANZA BLENASTOR"  });
            //list.Add(new Clientes { ruc = "0993305847001", codProveedor = "X1", proveedor = "ROMAC TRADING"  });
            //list.Add(new Clientes { ruc = "1790021130001", codProveedor = "17", proveedor = "PONTE SELVA S.A."  });
            //list.Add(new Clientes { ruc = "0990036152001", codProveedor = "ES", proveedor = "BAYER RADIOLOGIA"  });
            //list.Add(new Clientes { ruc = "0990036152001", codProveedor = "B3", proveedor = "BAYER INSUMOS MEDICOS"  });
            //list.Add(new Clientes { ruc = "0990036152001", codProveedor = "BU", proveedor = "BAYER CONSUMER CARE"  });
            //list.Add(new Clientes { ruc = "0990036152001", codProveedor = "BY", proveedor = "ALIANZA BAYER FARMA"  });
            //list.Add(new Clientes { ruc = "1891736270001", codProveedor = "NA", proveedor = "NEO-FARMACO"  });
            //list.Add(new Clientes { ruc = "1790085503001", codProveedor = "LA", proveedor = "LAMOSAN"  });
            //list.Add(new Clientes { ruc = "1791222032001", codProveedor = "MB", proveedor = "BRAUN MEDICAL"  });
            //list.Add(new Clientes { ruc = "0991222235001", codProveedor = "TU", proveedor = "TULIPANESA"  });
            //list.Add(new Clientes { ruc = "0992875968001", codProveedor = "MC", proveedor = "LABOMEDICA"  });
            //list.Add(new Clientes { ruc = "0909171043001", codProveedor = "IE", proveedor = "TECNOSONRISA"  });
            //list.Add(new Clientes { ruc = "1791343247001", codProveedor = "DU", proveedor = "DOUS - DOUS IMPORT"  });
            //list.Add(new Clientes { ruc = "1791343247001", codProveedor = "PG", proveedor = "DOUS - ST.IVES LABORAT"  });
            //list.Add(new Clientes { ruc = "1790750892001", codProveedor = "HK", proveedor = "HOSPIMEDIKKA"  });
            //list.Add(new Clientes { ruc = "1791362160001", codProveedor = "PB", proveedor = "ALIANZA PHARMABRAND FARMA"  });
            //list.Add(new Clientes { ruc = "1791362160001", codProveedor = "PM", proveedor = "ALIANZA PHARMABRAND OTC"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "Ñ1", proveedor = "BONAPHARM DISTRIBUCION EXCLUSIVA"  });
            //list.Add(new Clientes { ruc = "0990018855001", codProveedor = "NW", proveedor = "NEW YORKER"  });
            //list.Add(new Clientes { ruc = "0990008426001", codProveedor = "0", proveedor = "BOTICAS UNIDAS"  });
            //list.Add(new Clientes { ruc = "0390011024001", codProveedor = "WP", proveedor = "SAN ANTONIO"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "Z5", proveedor = "QUIFATEX DIST MEDICAMENTA"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "QL", proveedor = "QUIFATEX REPR CONSUMO PROTEC & GAMBLE"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "QK", proveedor = "QUIFATEX REPR ISISPHARMA"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "OD", proveedor = "QUIFATEX REPR LA ROCHE POSAY"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "UH", proveedor = "QUIFATEX PROP GOLOSINAS POS"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "BR", proveedor = "QUIFATEX PROP CONSUMO"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "ÂE", proveedor = "QUIFATEX REPR CONSUMO SUMMERS EVE"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "GÓ", proveedor = "QUIFATEX REPR CERAVE"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "IÂ", proveedor = "QUIFATEX DIST BAGO"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "AG", proveedor = "QUIFATEX REPR FARMA ALLERGAN"  });
            //list.Add(new Clientes { ruc = "1790371506001", codProveedor = "GA", proveedor = "QUIFATEX REPR GALDERMA"  });
            //list.Add(new Clientes { ruc = "1792405947001", codProveedor = "FV", proveedor = "IMPEXSUR"  });
            //list.Add(new Clientes { ruc = "0992216972001", codProveedor = "GT", proveedor = "GALIAFARM"  });
            //list.Add(new Clientes { ruc = "0993097527001", codProveedor = "1E", proveedor = "LABORATORIOS VITAE"  });
            //list.Add(new Clientes { ruc = "0991312080001", codProveedor = "SD", proveedor = "FRESENIUS KABI HOSPITALARIO"  });
            //list.Add(new Clientes { ruc = "0991312080001", codProveedor = "M7", proveedor = "FRESENIUS KABI NUTRICIONAL"  });
            //list.Add(new Clientes { ruc = "0992966866001", codProveedor = "CI", proveedor = "LICCOP"  });
            //list.Add(new Clientes { ruc = "1792752795001", codProveedor = "DF", proveedor = "SANAVIT"  });
            //list.Add(new Clientes { ruc = "0992505133001", codProveedor = "G1", proveedor = "GALENO"  });
            //list.Add(new Clientes { ruc = "1791897498001", codProveedor = "S1", proveedor = "LETERAGO - SIEGFRIED - SIEGFRIED"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "RM", proveedor = "LETERAGO - MEGALABS"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "WX", proveedor = "LETERAGO - GRUPO FARMA"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "S1", proveedor = "LETERAGO - SIEGFRIED - SIEGFRIED"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "AC", proveedor = "LETERAGO - ACROMAX"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "WZ", proveedor = "LETERAGO - SANFER"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "HP", proveedor = "LETERAGO - ETHICAL NUTRITION"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "RC", proveedor = "LETERAGO - FAES FARMA"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "ML", proveedor = "LETERAGO - PHARMEDICAL GILBERT"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "8B", proveedor = "LETERAGO - UNIPHARM"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "XC", proveedor = "LETERAGO - PHARMEDICAL ORDESA"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "RW", proveedor = "LETERAGO - TERRY"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "IT", proveedor = "LETERAGO - MEGALABS ESPECIALIDAD"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "BK", proveedor = "LETERAGO - SANULAC NUTRICION"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "LJ", proveedor = "LETERAGO - SIEGFRIED - GRAMON MILLET"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "DP", proveedor = "LETERAGO - SIEGFRIED - INTERPHARM"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "ÑN", proveedor = "LETERAGO - EUROFARMA"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "W5", proveedor = "LETERAGO - RODDOME"  });
            //list.Add(new Clientes { ruc = "0992262192001", codProveedor = "AÑ", proveedor = "LETERAGO - GENFAR"  });
            //list.Add(new Clientes { ruc = "1792168740001", codProveedor = "M ", proveedor = "MEAD JOHNSON - ENFAGROW"  });
            //list.Add(new Clientes { ruc = "1792168740001", codProveedor = "NB", proveedor = "MEAD JOHNSON - NUTRI"  });
            //list.Add(new Clientes { ruc = "1792168740001", codProveedor = "NX", proveedor = "MEAD JOHNSON - RECKITT BENCKISER"  });
            //list.Add(new Clientes { ruc = "1792168740001", codProveedor = "M9", proveedor = "MEAD JOHNSON - SUSTAGEN"  });
            //list.Add(new Clientes { ruc = "1790298345001", codProveedor = "DG", proveedor = "RENE CHARDON"  });
            //list.Add(new Clientes { ruc = "0990371458001", codProveedor = "BJ", proveedor = "BIC ECUADOR (ECUABIC)"  });
            //list.Add(new Clientes { ruc = "0990371458001", codProveedor = "1Z", proveedor = "BIC ESCOLAR"  });
            //list.Add(new Clientes { ruc = "0992878029001", codProveedor = "XX", proveedor = "BEAUTIK"  });
            //list.Add(new Clientes { ruc = "1791253930001", codProveedor = "US", proveedor = "ALIANZA P&G MCH"  });
            //list.Add(new Clientes { ruc = "1791768612001", codProveedor = "FE", proveedor = "CANDLECROSS"  });
            //list.Add(new Clientes { ruc = "0992893028001", codProveedor = "1J", proveedor = "NUTRIMED"  });
            //list.Add(new Clientes { ruc = "1791877969001", codProveedor = "KE", proveedor = "KENDALL LAB"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "SM", proveedor = "LAS FRAGANCIAS - KOLESTON"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "VE", proveedor = "LAS FRAGANCIAS - AVENE"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "GK", proveedor = "LAS FRAGANCIAS - HENKEL"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "1Q", proveedor = "LAS FRAGANCIAS - ORGANIX"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "FL", proveedor = "LAS FRAGANCIAS - JUST FOR MEN"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "FJ", proveedor = "LAS FRAGANCIAS - JOHN FRIEDA"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "ZM", proveedor = "LAS FRAGANCIAS - ZUUM"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "FZ", proveedor = "LAS FRAGANCIAS - NATUR-VIDA"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "DD", proveedor = "LAS FRAGANCIAS - ADIDAS"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "YO", proveedor = "LAS FRAGANCIAS - OXY"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "1O", proveedor = "LAS FRAGANCIAS - URIAGE"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "1X", proveedor = "LAS FRAGANCIAS - GARNIER"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "FC", proveedor = "LAS FRAGANCIAS - (PERF-BASE-ETC)"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "1S", proveedor = "LAS FRAGANCIAS - JELLY BELLY"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "1W", proveedor = "LAS FRAGANCIAS - UMBRO"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "L4", proveedor = "LAS FRAGANCIAS - BYLY"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "FK", proveedor = "LAS FRAGANCIAS - ALMAY"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "1R", proveedor = "LAS FRAGANCIAS - HUMIDEX"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "L6", proveedor = "LAS FRAGANCIAS - LIP ICE"  });
            //list.Add(new Clientes { ruc = "0190111881001", codProveedor = "L7", proveedor = "LAS FRAGANCIAS - MUSTELA"  });
            //list.Add(new Clientes { ruc = "0992628332001", codProveedor = "2X", proveedor = "DAMERSOL"  });
            //list.Add(new Clientes { ruc = "1791354206001", codProveedor = "SJ", proveedor = "SABIJERS"  });
            //list.Add(new Clientes { ruc = "1791256115001", codProveedor = "30", proveedor = "TUENTI - TELEFONICA"  });
            //list.Add(new Clientes { ruc = "1791256115001", codProveedor = "ZZ", proveedor = "MOVISTAR - TELEFONICA"  });
            //list.Add(new Clientes { ruc = "0991410465001", codProveedor = "VD", proveedor = "LABOVIDA"  });
            //list.Add(new Clientes { ruc = "0190507440001", codProveedor = "CQ", proveedor = "COSECHADELSUR"  });
            //list.Add(new Clientes { ruc = "1791356047001", codProveedor = "1K", proveedor = "QUANTUMPHARM"  });
            //list.Add(new Clientes { ruc = "0993265829001", codProveedor = "VL", proveedor = "JUNEFIELDTRADING"  });
            //list.Add(new Clientes { ruc = "1792269814001", codProveedor = "O2", proveedor = "INDUSTRIA DE PLASTICOS SANTOS ORTEGA E HIJOS"  });
            //list.Add(new Clientes { ruc = "1790475689001", codProveedor = "RS", proveedor = "ROCHE DIABETES CARE"  });
            //list.Add(new Clientes { ruc = "1790336352001", codProveedor = "LZ", proveedor = "LIRA S.A."  });
            //list.Add(new Clientes { ruc = "1791310233001", codProveedor = "OE", proveedor = "PRODIMEDA"  });
            //list.Add(new Clientes { ruc = "0992967528001", codProveedor = "EB", proveedor = "EL GUANABANAZO"  });
            //list.Add(new Clientes { ruc = "0991248021001", codProveedor = "EA", proveedor = "LANSEY - RECAMIER"  });
            //list.Add(new Clientes { ruc = "0991248021001", codProveedor = "H2", proveedor = "LANSEY - ACCESORIOS DE BELLEZA"  });
            //list.Add(new Clientes { ruc = "0991229353001", codProveedor = "HH", proveedor = "SUNCHODESA"  });
            //list.Add(new Clientes { ruc = "0992329777001", codProveedor = "DH", proveedor = "DE MUJERES"  });
            //list.Add(new Clientes { ruc = "0992329777001", codProveedor = "RQ", proveedor = "DE MUJERES - ROLDA"  });
            //list.Add(new Clientes { ruc = "0991255087001", codProveedor = "VV", proveedor = "BASICFARM"  });
            //list.Add(new Clientes { ruc = "0993134090001", codProveedor = "AI", proveedor = "MIDASOLUTIONS"  });
            //list.Add(new Clientes { ruc = "1791905490001", codProveedor = "ZN", proveedor = "ZONATRADE - ARKOPHARMA"  });
            //list.Add(new Clientes { ruc = "1791352688001", codProveedor = "QB", proveedor = "QUALA - MISCELANEOS"  });
            //list.Add(new Clientes { ruc = "1791352688001", codProveedor = "D-", proveedor = "QUALA - ALIMENTOS Y GOLOSINAS POS"  });
            //list.Add(new Clientes { ruc = "0992316861001", codProveedor = "SX", proveedor = "PSICOFARMA"  });
            //list.Add(new Clientes { ruc = "0190336298001", codProveedor = "LN", proveedor = "JARQUIFAR"  });
            //list.Add(new Clientes { ruc = "0992414499001", codProveedor = "TM", proveedor = "GENOMMALAB"  });
            //list.Add(new Clientes { ruc = "0992307668001", codProveedor = "HC", proveedor = "PHARMATECNO"  });
            //list.Add(new Clientes { ruc = "0992149930001", codProveedor = "TP", proveedor = "NATURPHARMA"  });
            //list.Add(new Clientes { ruc = "0990000360001", codProveedor = "0", proveedor = "TOFIS"  });
            //list.Add(new Clientes { ruc = "0992330066001", codProveedor = "FA", proveedor = "LA SANTE"  });
            //list.Add(new Clientes { ruc = "1791715772001", codProveedor = "OA", proveedor = "ECONOFARM"  });
            //list.Add(new Clientes { ruc = "1791830105001", codProveedor = "NM", proveedor = "NIPRO"  });
            //list.Add(new Clientes { ruc = "0991434879001", codProveedor = "VU", proveedor = "NATURE`S GARDEN"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "VM", proveedor = "DYVENPRO OTC-CONSUMO 1"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "DL", proveedor = "DYVENPRO OTC-CONSUMO 2"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "2Q", proveedor = "DYVENPRO OTC-CONSUMO 3"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "O0", proveedor = "DYVENPRO OTC-CONSUMO 4"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "DK", proveedor = "DYVENPRO OTC-CONSUMO 5"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "MZ", proveedor = "DYVENPRO OTC MULTIVITAMINICOS"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "Ñ5", proveedor = "DYVENPRO GENERAL"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "QI", proveedor = "DYVENPRO FARMA COMERCIAL 1"  });
            //list.Add(new Clientes { ruc = "0991249095001", codProveedor = "OÑ", proveedor = "DYVENPRO FARMA COMERCIAL 2"  });
            //list.Add(new Clientes { ruc = "1791873742001", codProveedor = "ZX", proveedor = "DIDELSA"  });
            //list.Add(new Clientes { ruc = "1802008456001", codProveedor = "5", proveedor = "INDUPHARMA"  });
            //list.Add(new Clientes { ruc = "1793072844001", codProveedor = "1L", proveedor = "MEDITEX"  });
            //list.Add(new Clientes { ruc = "1790462854001", codProveedor = "8X", proveedor = "JAMES BROWN PHARMA"  });
            //list.Add(new Clientes { ruc = "0993046965001", codProveedor = "FX", proveedor = "URBAN NUTRITION"  });
            //list.Add(new Clientes { ruc = "0992534575001", codProveedor = "OP", proveedor = "OPERFEL"  });
            //list.Add(new Clientes { ruc = "1792004772001", codProveedor = "XV", proveedor = "BALIARDA"  });
            //list.Add(new Clientes { ruc = "0992382015001", codProveedor = "XL", proveedor = "LABMAC"  });
            //list.Add(new Clientes { ruc = "0992675586001", codProveedor = "1M", proveedor = "FARCOVETSA"  });
            //list.Add(new Clientes { ruc = "1792118271001", codProveedor = "ZW", proveedor = "NAOS-SKIN - ECUADERM BIODERMA"  });
            //list.Add(new Clientes { ruc = "0992520310001", codProveedor = "IK", proveedor = "GREEN LIFE"  });
            //list.Add(new Clientes { ruc = "0991449868001", codProveedor = "3C", proveedor = "CEGA"  });
            //list.Add(new Clientes { ruc = "1707753198001", codProveedor = "PN", proveedor = "TORRES HADATHY - PROFOOTCARE"  });
            //list.Add(new Clientes { ruc = "1790032442001", codProveedor = "Q0", proveedor = "SIGMA"  });
            //list.Add(new Clientes { ruc = "1792162742001", codProveedor = "G3", proveedor = "AVALMARTI"  });
            //list.Add(new Clientes { ruc = "0993070300001", codProveedor = "11", proveedor = "DISGAMUSA"  });
            //list.Add(new Clientes { ruc = "1792192870001", codProveedor = "13", proveedor = "ALLIANCEPHARMA TECHNOLOGIES"  });
            //list.Add(new Clientes { ruc = "1792836026001", codProveedor = "18", proveedor = "GUTIS"  });
            //list.Add(new Clientes { ruc = "1792915953001", codProveedor = "1A", proveedor = "GEVBRANDS"  });
            //list.Add(new Clientes { ruc = "0992954019001", codProveedor = "1C", proveedor = "HEALTHYDRINKS"  });
            //list.Add(new Clientes { ruc = "0993055034001", codProveedor = "1H", proveedor = "ISNAVA"  });
            //list.Add(new Clientes { ruc = "1792017475001", codProveedor = "1T", proveedor = "LABORATORIOS INPELQUALITY"  });
            //list.Add(new Clientes { ruc = "1791773373001", codProveedor = "23", proveedor = "FARBIOPHARMA"  });
            //list.Add(new Clientes { ruc = "0992187743001", codProveedor = "26", proveedor = "DISTEL"  });
            //list.Add(new Clientes { ruc = "1792653398001", codProveedor = "2C", proveedor = "P-PAX"  });
            //list.Add(new Clientes { ruc = "0993115347001", codProveedor = "2F", proveedor = "BEAUTYCOMP"  });
            //list.Add(new Clientes { ruc = "0992944773001", codProveedor = "_W", proveedor = "CORPORACION APIX"  });
            //list.Add(new Clientes { ruc = "1792577780001", codProveedor = "2H", proveedor = "DIMPROKEL"  });
            //list.Add(new Clientes { ruc = "1792560446001", codProveedor = "36", proveedor = "EXELTISFARMA"  });
            //list.Add(new Clientes { ruc = "1792465214001", codProveedor = "39", proveedor = "BIOMOLEC"  });
            //list.Add(new Clientes { ruc = "1792816920001", codProveedor = "3S", proveedor = "TECHEALTH"  });
            //list.Add(new Clientes { ruc = "0993139548001", codProveedor = "4R", proveedor = "LACPHAR"  });
            //list.Add(new Clientes { ruc = "1191784355001", codProveedor = "56", proveedor = "BIOLACT-EC"  });
            //list.Add(new Clientes { ruc = "0993198501001", codProveedor = "5A", proveedor = "IDELIFE"  });
            //list.Add(new Clientes { ruc = "1792705959001", codProveedor = "5E", proveedor = "CASARA"  });
            //list.Add(new Clientes { ruc = "1792583438001", codProveedor = "50", proveedor = "DYCOMFAR S.A."  });
            //list.Add(new Clientes { ruc = "0993067334001", codProveedor = "6M", proveedor = "LFGM"  });
            //list.Add(new Clientes { ruc = "1792397294001", codProveedor = "6N", proveedor = "LACFARMA"  });
            //list.Add(new Clientes { ruc = "0190373185001", codProveedor = "6Q", proveedor = "BIONCOMEDICA"  });
            //list.Add(new Clientes { ruc = "191159311742", codProveedor = ";_", proveedor = "INTERVET CENTRAL AMERICA S DE RL"  });
            //list.Add(new Clientes { ruc = "1792319900001", codProveedor = "¬Ü", proveedor = "ESCOLLANOS CIA. LTDA."  });
            //list.Add(new Clientes { ruc = "0993148490001", codProveedor = "$$", proveedor = "PHARMACORE"  });
            //list.Add(new Clientes { ruc = "1792386896001", codProveedor = "¬~", proveedor = "BIOAMIGA CIA. LTDA."  });
            //list.Add(new Clientes { ruc = "0993237450001", codProveedor = "!A", proveedor = "DEEAM PHARMACEUTICAL"  });
            //list.Add(new Clientes { ruc = "0190102068001", codProveedor = "~Ô", proveedor = "LABORATORIOS PARACELSO"  });
            //list.Add(new Clientes { ruc = "1793098134001", codProveedor = "ÄY", proveedor = "KUR PACIFIC"  });
            //list.Add(new Clientes { ruc = "1891805175001", codProveedor = "¬^", proveedor = "GLOBAL PETS"  });
            //list.Add(new Clientes { ruc = "0990382875001", codProveedor = "7M", proveedor = "SPARTAN DEL ECUADOR"  });
            //list.Add(new Clientes { ruc = "1792066921001", codProveedor = "7S", proveedor = "GENECOM"  });
            //list.Add(new Clientes { ruc = "0992889780001", codProveedor = "92", proveedor = "D.H.C. INTERNACIONAL"  });
            //list.Add(new Clientes { ruc = "1791767071001", codProveedor = "96", proveedor = "COSMETICOS E-COS"  });
            //list.Add(new Clientes { ruc = "1793009727001", codProveedor = "7Â", proveedor = "KEIF ORGANICS"  });
            //list.Add(new Clientes { ruc = "0993141046001", codProveedor = "F7", proveedor = "EVOLUCIONECU"  });
            //list.Add(new Clientes { ruc = "0993011533001", codProveedor = "HF", proveedor = "MEDIBRALEN S.A."  });
            //list.Add(new Clientes { ruc = "1791984722001", codProveedor = "HJ", proveedor = "FARMACIAS ECONOMICAS - FARMACIAS MEDICITY"  });
            //list.Add(new Clientes { ruc = "1792192854001", codProveedor = "OK", proveedor = "BIOCEUTICALS S.A."  });
            //list.Add(new Clientes { ruc = "1891706967001", codProveedor = "QT", proveedor = "BIOALIMENTAR"  });
            //list.Add(new Clientes { ruc = "1792510457001", codProveedor = "QQ", proveedor = "BAZFEX"  });
            //list.Add(new Clientes { ruc = "0993124397001", codProveedor = "Û;", proveedor = "JOVENTO"  });
            //list.Add(new Clientes { ruc = "1792934583001", codProveedor = "ÔJ", proveedor = "LEARNER PHARMA"  });
            //list.Add(new Clientes { ruc = "1792982510001", codProveedor = "N5", proveedor = "VALPROPHARMA"  });
            //list.Add(new Clientes { ruc = "0992582588001", codProveedor = "WK", proveedor = "DERMAGE-ECUADOR"  });
            //list.Add(new Clientes { ruc = "1793091202001", codProveedor = "T3", proveedor = "NATUPRODUCTS"  });
            //list.Add(new Clientes { ruc = "1792206391001", codProveedor = "UL", proveedor = "PHYTOPHARMA"  });
            //list.Add(new Clientes { ruc = "1792308089001", codProveedor = "VZ", proveedor = "GEMATRIAECUADOR"  });
            //list.Add(new Clientes { ruc = "1791994531001", codProveedor = "PW", proveedor = "PANIJU"  });
            //list.Add(new Clientes { ruc = "1791994531001", codProveedor = "ZK", proveedor = "PANIJU - LENTES"  });
            //list.Add(new Clientes { ruc = "0990335028001", codProveedor = "¿F", proveedor = "MONDELEZ ECUADOR C. LTDA."  });
            //list.Add(new Clientes { ruc = "0991517545001", codProveedor = "P1", proveedor = "LABORATORIOS PEK"  });
            //list.Add(new Clientes { ruc = "0991417729001", codProveedor = "GO", proveedor = "ECOBEL"  });
            //list.Add(new Clientes { ruc = "0992636874001", codProveedor = "WI", proveedor = "WEIR"  });
            //list.Add(new Clientes { ruc = "1791411099001", codProveedor = "EZ", proveedor = "ARCA ECUADOR"  });
            //list.Add(new Clientes { ruc = "1791821505001", codProveedor = "K6", proveedor = "TARSIS"  });
            //list.Add(new Clientes { ruc = "1791253531001", codProveedor = "1F", proveedor = "BAXTER ECUADOR"  });
            //list.Add(new Clientes { ruc = "0990324379001", codProveedor = "P3", proveedor = "PRODUCTOS CRIS"  });
            //list.Add(new Clientes { ruc = "0992977159001", codProveedor = "_6", proveedor = "GLUCOPRO - MEDICINAS AL INSTANTE"  });
            //list.Add(new Clientes { ruc = "0993015350001", codProveedor = "XM", proveedor = "PARMENTIER"  });
            //list.Add(new Clientes { ruc = "1391839111001", codProveedor = "K2", proveedor = "LABORATORIOS SJPE"  });
            //list.Add(new Clientes { ruc = "1792196795001", codProveedor = "CV", proveedor = "BACTOBIOLOGY"  });
            //list.Add(new Clientes { ruc = "1792379830001", codProveedor = "N1", proveedor = "ACQUASPLENDOR"  });
            //list.Add(new Clientes { ruc = "1792283337001", codProveedor = "XH", proveedor = "MODACARBAN CIA. LTDA."  });
            //list.Add(new Clientes { ruc = "0991265759001", codProveedor = "GH", proveedor = "COSMETICORP"  });
            //list.Add(new Clientes { ruc = "0992732032001", codProveedor = "XF", proveedor = "DJANGO"  });
            //list.Add(new Clientes { ruc = "1791729587001", codProveedor = "G4", proveedor = "ANTURIOS"  });
            //list.Add(new Clientes { ruc = "1790322831001", codProveedor = "B6", proveedor = "BEBEMUNDO"  });
            //list.Add(new Clientes { ruc = "0990789061001", codProveedor = "TO", proveedor = "DIPOR - TONI"  });
            //list.Add(new Clientes { ruc = "0990789061001", codProveedor = "DÑ", proveedor = "DIPOR - ALIMENTOS Y GOLOSINAS POS"  });
            //list.Add(new Clientes { ruc = "0990789061001", codProveedor = "GL", proveedor = "DIPOR - TOPSY"  });
            //list.Add(new Clientes { ruc = "0993076279001", codProveedor = "0M", proveedor = "YOURPETS"  });
            //list.Add(new Clientes { ruc = "0992643579001", codProveedor = "OF", proveedor = "OROFARM"  });
            //list.Add(new Clientes { ruc = "1792167140001", codProveedor = "G2", proveedor = "GELCAPS"  });
            //list.Add(new Clientes { ruc = "0992914718001", codProveedor = "MX", proveedor = "IMPLAPETSA S.A."  });
            //list.Add(new Clientes { ruc = "0991413839001", codProveedor = "D7", proveedor = "DANIVET"  });
            //list.Add(new Clientes { ruc = "1791409167001", codProveedor = "P4", proveedor = "PASCOE LABORATORIO"  });
            //list.Add(new Clientes { ruc = "1790976343001", codProveedor = "D8", proveedor = "DIBEAL"  });
            //list.Add(new Clientes { ruc = "1790976343001", codProveedor = "OX", proveedor = "DIBEAL PETCARE"  });
            //list.Add(new Clientes { ruc = "1791999673001", codProveedor = "EL", proveedor = "EUROSTAGA"  });
            //list.Add(new Clientes { ruc = "1792056268001", codProveedor = "FW", proveedor = "GEDEONRICHTER"  });
            //list.Add(new Clientes { ruc = "1792324254001", codProveedor = "IW", proveedor = "ALEXXIAPHARMA"  });
            //list.Add(new Clientes { ruc = "1792201160001", codProveedor = "QD", proveedor = "ASERTIA COMERCIAL"  });
            //list.Add(new Clientes { ruc = "1790084604001", codProveedor = "T6", proveedor = "CONFITECA"  });
            //list.Add(new Clientes { ruc = "1792480280001", codProveedor = "78", proveedor = "CORPORACION MENBER"  });
            //list.Add(new Clientes { ruc = "1790205401001", codProveedor = "F3", proveedor = "FRITO LAY (SNACKS)"  });
            //list.Add(new Clientes { ruc = "1791269489001", codProveedor = "M5", proveedor = "ADITMAQ CIA. LTDA."  });
            //list.Add(new Clientes { ruc = "0990006792001", codProveedor = "T5", proveedor = "BIMBO"  });
            //list.Add(new Clientes { ruc = "1792108411001", codProveedor = "E2", proveedor = "EL KAFETAL"  });
            //list.Add(new Clientes { ruc = "1791769112001", codProveedor = "DO", proveedor = "ZAPHIR TRADE"  });
            //list.Add(new Clientes { ruc = "1790892875001", codProveedor = "HD", proveedor = "LOS COQUEIROS"  });
            //list.Add(new Clientes { ruc = "1790039269001", codProveedor = "I2", proveedor = "INGESA"  });
            //list.Add(new Clientes { ruc = "0992719761001", codProveedor = "V5", proveedor = "MEDRIGAL"  });
            //list.Add(new Clientes { ruc = "1792242460001", codProveedor = "PZ", proveedor = "DINHAR TRADING"  });
            //list.Add(new Clientes { ruc = "1792339952001", codProveedor = "Z2", proveedor = "BERKANA - BERKANA"  });
            //list.Add(new Clientes { ruc = "0990006776001", codProveedor = "I1", proveedor = "INALECSA"  });
            //list.Add(new Clientes { ruc = "0992502851001", codProveedor = "UF", proveedor = "ITALCOSMETIC"  });
            //list.Add(new Clientes { ruc = "1791881915001", codProveedor = "VH", proveedor = "LABVITALIS"  });
            //list.Add(new Clientes { ruc = "0991288449001", codProveedor = "R6", proveedor = "RESGASA"  });
            //list.Add(new Clientes { ruc = "1791888146001", codProveedor = "QJ", proveedor = "CORPORACION MAGMA ECUADOR S.A."  });
            //list.Add(new Clientes { ruc = "1790160793001", codProveedor = "YR", proveedor = "JABONERIA WILSON"  });
            //list.Add(new Clientes { ruc = "0992148772001", codProveedor = "UY", proveedor = "EDUARDY"  });
            //list.Add(new Clientes { ruc = "1792386411001", codProveedor = "UR", proveedor = "JASPHARM GENERICO"  });
            //list.Add(new Clientes { ruc = "1790693031001", codProveedor = "4F", proveedor = "LABORATORIOS LATURI"  });
            //list.Add(new Clientes { ruc = "1791823842001", codProveedor = "R5", proveedor = "RENASE"  });
            //list.Add(new Clientes { ruc = "0991442030001", codProveedor = "S9", proveedor = "SERES LABORATORIO"  });
            //list.Add(new Clientes { ruc = "0190360792001", codProveedor = "LÑ", proveedor = "LVXO DEL ECUADOR"  });
            //list.Add(new Clientes { ruc = "1291710359001", codProveedor = "OV", proveedor = "ORIENTAL"  });
            //list.Add(new Clientes { ruc = "1710683044001", codProveedor = "GÑ", proveedor = "MALDONADO ALAVA - DULCE GOTA"  });
            //list.Add(new Clientes { ruc = "1792083354001", codProveedor = "1Ñ", proveedor = "PROTISA"  });
            //list.Add(new Clientes { ruc = "0990178364001", codProveedor = "PV", proveedor = "PROVENCO"  });
            //list.Add(new Clientes { ruc = "0990987874001", codProveedor = "BG", proveedor = "BASSA COSMETICOS"  });
            //list.Add(new Clientes { ruc = "0990987874001", codProveedor = "BS", proveedor = "BASSA FARMA"  });
            //list.Add(new Clientes { ruc = "0103673463001", codProveedor = "IQ", proveedor = "ASIEL PHARMA"  });
            //list.Add(new Clientes { ruc = "0992113383001", codProveedor = "VJ", proveedor = "HEALTH AND GLOBAL"  });
            //list.Add(new Clientes { ruc = "1792122511001", codProveedor = "K9", proveedor = "ECOPACIFIC"  });
            //list.Add(new Clientes { ruc = "0909527517001", codProveedor = "O3", proveedor = "HOLGUIN MACIAS - NIKKA"  });
            //list.Add(new Clientes { ruc = "1390012949001", codProveedor = "Ú0", proveedor = "LA FABRIL - OTELO"  });
            //list.Add(new Clientes { ruc = "1390012949001", codProveedor = "94", proveedor = "LA FABRIL"  });
            //list.Add(new Clientes { ruc = "0990900388001", codProveedor = "U2", proveedor = "HANSACOM"  });
            //list.Add(new Clientes { ruc = "1791713494001", codProveedor = "UD", proveedor = "UNILIMPIO"  });
            //list.Add(new Clientes { ruc = "0992532629001", codProveedor = "W6", proveedor = "PHARMEDIC"  });
            //list.Add(new Clientes { ruc = "1791993020001", codProveedor = "XÑ", proveedor = "TERRAFERTIL S.A."  });
            //list.Add(new Clientes { ruc = "1792453712001", codProveedor = "Ó7", proveedor = "ASPEN FARMA"  });
            //list.Add(new Clientes { ruc = "1792453712001", codProveedor = "ÁÉ", proveedor = "ASPEN CONSUMER"  });
            //list.Add(new Clientes { ruc = "1790005739001", codProveedor = "Y8", proveedor = "TESALIA"  });
            //list.Add(new Clientes { ruc = "1790005739001", codProveedor = "GI", proveedor = "TESALIA - LA UNIVERSAL"  });
            //list.Add(new Clientes { ruc = "1792294770001", codProveedor = "Z8", proveedor = "BOSTON"  });
            //list.Add(new Clientes { ruc = "1792357039001", codProveedor = "ZB", proveedor = "ANDESSPIRULINA"  });
            //list.Add(new Clientes { ruc = "0992824387001", codProveedor = "ZG", proveedor = "JEACUAN"  });
            //list.Add(new Clientes { ruc = "0992744472001", codProveedor = "8W", proveedor = "MACRONEGOCIOS"  });
            //list.Add(new Clientes { ruc = "0922638754001", codProveedor = "8H", proveedor = "VIVERO PULLAS - RHK"  });
            //list.Add(new Clientes { ruc = "1792569397001", codProveedor = "8J", proveedor = "ALIANZA PFIZER PFE UPJOHN"  });
            //list.Add(new Clientes { ruc = "1792569397001", codProveedor = "@Q", proveedor = "ALIANZA PFIZER PFE BIOPHARMACEUTICALS"  });
            //list.Add(new Clientes { ruc = "1792569397001", codProveedor = "@Q", proveedor = "ALIANZA PFIZER PFE BIOPHARMACEUTICALS"  });
            //list.Add(new Clientes { ruc = "0990006687001", codProveedor = "QG", proveedor = "AGRIPAC"  });
            //list.Add(new Clientes { ruc = "0190409015001", codProveedor = "2W", proveedor = "IMPORTADORA COMERCIAL AMOROSO IMPAMOROSO"  });
            //list.Add(new Clientes { ruc = "1791268776001", codProveedor = "8Z", proveedor = "CORSUPERIOR"  });
            //list.Add(new Clientes { ruc = "1390050352001", codProveedor = "90", proveedor = "CONSERVAS ISABEL"  });
            //list.Add(new Clientes { ruc = "0990331928001", codProveedor = "AM", proveedor = "L. HENRIQUES"  });
            //list.Add(new Clientes { ruc = "0992739584001", codProveedor = "X2", proveedor = "FARMANACION"  });
            //list.Add(new Clientes { ruc = "1790775941001", codProveedor = "0X", proveedor = "MEDICAMENTA ESPECIALIDAD"  });
            //list.Add(new Clientes { ruc = "1792263824001", codProveedor = "ÑD", proveedor = "MARCORPHARMA"  });
            //list.Add(new Clientes { ruc = "0992260734001", codProveedor = "ÑE", proveedor = "DEFAN"  });
            //list.Add(new Clientes { ruc = "0990962545001", codProveedor = "ÑL", proveedor = "BARCELONA"  });
            //list.Add(new Clientes { ruc = "0991456120001", codProveedor = "ÑM", proveedor = "PAYPIER"  });
            //list.Add(new Clientes { ruc = "0992954817001", codProveedor = "9M", proveedor = "ARAPHARMA"  });
            //list.Add(new Clientes { ruc = "1792654424001", codProveedor = "9O", proveedor = "MAXIDUCTS"  });
            //list.Add(new Clientes { ruc = "1792623952001", codProveedor = "ÑQ", proveedor = "SOPHIA"  });
            //list.Add(new Clientes { ruc = "1792050901001", codProveedor = "ÑR", proveedor = "FARMTRADING"  });
            //list.Add(new Clientes { ruc = "0992555742001", codProveedor = "9T", proveedor = "PORTUGAL - GENERICO"  });
            //list.Add(new Clientes { ruc = "0992711574001", codProveedor = "ÑW", proveedor = "PHARMAX S.A."  });
            //list.Add(new Clientes { ruc = "1792488761001", codProveedor = "9W", proveedor = "FARMABION"  });
            //list.Add(new Clientes { ruc = "0992874260001", codProveedor = "Ñ8", proveedor = "MULTICOMPRAS"  });
            //list.Add(new Clientes { ruc = "0992852283001", codProveedor = "6-", proveedor = "PAKYSPRO"  });
            //list.Add(new Clientes { ruc = "1792385202001", codProveedor = "JX", proveedor = "OXIALFARM FARMA"  });
            //list.Add(new Clientes { ruc = "1792385202001", codProveedor = "JX", proveedor = "OXIALFARM ESPECIALIDAD"  });
            //list.Add(new Clientes { ruc = "0992916133001", codProveedor = "-V", proveedor = "SMART REMEDY"  });
            //list.Add(new Clientes { ruc = "1800401166001", codProveedor = "-Y", proveedor = "BUENAÑO LLERENA - TEXTIDOR"  });
            //list.Add(new Clientes { ruc = "1792021081001", codProveedor = "K-", proveedor = "VITADOR S.A."  });
            //list.Add(new Clientes { ruc = "1790027864001", codProveedor = "4S", proveedor = "LEVAPAN DEL ECUADOR"  });
            //list.Add(new Clientes { ruc = "0992176989001", codProveedor = "1B", proveedor = "ALIANZA AJECUADOR"  });
            //list.Add(new Clientes { ruc = "1713710570001", codProveedor = "0", proveedor = "ALVAREZ ROSERO - EQLOGIKAM"  });
            //list.Add(new Clientes { ruc = "1792715652001", codProveedor = "_A", proveedor = "ACINO PHARMA"  });
            //list.Add(new Clientes { ruc = "1791241355001", codProveedor = "_F", proveedor = "V.R. INDUSTRIA NATURISTA"  });
            //list.Add(new Clientes { ruc = "1792344565001", codProveedor = "_P", proveedor = "BONILLA ALARCON DISTRIBUCIONES"  });
            //list.Add(new Clientes { ruc = "0992859016001", codProveedor = "_R", proveedor = "GUM ECUADOR S.A"  });
            //list.Add(new Clientes { ruc = "1792578728001", codProveedor = "XO", proveedor = "READY"  });
            //list.Add(new Clientes { ruc = "1791870417001", codProveedor = "SL", proveedor = "I.T.C. ECUADOR S.A"  });
            //list.Add(new Clientes { ruc = "1101745576001", codProveedor = "_V", proveedor = "CASTILLO CARRION - GINAS"  });
            //list.Add(new Clientes { ruc = "1711738979001", codProveedor = "NN", proveedor = "METROPOLITANA TRADE"  });
            //list.Add(new Clientes { ruc = "0992541059001", codProveedor = "U5", proveedor = "CACIE DISTRIBUIDORA"  });
            //list.Add(new Clientes { ruc = "0991260730001", codProveedor = "G7", proveedor = "IMPORFARMA"  });

            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultContext"].ConnectionString))
            {
                cnn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clientes WHERE ACTIVO = 1", cnn);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Clientes
                    {
                        Id = int.Parse(dr["Id"].ToString()),
                        ruc = dr["ruc"].ToString(),
                        proveedor = dr["proveedor"].ToString(),
                        codProveedor = dr["codProveedor"].ToString(),
                        Activo = true
                    });
                }
            }

            return list;
        }
    }
}
