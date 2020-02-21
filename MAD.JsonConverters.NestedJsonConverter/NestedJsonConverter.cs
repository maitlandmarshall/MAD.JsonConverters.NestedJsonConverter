using Humanizer;
using MAD.JsonConverters.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MAD.JsonConverters.NestedJsonConverterNS
{
    public class NestedJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject readerObj = JObject.Load(reader);
            PropertyInfo[] objectTypeProperties = objectType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);

            object result = Activator.CreateInstance(objectType);

            foreach (PropertyInfo p in objectTypeProperties)
            {
                NestedJsonPropertyAttribute nestedJsonPropertyAttribute = p.GetCustomAttribute<NestedJsonPropertyAttribute>();

                if (nestedJsonPropertyAttribute == null)
                {
                    string key = (readerObj as IDictionary<string, JToken>)
                        .Keys
                        .FirstOrDefault(y => y.ToLower() == p.Name.ToLower());

                    // Don't do anything when you can't find a property.
                    if (String.IsNullOrEmpty(key))
                        continue;

                    object value = readerObj[key].ToObject(p.PropertyType);

                    p.SetValue(result, value);
                }
                else
                {
                    string propertyPath = nestedJsonPropertyAttribute.Path;
                    JToken nestedToken = readerObj;

                    foreach (string path in propertyPath.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        nestedToken = nestedToken[path];
                    }

                    object nestedValue = nestedToken.ToObject(p.PropertyType);
                    p.SetValue(result, nestedValue);
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
