using MAD.JsonConverters.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests
{
    [TestClass]
    public class NestedConverterTests
    {
        private string NestedExample1Json;

        [TestInitialize]
        public void Init()
        {
            this.NestedExample1Json = File.ReadAllText("NestedJsonExample1.js");
        }

        [TestMethod]
        public void ReadJson_SimpleNestedStructure_FlattensIntoSingleObject()
        {
            NestedJsonExample1Model result = JsonConvert.DeserializeObject<NestedJsonExample1Model>(this.NestedExample1Json, new NestedJsonConverter());

            Assert.IsTrue(result.Description == "Eat a berry ya mong");
            Assert.IsTrue(result.IsActive == true);
            Assert.IsTrue(result.Items.Count == 3);
        }
    }
}
