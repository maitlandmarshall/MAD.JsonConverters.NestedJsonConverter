using System;

namespace MAD.JsonConverters.NestedJsonConverterNS
{
    public class NestedJsonPropertyAttribute : Attribute
    {
        public string Path { get; set; }

        public NestedJsonPropertyAttribute(string path)
        {
            this.Path = path;
        }
    }
}
