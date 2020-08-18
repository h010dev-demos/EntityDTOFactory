using EntityDTOFactory.Helpers;
using EntityDTOFactory.Demo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EntityDTOFactoryTests
{
    [TestClass]
    public class TypeNameResolverTests
    {
        [TestMethod]
        public void ConvertType_TypeIsNotEntityOrDTO_ReturnsType()
        {
            // Arrange
            Type type = typeof(Bird);
            Type expectedType = typeof(Bird);

            // Act
            Type outType = TypeNameResolver.ConvertType(type);

            // Assert
            Assert.AreEqual(expectedType, outType, $"{expectedType} does not match {outType}");
        }

        [TestMethod]
        public void ConvertType_FromEntityToDTO_ReturnsDTO()
        {
            // Arrange
            Type entityType = typeof(Pigeon);
            Type expectedDTOType = typeof(PigeonDTO);

            // Act
            Type dtoType = TypeNameResolver.ConvertType(entityType);

            // Assert
            Assert.AreEqual(expectedDTOType, dtoType, $"{expectedDTOType} does not match {dtoType}");
        }

        [TestMethod]
        public void ConvertType_FromDTOToEntity_ReturnsEntity()
        {
            // Arrange
            Type dtoType = typeof(PigeonDTO);
            Type expectedEntityType = typeof(Pigeon);

            // Act
            Type entityType = TypeNameResolver.ConvertType(dtoType);

            // Assert
            Assert.AreEqual(expectedEntityType, entityType, $"{expectedEntityType} does not match {entityType}");
        }

        [TestMethod]
        public void ToDTO_DTOExists_ReturnsDTO()
        {
            // Arrange
            Type entityType = typeof(Pigeon);
            Type expectedDTOType = typeof(PigeonDTO);

            // Act
            Type dtoType = TypeNameResolver.ToDTO(entityType);

            // Assert
            Assert.AreEqual(expectedDTOType, dtoType, $"{expectedDTOType} does not match {dtoType}");
        }

        [TestMethod]
        public void FromDTO_EntityExists_ReturnsEntity()
        {
            // Arrange
            Type dtoType = typeof(PigeonDTO);
            Type expectedEntityType = typeof(Pigeon);

            // Act
            Type entityType = TypeNameResolver.FromDTO(dtoType);

            // Assert
            Assert.AreEqual(expectedEntityType, entityType, $"{expectedEntityType} does not match {entityType}");
        }
    }
}
