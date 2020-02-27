using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MAD.JsonConverters.NestedJsonConverter.Tests")]
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

            IEnumerable<PropertyInfo> objectTypeProperties = objectType
                .GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                // OrderBy anything with * so it appears last. Otherwise the wildcard operator will find an already mapped property.
                .OrderBy(y => y.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName == "*");

            object result = Activator.CreateInstance(objectType);

            List<string> mappedProperties = new List<string>();

            foreach (PropertyInfo p in objectTypeProperties)
            {
                JsonPropertyAttribute jsonPropertyAttribute = p.GetCustomAttribute<JsonPropertyAttribute>();

                string jsonMappedPropName = jsonPropertyAttribute?.PropertyName ?? p.Name;

                JToken nestedToken = readerObj;
                string key = String.Empty;

                foreach (string pathSegment in jsonMappedPropName.Split('.'))
                {
                    if (nestedToken is null)
                        break;

                    if (pathSegment != "*")
                    {
                        key = (nestedToken as IDictionary<string, JToken>)
                            .Keys
                            .FirstOrDefault(y => y.ToLower() == pathSegment.ToLower());
                    }
                    else
                    {
                        key = (nestedToken as IDictionary<string, JToken>)
                             .Keys
                             .FirstOrDefault(y => !mappedProperties.Contains(y, StringComparer.OrdinalIgnoreCase));
                    }

                    nestedToken = nestedToken.Value<JToken>(key);
                }

                if (nestedToken is null)
                    break;

                mappedProperties.Add(jsonMappedPropName);

                object nestedValue;

                // If the JsonToken isn't an array, but the C# property is
                // Load the single object into the C# array
                if (nestedToken.Type != JTokenType.Array
                    && typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType))
                {
                    IList collectionPropertyInstance;
                    Type collectionPropertyItemType;

                    // Does the property type have a parameterless constructor? i.e List<>
                    if (p.PropertyType.GetConstructors().Where(y => y.GetParameters().Length == 0).Any())
                    {
                        collectionPropertyInstance = Activator.CreateInstance(p.PropertyType) as IList;
                        collectionPropertyItemType = p.PropertyType.GetGenericArguments().FirstOrDefault();
                    }
                    // Does it have a constructor? i.e int[]
                    else
                    {
                        collectionPropertyInstance = Activator.CreateInstance(p.PropertyType, new object[] { 1 }) as IList;
                        collectionPropertyItemType = p.PropertyType.GetElementType();
                    }

                    object singlePropertyInstance = nestedToken.ToObject(collectionPropertyItemType);

                    // Was the collection a fixed array? object[1]
                    if (collectionPropertyInstance.Count == 1)
                        collectionPropertyInstance[0] = singlePropertyInstance;

                    // Or was the collection a form of List<>
                    else
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

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
