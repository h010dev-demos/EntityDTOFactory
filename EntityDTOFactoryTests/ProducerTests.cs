using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using EntityDTOFactory.Demo;
using EntityDTOFactory.Factory;

namespace EntityDTOFactoryTests
{
    [TestClass]
    public class ProducerTests
    {
        public static IList<AnimalDTO> animalDTOsBase = new List<AnimalDTO>();
        public static IList<Animal> animalsBase = new List<Animal>();
        public static AnimalDTO animalDTOBase;
        public static Animal animalBase;

        [ClassInitialize]
        public static void InitializeEntitiesAndDTOs(TestContext testContext)
        {
            animalDTOBase = new PigeonDTO()
            {
                Name = "Pidgey",
                MaxAltitude = 100,
                IsReal = true,
                Skill = SpecialSkill.FLY
            };
            animalBase = new Pigeon("Pidgey", 100, true, 0, SpecialSkill.FLY);

            foreach (int value in Enumerable.Range(1, 10))
            {
                Animal pigeon = new Pigeon($"PidgeyNo.{value}", value * 100, false, value - 1, SpecialSkill.FLY);
                Animal hawk = new Hawk($"FearowNo.{value}", value * 200, false, value - 1, SpecialSkill.FLY, (Bird)pigeon);
                Animal rattlesnake = new Rattlesnake($"RattlesnakeNo.{value}", value * 10, true, SpecialSkill.BITE);

                animalsBase.Add(pigeon);
                animalsBase.Add(hawk);
                animalsBase.Add(rattlesnake);

                AnimalDTO pigeonDTO = new PigeonDTO()
                {
                    Name = $"PidgeyNo.{value}",
                    MaxAltitude = value * 100,
                    IsReal = false,
                    Skill = SpecialSkill.FLY
                };

                AnimalDTO hawkDTO = new HawkDTO()
                {
                    Name = $"FearowNo.{value}",
                    MaxAltitude = value * 200,
                    IsReal = false,
                    Skill = SpecialSkill.FLY,
                    FlyingPartner = (Bird)pigeon
                };

                AnimalDTO rattlesnakeDTO = new RattlesnakeDTO()
                {
                    Name = $"RattlesnakeNo.{value}",
                    Length = value * 10,
                    IsPoisonous = true,
                    Skill = SpecialSkill.BITE
                };

                animalDTOsBase.Add(pigeonDTO);
                animalDTOsBase.Add(hawkDTO);
                animalDTOsBase.Add(rattlesnakeDTO);
            }
        }

        [TestMethod]
        public void ConvertCollection_CollectionOfDTOs_ReturnsCollectionOfEntities()
        {
            // Arrange
            Producer<AnimalDTO, Animal> producer = new Producer<AnimalDTO, Animal>();
            IList<AnimalDTO> animalDTOs = animalDTOsBase;
            IList<Animal> expectedAnimals = animalsBase;
            IList<Animal> animals;

            // Act
            animals = producer.ConvertCollection(animalDTOs);

            // Assert
            foreach (Animal animal in animals)
            {
                int index = animals.IndexOf(animal);
                Animal expected = expectedAnimals[index];

                switch (animal)
                {
                    case Pigeon _:
                        {
                            Pigeon pigeon = animal as Pigeon;
                            Pigeon expectedPigeon = expected as Pigeon;
                            foreach (PropertyInfo property in pigeon.GetType().GetProperties())
                            {
                                Assert.AreEqual(
                                    expectedPigeon.GetType().GetProperty(property.Name), 
                                    pigeon.GetType().GetProperty(property.Name), 
                                    $"{property.Name} does not match.");
                            }
                        }
                        break;
                    case Hawk _:
                        {
                            Hawk hawk = animal as Hawk;
                            Hawk expectedHawk = expected as Hawk;
                            foreach (PropertyInfo property in hawk.GetType().GetProperties())
                            {
                                Assert.AreEqual(
                                    expectedHawk.GetType().GetProperty(property.Name),
                                    hawk.GetType().GetProperty(property.Name),
                                    $"{property.Name} does not match.");
                            }
                        }
                        break;
                    case Rattlesnake _:
                        {
                            Rattlesnake rattlesnake = animal as Rattlesnake;
                            Rattlesnake expectedRattleSnake = expected as Rattlesnake;
                            foreach (PropertyInfo property in rattlesnake.GetType().GetProperties())
                            {
                                Assert.AreEqual(
                                    expectedRattleSnake.GetType().GetProperty(property.Name),
                                    rattlesnake.GetType().GetProperty(property.Name),
                                    $"{property.Name} does not match.");
                            }
                        }
                        break;
                }
            }
        }

