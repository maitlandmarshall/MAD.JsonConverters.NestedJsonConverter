using MAD.JsonConverters.NestedJsonConverterNS;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests
{
    public class NestedJsonExample2Model
    {
        public class HeaderModel
        {
            public string Description { get; set; }
            public bool IsActive { get; set; }
        }

        [NestedJsonProperty("details.header")]
        public HeaderModel Header { get; set; }
    }
}
