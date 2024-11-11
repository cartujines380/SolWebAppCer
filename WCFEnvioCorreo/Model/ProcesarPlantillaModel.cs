using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WCFEnvioCorreo.Model
{
    public partial class ProcesarPlantillaModel
    {
        [JsonProperty("type_string")]
        public string TypeString { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("localization")]
        public string Localization { get; set; }

        [JsonProperty("variables")]
        public Dictionary<string, string> Variables { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("output_file")]
        public string OutputFile { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("cabecera")]
        public Variables Cabecera { get; set; }

        [JsonProperty("detalle")]
        public List<Variables> Detalle { get; set; }
    }

    public partial class Variables
    {
        [JsonProperty("additionalProp1")]
        public string AdditionalProp1 { get; set; }

        [JsonProperty("additionalProp2")]
        public string AdditionalProp2 { get; set; }

        [JsonProperty("additionalProp3")]
        public string AdditionalProp3 { get; set; }
    }
}
