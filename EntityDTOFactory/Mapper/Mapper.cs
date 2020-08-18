using EntityDTOFactory.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityDTOFactory.Mapper
{
    /// <summary>
    /// Maps an object to another.
    /// Default behavior is to match objects by a 'DTO' suffix.
    /// Example: Entity maps to EntityDTO and vice versa.
    /// This behavior can be changed within the TypeNameResolver class.
    /// </summary>
    /// <typeparam name="TSource">Object we are mapping from</typeparam>
    /// <typeparam name="TDestination">Object we are mapping to</typeparam>
    public class Mapper<TSource, TDestination>
    {
        public Dictionary<Type, ObjectMap> objectMaps = new Dictionary<Type, ObjectMap>();

        public Mapper()
        {
            SetObjectMaps<TSource>();
            SetObjectMaps<TDestination>();

            // once all object maps are created, update their property to companion maps
            foreach (ObjectMap objectMap in objectMaps.Values)
            {
                objectMap.MapPropertyNamesToCompanionPropertyNames(objectMaps);
            }
        }

        /// <summary>
        /// Initializes the object maps for the specified type
        /// </summary>
        private void SetObjectMaps<T>()
        {
            Type[] types = Assembly.GetAssembly(typeof(T)).GetTypes();

            foreach (Type type in types)
            {
                if (!typeof(T).IsAssignableFrom(type) || type == typeof(T))
                {
                    continue;
                }

                ObjectMap objectMap = new ObjectMap(type);

                // could not find a matching type
                if (objectMap.Type == objectMap.CompanionType)
                    continue;

                objectMaps[type] = objectMap;
            }
        }

        /// <summary>
        /// Using the values from the source type, create an array of default constructor arguments
        /// to be passed to the destination type.
        /// </summary>
        public object[] GenerateConstructorArguments(TSource source)
        {
            Type sourceType = source.GetType();
            Type destinationType = objectMaps[sourceType].CompanionType;

            IList<object> arguments = new List<object>();
            ObjectMap destinationMap = objectMaps[destinationType];

            foreach (ParameterInfo parameter in destinationMap.Parameters)
            {
                object value;

                string parameterName = parameter.Name;
                string propertyName = destinationMap.ParameterToProperty[parameterName] ?? null;

                // a matching property was found
                if (propertyName != null)
                {
                    string sourcePropertyName = destinationMap.PropertyToCompanionProperty[propertyName];

                    // the property does not exist in the companion object
                    if (sourcePropertyName == null)
                    {
                        value = destinationMap.ParameterToObject[parameterName];
                        arguments.Add(value);
                        continue;
                    }
                        
                    value = sourceType.GetProperty(sourcePropertyName).GetValue(source);
                }
                else
                {
                    value = destinationMap.ParameterToObject[parameterName];
                }

                arguments.Add(value);
            }

            return arguments.ToArray();
        }

        /// <summary>
        /// Using the source values, update the destination's properties.
        /// </summary>
        /// <returns>The destination object with updated properties</returns>
        public TDestination Map(TSource source, TDestination destination)
        {
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            ObjectMap destinationMap = objectMaps[destinationType];

            // the constructor set all relevant properties, so no need to do it again
            if (destinationMap.ExcludedProperties.Count == 0)
                return destination;

            object value;

            foreach (string excludedPropertyName in destinationMap.ExcludedProperties)
            {
                string companionPropertyName = destinationMap.PropertyToCompanionProperty[excludedPropertyName] ?? null;

                // a matching property was found
                if (companionPropertyName != null)
                {
                    value = sourceType.GetProperty(companionPropertyName).GetValue(source);
                    destinationType.GetProperty(excludedPropertyName).SetValue(destination, value);
                }
            }

            return destination;
        }
    }
}
