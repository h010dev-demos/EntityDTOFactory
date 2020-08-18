using EntityDTOFactory.Demo;
using EntityDTOFactory.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EntityDTOFactoryTests
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void Mapper_ObjectsHaveBaseClassesWithNoEntityOrDTO_ReturnsObjectMapCollection()
        {
            // Arrange
            Dictionary<Type, ObjectMap> objectMaps;
            Dictionary<string, string> expectedMaps = new Dictionary<string, string>();
            Dictionary<string, string> actualMaps = new Dictionary<string, string>();
            Dictionary<Type, ObjectMap> expectedObjectMaps = new Dictionary<Type, ObjectMap>()
            {
                { typeof(Rattlesnake), new ObjectMap(typeof(RattlesnakeDTO)) },
                { typeof(RattlesnakeDTO), new ObjectMap(typeof(Rattlesnake)) },
                { typeof(Pigeon), new ObjectMap(typeof(PigeonDTO)) },
                { typeof(PigeonDTO), new ObjectMap(typeof(Pigeon)) },
                { typeof(Hawk), new ObjectMap(typeof(HawkDTO)) },
                { typeof(HawkDTO), new ObjectMap(typeof(Hawk)) }
            };

            foreach ((Type type, ObjectMap map) in expectedObjectMaps)
            {
                expectedMaps[type.Name] = map.GetType().Name;
            }

            // Act
            Mapper<Animal, AnimalDTO> mapper = new Mapper<Animal, AnimalDTO>();
            objectMaps = mapper.objectMaps;

            foreach ((Type type, ObjectMap map) in objectMaps)
            {
                actualMaps[type.Name] = map.GetType().Name;
            }

            // Assert
            CollectionAssert.AreEquivalent(expectedMaps, actualMaps, "Objects did not map as expected");
        }

        [TestMethod]
        public void GenerateConstructorArguments_ConstructorHasNoArgs_ReturnsEmptyArray()
        {
            // Arrange
            Mapper<Animal, AnimalDTO> mapper = new Mapper<Animal, AnimalDTO>();
            Animal rattlenake = new Rattlesnake("", 0.0, false, SpecialSkill.BITE);
            object[] expectedArgs = new object[0];
            object[] actualArgs;

            // Act
            actualArgs = mapper.GenerateConstructorArguments(rattlenake);

            // Assert
            CollectionAssert.AreEqual(expectedArgs, actualArgs, "Constructor args were not generated as expected");
        }

        [TestMethod]
        public void GenerateConstructorArguments_ConstructorHasArgs_ReturnsArray()
        {
            // Arrange
            Mapper<AnimalDTO, Animal> mapper = new Mapper<AnimalDTO, Animal>();
            AnimalDTO rattlenakeDTO = new RattlesnakeDTO()
            {
                Name = "",
                Length = 0.0,
                IsPoisonous = false,
                Skill = SpecialSkill.BITE
            };

            object[] expectedArgs = new object[] { "", 0.0, false, SpecialSkill.BITE };
            object[] actualArgs;

            // Act
            actualArgs = mapper.GenerateConstructorArguments(rattlenakeDTO);

            // Assert
            CollectionAssert.AreEqual(expectedArgs, actualArgs, "Constructor args were not generated as expected");
        }

        [TestMethod]
        public void Map_NoExcludedProperties_ReturnsSameObject()
        {
            // Arrange
            Mapper<AnimalDTO, Animal> mapper = new Mapper<AnimalDTO, Animal>();
            AnimalDTO rattlenakeDTO = new RattlesnakeDTO()
            {
                Name = "Name",
                Length = 10.0,
                IsPoisonous = true,
                Skill = SpecialSkill.ROLLOVER
            };
            Animal expectedRattlesnake = new Rattlesnake("", 0.0, false, SpecialSkill.BITE);
            Animal actualRattlesnake;

            // Act
            actualRattlesnake = (Rattlesnake)mapper.Map(rattlenakeDTO, expectedRattlesnake);
            expectedRattlesnake = new Rattlesnake("", 0.0, false, SpecialSkill.BITE);

            // Assert
            foreach (PropertyInfo property in actualRattlesnake.GetType().GetProperties())
            {
                object expectedValue = expectedRattlesnake
                    .GetType()
                    .GetProperty(property.Name)
                    .GetValue(expectedRattlesnake);

                object actualValue = property
                    .GetValue(actualRattlesnake);

                Assert.AreEqual(expectedValue, actualValue, $"{property.Name} is not as expected");
            }
        }

        [TestMethod]
        public void Map_HasExcludedProperties_ReturnsUpdatedObject()
        {
            // Arrange
            Mapper<Animal, AnimalDTO> mapper = new Mapper<Animal, AnimalDTO>();
            AnimalDTO expectedRattlesnakeDTO = new RattlesnakeDTO()
            {
                Name = "Name",
                Length = 10.0,
                IsPoisonous = true,
                Skill = SpecialSkill.BITE
            };
            Animal rattlesnake = new Rattlesnake("Name", 10.0, true, SpecialSkill.BITE);
            AnimalDTO actualRattlesnakeDTO;

            // Act
            actualRattlesnakeDTO = (RattlesnakeDTO)mapper.Map(rattlesnake, expectedRattlesnakeDTO);
            expectedRattlesnakeDTO = new RattlesnakeDTO()
            {
                Name = "Name",
                Length = 10.0,
                IsPoisonous = true,
                Skill = SpecialSkill.BITE
            };

            // Assert
            foreach (PropertyInfo property in actualRattlesnakeDTO.GetType().GetProperties())
            {
                object expectedValue = expectedRattlesnakeDTO
                    .GetType()
                    .GetProperty(property.Name)
                    .GetValue(expectedRattlesnakeDTO);

                object actualValue = property
                    .GetValue(actualRattlesnakeDTO);

                Assert.AreEqual(expectedValue, actualValue, $"{property.Name} is not as expected");
            }
        }
    }
}
