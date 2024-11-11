using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace WCFEnvioCorreo.Model
{
    public partial class EnviarNotificacionModel
    {
        [JsonProperty("sms")]
        public List<Sm> Sms { get; set; }

        [JsonProperty("correos")]
        public List<Correo> Correos { get; set; }
    }

    public partial class Correo
    {
        [JsonProperty("adjuntos")]
        public List<Adjunto> Adjuntos { get; set; }

        [JsonProperty("asunto")]
        public string Asunto { get; set; }

        [JsonProperty("remitente")]
        public string Remitente { get; set; }

        [JsonProperty("destinatarios")]
        public List<Destinatario> Destinatarios { get; set; }

        [JsonProperty("cuerpo_correo")]
        public string CuerpoCorreo { get; set; }

        [JsonProperty("Localization")]
        public string Localization { get; set; }
    }

    public partial class Adjunto
    {
        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("contenido")]
        public string Contenido { get; set; }
    }

    public partial class Destinatario
    {
        [JsonProperty("contacto")]
        public string Contacto { get; set; }

        [JsonProperty("fecha_envio")]
        public DateTimeOffset FechaEnvio { get; set; }
    }

    public partial class Sm
    {
        [JsonProperty("opid")]
        public string Opid { get; set; }

        [JsonProperty("origen")]
        public string Origen { get; set; }

        [JsonProperty("numeros")]
        public List<Destinatario> Numeros { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }
    }

}

