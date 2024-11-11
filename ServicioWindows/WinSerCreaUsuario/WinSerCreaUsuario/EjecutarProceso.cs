using Quartz;
using System.Threading.Tasks;

namespace WinSerCreaUsuario
{
    public class EjecutarProceso : IJob
    {
        void IJob.Execute(IJobExecutionContext context)
        {
            Execute(context);
        }

        public Task Execute(IJobExecutionContext context)
        {
            return ClsCreaUsuario.EjecutarJob();
        }
    }
}
