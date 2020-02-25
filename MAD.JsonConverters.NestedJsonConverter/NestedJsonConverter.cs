using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            IDictionary<string, string> mappedJsonKeysToClassKeys = new Dictionary<string, string>();
            List<string> mappedProperties = new List<string>();

            foreach (PropertyInfo p in objectTypeProperties)
            {
                JsonPropertyAttribute jsonPropertyAttribute = p.GetCustomAttribute<JsonPropertyAttribute>();

                string jsonMappedPropName = jsonPropertyAttribute?.PropertyName ?? p.Name;

                JToken nestedToken = readerObj;

                foreach (string pathSegment in jsonMappedPropName.Split('.'))
                {
                    string key;

                    if (pathSegment != "*") 
                    {
                        key = (readerObj as IDictionary<string, JToken>)
                            .Keys
                            .FirstOrDefault(y => y.ToLower() == jsonMappedPropName.ToLower());
                    }
                    else
                    {
                        key = (readerObj as IDictionary<string, JToken>)
                             .Keys
                             .FirstOrDefault(y => !mappedProperties.Contains(y));
                    }

                    mappedProperties.Add(key);

                    // Don't do anything when you can't find a property.
                    if (string.IsNullOrEmpty(key))
                    {
                        break;
                    }

                    nestedToken = nestedToken[key];

                    object nestedValue;

                    // If the JsonToken isn't an array, but the C# property is
                    // Load the single object into the C# array
                    if (nestedToken.Type != JTokenType.Array
                     && typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType))
                    {
                        IList collectionPropertyInstance = Activator.CreateInstance(p.PropertyType) as IList;
                        object singlePropertyInstance = nestedToken.ToObject(p.PropertyType.GetGenericArguments().FirstOrDefault());

                        collectionPropertyInstance.Add(singlePropertyInstance);

                        nestedValue = collectionPropertyInstance;
                    } 
                    else
                    {
                        if (nestedToken.Type == JTokenType.Null)
                            continue;
                        else
                            nestedValue = nestedToken.ToObject(p.PropertyType);
                    }

                    
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
