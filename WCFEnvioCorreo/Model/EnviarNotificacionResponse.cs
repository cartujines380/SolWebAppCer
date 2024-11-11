namespace WCFEnvioCorreo.Model
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class EnviarNotificacionResponse
    {
        [JsonProperty("correos")]
        public List<Correo> Correos { get; set; }
    }

}
