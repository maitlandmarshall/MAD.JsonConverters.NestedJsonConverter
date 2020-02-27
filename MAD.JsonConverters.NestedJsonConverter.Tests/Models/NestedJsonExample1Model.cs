using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests.Models
{
    public class NestedJsonExample1Model
    {
        [JsonProperty("header.description")]
        public string Description { get; set; }

        [JsonProperty("header.isActive")]
        public bool IsActive { get; set; }

        public List<string> Items { get; set; }
    }
}
