using EntityDTOFactory.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityDTOFactory.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Creates a map that ties the current object's properties to those of its companion
        /// </summary>
        public static void MapPropertyNamesToCompanionPropertyNames(this ObjectMap objectMap, Dictionary<Type, ObjectMap> objectMaps)
        {
            ObjectMap companionObjectMap = objectMaps[objectMap.CompanionType];
            PropertyInfo[] companionProperties = companionObjectMap.Properties;

            if (companionProperties == null)
                return;

            foreach (PropertyInfo property in objectMap.Properties)
            {
                string propertyName = property.Name;
                string propertyNameLowerCase = propertyName.ToLower();

                PropertyInfo match = companionProperties.Where(prop => prop.Name.ToLower() == propertyNameLowerCase).FirstOrDefault();

                // companion property exists
                if (match != null)
                {
                    string companionPropertyName = match.Name;
                    objectMap.PropertyToCompanionProperty[property.Name] = companionPropertyName;
                }
                else
                {
                    objectMap.PropertyToCompanionProperty[propertyName] = null;
                }
            }
        }
    }
}
