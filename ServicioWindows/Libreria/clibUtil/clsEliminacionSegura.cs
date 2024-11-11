using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace FE.Utileria
{
                  
    public class clsEliminacionSegura
    {
        private string _clase;
        private string _metodo;

        public clsEliminacionSegura()
        {
            _clase = this.GetType().Name;
        }

        public bool EliminarArchivo(string PI_Archivo)
        {
            bool lv_Retorno = true;
            _metodo = MethodBase.GetCurrentMethod().Name;
            try
            {
               string ArchivoSdelete =   System.Configuration.ConfigurationManager.AppSettings["Sdelete"];

               using (Process p = new Process())
               {
                   p.StartInfo.FileName = ArchivoSdelete;
                   p.StartInfo.Arguments = string.Format("-p {0} -q \"{1}\"", 1, PI_Archivo);
                   p.StartInfo.CreateNoWindow = true;
                   p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                   p.StartInfo.UseShellExecute = false;
                   p.StartInfo.RedirectStandardOutput = true;
                   p.Start();
                   string output = p.StandardOutput.ReadToEnd();
                   p.WaitForExit();

                   lv_Retorno = !System.IO.File.Exists(PI_Archivo);
                   
               }

            }
            catch (Exception ex)
            {
                lv_Retorno = false;
            }

            return lv_Retorno;
        }
    }
}
