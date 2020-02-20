using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS
{
    internal class FlatDictionaryFactory
    {
        private readonly PathPascalizer pathPascalizer = new PathPascalizer();

        public IDictionary<string, object> Create(JsonReader reader)
        {
            Dictionary<string, object> flattenedJson = new Dictionary<string, object>();

            do
            {
                string path = this.pathPascalizer.PascalizePath(reader.Path);

                switch (reader.TokenType)
                {
                    case JsonToken.Integer:
                    case JsonToken.Float:
                    case JsonToken.String:
                    case JsonToken.Boolean:
                    case JsonToken.Null:
                    case JsonToken.Date:
                    case JsonToken.Bytes:
                        flattenedJson[path] = reader.Value;
                        break;
                }
            }
            while (reader.Read());

            return flattenedJson;
        }
    }
}
