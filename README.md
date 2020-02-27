Repository
==========

This project is licensed under the MIT license.

NestedJsonConverter
--------------------
NestedJsonConverter is a JsonConverter for [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) and helps you deserialize nested JSON payloads into a flat model. Quick and easy to use.

### Installation

NestedJsonConverter is available on [NuGet](https://www.nuget.org/packages/MAD.JsonConverters.NestedJsonConverter).

### Usage

The following code shows you how to use the NestedJsonConverter.

```cs
public class NestedJsonExample2Model
{
    public class HeaderModel
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    [JsonProperty("details.header")]
    public HeaderModel Header { get; set; }
}

string json = 
@"{
    details: {
        header: {
            description: "steak is gr8",
            isActive: true
        }
    }
}";

NestedJsonExample2Model result = JsonConvert.DeserializeObject<NestedJsonExample2Model>(json, new NestedJsonConverter());
```
Or simply mark your class with the JsonConverter attribute and DeserializeObject will automatically use it.
```cs
[JsonConverter(typeof(NestedJsonConverter))
public class NestedJsonExample2Model
{
    public class HeaderModel
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    [JsonProperty("details.header")]
    public HeaderModel Header { get; set; }
}
```
### Wildcards

NestedJsonConverter also supports a single wildcard in your C# model. Use this if you have a json payload and it randomly changes one of the fields key.

```cs
public class DynamicResponseFromSomeShittyApiModel <TEntity>
{
    public int ResponseCode { get; set; }

    [JsonProperty("*")]
    public TEntity Result { get; set; }

    public bool ILikeClams { get; set; }
}

string json =
@"{
    responseCode: 1337,
    randomKeyForSomeStupidReason: ['smell me donkey bitch'],
    iLikeClams: true
}";

DynamicResponseFromSomeShittyApiModel<string[]> model = 
    JsonConvert.DeserializeObject<DynamicResponseFromSomeShittyApiModel<string[]>>(json, new NestedJsonConverter());
```

Contributing
==========

Go for it.
