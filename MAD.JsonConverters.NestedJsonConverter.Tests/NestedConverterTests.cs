﻿using MAD.JsonConverters.NestedJsonConverterNS.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests
{
    [TestClass]
    public class NestedConverterTests
    {
        private string nestedExample1Json;
        private string nestedExample2Json;

        private string singleObjectAsNestedProperty;

        [TestInitialize]
        public void Init()
        {
            this.nestedExample1Json = File.ReadAllText("NestedJsonExample1.js");
            this.nestedExample2Json = File.ReadAllText("NestedJsonExample2.js");
            this.singleObjectAsNestedProperty = File.ReadAllText("JsonWithAnObjectProperty.js");
        }

        [TestMethod]
        public void ReadJson_Example1_FlattensIntoSingleObject()
        {
            NestedJsonExample1Model result = JsonConvert.DeserializeObject<NestedJsonExample1Model>(this.nestedExample1Json, new NestedJsonConverter());

            Assert.IsTrue(result.Description == "Eat a berry ya mong");
            Assert.IsTrue(result.IsActive == true);
            Assert.IsTrue(result.Items.Count == 3);
        }

        [TestMethod]
        public void ReadJson_Example2_DeserializesNestedClass()
        {
            NestedJsonExample2Model result = JsonConvert.DeserializeObject<NestedJsonExample2Model>(this.nestedExample2Json, new NestedJsonConverter());

            Assert.IsNotNull(result.Header);
            Assert.IsTrue(result.Header.Description == "steak is gr8");
            Assert.IsTrue(result.Header.IsActive);
        }

        [TestMethod]
        public void ReadJson_SingleNestedObject_CSharpArrayHandlesSingleJsonObjects()
        {
            ModelWithAnArrayProperty result = JsonConvert.DeserializeObject<ModelWithAnArrayProperty>(this.singleObjectAsNestedProperty, new NestedJsonConverter());

            Assert.IsTrue(result.Invoices.First().InvoiceNumber == "101001");
        }

        [TestMethod]
        public void ReadJson_DynamicKeyInJson_JsonPropertyWithStarOperatorMatches()
        {
            string json =
@"{
    responseCode: 1337,
    randomKeyForSomeStupidReason: ['smell me donkey bitch'],
    iLikeClams: true
}";

            DynamicResponseFromSomeShittyApiModel<string[]> model = 
                JsonConvert.DeserializeObject<DynamicResponseFromSomeShittyApiModel<string[]>>(json, new NestedJsonConverter());

            Assert.AreEqual(model.ResponseCode, 1337);
            CollectionAssert.Contains(model.Result, "smell me donkey bitch");
            Assert.IsTrue(model.ILikeClams);
        }

        [TestMethod]
        public void ReadJson_MultipleWildcardsInModel_ThrowsException()
        {
            string json =
@"{
    minions: ['jerry', 'rick'],
    plebs: ['smegma']
}";

            Assert.ThrowsException<JsonSerializationException>(() =>
            {
                MultipleWildcardModel model =
                JsonConvert.DeserializeObject<MultipleWildcardModel>(json, new NestedJsonConverter());
            });
        }
    }
}
