using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests.Models
{
    public class DynamicResponseFromSomeShittyApiModel <TEntity>
    {
        public int ResponseCode { get; set; }
        
        [JsonProperty("*")]
        public TEntity Result { get; set; }

        public bool ILikeClams { get; set; }
    }
}
