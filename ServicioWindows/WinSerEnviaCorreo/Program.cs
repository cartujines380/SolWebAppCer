using FE.ServicioSendCorreo;
using System.ServiceProcess;

namespace clibAccesoDatos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new servicioCorreo() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
