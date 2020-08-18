using System;
using System.Text.RegularExpressions;

namespace EntityDTOFactory.Helpers
{
    public static class TypeNameResolver
    {
        /// <summary>
        /// Attempts to convert an Entity to DTO or vice versa
        /// </summary>
        public static Type ConvertType(Type type)
        {
            Type _type;

            Type dto = ToDTO(type);
            Type entity = FromDTO(type);

            switch (type)
            {
                case var _ when type.Name == dto.Name:
                    _type = entity;
                    break;
                case var _ when type.Name == entity.Name:
                    _type = dto;
                    break;
                default:
                    _type = type;
                    break;
            }

            return _type;
        }

        public static Type ToDTO(Type type)
        {
            Type result = Type.GetType($"{type.FullName}DTO");

            if (result == null)
                return type;

            return result;
        }
        public static Type FromDTO(Type type)
        {
            Type result = Type.GetType(Regex.Replace(type.FullName, @"(DTO)$", ""));

            if (result == null)
                return type;

            return result;
        }
    }
}
