
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.IO;

namespace clibLogger
{
    public class clsLogger
    {
        
        public clsLogger()
        {
        }
        public void Graba_Log_Warn(string PI_Texto)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["LogInfo"].Equals("S"))
                    Graba_Log(PI_Texto, "WARN");
            }
            catch (Exception ex)
            {
            }
        }

        public void Graba_Log_Fatal(string PI_Texto)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["LogError"].Equals("S"))
                    Graba_Log(PI_Texto, "FATAL");
            }
            catch (Exception ex)
            {
            }
        }

        public void Graba_Log_Debug(string PI_Texto, string PI_Identificador = "0")
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["LogInfo"].Equals("S"))
                    Graba_Log(PI_Texto, "DEBUG");
            }
            catch (Exception ex)
            {
            }
        }

        public void Graba_Log_Info(string PI_Texto)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["LogInfo"].Equals("S"))
                    Graba_Log(PI_Texto, "INFO");
            }
            catch (Exception ex)
            {
            }
        }

        public void Graba_Log_Info(string PI_Texto, string PI_Identificador = "0")
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["LogInfo"].Equals("S"))
                    Graba_Log(PI_Texto, "INFO");
            }
            catch (Exception ex)
            {
            }
        }

        public void Graba_Log_Error(string PI_Texto, string PI_Identificador = "0")
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["LogError"].Equals("S"))
                    Graba_Log(PI_Texto, "ERROR");
            }
            catch (Exception ex)
            {
            }
        }

        public void Graba_Log(string Datos, string Tipo)
        {
            StreamWriter sw = default(StreamWriter);
            System.IO.DirectoryInfo dir;
            string NombreArchivo = null;
            string Archivo = null;
            try
            {

                if (System.Configuration.ConfigurationManager.AppSettings["Auditar"].Equals("S"))
                {
                    //Pregunto si el Directorio Existe

                    NombreArchivo = System.Configuration.ConfigurationManager.AppSettings["ArchivoLog"];
                    NombreArchivo = NombreArchivo.Replace("|dd", DateTime.Now.ToString("dd"));
                    NombreArchivo = NombreArchivo.Replace("|MM", DateTime.Now.ToString("MM"));
                    NombreArchivo = NombreArchivo.Replace("|yyyy", DateTime.Now.ToString("yyyy"));
                    NombreArchivo = NombreArchivo.Replace("|HH", DateTime.Now.ToString("HH"));

                    dir = new DirectoryInfo(System.IO.Path.GetDirectoryName(NombreArchivo));
                    Archivo = System.IO.Path.Combine(dir.FullName, NombreArchivo);

                    if (!(dir.Exists))
                    {
                        dir.Create();
                    }
                    //Trace.Listeners.Add(New TextWriterTraceListener(Archivo))
                    //Trace.WriteLine(Datos)
                    //Trace.Flush()
                    //Trace.Close()


                    FileStream objStream = new FileStream(Archivo, FileMode.Append, FileAccess.Write);
                    TextWriterTraceListener objTraceListener = new TextWriterTraceListener(objStream);
                    Trace.Listeners.Add(objTraceListener);
                    Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:fff") + " " + Tipo + " " + Datos.ToString());

                    Trace.Flush();
                    Trace.Close();

                    objStream.Close();

                }
            }
            catch (Exception ex)
            {
                //GrabaEventLog("clsError", "Graba_log", -1000, ex.Message);
            }
        }
    }
}
