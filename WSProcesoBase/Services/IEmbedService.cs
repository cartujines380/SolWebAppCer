using PowerBIEmbedded_AppOwnsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PowerBIEmbedded_AppOwnsData.Services
{
    public interface IEmbedService
    {
        EmbedConfig EmbedConfig { get; }
        TileEmbedConfig TileEmbedConfig { get; }

        bool EmbedReport(string userName, string roles);


    }
}
