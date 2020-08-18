using EntityDTOFactory.Demo;
using EntityDTOFactory.Mapper;
using EntityDTOFactory.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EntityDTOFactoryTests
{
    [TestClass]
    public class ExtensionsTests
    {
        public static Dictionary<Type, ObjectMap> objectMaps;

        [ClassInitialize]
        public static void InitializeObjectMap(TestContext testContext)
        {
            objectMaps = new Dictionary<Type, ObjectMap>();
        }

        [TestMethod]
        public void MapPropertyNamesToCompanionPropertyNames_OneToOne_ReturnsUpdatedObjectMap()
        {
            // Arrange
            ObjectMap rattlesnake = new ObjectMap(typeof(Rattlesnake));
            ObjectMap rattlesnakeDTO = new ObjectMap(typeof(RattlesnakeDTO));

            objectMaps[typeof(Rattlesnake)] = rattlesnake;
            objectMaps[typeof(RattlesnakeDTO)] = rattlesnakeDTO;

            Dictionary<string, string> propertyMap;
            Dictionary<string, string> expectedPropertyMap = new Dictionary<string, string>()
            {
                { "Name", "Name" },
                { "Length", "Length" },
                { "IsPoisonous", "IsPoisonous" },
                { "Skill", "Skill" }
            };

            // Act
            rattlesnake.MapPropertyNamesToCompanionPropertyNames(objectMaps);
            propertyMap = rattlesnake.PropertyToCompanionProperty;

            // Assert
            CollectionAssert.AreEquivalent(expectedPropertyMap, propertyMap, "Properties did not map as expected");
        }

        [TestMethod]
        public void MapPropertyNamesToCompanionPropertyNames_NotOneToOne_ReturnsUpdatedObjectMap()
        {
            // Arrange
            ObjectMap pigeon = new ObjectMap(typeof(Pigeon));
            ObjectMap pigeonDTO = new ObjectMap(typeof(PigeonDTO));

            objectMaps[typeof(Pigeon)] = pigeon;
            objectMaps[typeof(PigeonDTO)] = pigeonDTO;

            Dictionary<string, string> propertyMap;
            Dictionary<string, string> expectedPropertyMap = new Dictionary<string, string>()
            {
                { "Name", "Name" },
                { "MaxAltitude", "MaxAltitude" },
                { "IsReal", "IsReal" },
                { "Skill", "Skill" },
                { "Index", null }
            };

            // Act
            pigeon.MapPropertyNamesToCompanionPropertyNames(objectMaps);
            propertyMap = pigeon.PropertyToCompanionProperty;

            // Assert
            CollectionAssert.AreEquivalent(expectedPropertyMap, propertyMap, "Properties did not map as expected");
        }
    }
}
