using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AngularJSAuthentication.API.Custom
{
     
    public class TransaccionUser
    {
        public string TransaccionId { get; set; }
        public string UserId { get; set; }


    }
     
    public class Menu
    {

        [MaxLength(50)]
        public string ApplicationId { get; set; }


        public int Id { get; set; }

        public int Orden { get; set; }

        [MaxLength(50)]
        public string CodTransaccion { get; set; }

        [MaxLength(200)]
        public string Descripcion { get; set; }

        [MaxLength(200)]
        public string Contenido { get; set; }

        [MaxLength(500)]
        public string Url { get; set; }

        public virtual  List<Submenu>  Submenues{ get; set; }

    }
     
    public class Submenu    
    {
        [MaxLength(50)]
        public string ApplicationId { get; set; }

        public int Id { get; set; }

        public int Orden { get; set; }

        [MaxLength(50)]
        public string CodTransaccion { get; set; }

        [MaxLength(200)]
        public string Descripcion { get; set; }

        [MaxLength(200)]
        public string Contenido { get; set; }

        [MaxLength(500)]
        public string Url { get; set; }

        public int MenuId { get; set; }
         

        public virtual List<Item> Items { get; set; }

    }

    public class Item   
    {
        [MaxLength(50)]
        public string ApplicationId { get; set; }

        public int Id { get; set; }

        public int Orden { get; set; }

        [MaxLength(50)]
        public string CodTransaccion { get; set; }

        [MaxLength(200)]
        public string Descripcion { get; set; }

        [MaxLength(200)]
        public string Contenido { get; set; }

        [MaxLength(500)]
        public string Url { get; set; }

        public int SubMenuId { get; set; }
         


    }


    //public class MenuSer
    //{

    //    [MaxLength(50)]
    //    public string ApplicationId { get; set; }


    //    public int Id { get; set; }

    //    [MaxLength(50)]
    //    public string CodTransaccion { get; set; }

    //    [MaxLength(200)]
    //    public string Descripcion { get; set; }

    //    [MaxLength(200)]
    //    public string Contenido { get; set; }

    //    [MaxLength(500)]
    //    public string Url { get; set; }

    //    public virtual List<SubmenuSer> Submenues { get; set; }

    //}

    //[Serializable]
    //[DataContract(IsReference = true)]
    //public class SubmenuSer
    //{
    //    [MaxLength(50)]
    //    public string ApplicationId { get; set; }

    //    public int Id { get; set; }


    //    [MaxLength(50)]
    //    public string CodTransaccion { get; set; }

    //    [MaxLength(200)]
    //    public string Descripcion { get; set; }

    //    [MaxLength(200)]
    //    public string Contenido { get; set; }

    //    [MaxLength(500)]
    //    public string Url { get; set; }

    //    public int MenuId { get; set; }

    //    public virtual Menu Menu { get; set; }

    //    public virtual List<ItemSer> Items { get; set; }

    //}

    //public class ItemSer
    //{
    //    [MaxLength(50)]
    //    public string ApplicationId { get; set; }

    //    public int Id { get; set; }

    //    [MaxLength(50)]
    //    public string CodTransaccion { get; set; }

    //    [MaxLength(200)]
    //    public string Descripcion { get; set; }

    //    [MaxLength(200)]
    //    public string Contenido { get; set; }

    //    [MaxLength(500)]
    //    public string Url { get; set; }

    //    public int SubMenuId { get; set; }
         

    //}

}