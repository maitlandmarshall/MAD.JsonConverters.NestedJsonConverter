using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests.Models
{
    public class MultipleWildcardModel
    {
        [JsonProperty("*")]
        public List<string> Dogs { get; set; }

        [JsonProperty("*")]
        public List<string> Cats { get; set; }
    }
}
