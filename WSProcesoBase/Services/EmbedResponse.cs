using PowerBIEmbedded_AppOwnsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSProcesoBase.Services
{
    public class EmbedResponse
    {
        private EmbedConfig _EmbedConfig;
        private Boolean _Resultado;

        public EmbedConfig EmbedConfig { get => _EmbedConfig; set => _EmbedConfig = value; }
        public bool Resultado { get => _Resultado; set => _Resultado = value; }
    }
}