using EntityDTOFactory.Demo;
using EntityDTOFactory.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EntityDTOFactoryTests
{
    [TestClass]
    public class ObjectMapTests
    {
        [TestMethod]
        public void ObjectMap_ObjectIsNotEntityOrDTO_ReturnsBasicObjectMap()
        {
            // Arrange
            Type objectType = typeof(Bird);

            // Act
            ObjectMap map = new ObjectMap(objectType);

            // Assert
            Assert.AreEqual(null, map.Properties, $"{objectType} failed to cause constructor to exit");
        }

        [TestMethod]
        public void SelectIdealConstructor_DefaultConstructorOnly_ReturnsEmptyConstructorInfo()
        {
            // Arrange
            Type objectType = typeof(PigeonDTO);

            // Act
            ObjectMap map = new ObjectMap(objectType);
            map.SelectIdealConstructor();
            int expectedConstructorParameters = 0;

            // Assert
            Assert.AreEqual(expectedConstructorParameters, map.Parameters.Length, $"{map} failed to return an empty constructor");
        }

        [TestMethod]
        public void GenerateConstructorArguments_DefaultConstructorOnly_ReturnsEmptyArray()
        {
            // Arrange
            Type objectType = typeof(PigeonDTO);

            // Act
            ObjectMap map = new ObjectMap(objectType);
            object[] arguments = map.GenerateConstructorArguments();
            object[] expectedArguments = new object[] { };

            // Assert
            Assert.AreEqual(expectedArguments.Length, arguments.Length, $"{map} failed to return an empty argument array");
        }

        [TestMethod]
        public void GenerateConstructorArguments_SingleConstructorWithParameters_ReturnsArray()
        {
            // Arrange
            Type objectType = typeof(Pigeon);

            // Act
            ObjectMap map = new ObjectMap(objectType);
            object[] arguments = map.GenerateConstructorArguments();
            object[] expectedArguments = new object[] { null, 0, false, 0, SpecialSkill.ROLLOVER };

            // Assert
            for (int i = 0; i < arguments.Length; i++)
            {
                Assert.AreEqual(expectedArguments[i], arguments[i], $"{arguments[i]} does not match {expectedArguments[i]}");
            }
        }

        [TestMethod]
        public void MapParameterNamesToPropertyNames_DefaultConstructorOnly_ReturnsParameterDict()
        {
            // Arrange
            ObjectMap map = new ObjectMap(typeof(PigeonDTO));
            Dictionary<string, string> expectedParameterMap = new Dictionary<string, string>();
            Dictionary<string, string> actualParameterMap;

            // Act
            map.MapParameterNamesToPropertyNames();
            actualParameterMap = map.ParameterToProperty;

            // Assert
            CollectionAssert.AreEquivalent(expectedParameterMap, actualParameterMap, "Parameter map is not as expected");
        }

        [TestMethod]
        public void MapParameterNamesToPropertyNames_ConstructorWithArgs_ReturnsParameterDict()
        {
            // Arrange
            ObjectMap map = new ObjectMap(typeof(Pigeon));
            Dictionary<string, string> expectedParameterMap = new Dictionary<string, string>()
            {
                { "name", "Name" },
                { "maxAltitude", "MaxAltitude" },
                { "isReal", "IsReal" },
                { "index", "Index" },
                { "skill", "Skill" }
            };
            Dictionary<string, string> actualParameterMap;

            // Act
            map.MapParameterNamesToPropertyNames();
            actualParameterMap = map.ParameterToProperty;

            // Assert
            CollectionAssert.AreEquivalent(expectedParameterMap, actualParameterMap, "Parameter map is not as expected");
        }

        [TestMethod]
        public void GetPropertiesExcludedFromConstructor_DefaultConstructorOnly_ReturnsAllProperties()
        {
            // Arrange
            ObjectMap map = new ObjectMap(typeof(PigeonDTO));
            List<string> expectedProperties = new List<string>()
            {
                "Name",
                "MaxAltitude",
                "IsReal",
                "Skill"
            };
            List<string> actualProperties;

            // Act
            map.GetPropertiesExcludedFromConstructor();
            actualProperties = (List<string>)map.ExcludedProperties;

            // Assert
            CollectionAssert.AreEquivalent(expectedProperties, actualProperties, "Excluded properties are not as expected");
        }

        [TestMethod]
        public void GetPropertiesExcludedFromConstructor_NoExcludedProperties_ReturnsNoProperties()
        {
            // Arrange
            ObjectMap map = new ObjectMap(typeof(Pigeon));
            List<string> expectedProperties = new List<string>();
            List<string> actualProperties;

            // Act
            map.GetPropertiesExcludedFromConstructor();
            actualProperties = (List<string>)map.ExcludedProperties;

            // Assert
            CollectionAssert.AreEquivalent(expectedProperties, actualProperties, "Excluded properties are not as expected");
        }
    }
}