        [TestMethod]
        public void ConvertCollection_CollectionOfEntities_ReturnsCollectionOfDTOs()
        {
            // Arrange
            Producer<Animal, AnimalDTO> producer = new Producer<Animal, AnimalDTO>();
            IList<Animal> animals = animalsBase;
            IList<AnimalDTO> expectedAnimalDTOs = animalDTOsBase;
            IList<AnimalDTO> animalDTOs;

            // Act
            animalDTOs = producer.ConvertCollection(animals);

            // Assert
            foreach (AnimalDTO animalDTO in animalDTOs)
            {
                int index = animalDTOs.IndexOf(animalDTO);
                AnimalDTO expected = expectedAnimalDTOs[index];

                switch (animalDTO)
                {
                    case PigeonDTO _:
                        {
                            PigeonDTO pigeon = animalDTO as PigeonDTO;
                            PigeonDTO expectedPigeon = expected as PigeonDTO;
                            foreach (PropertyInfo property in pigeon.GetType().GetProperties())
                            {
                                Assert.AreEqual(
                                    expectedPigeon.GetType().GetProperty(property.Name),
                                    pigeon.GetType().GetProperty(property.Name),
                                    $"{property.Name} does not match.");
                            }
                        }
                        break;
                    case HawkDTO _:
                        {
                            HawkDTO hawk = animalDTO as HawkDTO;
                            HawkDTO expectedHawk = expected as HawkDTO;
                            foreach (PropertyInfo property in hawk.GetType().GetProperties())
                            {
                                Assert.AreEqual(
                                    expectedHawk.GetType().GetProperty(property.Name),
                                    hawk.GetType().GetProperty(property.Name),
                                    $"{property.Name} does not match.");
                            }
                        }
                        break;
                    case RattlesnakeDTO _:
                        {
                            RattlesnakeDTO rattlesnake = animalDTO as RattlesnakeDTO;
                            RattlesnakeDTO expectedRattleSnake = expected as RattlesnakeDTO;
                            foreach (PropertyInfo property in rattlesnake.GetType().GetProperties())
                            {
                                Assert.AreEqual(
                                    expectedRattleSnake.GetType().GetProperty(property.Name),
                                    rattlesnake.GetType().GetProperty(property.Name),
                                    $"{property.Name} does not match.");
                            }
                        }
                        break;
                }
            }
        }

        [TestMethod]
        public void Produce_FromDTOToEntity_ReturnsEntity()
        {
            // Arrange
            Producer<AnimalDTO, Animal> producer = new Producer<AnimalDTO, Animal>();
            PigeonDTO dto = animalDTOBase as PigeonDTO;
            Pigeon entity = animalBase as Pigeon;

            // Act
            Pigeon producedEntity = (Pigeon)producer.Produce(dto);

            // Assert
            Assert.AreEqual(entity.Name, producedEntity.Name, "Name does not match");
            Assert.AreEqual(entity.MaxAltitude, producedEntity.MaxAltitude, "Altitude does not match");
            Assert.AreEqual(entity.IsReal, producedEntity.IsReal, "IsReal does not match");
            Assert.AreEqual(entity.Skill, producedEntity.Skill, "Skill does not match");
        }

        [TestMethod]
        public void Produce_FromEntityToDTO_ReturnsDTO()
        {
            // Arrange
            Producer<Animal, AnimalDTO> producer = new Producer<Animal, AnimalDTO>();
            PigeonDTO dto = animalDTOBase as PigeonDTO;
            Pigeon entity = animalBase as Pigeon;

            // Act
            PigeonDTO producedDTO = (PigeonDTO)producer.Produce(entity);

            // Assert
            Assert.AreEqual(dto.Name, producedDTO.Name, "Name does not match");
            Assert.AreEqual(dto.MaxAltitude, producedDTO.MaxAltitude, "Altitude does not match");
            Assert.AreEqual(dto.IsReal, producedDTO.IsReal, "IsReal does not match");
            Assert.AreEqual(dto.Skill, producedDTO.Skill, "Skill does not match");
        }
    }
}
