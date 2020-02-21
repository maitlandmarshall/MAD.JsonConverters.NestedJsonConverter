using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests
{
    public class NestedJsonExample1Model
    {
        [NestedJsonProperty("header.description")]
        public string Description { get; set; }

        [NestedJsonProperty("header.isActive")]
        public bool IsActive { get; set; }

        public List<string> Items { get; set; }
    }
}
