using EntityDTOFactory.Demo;
using EntityDTOFactory.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityDTOFactoryTests
{
    [TestClass]
    public class FactoryTests
    {
        public static Pigeon entity1;
        public static Hawk entity2;
        public static HawkDTO dto;

        [ClassInitialize]
        public static void InitializeEntityAndDTO(TestContext testContext)
        {
            entity1 = new Pigeon("Pidgey", 100, false, 0, SpecialSkill.FETCH);
            entity2 = new Hawk("Fearow", 200, true, 0, SpecialSkill.FLY, entity1);
            dto = new HawkDTO();
        }

        [TestMethod]
        public void CreateObject_NewEntityObject_ReturnsEntity()
        {
            // Arrange
            Pigeon pigeon = entity1;
            Hawk hawk = entity2;

            // Act
            Factory<Hawk> factory = new Factory<Hawk>();
            factory.map["Hawk"] = typeof(Hawk);
            Hawk newHawk = factory.CreateObject("Hawk", "Fearow", 200, true, 0, SpecialSkill.FLY, pigeon);

            // Assert
            Assert.AreEqual(hawk.Name, newHawk.Name, "Name does not match");
            Assert.AreEqual(hawk.MaxAltitude, newHawk.MaxAltitude, "MaxAltitude does not match");
            Assert.AreEqual(hawk.IsReal, newHawk.IsReal, "IsReal does not match");
            Assert.AreEqual(hawk.Index, newHawk.Index, "Index does not match");
            Assert.AreEqual(hawk.Skill, newHawk.Skill, "Skill does not match");
            Assert.AreEqual(hawk.FlyingPartner, newHawk.FlyingPartner, "FlyingPartner does not match");
        }

        [TestMethod]
        public void CreateObject_NewDTOObject_ReturnsDTO()
        {
            // Arrange
            HawkDTO hawkDTO = new HawkDTO();

            // Act
            Factory<HawkDTO> factory = new Factory<HawkDTO>();
            factory.map["HawkDTO"] = typeof(HawkDTO);
            HawkDTO newHawkDTO = factory.CreateObject("HawkDTO");

            // Assert
            Assert.AreEqual(hawkDTO.Name, newHawkDTO.Name, "Name does not match");
            Assert.AreEqual(hawkDTO.MaxAltitude, newHawkDTO.MaxAltitude, "MaxAltitude does not match");
            Assert.AreEqual(hawkDTO.IsReal, newHawkDTO.IsReal, "IsReal does not match");
            Assert.AreEqual(hawkDTO.Skill, newHawkDTO.Skill, "Skill does not match");
            Assert.AreEqual(hawkDTO.FlyingPartner, newHawkDTO.FlyingPartner, "FlyingPartner does not match");
        }
    }
}
